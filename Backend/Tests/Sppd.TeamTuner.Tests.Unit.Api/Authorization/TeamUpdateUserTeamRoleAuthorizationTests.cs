using System;
using System.Threading.Tasks;

using Sppd.TeamTuner.Authorization;
using Sppd.TeamTuner.Authorization.Resources;
using Sppd.TeamTuner.Core;

using Xunit;

namespace Sppd.TeamTuner.Tests.Unit.Api.Authorization
{
    /// <summary>
    ///     Tests authorization for team member role update operations.
    /// </summary>
    /// <seealso cref="AuthorizationTestsBase" />
    public class TeamUpdateUserTeamRoleAuthorizationTests : AuthorizationTestsBase
    {
        [Fact]
        public async Task AdminUserCanUpdateAnyUserRoleTest()
        {
            // Arrange
            SetCurrentUser(AdminUser);
            var resource = new CanUpdateMemberTeamRoleResource
                           {
                               UserId = Guid.NewGuid(),
                               TeamId = Guid.NewGuid(),
                               Role = CoreConstants.Authorization.Roles.MEMBER
                           };

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_UPDATE_USER_TEAM_ROLE);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task CoLeaderCannotPromoteCoLeaderToLeaderInOwnTeamRoleTest()
        {
            // Arrange
            SetCurrentUser(CoLeaderTeam1);
            var resource = new CanUpdateMemberTeamRoleResource
                           {
                               UserId = CoLeaderTeam1.Id,
                               // ReSharper disable once PossibleInvalidOperationException
                               TeamId = LeaderTeam1.TeamId.Value,
                               Role = CoreConstants.Authorization.Roles.LEADER
                           };

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_UPDATE_USER_TEAM_ROLE);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task CoLeaderCannotPromoteMemberToCoLeaderInOwnTeamRoleTest()
        {
            // Arrange
            SetCurrentUser(CoLeaderTeam1);
            var resource = new CanUpdateMemberTeamRoleResource
                           {
                               UserId = MemberTeam1.Id,
                               // ReSharper disable once PossibleInvalidOperationException
                               TeamId = LeaderTeam1.TeamId.Value,
                               Role = CoreConstants.Authorization.Roles.CO_LEADER
                           };

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_UPDATE_USER_TEAM_ROLE);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task CoLeaderCannotPromoteMemberToLeaderInOwnTeamRoleTest()
        {
            // Arrange
            SetCurrentUser(CoLeaderTeam1);
            var resource = new CanUpdateMemberTeamRoleResource
                           {
                               UserId = MemberTeam1.Id,
                               // ReSharper disable once PossibleInvalidOperationException
                               TeamId = LeaderTeam1.TeamId.Value,
                               Role = CoreConstants.Authorization.Roles.LEADER
                           };

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_UPDATE_USER_TEAM_ROLE);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task LeaderCanDemoteCoLeaderToMemberInOwnTeamRoleTest()
        {
            // Arrange
            SetCurrentUser(LeaderTeam1);
            var resource = new CanUpdateMemberTeamRoleResource
                           {
                               UserId = CoLeaderTeam1.Id,
                               // ReSharper disable once PossibleInvalidOperationException
                               TeamId = LeaderTeam1.TeamId.Value,
                               Role = CoreConstants.Authorization.Roles.MEMBER
                           };

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_UPDATE_USER_TEAM_ROLE);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task LeaderCannotDemoteHimselfToCoLeaderRoleTest()
        {
            // Arrange
            SetCurrentUser(LeaderTeam1);
            var resource = new CanUpdateMemberTeamRoleResource
                           {
                               UserId = LeaderTeam1.Id,
                               // ReSharper disable once PossibleInvalidOperationException
                               TeamId = LeaderTeam1.TeamId.Value,
                               Role = CoreConstants.Authorization.Roles.CO_LEADER
                           };

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_UPDATE_USER_TEAM_ROLE);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task LeaderCannotDemoteHimselfToMemberRoleTest()
        {
            // Arrange
            SetCurrentUser(LeaderTeam1);
            var resource = new CanUpdateMemberTeamRoleResource
                           {
                               UserId = LeaderTeam1.Id,
                               // ReSharper disable once PossibleInvalidOperationException
                               TeamId = LeaderTeam1.TeamId.Value,
                               Role = CoreConstants.Authorization.Roles.MEMBER
                           };

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_UPDATE_USER_TEAM_ROLE);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task LeaderCannotPromoteMemberToCoLeaderInOtherTeamRoleTest()
        {
            // Arrange
            SetCurrentUser(LeaderTeam2);
            var resource = new CanUpdateMemberTeamRoleResource
                           {
                               UserId = MemberTeam1.Id,
                               // ReSharper disable once PossibleInvalidOperationException
                               TeamId = LeaderTeam2.TeamId.Value,
                               Role = CoreConstants.Authorization.Roles.CO_LEADER
                           };

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_UPDATE_USER_TEAM_ROLE);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task LeaderCanPromoteCoLeaderToLeaderInOwnTeamRoleTest()
        {
            // Arrange
            SetCurrentUser(LeaderTeam1);
            var resource = new CanUpdateMemberTeamRoleResource
                           {
                               UserId = CoLeaderTeam1.Id,
                               // ReSharper disable once PossibleInvalidOperationException
                               TeamId = LeaderTeam1.TeamId.Value,
                               Role = CoreConstants.Authorization.Roles.LEADER
                           };

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_UPDATE_USER_TEAM_ROLE);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task LeaderCanPromoteMemberToCoLeaderInOwnTeamRoleTest()
        {
            // Arrange
            SetCurrentUser(LeaderTeam1);
            var resource = new CanUpdateMemberTeamRoleResource
                           {
                               UserId = MemberTeam1.Id,
                               // ReSharper disable once PossibleInvalidOperationException
                               TeamId = LeaderTeam1.TeamId.Value,
                               Role = CoreConstants.Authorization.Roles.CO_LEADER
                           };

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_UPDATE_USER_TEAM_ROLE);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task LeaderCanPromoteMemberToLeaderInOwnTeamRoleTest()
        {
            // Arrange
            SetCurrentUser(LeaderTeam1);
            var resource = new CanUpdateMemberTeamRoleResource
                           {
                               UserId = MemberTeam1.Id,
                               // ReSharper disable once PossibleInvalidOperationException
                               TeamId = LeaderTeam1.TeamId.Value,
                               Role = CoreConstants.Authorization.Roles.LEADER
                           };

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_UPDATE_USER_TEAM_ROLE);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }
    }
}