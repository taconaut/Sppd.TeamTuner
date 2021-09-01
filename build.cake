//////////////////////////////////////////////////////////////////////
// Tools & Add-ins
//////////////////////////////////////////////////////////////////////

#tool nuget:?package=Codecov&version=1.13.0

#addin nuget:?package=Cake.Coverlet&version=2.5.4
#addin nuget:?package=Cake.Codecov&version=1.0.1
#addin nuget:?package=Cake.Npm&version=1.0.0
#addin nuget:?package=Cake.CodeGen.NSwag&version=1.2.0&loaddependencies=true
#addin nuget:?package=Cake.Git&version=1.1.0

//////////////////////////////////////////////////////////////////////
// Arguments
//////////////////////////////////////////////////////////////////////

var target = Argument ("target", "Default");
var configuration = Argument ("configuration", "Release");
var version = Argument ("applicationVersion", "1.0.0.0");

//////////////////////////////////////////////////////////////////////
// Preparation
//////////////////////////////////////////////////////////////////////

// Directories

// Existing
var projectDir = Directory ("./");
var backendDir = Directory ("./Backend");
var frontendDir = Directory ("./Frontend");
var frontendDistDir = MakeAbsolute (MakeAbsolute (frontendDir).Combine (Directory ("dist")));

// To be created
var artifactsDir = MakeAbsolute (Directory ("./artifacts"));
var testOutputDir = MakeAbsolute (Directory ("./test-output"));
var testCoverageResultsDir = MakeAbsolute (testOutputDir.Combine (Directory ("coverage-results")));
var testResultsDir = MakeAbsolute (testOutputDir.Combine (Directory ("test-results")));

// File paths
var solutionPath = backendDir.Path + "/Sppd.TeamTuner.sln";
var teamTunerProjectPath = backendDir.Path + "/Sppd.TeamTuner/Sppd.TeamTuner.csproj";
var apiClientFilePath = frontendDir.Path + "/src/api.ts";

// File names
var unitTestResultsFileName = "coverage-results-unit.opencover.xml";
var integrationTestResultsFileName = "coverage-results-integration.opencover.xml";
var apiTestResultsFileName = "coverage-results-api.opencover.xml";

// URIs
var localApiUri = "https://localhost:44336/swagger/v1/swagger.json";

//////////////////////////////////////////////////////////////////////
// Tasks
//////////////////////////////////////////////////////////////////////

Task ("Common-Clean")
    .Does (() => {
        CleanDirectory (artifactsDir);
        Information ($"Deleted all content in '{artifactsDir}'");
    });

Task ("Backend-Clean")
    .Does (() => {
        CleanDirectory (testOutputDir);
        Information ($"Deleted all content in '{testOutputDir}'");
        CleanDirectory (testCoverageResultsDir);
        Information ($"Deleted all content in '{testCoverageResultsDir}'");
        CleanDirectory (testResultsDir);
        Information ($"Deleted all content in '{testResultsDir}'");
        CleanDirectories ($"./**/obj/{configuration}");
        Information ($"Deleted all content for './**/obj/{configuration}'");
        CleanDirectories ($"./**/bin/{configuration}");
        Information ($"Deleted all content for './**/bin/{configuration}'");
    });

Task ("Frontend-Clean")
    .Does (() => {
        CleanDirectory (frontendDistDir);
        Information ($"Deleted all content in '{frontendDistDir}'");
    });

Task ("Clean")
    .IsDependentOn ("Common-Clean")
    .IsDependentOn ("Backend-Clean")
    .IsDependentOn ("Frontend-Clean");

Task ("Backend-Restore-NuGet-Packages")
    .IsDependentOn ("Backend-Clean")
    .Does (() => {
        NuGetRestore (solutionPath);
        Information ($"Restored nuget packages for solution '{solutionPath}'");
    });

Task ("Backend-Build")
    .IsDependentOn ("Backend-Restore-NuGet-Packages")
    .Does (() => {
        DotNetCoreBuild (
            solutionPath,
            new DotNetCoreBuildSettings {
                NoRestore = true,
                    Configuration = configuration
            }
        );
        Information ($"Finished building backend for solution '{solutionPath}'");
    });

