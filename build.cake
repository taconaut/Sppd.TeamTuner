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

//////////////////////////////////////////////////////////////////////
// Tasks
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{    
    CleanDirectory(artifacts);
    CleanDirectories("./**/obj/*.*");
    CleanDirectories($"./**/bin/{configuration}/*.*");
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
        }
    );
});

Task("Run-Integration-Tests")
    .IsDependentOn("Build")
    .DoesForEach(
        GetFiles("./**/*.Tests.Integration.csproj"),
        testProject => 
{
    DotNetCoreTest(
        testProject.FullPath,
        new DotNetCoreTestSettings{
            NoBuild = true,
            NoRestore = true,
            Configuration = configuration
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
        }
    );
});

Task("Run-All-Tests")
    .IsDependentOn("Build")
    .IsDependentOn("Run-Unit-Tests")
    .IsDependentOn("Run-Integration-Tests")
    .IsDependentOn("Run-API-Tests");

//////////////////////////////////////////////////////////////////////
// Execution
//////////////////////////////////////////////////////////////////////

RunTarget(target);
