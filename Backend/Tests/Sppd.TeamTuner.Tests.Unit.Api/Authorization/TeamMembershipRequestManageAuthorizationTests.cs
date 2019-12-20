using System;
using System.Threading.Tasks;

using Sppd.TeamTuner.Authorization;
using Sppd.TeamTuner.Authorization.Resources;

using Xunit;

namespace Sppd.TeamTuner.Tests.Unit.Api.Authorization
{
    /// <summary>
    ///     Tests authorization for team membership management (accept/reject) operations.
    /// </summary>
    /// <seealso cref="AuthorizationTestsBase" />
    public class TeamMembershipRequestManageAuthorizationTests : AuthorizationTestsBase
    {
        [Fact]
        public async Task AdminUserCanManageAnyTeamMembershipRequestTest()
        {
            // Arrange
            SetCurrentUser(AdminUser);
            var resource = new CanManageTeamMembershipRequestsResource {TeamId = Guid.NewGuid()};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_MANAGE_TEAM_MEMBERSHIP_REQUESTS);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task CoLeaderCanManageTeamMembershipRequestInOwnTeamTest()
        {
            // Arrange
            SetCurrentUser(CoLeaderTeam1);
            var resource = new CanManageTeamMembershipRequestsResource {TeamId = CoLeaderTeam1.TeamId};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_MANAGE_TEAM_MEMBERSHIP_REQUESTS);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task CoLeaderCannotManageTeamMembershipRequestInOtherTeamTest()
        {
            // Arrange
            SetCurrentUser(CoLeaderTeam1);
            var resource = new CanManageTeamMembershipRequestsResource {TeamId = CoLeaderTeam2.TeamId};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_MANAGE_TEAM_MEMBERSHIP_REQUESTS);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task LeaderCanManageTeamMembershipRequestInOwnTeamTest()
        {
            // Arrange
            SetCurrentUser(LeaderTeam1);
            var resource = new CanManageTeamMembershipRequestsResource {TeamId = LeaderTeam1.TeamId};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_MANAGE_TEAM_MEMBERSHIP_REQUESTS);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task LeaderCannotManageTeamMembershipRequestInOtherTeamTest()
        {
            // Arrange
            SetCurrentUser(LeaderTeam1);
            var resource = new CanManageTeamMembershipRequestsResource {TeamId = LeaderTeam2.TeamId};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_MANAGE_TEAM_MEMBERSHIP_REQUESTS);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task MemberCannotManageTeamMembershipRequestInOtherTeamTest()
        {
            // Arrange
            SetCurrentUser(MemberTeam1);
            var resource = new CanManageTeamMembershipRequestsResource {TeamId = MemberTeam2.TeamId};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_MANAGE_TEAM_MEMBERSHIP_REQUESTS);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task MemberCannotManageTeamMembershipRequestInOwnTeamTest()
        {
            // Arrange
            SetCurrentUser(MemberTeam1);
            var resource = new CanManageTeamMembershipRequestsResource {TeamId = CoLeaderTeam1.TeamId};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_MANAGE_TEAM_MEMBERSHIP_REQUESTS);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }
    }
}