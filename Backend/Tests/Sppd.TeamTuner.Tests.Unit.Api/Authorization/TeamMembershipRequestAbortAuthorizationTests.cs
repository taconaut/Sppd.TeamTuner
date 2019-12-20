using System;
using System.Threading.Tasks;

using Sppd.TeamTuner.Authorization;
using Sppd.TeamTuner.Authorization.Resources;

using Xunit;

namespace Sppd.TeamTuner.Tests.Unit.Api.Authorization
{
    /// <summary>
    ///     Tests authorization for team membership abort operations.
    /// </summary>
    /// <seealso cref="AuthorizationTestsBase" />
    public class TeamMembershipRequestAbortAuthorizationTests : AuthorizationTestsBase
    {
        [Fact]
        public async Task AdminUserCanAbortAnyTeamMembershipRequestTest()
        {
            // Arrange
            SetCurrentUser(AdminUser);
            var resource = new CanAbortTeamMembershipResource {UserId = Guid.NewGuid()};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_ABORT_TEAM_MEMBERSHIP_REQUESTS);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task CurrentUserCanAbortHisTeamMembershipRequestTest()
        {
            // Arrange
            SetCurrentUser(MemberTeam1);
            var resource = new CanAbortTeamMembershipResource {UserId = MemberTeam1.Id};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_ABORT_TEAM_MEMBERSHIP_REQUESTS);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task CurrentUserCannotAbortOtherTeamMembershipRequestTest()
        {
            // Arrange
            SetCurrentUser(MemberTeam1);
            var resource = new CanAbortTeamMembershipResource {UserId = MemberTeam2.Id};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_ABORT_TEAM_MEMBERSHIP_REQUESTS);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }
    }
}