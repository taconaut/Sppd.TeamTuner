using System;
using System.Threading.Tasks;

using Sppd.TeamTuner.Authorization;
using Sppd.TeamTuner.Authorization.Resources;

using Xunit;

namespace Sppd.TeamTuner.Tests.Unit.Api.Authorization
{
    /// <summary>
    ///     Tests authorization for team delete operations.
    /// </summary>
    /// <seealso cref="AuthorizationTestsBase" />
    public class TeamDeleteAuthorizationTests : AuthorizationTestsBase
    {
        [Fact]
        public async Task AdminUserCanDeleteAnyTeamTest()
        {
            // Arrange
            SetCurrentUser(AdminUser);
            var resource = new CanDeleteTeamResource {TeamId = Guid.NewGuid()};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_DELETE_TEAM);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task TeamCoLeaderCannotDeleteOtherTeamTest()
        {
            // Arrange
            SetCurrentUser(CoLeaderTeam1);
            var resource = new CanDeleteTeamResource {TeamId = LeaderTeam2.TeamId};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_DELETE_TEAM);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task TeamCoLeaderCannotDeleteOwnTeamTest()
        {
            // Arrange
            SetCurrentUser(CoLeaderTeam1);
            var resource = new CanDeleteTeamResource {TeamId = CoLeaderTeam1.TeamId};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_DELETE_TEAM);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task TeamLeaderCanDeleteOwnTeamTest()
        {
            // Arrange
            SetCurrentUser(LeaderTeam1);
            var resource = new CanDeleteTeamResource {TeamId = LeaderTeam1.TeamId};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_DELETE_TEAM);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task TeamLeaderCannotDeleteOtherTeamTest()
        {
            // Arrange
            SetCurrentUser(LeaderTeam1);
            var resource = new CanDeleteTeamResource {TeamId = LeaderTeam2.TeamId};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_DELETE_TEAM);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task TeamMemberCannotDeleteOtherTeamTest()
        {
            // Arrange
            SetCurrentUser(MemberTeam1);
            var resource = new CanDeleteTeamResource {TeamId = LeaderTeam2.TeamId};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_DELETE_TEAM);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task TeamMemberCannotDeleteOwnTeamTest()
        {
            // Arrange
            SetCurrentUser(MemberTeam1);
            var resource = new CanDeleteTeamResource {TeamId = MemberTeam1.TeamId};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_DELETE_TEAM);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }
    }
}