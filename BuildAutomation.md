# Build automation

[Cake](https://cakebuild.net/) (c# make) is being used as build automation tool. The available targets can be ran by using the PowerShell (`build.ps1`) on Windows and the shell script (`build.sh`) for UNIX systems.

Example: `.\build.ps1 -Target Zip-Package`

The targets have dependencies between each other. Run a target to see the executed/dependent targets.<br>
Note that the targets are explicitly listed in `.\build.ps1`, which allows to use autocomplete with `.\build.ps1 -Target ` and hitting the TAB key.
# Targets
The below sections group the available targets by functionality.

The scopes have the following meaning:
- *Common*: Global to the project; not directly related to either the frontend or backend.
- *Backend*: Only related to the backend located in the `./Backend` directory.
- *Frontend*: Only related to the frontend located in the `./Frontend` directory.
- *Global*: Related to common, backend and frontend.
  
## Setup
| Scope    | Target name                    | Description                                    |
| -------- | ------------------------------ | ---------------------------------------------- |
| Backend  | Backend-Restore-NuGet-Packages | Restores the nuget packages                    |
| Frontend | Frontend-Npm-Install           | Installs NPM packages (executes `npm install`) |

## Clean
| Scope    | Target name    | Description                                                                |
| -------- | -------------- | -------------------------------------------------------------------------- |
| Common   | Common-Clean   | Deletes all files and folders to delete, belonging to the entire project   |
| Backend  | Backend-Clean  | Deletes all files and folders to delete, belonging to the backend          |
| Frontend | Frontend-Clean | Deletes all files and folders to delete, belonging to the frontend         |
| Global   | Clean          | Deletes all files and folders to delete, from common, backend and frontend |

## Build
| Scope    | Target name    | Description                     |
| -------- | -------------- | ------------------------------- |
| Backend  | Backend-Build  | Builds the backend              |
| Frontend | Frontend-Build | Builds the frontend             |
| Global   | Build          | Builds the backend and frontend |

## Package
| Scope    | Target name          | Description                                                                                 |
| -------- | -------------------- | ------------------------------------------------------------------------------------------- |
| Backend  | Backend-Test-Package | Builds the backend, runs all tests and copies the built output into the artifacts directory |
| Frontend | Frontend-Package     | Builds the frontend and copies the built output into the artifacts directory                |
| Global   | Zip-Package          | Produces a ZIP file containing a ready to deploy backend and frontend                       |

## Test
| Scope   | Target name                   | Description                                      |
| ------- | ----------------------------- | ------------------------------------------------ |
| Backend | Backend-Run-Unit-Tests        | Runs the backend unit tests                      |
| Backend | Backend-Run-Integration-Tests | Runs the backend integration tests               |
| Backend | Backend-Run-API-Tests         | Runs the backend API tests                       |
| Backend | Backend-Run-All-Tests         | Runs the backend unit, integration and API tests |

## API client generation
| Scope    | Target name                  | Description                                                                                                                                                                                         |
| -------- | ---------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Frontend | Frontend-Generate-API-Client | Generates [api.ts](./Frontend/src/api.ts) from the swagger definition. Note that the backend must be running to have the swagger.json available at  https://localhost:44336/swagger/v1/swagger.json |

## Versioning
| Scope   | Target name             | Description                                                                                                                                                                         |
| ------- | ----------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Backend | Backend-Release-Prepare | Sets the file `AssemblyVersion`, `FileVersion` and `Version` in the `*.csproj` files. It can be used like this: `.\build.ps1 -Target=Backend-Release-Prepare --applicationVersion=2.2.2.2` |

## Codecov
| Scope   | Target name                  | Description                                                                                                 |
| ------- | ---------------------------- | ----------------------------------------------------------------------------------------------------------- |
| Backend | Backend-Upload-Codecov       | Uploads the test coverage results to [Codecov ](https://codecov.io/gh/taconaut/Sppd.TeamTuner)              |
| Backend | Backend-Run-Upload-All-Tests | Runs all backend tests and uploads the results to [Codecov ](https://codecov.io/gh/taconaut/Sppd.TeamTuner) |

# How to add a new target
- Specify the target in [build.cake](./build.cake)
- Add the target to `ValidateSet` in [build.ps1](./build.ps1)