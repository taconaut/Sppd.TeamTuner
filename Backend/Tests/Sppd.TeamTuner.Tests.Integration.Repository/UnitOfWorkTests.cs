using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;

using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Infrastructure.DataAccess.EF;

using Xunit;

namespace Sppd.TeamTuner.Tests.Integration.DataAccess
{
    public class UnitOfWorkTests
    {
        /// <summary>
        ///     Performs create/update/delete operations and makes sure nothing would be committed after calling
        ///     <see cref="IUnitOfWork.CommitAsync" /> if <see cref="IUnitOfWork.Rollback" /> is being called.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        [SkippableTheory]
        [InlineData("InMemory")]
        public async Task RollbackTest(string provider)
        {
            Skip.If(provider == "MsSql" && !SkipHelper.IsWindowsOS(), "Ignore when not executing on Windows");

            var testManager = new TestEnvironmentServiceManager();

            async Task Test()
            {
                // Arrange
                var addUser = new TeamTunerUser
                              {
                                  Email = "a3@b.c",
                                  Description = "Description3",
                                  Name = "Name3",
                                  SppdName = "SppdName3",
                                  PasswordHash = Encoding.UTF8.GetBytes("A"),
                                  PasswordSalt = Encoding.UTF8.GetBytes("A"),
                                  ApplicationRole = CoreConstants.Authorization.Roles.USER
                              };

                // Act
                List<EntityEntry> changedEntitiesBeforeRollback = null;
                List<EntityEntry> changedEntitiesAfterRollback = null;
                using (var scope = testManager.ServiceProvider.CreateScope())
                {
                    var userRepository = scope.ServiceProvider.GetService<IRepository<TeamTunerUser>>();
                    var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
                    var changeTracker = scope.ServiceProvider.GetService<TeamTunerContext>().ChangeTracker;

                    var isEntityEntryChanged = new Func<EntityEntry, bool>(entry =>
                        entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted);

                    // Add
                    userRepository.Add(addUser);

                    // Update
                    var updateUser = (await userRepository.GetAllAsync()).First();
                    updateUser.Email += ".tutu";

                    // Delete
                    var deleteUser = (await userRepository.GetAllAsync()).Last();
                    await userRepository.DeleteAsync(deleteUser.Id);

                    changeTracker.DetectChanges();

                    changedEntitiesBeforeRollback = changeTracker.Entries().Where(isEntityEntryChanged).ToList();

                    unitOfWork.Rollback();

                    changedEntitiesAfterRollback = changeTracker.Entries().Where(isEntityEntryChanged).ToList();
                }

                // Assert
                Assert.Equal(3, changedEntitiesBeforeRollback.Count);
                Assert.Empty(changedEntitiesAfterRollback);
            }

            await testManager.ExecuteTestForProvider(provider, Test);
        }
    }
}