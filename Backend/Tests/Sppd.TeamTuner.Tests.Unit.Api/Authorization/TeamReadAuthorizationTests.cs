using System;
using System.Threading.Tasks;

using Sppd.TeamTuner.Authorization;
using Sppd.TeamTuner.Authorization.Resources;
using Sppd.TeamTuner.Core.Domain.Entities;

using Xunit;

namespace Sppd.TeamTuner.Tests.Unit.Api.Authorization
{
    /// <summary>
    ///     Tests authorization for team read operations.
    /// </summary>
    /// <seealso cref="AuthorizationTestsBase" />
    public class TeamReadAuthorizationTests : AuthorizationTestsBase
    {
        [Fact]
        public async Task AdminUserCanReadAnyTeamTest()
        {
            // Arrange
            SetCurrentUser(AdminUser);
            var resource = new CanReadTeamResource {TeamId = Guid.NewGuid()};

            // Act
            var authorizationResult = await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_READ_TEAM);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task UnknownUserCannotReadUnknownTeamTest()
        {
            // Arrange
            SetCurrentUser(new TeamTunerUser());
            var resource = new CanReadTeamResource {TeamId = Guid.NewGuid()};

            // Act
            var authorizationResult = await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_READ_TEAM);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task UserCannotReadOtherTeam()
        {
            // Arrange
            SetCurrentUser(MemberTeam1);
            var resource = new CanReadTeamResource {TeamId = LeaderTeam2.TeamId};

            // Act
            var authorizationResult = await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_READ_TEAM);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task UserCanReadOwnTeam()
        {
            // Arrange
            SetCurrentUser(MemberTeam1);
            var resource = new CanReadTeamResource {TeamId = MemberTeam1.TeamId};

            // Act
            var authorizationResult = await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_READ_TEAM);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }
    }
}