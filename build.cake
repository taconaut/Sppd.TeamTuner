//////////////////////////////////////////////////////////////////////
// Tools & Add-ins
//////////////////////////////////////////////////////////////////////

#tool nuget:?package=Codecov&version=1.5.0

#addin nuget:?package=Cake.Coverlet&version=2.2.1
#addin nuget:?package=Cake.Codecov&version=0.6.0

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
var backendDir = Directory ("./Backend");
var buildDir = backendDir + Directory ("Sppd.TeamTuner/bin") + Directory (configuration);

// To be created
var artifactsDir = MakeAbsolute (Directory ("./artifacts"));
var testOutputDir = MakeAbsolute (Directory ("./test-output"));
var testCoverageResultsDir = MakeAbsolute (testOutputDir.Combine (Directory ("coverage-results")));
var testResultsDir = MakeAbsolute (testOutputDir.Combine (Directory ("test-results")));

// File paths
var solutionPath = backendDir.Path + "/Sppd.TeamTuner.sln";
var teamTunerProjectPath = backendDir.Path + "/Sppd.TeamTuner/Sppd.TeamTuner.csproj";

// File names
var unitTestResultsFileName = "coverage-results-unit.opencover.xml";
var integrationTestResultsFileName = "coverage-results-integration.opencover.xml";
var apiTestResultsFileName = "coverage-results-api.opencover.xml";

//////////////////////////////////////////////////////////////////////
// Tasks
//////////////////////////////////////////////////////////////////////

Task ("Clean")
    .Does (() => {
        CleanDirectory (artifactsDir);
        CleanDirectory (testOutputDir);
        CleanDirectory (testCoverageResultsDir);
        CleanDirectory (testResultsDir);
        CleanDirectories ($"./**/obj/{configuration}");
        CleanDirectories ($"./**/bin/{configuration}");
    });

Task ("Restore-NuGet-Packages")
    .IsDependentOn ("Clean")
    .Does (() => {
        NuGetRestore (solutionPath);
    });

Task ("Build")
    .IsDependentOn ("Restore-NuGet-Packages")
    .Does (() => {
        DotNetCoreBuild (
            solutionPath,
            new DotNetCoreBuildSettings {
                NoRestore = true,
                    Configuration = configuration
            }
        );
    });

//////////////////////////////////////////////////////////////////////
////// Packaging
//////////////////////////////////////////////////////////////////////

Task ("Package-Backend")
    .IsDependentOn ("Build")
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

Task ("Zip-Package")
    .IsDependentOn ("Package-Backend")
    .Does (() => {
        Zip (artifactsDir, $"{artifactsDir}/Sppd.TeameTuner.zip");
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

Task ("Run-Unit-Tests")
    .IsDependentOn ("Build")
    .DoesForEach (
        GetFiles ("./**/*.Tests.Unit.csproj"),
        testProject => {
            testSettings.ArgumentCustomization = args => args.Append ("--logger:trx;LogFileName=test-results-unit.xml");
            coverletSettings.CoverletOutputName = $"coverage-results-unit.opencover.xml";

            DotNetCoreTest (testProject.FullPath, testSettings, coverletSettings);
        });

Task ("Run-Integration-Tests")
    .IsDependentOn ("Build")
    .DoesForEach (
        GetFiles ("./**/*.Tests.Integration.*csproj"),
        testProject => {
            testSettings.ArgumentCustomization = args => args.Append ("--logger:trx;LogFileName=test-results-integration.xml");
            coverletSettings.CoverletOutputName = $"coverage-results-integration.opencover.xml";

            DotNetCoreTest (testProject.FullPath, testSettings, coverletSettings);
        });

Task ("Run-API-Tests")
    .IsDependentOn ("Build")
    .DoesForEach (
        GetFiles ("./**/*.Tests.Integration.Api.csproj"),
        testProject => {
            testSettings.ArgumentCustomization = args => args.Append ("--logger:trx;LogFileName=test-results-api.xml");
            coverletSettings.CoverletOutputName = $"coverage-results-api.opencover.xml";

            DotNetCoreTest (testProject.FullPath, testSettings, coverletSettings);
        });

Task ("Run-All-Tests")
    .IsDependentOn ("Build")
    .IsDependentOn ("Run-Unit-Tests")
    .IsDependentOn ("Run-Integration-Tests")
    .IsDependentOn ("Run-API-Tests");

//////////////////////////////////////////////////////////////////////
// Codecov
//////////////////////////////////////////////////////////////////////

Task ("Upload-Coverage")
    .Does (() => {
        Codecov ($"{testCoverageResultsDir}/{unitTestResultsFileName}", "4983ef47-a570-4002-b7bf-3e102d8d9011");
        Codecov ($"{testCoverageResultsDir}/{integrationTestResultsFileName}", "4983ef47-a570-4002-b7bf-3e102d8d9011");
        Codecov ($"{testCoverageResultsDir}/{apiTestResultsFileName}", "4983ef47-a570-4002-b7bf-3e102d8d9011");
    });

Task ("Run-Upload-All-Tests")
    .IsDependentOn ("Run-All-Tests")
    .IsDependentOn ("Upload-Coverage");

//////////////////////////////////////////////////////////////////////
// Default
//////////////////////////////////////////////////////////////////////

Task ("Default")
    .IsDependentOn ("Build");

//////////////////////////////////////////////////////////////////////
// Execution
//////////////////////////////////////////////////////////////////////

RunTarget (target);