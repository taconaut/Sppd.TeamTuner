# Deployment

The deployment process isn't automated for the project, as the build and deployment environment  can't be known beforehand. This document will show a PowerShell script, which is being used for nightly deploys with [Jenkins](https://jenkins.io/) to deploy the nightly build ([backend API](http://sppdteamtuner.hopto.org:1702/swagger/index.html), [frontend](http://sppdteamtuner.hopto.org:1703/)).

## Environment 
- Two IIS app pools and sites; one each for backend and frontend.
- Jenkins is installed on the same server IIS hosts the application.

## Deployment procedure
- Build, test and package the backend and frontend with the cake target `Zip-Package`.
- Stop both app pools.
- Delete all currently deployed files in the site directories.
- Copy the built artifacts (backend and frontend) from `.\artifacts` into the site folder.
- Configure the backend by copying the productive `appsettings.json` and `log4net.config`, available in a dedicated directory of the server.
- Configure the frontend by setting the `apiUrl` in `index.html`. Note that `index.html` can't be considered to be a HTML file after compilation as all quotes have been stripped.
- Restart both app pools.

## PowerShell scripts
There is the build script, which runs `Release-Prepare` to write the version info (including the GIT commit hash it was built from) in the `*.csproj`s, before building and testing the application:
```
# Build, test and package
.\build.ps1 -target Release-Prepare
.\build.ps1 -target Zip-Package
```

And the post build script, which is only being triggered if the build and tests were successful:
```
# Stop app pools
C:\Windows\System32\inetsrv\appcmd stop apppool /apppool.name:Sppd.TeamTuner-Backend-Nightly
C:\Windows\System32\inetsrv\appcmd stop apppool /apppool.name:Sppd.TeamTuner-Frontend-Nightly

# Define deploy and source directories
[string]$backendDestinationDirectory = "C:\IIS_HOSTING\Sppd.TeamTuner-Backend-Nightly"
[string]$frontendDestinationDirectory = "C:\IIS_HOSTING\Sppd.TeamTuner-Frontend-Nightly"

[string]$backendSourceDirectory  = "artifacts\Backend\*"
[string]$frontendSourceDirectory  = "artifacts\Frontend\*"

# Delete all deployed backend and frontend files
Remove-Item "$backendDestinationDirectory\*" -Recurse -Force
Remove-Item "$frontendDestinationDirectory\*" -Recurse -Force

# Copy back- and frontend
Copy-item -Force -Recurse $backendSourceDirectory -Destination $backendDestinationDirectory
Copy-item -Force -Recurse $frontendSourceDirectory  -Destination $frontendDestinationDirectory 

# Copy backend configurations
[string]$sourceDirectory  = "E:\JenkinsBuilds\DeployConfigurations\Sppd.TeamTuner-Backend-Nightly\Config\*"
[string]$destinationDirectory = "$backendDestinationDirectory\Config"
Copy-item -Force $sourceDirectory -Destination $destinationDirectory

# Set API URL in frontend index.html
[string]$frontendIndexHtmlFilePath = $frontendDestinationDirectory + "\index.html"
((Get-Content -path $frontendIndexHtmlFilePath -Raw) -replace 'https://localhost:44336/','http://sppdteamtuner.hopto.org:1702/') | Set-Content -Path $frontendIndexHtmlFilePath

# Start app pools
C:\Windows\System32\inetsrv\appcmd start apppool /apppool.name:Sppd.TeamTuner-Backend-Nightly
C:\Windows\System32\inetsrv\appcmd start apppool /apppool.name:Sppd.TeamTuner-Frontend-Nightly
```