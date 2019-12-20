using System;
using System.Threading.Tasks;

using Sppd.TeamTuner.Authorization;
using Sppd.TeamTuner.Authorization.Resources;

using Xunit;

namespace Sppd.TeamTuner.Tests.Unit.Api.Authorization
{
    /// <summary>
    ///     Tests authorization for team update operations.
    /// </summary>
    /// <seealso cref="AuthorizationTestsBase" />
    public class TeamUpdateAuthorizationTests : AuthorizationTestsBase
    {
        [Fact]
        public async Task AdminUserCanUpdateAnyTeamTest()
        {
            // Arrange
            SetCurrentUser(AdminUser);
            var resource = new CanUpdateTeamResource {TeamId = Guid.NewGuid()};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_UPDATE_TEAM);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task TeamCoLeaderCannotUpdateOtherTeamTest()
        {
            // Arrange
            SetCurrentUser(CoLeaderTeam1);
            var resource = new CanUpdateTeamResource {TeamId = LeaderTeam2.TeamId};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_UPDATE_TEAM);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task TeamCoLeaderCanUpdateOwnTeamTest()
        {
            // Arrange
            SetCurrentUser(CoLeaderTeam1);
            var resource = new CanUpdateTeamResource {TeamId = CoLeaderTeam1.TeamId};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_UPDATE_TEAM);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task TeamLeaderCannotUpdateOtherTeamTest()
        {
            // Arrange
            SetCurrentUser(LeaderTeam1);
            var resource = new CanUpdateTeamResource {TeamId = LeaderTeam2.TeamId};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_UPDATE_TEAM);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task TeamLeaderCanUpdateOwnTeamTest()
        {
            // Arrange
            SetCurrentUser(LeaderTeam1);
            var resource = new CanUpdateTeamResource {TeamId = LeaderTeam1.TeamId};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_UPDATE_TEAM);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task TeamMemberCannotUpdateOtherTeamTest()
        {
            // Arrange
            SetCurrentUser(MemberTeam1);
            var resource = new CanUpdateTeamResource {TeamId = LeaderTeam2.TeamId};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_UPDATE_TEAM);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task TeamMemberCannotUpdateOwnTeamTest()
        {
            // Arrange
            SetCurrentUser(MemberTeam1);
            var resource = new CanUpdateTeamResource {TeamId = MemberTeam1.TeamId};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_UPDATE_TEAM);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }
    }
}