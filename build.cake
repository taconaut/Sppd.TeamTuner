//////////////////////////////////////////////////////////////////////
// Tools & Add-ins
//////////////////////////////////////////////////////////////////////

#tool nuget:?package=Codecov&version=1.4.0

#addin nuget:?package=Cake.Coverlet&version=2.2.1
#addin nuget:?package=Cake.Codecov&version=0.6.0

//////////////////////////////////////////////////////////////////////
// Arguments
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// Preparation
//////////////////////////////////////////////////////////////////////

var buildDir = Directory("./Backend/Sppd.TeamTuner/bin") + Directory(configuration);
var artifacts = MakeAbsolute(Directory("./artifacts"));
var solution = "./Backend/Sppd.TeamTuner.sln";
var teamTunerProject = "./Backend/Sppd.TeamTuner/Sppd.TeamTuner.csproj";

// Tests
var testCoverageResults = MakeAbsolute(Directory("./coverage-results"));
var unitTestResultsFileName = "coverage-results-unit.opencover.xml";
var integrationTestResultsFileName = "coverage-results-integration.opencover.xml";
var apiTestResultsFileName = "coverage-results-api.opencover.xml";

//////////////////////////////////////////////////////////////////////
// Tasks
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{    
    CleanDirectory(artifacts);
    CleanDirectory(testCoverageResults);
    CleanDirectories("./**/obj/{configuration}");
    CleanDirectories($"./**/bin/{configuration}");
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solution);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    DotNetCoreBuild(
        solution,
        new DotNetCoreBuildSettings{
            NoRestore = true,
            Configuration = configuration
        }
    );
});

//////////////////////////////////////////////////////////////////////
////// Packaging
//////////////////////////////////////////////////////////////////////

Task("Package-Backend")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetCorePublish(
        teamTunerProject,
        new DotNetCorePublishSettings{
            NoBuild = true,
            NoRestore = true,
            Configuration = configuration,
            OutputDirectory = $"{artifacts}/Backend"
        }
    );
});

Task("Zip-Package")
    .IsDependentOn("Package-Backend")
    .Does(() =>
{
    Zip(artifacts, $"{artifacts}/Sppd.TeameTuner.zip");
});

//////////////////////////////////////////////////////////////////////
////// Tests
//////////////////////////////////////////////////////////////////////

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .DoesForEach(
        GetFiles("./**/*.Tests.Unit.csproj"),
        testProject => 
{
    DotNetCoreTest(
        testProject.FullPath,
        new DotNetCoreTestSettings{
            NoBuild = true,
            NoRestore = true,
            Configuration = configuration
        },
        new CoverletSettings {
            CollectCoverage = true,
            CoverletOutputFormat = CoverletOutputFormat.opencover,
            CoverletOutputDirectory = testCoverageResults,
            CoverletOutputName = $"coverage-results-unit.opencover.xml",
            Exclude = new List<string>{"[xunit*]*", "[Sppd.TeamTuner.Tests.*]*"},
            Include = new List<string>{"[Sppd.TeamTuner*]*"}
        }
    );
});

Task("Run-Integration-Tests")
    .IsDependentOn("Build")
    .DoesForEach(
        GetFiles("./**/*.Tests.Integration.*csproj"),
        testProject => 
{
    DotNetCoreTest(
        testProject.FullPath,
        new DotNetCoreTestSettings{
            NoBuild = true,
            NoRestore = true,
            Configuration = configuration
        },
        new CoverletSettings {
            CollectCoverage = true,
            CoverletOutputFormat = CoverletOutputFormat.opencover,
            CoverletOutputDirectory = testCoverageResults,
            CoverletOutputName = $"coverage-results-integration.opencover.xml",
            Exclude = new List<string>{"[xunit*]*", "[Sppd.TeamTuner.Tests.*]*"},
            Include = new List<string>{"[Sppd.TeamTuner*]*"}
        }
    );
});

Task("Run-API-Tests")
    .IsDependentOn("Build")
    .DoesForEach(
        GetFiles("./**/*.Tests.Integration.Api.csproj"),
        testProject => 
{
    DotNetCoreTest(
        testProject.FullPath,
        new DotNetCoreTestSettings{
            NoBuild = true,
            NoRestore = true,
            Configuration = configuration
        },
        new CoverletSettings {
            CollectCoverage = true,
            CoverletOutputFormat = CoverletOutputFormat.opencover,
            CoverletOutputDirectory = testCoverageResults,
            CoverletOutputName = "coverage-results-api.opencover.xml",
            Exclude = new List<string>{"[xunit*]*", "[Sppd.TeamTuner.Tests.*]*"},
            Include = new List<string>{"[Sppd.TeamTuner*]*"}
        }
    );
});

Task("Run-All-Tests")
    .IsDependentOn("Build")
    .IsDependentOn("Run-Unit-Tests")
    .IsDependentOn("Run-Integration-Tests")
    .IsDependentOn("Run-API-Tests");

//////////////////////////////////////////////////////////////////////
// Codecov
//////////////////////////////////////////////////////////////////////

Task("Upload-Coverage")
    .Does(() =>
{
    Codecov($"{testCoverageResults}/{unitTestResultsFileName}", "4983ef47-a570-4002-b7bf-3e102d8d9011");
    Codecov($"{testCoverageResults}/{integrationTestResultsFileName}", "4983ef47-a570-4002-b7bf-3e102d8d9011");
    Codecov($"{testCoverageResults}/{apiTestResultsFileName}", "4983ef47-a570-4002-b7bf-3e102d8d9011");
});

Task("Run-Upload-All-Tests")
    .IsDependentOn("Run-All-Tests")
    .IsDependentOn("Upload-Coverage");

//////////////////////////////////////////////////////////////////////
// Execution
//////////////////////////////////////////////////////////////////////

RunTarget(target);
