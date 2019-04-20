using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Exceptions;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Shared;

using Xunit;

namespace Sppd.TeamTuner.Tests.Integration
{
    public class RepositoryTests : RepositoryTestsBase
    {
        /// <summary>
        ///     Tests that it is possible to save/delete/save the same <see cref="TeamTunerUser" /> if he has a different Id.
        /// </summary>
        [Fact]
        public async Task CanCreateDeleteCreateUserWithDifferentIdButSamePropertiesTest()
        {
            // Arrange
            var user = new TeamTunerUser
                       {
                           Email = "a@b.c",
                           Description = "Description",
                           Name = "Name",
                           SppdName = "SppdName",
                           PasswordHash = Encoding.UTF8.GetBytes("A"),
                           PasswordSalt = Encoding.UTF8.GetBytes("A"),
                           ApplicationRole = CoreConstants.Auth.Roles.USER
                       };

            // Act

            // Create user
            using (var scope = ServiceProvider.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetService<IRepository<TeamTunerUser>>();
                var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();

                userRepository.Add(user);
                await unitOfWork.CommitAsync();
            }

            // Delete user
            using (var scope = ServiceProvider.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetService<IRepository<TeamTunerUser>>();
                var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();

                await userRepository.DeleteAsync(user.Id);
                await unitOfWork.CommitAsync();
            }

            // Create user
            user.Id = Guid.NewGuid();
            using (var scope = ServiceProvider.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetService<IRepository<TeamTunerUser>>();
                var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();

                userRepository.Add(user);
                await unitOfWork.CommitAsync();
            }

            // Assert
            // No assertions required as an exception would make the test fail
        }

