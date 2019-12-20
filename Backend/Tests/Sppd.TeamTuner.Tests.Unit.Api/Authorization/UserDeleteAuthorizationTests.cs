using System;
using System.Threading.Tasks;

using Sppd.TeamTuner.Authorization;
using Sppd.TeamTuner.Authorization.Resources;

using Xunit;

namespace Sppd.TeamTuner.Tests.Unit.Api.Authorization
{
    /// <summary>
    ///     Tests authorization for user delete operations.
    /// </summary>
    /// <seealso cref="AuthorizationTestsBase" />
    public class UserDeleteAuthorizationTests : AuthorizationTestsBase
    {
        [Fact]
        public async Task AdminUserCanDeleteAnyUserTest()
        {
            // Arrange
            SetCurrentUser(AdminUser);
            var resource = new CanDeleteUserResource {UserId = Guid.NewGuid()};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_DELETE_USER);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task UserCanDeleteOwnUserTest()
        {
            // Arrange
            SetCurrentUser(MemberTeam1);
            var resource = new CanDeleteUserResource {UserId = MemberTeam1.Id};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_DELETE_USER);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task UserCannotDeleteOtherUserTest()
        {
            // Arrange
            SetCurrentUser(MemberTeam1);
            var resource = new CanDeleteUserResource {UserId = CoLeaderTeam1.Id};

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_DELETE_USER);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }
    }
}