Task ("Frontend-Npm-Install")
    .Does (() => {
        var settings = new NpmInstallSettings {
        WorkingDirectory = frontendDir,
        Production = false
        };

        NpmInstall (settings);
        Information ($"Executed 'npm install' for frontend located in '{frontendDir}'");
    });

Task ("Frontend-Build")
    .IsDependentOn ("Frontend-Clean")
    .IsDependentOn ("Frontend-Npm-Install")
    .Does (() => {
        var settings = new NpmRunScriptSettings {
        ScriptName = "build",
        WorkingDirectory = frontendDir
        };

        NpmRunScript (settings);
        Information ($"Executed 'npm run build' for frontend located in '{frontendDir}'");
    });

Task ("Build")
    .IsDependentOn ("Backend-Build")
    .IsDependentOn ("Frontend-Build");

//////////////////////////////////////////////////////////////////////
////// Packaging
//////////////////////////////////////////////////////////////////////

Task ("Backend-Test-Package")
    .IsDependentOn ("Common-Clean")
    .IsDependentOn ("Backend-Run-All-Tests")
    .Does (() => {
        var publishSettings = new DotNetCorePublishSettings {
        NoBuild = true,
        NoRestore = true,
        Configuration = configuration,
        OutputDirectory = $"{artifactsDir}/Backend"
        };
        DotNetCorePublish (
            teamTunerProjectPath,
            publishSettings
        );

        Information ($"The packaged backend is available in '{publishSettings.OutputDirectory}");
    });

Task ("Frontend-Package")
    .IsDependentOn ("Common-Clean")
    .IsDependentOn ("Frontend-Build")
    .Does (() => {
        var frontendPackageDirectory = $"{artifactsDir}/Frontend";
        CopyDirectory (frontendDistDir, frontendPackageDirectory);
        Information ($"The packaged frontend is available in '{frontendPackageDirectory}");
    });

Task ("Zip-Package")
    .IsDependentOn ("Backend-Test-Package")
    .IsDependentOn ("Frontend-Package")
    .Does (() => {
        var gitCommitHash = GitLogTip (projectDir).Sha;
        // TODO: handle tag version once release process has been defined
        var version = gitCommitHash?.Substring (0, 8) ?? "Unknown";
        var zipFilePath = $"{artifactsDir}/Sppd.TeameTuner-{version}.zip";
        Zip (artifactsDir, zipFilePath);
        Information ($"The zipped package is available in '{zipFilePath}");
    });

//////////////////////////////////////////////////////////////////////
// Versioning
//////////////////////////////////////////////////////////////////////

Task ("Release-Prepare")
    .IsDependentOn ("Backend-Release-Prepare");

Task ("Backend-Release-Prepare")
    .Does (() => {
        // Compute the package version
        var gitCommitHash = GitLogTip (projectDir)?.Sha;
        var gitVersion = gitCommitHash?.Substring (0, 8) ?? "Unknown";
        var currentDateTime = DateTime.UtcNow.ToString ("yyyyMMddHHmmss");
        var packageVersion = $"{version}-{gitVersion}-{currentDateTime}";

        // Versions having to be modified in *.csproj
        var versionXPath = "/Project/PropertyGroup/Version";
        var assemblyVersionXPath = "/Project/PropertyGroup/AssemblyVersion";
        var fileVersionXPath = "/Project/PropertyGroup/FileVersion";

        // Set versions for all projects (excpet testing)
        var projectFilePaths = GetFiles ("./**/*.csproj");
        foreach (var projectFilePath in projectFilePaths) {
            if (projectFilePath.FullPath.Contains ("Tests")) {
                // Do not version test projects
                continue;
            }

            XmlPoke (projectFilePath, versionXPath, packageVersion);
            XmlPoke (projectFilePath, assemblyVersionXPath, version);
            XmlPoke (projectFilePath, fileVersionXPath, version);
        }

        Information ($"Updated projects with AssemblyVersion/FileVersion='{version}' and Version='{packageVersion}'");
    });

//////////////////////////////////////////////////////////////////////
////// Client API generation
//////////////////////////////////////////////////////////////////////