        /// <summary>
        ///     Tests that the a <see cref="TeamTunerUser" /> having the same unique properties but a different Id cannot be saved.
        /// </summary>
        [Fact]
        public async Task CannotCreateSameUserTwiceTest()
        {
            // Arrange
            var user = new TeamTunerUser
                       {
                           Email = "a2@b.c",
                           Description = "Description2",
                           Name = "Name2",
                           SppdName = "SppdName2",
                           PasswordHash = Encoding.UTF8.GetBytes("A"),
                           PasswordSalt = Encoding.UTF8.GetBytes("A"),
                           ApplicationRole = CoreConstants.Auth.Roles.USER
                       };

            // Act

            // Create user
            using (var scope = ServiceProvider.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetService<IRepository<TeamTunerUser>>();
                var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();

                userRepository.Add(user);
                await unitOfWork.CommitAsync();
            }

            // Create user
            user.Id = Guid.NewGuid();
            Exception exception = null;
            using (var scope = ServiceProvider.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetService<IRepository<TeamTunerUser>>();
                var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();

                userRepository.Add(user);
                try
                {
                    await unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            }

            // Assert
            Assert.NotNull(exception);
        }

        /// <summary>
        ///     Tests that an update fails if the <see cref="BaseEntity.Version" /> has been modified.
        /// </summary>
        [Fact]
        public async Task CannotUpdateWithDifferentVersionTest()
        {
            // Arrange
            var user = new TeamTunerUser
                       {
                           Email = "a3@b.c",
                           Description = "Description3",
                           Name = "Name3",
                           SppdName = "SppdName3",
                           PasswordHash = Encoding.UTF8.GetBytes("A"),
                           PasswordSalt = Encoding.UTF8.GetBytes("A"),
                           ApplicationRole = CoreConstants.Auth.Roles.USER
                       };
            var updatedEmail = "tutu@tata.com";
            // Act

            // Create user
            using (var scope = ServiceProvider.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetService<IRepository<TeamTunerUser>>();
                var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();

                userRepository.Add(user);
                await unitOfWork.CommitAsync();
            }

            // Update properties
            user.Email = updatedEmail;
            user.Version[0]++;

            // Update user
            ConcurrentUpdateException exception = null;
            using (var scope = ServiceProvider.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetService<IRepository<TeamTunerUser>>();
                var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();

                userRepository.Update(user);
                try
                {
                    await unitOfWork.CommitAsync();
                }
                catch (ConcurrentUpdateException ex)
                {
                    exception = ex;
                }
            }

            // Assert
            Assert.NotNull(exception);
        }

        /// <summary>
        ///     Tests that an update fails if the <see cref="BaseEntity.Version" /> has been modified.
        /// </summary>
        [Fact]
        public async Task CanUpdateWithSameVersionTest()
        {
            // Arrange
            var user = new TeamTunerUser
                       {
                           Email = "a3@b.c",
                           Description = "Description3",
                           Name = "Name3",
                           SppdName = "SppdName3",
                           PasswordHash = Encoding.UTF8.GetBytes("A"),
                           PasswordSalt = Encoding.UTF8.GetBytes("A"),
                           ApplicationRole = CoreConstants.Auth.Roles.USER
                       };
            var updatedEmail = "tutu@tata.com";
            // Act

            // Create user
            using (var scope = ServiceProvider.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetService<IRepository<TeamTunerUser>>();
                var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();

                userRepository.Add(user);
                await unitOfWork.CommitAsync();
            }

            var initialVersion = user.Version;

            // Update user
            user.Email = updatedEmail;
            using (var scope = ServiceProvider.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetService<IRepository<TeamTunerUser>>();
                var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();

                userRepository.Update(user);
                await unitOfWork.CommitAsync();
            }

            var updatedVersion = user.Version;

            // Assert
            Assert.Equal(user.Email, updatedEmail);
            Assert.NotNull(initialVersion);
            Assert.NotNull(updatedVersion);
            Assert.NotEqual(initialVersion, updatedVersion);
        }

        /// <summary>
        ///     Tests that specified navigation properties do get loaded.
        /// </summary>
        [Fact]
        public async Task DoesLoadSpecifiedNavigationPropertiesTest()
        {
            // Arrange
            var teamId = Guid.Parse(TestingConstants.Team.HOLY_COW_ID);

            Team teamWithUsers;
            using (var scope = ServiceProvider.CreateScope())
            {
                var teamRepository = scope.ServiceProvider.GetService<IRepository<Team>>();
                teamWithUsers = await teamRepository.GetAsync(teamId, new[] {nameof(Team.Users)});
            }

            // Assert
            Assert.True(teamWithUsers.Users.Any());
        }

        /// <summary>
        ///     Tests that nested navigation properties get loaded when specified.
        /// </summary>
        [Fact]
        public async Task DoesLoadSpecifiedNestedNavigationPropertiesTest()
        {
            // Arrange
            var teamId = Guid.Parse(TestingConstants.Team.HOLY_COW_ID);

            // Act
            Team team;
            using (var scope = ServiceProvider.CreateScope())
            {
                var teamRepository = scope.ServiceProvider.GetService<IRepository<Team>>();
                team = await teamRepository.GetAsync(teamId, new[] {string.Join(".", nameof(Team.Users), nameof(TeamTunerUser.Federation))});
            }

            // Assert
            Assert.True(team.Users.Any());
            Assert.Contains(team.Users.Select(u => u.Federation), federation => federation != null);
        }

        /// <summary>
        ///     Tests that unspecified navigation properties do not get loaded.
        /// </summary>
        [Fact]
        public async Task DoesNotLoadUnspecifiedNavigationPropertiesTest()
        {
            // Arrange
            var teamId = Guid.Parse(TestingConstants.Team.HOLY_COW_ID);

            // Act
            Team teamWithoutUsers;
            using (var scope = ServiceProvider.CreateScope())
            {
                var teamRepository = scope.ServiceProvider.GetService<IRepository<Team>>();
                teamWithoutUsers = await teamRepository.GetAsync(teamId);
            }

            // Assert
            Assert.False(teamWithoutUsers.Users.Any());
        }
    }
}