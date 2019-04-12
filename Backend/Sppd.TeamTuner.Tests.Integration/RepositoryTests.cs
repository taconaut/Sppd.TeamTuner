using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Shared;

using Xunit;

namespace Sppd.TeamTuner.Tests.Integration
{
    public class RepositoryTests : RepositoryTestsBase
    {
        /// <summary>
        ///     Tests that only navigation properties which have been explicitly requested from the repository will get loaded:
        ///     1. Load a <see cref="Team" /> in its own scope without specifying to load <see cref="Team.Users" />.
        ///     2. Load a <see cref="Team" /> in its own scope with specifying to load <see cref="Team.Users" />.
        ///     3. Assert that <see cref="Team.Users" /> have only been loaded for 2.
        /// </summary>
        [Fact]
        public async Task NavigationPropertyLoadingTest()
        {
            // Arrange
            var teamId = Guid.Parse(TestingConstants.Team.HOLY_COW);

            // Act
            Team teamWithoutUsers;
            using (var scope = ServiceProvider.CreateScope())
            {
                var teamRepository = scope.ServiceProvider.GetService<IRepository<Team>>();
                teamWithoutUsers = await teamRepository.GetAsync(teamId);
            }

            Team teamWithUsers;
            using (var scope = ServiceProvider.CreateScope())
            {
                var teamRepository = scope.ServiceProvider.GetService<IRepository<Team>>();
                teamWithUsers = await teamRepository.GetAsync(teamId, new[] {nameof(Team.Users)});
            }

            // Assert
            Assert.False(teamWithoutUsers.Users.Any());
            Assert.True(teamWithUsers.Users.Any());
        }

        /// <summary>
        ///     Tests that nested navigation properties get loaded when specified:
        ///     1. Load a <see cref="Team" /> in its own scope with specifying to load 'Users.Federation'.
        ///     2. Assert that <see cref="Team.Users" /> have only been loaded and that users have non-null
        ///     <see cref="Federation" />.
        /// </summary>
        [Fact]
        public async Task NestedNavigationPropertyLoadingTest()
        {
            // Arrange
            var teamId = Guid.Parse(TestingConstants.Team.HOLY_COW);

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
    }
}