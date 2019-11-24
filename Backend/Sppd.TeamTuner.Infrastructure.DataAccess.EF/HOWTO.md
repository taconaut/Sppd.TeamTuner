# EF DataAccess HOWTOs

## How to generate migrations
Migrations have to be generated separately for MsSql and Sqlite. Do this once for each:
- Set the provider for which to generate the migration in the `Database` section of [appsetting.json](../Sppd.TeamTuner/Config/appsettings.json).
- In Visual Studio or a PowerShell console, execute `Add-Migration <MigrationName>` to generate the migration.
- Make sure the generated migration contains all expected constraints.
- If a new entitiy has been added, follow the below `How to create Sqlite trigger to update the entity version on create/update/delete`

## How to create Sqlite trigger to update the entity version on create/update/delete
Sqlite doesn't support the [concurrency token](https://docs.microsoft.com/en-us/ef/core/modeling/concurrency) out of the box; a trigger has to be created for it to work properly.

- Open the generated migration and add following code to the end of the `Up()` method (replace all `<EntityName>`):
```
        migrationBuilder.Sql(
          @"CREATE TRIGGER Set<EntityName>VersionOnUpdate
          AFTER UPDATE ON <EntityName>
          BEGIN
              UPDATE <EntityName>
              SET Version = randomblob(8)
              WHERE rowid = NEW.rowid;
          END");
        migrationBuilder.Sql(
          @"CREATE TRIGGER Set<EntityName>VersionOnInsert
          AFTER INSERT ON <EntityName>
          BEGIN
              UPDATE <EntityName>
              SET Version = randomblob(8)
              WHERE rowid = NEW.rowid;
          END");
```

- Open the generated migration and add following code to the end of the `Down()` method (replace all `<EntityName>`):
```
        migrationBuilder.Sql("DROP TRIGGER Set<EntityName>VersionOnUpdate");
```