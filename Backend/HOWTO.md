# Backend HOWTOs

## How to add a new entitiy

- Create the new entity in [Core](./Sppd.TeamTuner.Core/Domain/Entities)
- Make it extend [BaseEntity](./Sppd.TeamTuner.Core/Domain/Entities/BaseEntity.cs)
- Configure it in the [DbContext.OnModelCreating()](./Sppd.TeamTuner.Infrastructure.DataAccess.EF/TeamTunerContext.cs) and create a new method in [TeamTunerContext.EntityConfigurations](./Sppd.TeamTuner.Infrastructure.DataAccess.EF/TeamTunerContext.EntityConfigurations.cs) where the constraints will be configured.
- [Generate migrations](./Sppd.TeamTuner.Infrastructure.DataAccess.EF/HOWTO.md)
- [How to create Sqlite trigger to update the entity version on create/update/delete](./Sppd.TeamTuner.Infrastructure.DataAccess.EF/HOWTO.md)
- Create a repository interface extending [IRepository](./Sppd.TeamTuner.Core/Repositories/IRepository.cs) in [Repositories](./Sppd.TeamTuner.Core/Repositories). Create it even if it remains empty, we want a dedicated repository for every entity.
- Implent the repository interface in [Repositories](./Sppd.TeamTuner.Infrastructure.DataAccess.EF/Repositories)

Now everything is ready to use the entity in the business and api layers