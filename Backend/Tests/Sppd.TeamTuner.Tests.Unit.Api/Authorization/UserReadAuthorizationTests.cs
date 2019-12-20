using System;
using System.Threading.Tasks;

using Sppd.TeamTuner.Authorization;
using Sppd.TeamTuner.Authorization.Resources;
using Sppd.TeamTuner.Core.Domain.Entities;

using Xunit;

namespace Sppd.TeamTuner.Tests.Unit.Api.Authorization
{
    /// <summary>
    ///     Tests authorization for user read operations.
    /// </summary>
    /// <seealso cref="AuthorizationTestsBase" />
    public class UserReadAuthorizationTests : AuthorizationTestsBase
    {
        [Fact]
        public async Task AdminUserCanReadAnyUserTest()
        {
            // Arrange
            SetCurrentUser(AdminUser);
            var resource = new CanReadUserResource {UserId = Guid.NewGuid()};

            // Act
            var authorizationResult = await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_READ_USER);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task UnknownUserCannotReadUserTest()
        {
            // Arrange
            SetCurrentUser(new TeamTunerUser());
            var resource = new CanReadUserResource {UserId = Guid.NewGuid()};

            // Act
            var authorizationResult = await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_READ_USER);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task UserCannotReadUserFromOtherTeamTest()
        {
            // Arrange
            SetCurrentUser(MemberTeam1);
            var resource = new CanReadUserResource {UserId = LeaderTeam2.Id};

            // Act
            var authorizationResult = await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_READ_USER);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task UserCannotReadUserInTeamWithUserVisibility()
        {
            // Arrange
            SetCurrentUser(CoLeaderTeam1);
            var resource = new CanReadUserResource {UserId = MemberTeam1.Id};

            // Act
            var authorizationResult = await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_READ_USER);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task UserCanReadOwnUserTest()
        {
            // Arrange
            SetCurrentUser(MemberTeam1);
            var resource = new CanReadUserResource {UserId = MemberTeam1.Id};

            // Act
            var authorizationResult = await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_READ_USER);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task UserCanReadUserInTeamWithTeamVisibility()
        {
            // Arrange
            SetCurrentUser(MemberTeam1);
            var resource = new CanReadUserResource {UserId = CoLeaderTeam1.Id};

            // Act
            var authorizationResult = await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_READ_USER);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }
    }
}