using System;
using System.Threading.Tasks;

using Sppd.TeamTuner.Authorization;
using Sppd.TeamTuner.Authorization.Resources;

using Xunit;

namespace Sppd.TeamTuner.Tests.Unit.Api.Authorization
{
    /// <summary>
    ///     Tests authorization for user update operations.
    /// </summary>
    /// <seealso cref="AuthorizationTestsBase" />
    public class UserUpdateAuthorizationTests : AuthorizationTestsBase
    {
        [Fact]
        public async Task AdminUserCanUpdateAnyUserTest()
        {
            // Arrange
            SetCurrentUser(AdminUser);
            var resource = new CanUpdateUserResource {UserId = Guid.NewGuid()};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_UPDATE_USER);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task UserCannotUpdateOtherUserTest()
        {
            // Arrange
            SetCurrentUser(MemberTeam1);
            var resource = new CanUpdateUserResource {UserId = CoLeaderTeam1.Id};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_UPDATE_USER);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task UserCanUpdateOwnUserTest()
        {
            // Arrange
            SetCurrentUser(MemberTeam1);
            var resource = new CanUpdateUserResource {UserId = MemberTeam1.Id};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_UPDATE_USER);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }
    }
}