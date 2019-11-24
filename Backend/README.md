# Backend

The backend is an ASP.<span/>net core 2.2 application exposing a RESTful API and persisting data in a database.

There are a few concepts that have been put into place, which you should be aware of, before starting to code in this project.

# Concept
The backend is structured according to [onion architecture](https://www.codeguru.com/csharp/csharp/cs_misc/designtechniques/understanding-onion-architecture.html). There is the main ASP.<span/>net core application `Sppd.TeamTuner`, which uses the functionality defined in `Sppd.TeamTuner.Core` and implemented in `Sppd.TeamTuner.Infrastructure.*`.

To register the implementations, all classes implementing [IStartupRegistrator](./Sppd.TeamTuner.Core/IStartupRegistrator.cs) will be instantiated (they need a constructor without parameters), `ConfigureServices` and `Configure` will then be called from the respective method in [Startup](./Sppd.TeamTuner/Startup.cs).

# Project structure
## Sppd.TeamTuner
The ASP.<span/>net core application, which is the entry point, bootstrapping the application and exposing the RESTful API.

## Sppd.TeamTuner.Core
Every other assembly references this one. It defines the entities belonging to the application, along with interfaces for functionality like repositories or services. But it does not contain any implementations

## Sppd.TeamTuner.Infrastructure.*
All infrastructure projects contain implementations, which are being registered by the project itself.

### Sppd.TeamTuner.Infrastructure.DataAccess.EF
Entity Framework implementation of the data access. SQL and Sqlite are supported. It can be configured in [appsettings.json](./Sppd.TeamTuner/Config/appsettings.json). Additionally, there is a InMemory implementation, which can be used for tests.

### Sppd.TeamTuner.Infrastructure.Feinwaru
Imports cards from the [Feinwaru API](https://sppd.feinwaru.com/). Thanks a lot for letting me use the API!

### Sppd.TeamTuner.Infrastructure.Jobs
Registers [Hangfire](https://www.hangfire.io/) and defines the jobs; currently only the job importing cards from Feinwaru.

## Sppd.TeamTuner.Tests.*
Unit, Integration or API tests

# Do's and Don'ts
- In the API layer (`Sppd.TeamTuner`), the controllers will always use services, but never repositories (even if it is technically possible).
- In the Business layer (`Sppd.TeamTuner.Infrastructure.*`), the services will not use other services, which are using repositories, but use repositories instead. This is to avoid circular dependencies.
- In the data access layer (`Sppd.TeamTuner.Infrastructure.DataAccess.*`), the repositories will never expose `IQueryable`, but always materialized lists or single objects.
- A repository will never persist data, this must be handled in the business layer through a unit of work.

# Execution example
Let's take updating a user as an example.
- User changes some properties in the frontend and saves.
- Frontend calls the API with the updated data.
- The backend controller receives a DTO (Data Transfer Object) as method parameter.<br>
`public async Task<ActionResult<UserResponseDto>> UpdateUser([FromBody] UserUpdateRequestDto userRequestDto)`
- The controller checks if the user is authorized to update the entity.<br>
`await AuthorizeAsync(AuthorizationConstants.Policies.CAN_UPDATE_USER, userRequestDto.Id);`
- The DTO is being converted to an entity (using automapper).<br>
`_mapper.Map<TeamTunerUser>(userRequestDto);`
- The controller calls a update method on a service.<br>
`var updatedUser = await _userService.UpdateAsync(user, userRequestDto.PropertiesToUpdate);`
- The service calls an update method on the repository.<br>
`Repository.Update(storedEntity);`
- The service calls SaveChanges on the unit of work.<br>
`await UnitOfWork.CommitAsync();`
- The controller converts the updated user entity to a UserDto (using automapper) and returns it to the caller.<br>
`return Ok(_mapper.Map<UserResponseDto>(updatedUser));`