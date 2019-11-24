//////////////////////////////////////////////////////////////////////
// Tools & Add-ins
//////////////////////////////////////////////////////////////////////

#tool nuget:?package=Codecov&version=1.8.0

#addin nuget:?package=Cake.Coverlet&version=2.3.4
#addin nuget:?package=Cake.Codecov&version=0.7.0
#addin nuget:?package=Cake.Npm&version=0.17.0
#addin nuget:?package=Cake.CodeGen.NSwag&version=1.2.0&loaddependencies=true
#addin nuget:?package=Cake.Git&version=0.21.0

//////////////////////////////////////////////////////////////////////
// Arguments
//////////////////////////////////////////////////////////////////////

var target = Argument ("target", "Default");
var configuration = Argument ("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// Preparation
//////////////////////////////////////////////////////////////////////

// Directories

// Existing
var projectDir = Directory("./");
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
    });

Task ("Backend-Clean")
    .Does (() => {
        CleanDirectory (testOutputDir);
        CleanDirectory (testCoverageResultsDir);
        CleanDirectory (testResultsDir);
        CleanDirectories ($"./**/obj/{configuration}");
        CleanDirectories ($"./**/bin/{configuration}");
    });

Task ("Frontend-Clean")
    .Does (() => {
        CleanDirectory (frontendDistDir);
    });

Task ("Clean")
    .IsDependentOn ("Common-Clean")
    .IsDependentOn ("Backend-Clean")
    .IsDependentOn ("Frontend-Clean");



Task ("Backend-Restore-NuGet-Packages")
    .IsDependentOn ("Backend-Clean")
    .Does (() => {
        NuGetRestore (solutionPath);
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
    });

Task ("Frontend-Npm-Install")
    .Does (() => {
        var settings = new NpmInstallSettings {
            WorkingDirectory = frontendDir,
            Production = false
        };

        NpmInstall(settings);
    });

Task ("Frontend-Build")
    .IsDependentOn ("Frontend-Clean")
    .IsDependentOn("Frontend-Npm-Install")
    .Does (() => {
        var settings = new NpmRunScriptSettings {
            ScriptName = "build",
            WorkingDirectory = frontendDir 
        };

        NpmRunScript(settings);
    });

Task ("Build")
    .IsDependentOn ("Backend-Build")
    .IsDependentOn ("Frontend-Build");

//////////////////////////////////////////////////////////////////////
////// Packaging
//////////////////////////////////////////////////////////////////////

Task ("Backend-Test-Package")
    .IsDependentOn("Common-Clean")
    .IsDependentOn("Backend-Run-All-Tests")
    .Does (() => {
        DotNetCorePublish (
            teamTunerProjectPath,
            new DotNetCorePublishSettings {
                NoBuild = true,
                NoRestore = true,
                Configuration = configuration,
                OutputDirectory = $"{artifactsDir}/Backend"
            }
        );
    });

Task ("Frontend-Package")
    .IsDependentOn("Common-Clean")
    .IsDependentOn ("Frontend-Build")
    .Does (() => {
        CopyDirectory(frontendDistDir, $"{artifactsDir}/Frontend");
    });

Task ("Zip-Package")
    .IsDependentOn ("Backend-Test-Package")
    .IsDependentOn ("Frontend-Package")
    .Does (() => {
        var gitCommitHash = GitLogTip(projectDir).Sha;
        // TODO: handle tag version once release rpocess has been defined
        var version = gitCommitHash?.Substring(0, 8) ?? "Unknown";
        Zip (artifactsDir, $"{artifactsDir}/Sppd.TeameTuner-{version}.zip");
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

        NSwag.FromJsonSpecification(new Uri(localApiUri))
        .GenerateTypeScriptClient(apiClientFilePath, settings);
    });

//////////////////////////////////////////////////////////////////////
////// Tests
//////////////////////////////////////////////////////////////////////

var coverletSettings = new CoverletSettings {
    CollectCoverage = true,
    CoverletOutputFormat = CoverletOutputFormat.opencover,
    CoverletOutputDirectory = testCoverageResultsDir,
    Exclude = new List<string> { "[xunit*]*", "[Sppd.TeamTuner.Tests.*]*" },
    Include = new List<string> { "[Sppd.TeamTuner*]*" }
};

var testSettings = new DotNetCoreTestSettings {
    NoBuild = true,
    NoRestore = true,
    Configuration = configuration,
    ResultsDirectory = testResultsDir
};

Task ("Backend-Run-Unit-Tests")
    .IsDependentOn ("Backend-Build")
    .DoesForEach (
        GetFiles ("./**/*.Tests.Unit.csproj"),
        testProject => {
            testSettings.ArgumentCustomization = args => args.Append ("--logger:trx;LogFileName=test-results-unit.xml");
            coverletSettings.CoverletOutputName = "coverage-results-unit.opencover.xml";

            DotNetCoreTest (testProject.FullPath, testSettings, coverletSettings);
        });

Task ("Backend-Run-Integration-Tests")
    .IsDependentOn ("Backend-Build")
    .DoesForEach (
        GetFiles ("./**/*.Tests.Integration*.csproj"),
        testProject => {
            testSettings.ArgumentCustomization = args => args.Append ("--logger:trx;LogFileName=test-results-integration.xml");
            coverletSettings.CoverletOutputName = "coverage-results-integration.opencover.xml";

            DotNetCoreTest (testProject.FullPath, testSettings, coverletSettings);
        });

Task ("Backend-Run-API-Tests")
    .IsDependentOn ("Backend-Build")
    .DoesForEach (
        GetFiles ("./**/*.Tests.Api*.csproj"),
        testProject => {
            testSettings.ArgumentCustomization = args => args.Append ("--logger:trx;LogFileName=test-results-api.xml");
            coverletSettings.CoverletOutputName = "coverage-results-api.opencover.xml";

            DotNetCoreTest (testProject.FullPath, testSettings, coverletSettings);
        });

Task ("Backend-Run-All-Tests")
    .IsDependentOn ("Backend-Build")
    .IsDependentOn ("Backend-Run-Unit-Tests")
    .IsDependentOn ("Backend-Run-Integration-Tests")
    .IsDependentOn ("Backend-Run-API-Tests");

//////////////////////////////////////////////////////////////////////
// Codecov
//////////////////////////////////////////////////////////////////////

Task ("Backend-Upload-Codecov")
    .Does (() => {
        Codecov ($"{testCoverageResultsDir}/{unitTestResultsFileName}", "4983ef47-a570-4002-b7bf-3e102d8d9011");
        Codecov ($"{testCoverageResultsDir}/{integrationTestResultsFileName}", "4983ef47-a570-4002-b7bf-3e102d8d9011");
        Codecov ($"{testCoverageResultsDir}/{apiTestResultsFileName}", "4983ef47-a570-4002-b7bf-3e102d8d9011");
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