Task ("Frontend-Generate-API-Client")
    .Does (() => {
        // The frontend API client is being generated from the swagger json made available when running the backend.
        // This process could be improved by using the project itself as a source, there are currently following issues I'm aware of
        // 1) When trying to do this from NSwagStudio, the process hangs and never finishes.
        // 2) Cake.CodeGen.NSwag does not expose this yet.
        var settings = new TypeScriptClientGeneratorSettings {
            ClassName = "{controller}Client",
            Template = TypeScriptTemplate.Axios,
            OperationNameGenerator = new MultipleClientsFromFirstTagAndOperationIdGenerator(),
            ExceptionClass = "ApiException",
            GenerateDtoTypes = true
        };

        NSwag.FromJsonSpecification (new Uri (localApiUri))
            .GenerateTypeScriptClient (apiClientFilePath, settings);

        Information ($"The client API has been updated in '{apiClientFilePath}'");
    });

//////////////////////////////////////////////////////////////////////
////// Tests
//////////////////////////////////////////////////////////////////////

Task ("Backend-Run-All-Tests")
    .IsDependentOn ("Backend-Build")
    .IsDependentOn ("Backend-Run-Unit-Tests")
    .IsDependentOn ("Backend-Run-Integration-Tests")
    .IsDependentOn ("Backend-Run-API-Tests");

Task ("Backend-Run-Unit-Tests")
    .IsDependentOn ("Backend-Build")
    .Does (() => {
        RunTests(GetFiles("./**/Sppd.TeamTuner.Tests.Unit*.csproj"));
    });

Task ("Backend-Run-Integration-Tests")
    .IsDependentOn ("Backend-Build")
    .Does (() => {
        RunTests(GetFiles("./**/Sppd.TeamTuner.Tests.Integration*.csproj"));
    });

Task ("Backend-Run-API-Tests")
    .IsDependentOn ("Backend-Build")
    .Does (() => {
        RunTests(GetFiles("./**/Sppd.TeamTuner.Tests.Api*.csproj"));
    });

var _coverletSettings = new CoverletSettings {
    CollectCoverage = true,
    CoverletOutputFormat = CoverletOutputFormat.opencover,
    CoverletOutputDirectory = testCoverageResultsDir,
    Exclude = new List<string> { "[xunit*]*", "[Sppd.TeamTuner.Tests.*]*" },
    Include = new List<string> { "[Sppd.TeamTuner*]*" }
};

var _testSettings = new DotNetCoreTestSettings {
    NoBuild = true,
    NoRestore = true,
    Configuration = configuration,
    ResultsDirectory = testResultsDir
};

private void RunTests(FilePathCollection projectFiles){
    foreach(var projectFile in projectFiles)
    {
        var projectFilePath = projectFile.Segments.Last();
        var testResultFileName = $"test-results-{projectFilePath}.xml";
        var coverageResultFileName = $"coverage-results-{projectFilePath}.opencover.xml";
        _testSettings.ArgumentCustomization = args => args.Append ($"--logger:trx;LogFileName={testResultFileName}");
        _coverletSettings.CoverletOutputName = coverageResultFileName;

        DotNetCoreTest (projectFile.FullPath, _testSettings, _coverletSettings);
    }
}

//////////////////////////////////////////////////////////////////////
// Codecov
//////////////////////////////////////////////////////////////////////

Task ("Backend-Upload-Codecov")
    .Does (() => {
        var coverageFiles = GetFiles($"{testCoverageResultsDir}/*");
        foreach(var coverageFile in coverageFiles)
        {
            Codecov (coverageFile.ToString(), "4983ef47-a570-4002-b7bf-3e102d8d9011");
        }
    });

Task ("Backend-Run-Upload-All-Tests")
    .IsDependentOn ("Backend-Run-All-Tests")
    .IsDependentOn ("Backend-Upload-Codecov");

//////////////////////////////////////////////////////////////////////
// Default
//////////////////////////////////////////////////////////////////////

Task ("Default")
    .IsDependentOn ("Backend-Build");

//////////////////////////////////////////////////////////////////////
// Execution
//////////////////////////////////////////////////////////////////////

RunTarget (target);