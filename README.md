# Sppd.TeamTuner
[![Build Status](https://travis-ci.com/taconaut/Sppd.TeamTuner.svg?branch=master)](https://travis-ci.com/taconaut/Sppd.TeamTuner) [![codecov](https://codecov.io/gh/taconaut/Sppd.TeamTuner/branch/master/graph/badge.svg)](https://codecov.io/gh/taconaut/Sppd.TeamTuner)

## Introduction
Sppd stands for [South Park Phone Destroyer](https://southparkphonedestroyer.com/), a mobile gaim available for [Android](https://play.google.com/store/apps/details?id=com.ubisoft.dragonfire&hl=en) and [iOS](https://apps.apple.com/us/app/south-park-phone-destroyer/id1106442030) devices.

This project wants to make team planning easier :
- Everyone can create a profile with on the website.
- Everyone can create a new team or request to join an existing one.
- Everyone can set the level for his cards.
- The card levels of all team members are being aggregated for the team. This makes it easier to select the cards to upgrade for team wars. (TODO)
- [...more to come...]

## Origin
This project has been started mainly out of technological interest to 
- get a hands on experience with .net core 
- figure out how to build a frontend with a modern technology (Vue)
- find ways to fill the gaps (e.g. generate frontend API client classes) in order to have a workflow as easy as possible
- set up [continuous integration](https://en.wikipedia.org/wiki/Continuous_integration) and [continuous delivery](https://en.wikipedia.org/wiki/Continuous_delivery)  (CI/CD) to guarantee good quality.

I'm also using this project as a playground to test new concepts and define the best practices I'd like to apply in other projects. If you have any improvements to propose please do.

***
## Technical
[This GIT repository](https://github.com/taconaut/Sppd.TeamTuner) contains the backend, exposing a RESTful API, which is an [ASP.net](https://dotnet.microsoft.com/apps/aspnet) core 2.2 application and the frontend, consuming the API and using the data, which is a [Vue 2](https://vuejs.org/v2/guide/) application.

**Details:**
- [Backend](./Backend/README.md)
- [Frontend](./Frontend/README.md)

### Prerequisits
- [Node.js](https://nodejs.org/en/): Frontend (using the current version, node 12.13.1 with npm 6.12.1, by the time of writing this)
- [.NET Core 2.2 SDK](https://dotnet.microsoft.com/download/dotnet-core/2.2): Backend
- [SQL Express](https://www.microsoft.com/en-us/sql-server/sql-server-editions-express): Default database (this is not absolutely required as Sqlite can also be used, but strongly recommended)

### Run the application in development mode
#### Backend
Install the dependencies once and run the backend.

Command line (from *Backend* directory):
```
dotnet restore ./Sppd.TeamTuner/Sppd.TeamTuner.csproj
dotnet run --project ./Sppd.TeamTuner/Sppd.TeamTuner.csproj --launch-profile Development
```

#### Frontend
Install the dependencies once and serve the frontend.

Command line (from *Frontend* directory):
```
npm install
npm run serve
```

### Build automation
[Cake](https://cakebuild.net/) (c# make) is being used as build automation tool. Targets have been defined to build, test, package the backend along with the frontend; it can also upload test coverage results to [Codecov](https://codecov.io/).

Example: Execute `.\build.ps1 -Target Zip-Package` in a powershell console to package the entire application.

**Details:**
- [Build automation](./BuildAutomation.md)
- [Deployment](./Deployment.md)

### Tools
It is not mandatory to use below tools, but they have proven useful.

**Backend:**
- [Visual Studio Community](https://visualstudio.microsoft.com/vs/community/): Development
- [ReSharper](https://www.jetbrains.com/resharper/): Code formatting (a [shared configuration](./backend/Sppd.TeamTuner.sln.DotSettings) is checked in)
- [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms): Database access
- [Log4View](https://www.log4view.com/): Log viewer

**Frontend:**
- [Visual Studio Code](https://code.visualstudio.com/): Development (recommended extensions: Babel JavaScript, ESLint, PowerSell, Vetur)

**General**
- [Notepad++](https://notepad-plus-plus.org/): For anything related to text, json, xml, etc. files
- [Visual Studio Code](https://code.visualstudio.com/): Cake scripts and markdown documentation (recommended extensions: Cake, Markdown All in One)

### Dependencies
Please check the [GitHub Dependency Graph](https://github.com/taconaut/Sppd.TeamTuner/network/dependencies) for the complete list.

## Contributing
If you would like to participate in the project, please submit a pull request or get in touch with me if you have any questions.