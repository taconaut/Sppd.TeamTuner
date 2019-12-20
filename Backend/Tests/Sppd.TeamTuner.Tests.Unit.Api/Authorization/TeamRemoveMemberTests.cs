using System;
using System.Threading.Tasks;

using Sppd.TeamTuner.Authorization;
using Sppd.TeamTuner.Authorization.Resources;

using Xunit;

namespace Sppd.TeamTuner.Tests.Unit.Api.Authorization
{
    /// <summary>
    ///     Tests authorization for team member removal operations.
    /// </summary>
    /// <seealso cref="AuthorizationTestsBase" />
    public class TeamRemoveMemberTests : AuthorizationTestsBase
    {
        [Fact]
        public async Task AdminUserCanRemoveAnyTeamMemberTest()
        {
            // Arrange
            SetCurrentUser(AdminUser);
            var resource = new CanRemoveTeamMemberResource
                           {
                               UserId = Guid.NewGuid(),
                               TeamId = Guid.NewGuid()
                           };

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_REMOVE_TEAM_MEMBER);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task CoLeaderCannotRemoveCoLeaderInOwnTeamTest()
        {
            // Arrange
            SetCurrentUser(CoLeaderTeam1);
            var resource = new CanRemoveTeamMemberResource
                           {
                               UserId = CoLeader2Team1.Id,
                               // ReSharper disable once PossibleInvalidOperationException
                               TeamId = CoLeaderTeam1.TeamId.Value
                           };

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_REMOVE_TEAM_MEMBER);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task CoLeaderCannotRemoveLeaderInOwnTeamTest()
        {
            // Arrange
            SetCurrentUser(CoLeaderTeam1);
            var resource = new CanRemoveTeamMemberResource
                           {
                               UserId = LeaderTeam1.Id,
                               // ReSharper disable once PossibleInvalidOperationException
                               TeamId = CoLeaderTeam1.TeamId.Value
                           };

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_REMOVE_TEAM_MEMBER);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task CoLeaderCanRemoveHimselfTest()
        {
            // Arrange
            SetCurrentUser(CoLeaderTeam1);
            var resource = new CanRemoveTeamMemberResource
                           {
                               UserId = CoLeaderTeam1.Id,
                               // ReSharper disable once PossibleInvalidOperationException
                               TeamId = CoLeaderTeam1.TeamId.Value
                           };

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_REMOVE_TEAM_MEMBER);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task CoLeaderCanRemoveMemberInOwnTeamTest()
        {
            // Arrange
            SetCurrentUser(CoLeaderTeam1);
            var resource = new CanRemoveTeamMemberResource
                           {
                               UserId = MemberTeam1.Id,
                               // ReSharper disable once PossibleInvalidOperationException
                               TeamId = CoLeaderTeam1.TeamId.Value
                           };

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_REMOVE_TEAM_MEMBER);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task LeaderCannotRemoveCoLeaderInOtherTeamTest()
        {
            // Arrange
            SetCurrentUser(LeaderTeam1);
            var resource = new CanRemoveTeamMemberResource
                           {
                               UserId = CoLeaderTeam2.Id,
                               // ReSharper disable once PossibleInvalidOperationException
                               TeamId = LeaderTeam1.TeamId.Value
                           };

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_REMOVE_TEAM_MEMBER);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task LeaderCannotRemoveHimselfTest()
        {
            // Arrange
            SetCurrentUser(LeaderTeam1);
            var resource = new CanRemoveTeamMemberResource
                           {
                               UserId = LeaderTeam1.Id,
                               // ReSharper disable once PossibleInvalidOperationException
                               TeamId = LeaderTeam1.TeamId.Value
                           };

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_REMOVE_TEAM_MEMBER);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task LeaderCannotRemoveMemberInOtherTeamTest()
        {
            // Arrange
            SetCurrentUser(LeaderTeam1);
            var resource = new CanRemoveTeamMemberResource
                           {
                               UserId = MemberTeam2.Id,
                               // ReSharper disable once PossibleInvalidOperationException
                               TeamId = LeaderTeam1.TeamId.Value
                           };

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_REMOVE_TEAM_MEMBER);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task LeaderCanRemoveCoLeaderInOwnTeamTest()
        {
            // Arrange
            SetCurrentUser(LeaderTeam1);
            var resource = new CanRemoveTeamMemberResource
                           {
                               UserId = CoLeaderTeam1.Id,
                               // ReSharper disable once PossibleInvalidOperationException
                               TeamId = LeaderTeam1.TeamId.Value
                           };

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_REMOVE_TEAM_MEMBER);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task LeaderCanRemoveMemberInOwnTeamTest()
        {
            // Arrange
            SetCurrentUser(LeaderTeam1);
            var resource = new CanRemoveTeamMemberResource
                           {
                               UserId = MemberTeam1.Id,
                               // ReSharper disable once PossibleInvalidOperationException
                               TeamId = LeaderTeam1.TeamId.Value
                           };

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_REMOVE_TEAM_MEMBER);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task MemberCannotRemoveCoLeaderTest()
        {
            // Arrange
            SetCurrentUser(MemberTeam1);
            var resource = new CanRemoveTeamMemberResource
                           {
                               UserId = CoLeaderTeam1.Id,
                               // ReSharper disable once PossibleInvalidOperationException
                               TeamId = MemberTeam1.TeamId.Value
                           };

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_REMOVE_TEAM_MEMBER);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task MemberCannotRemoveLeaderTest()
        {
            // Arrange
            SetCurrentUser(MemberTeam1);
            var resource = new CanRemoveTeamMemberResource
                           {
                               UserId = LeaderTeam1.Id,
                               // ReSharper disable once PossibleInvalidOperationException
                               TeamId = MemberTeam1.TeamId.Value
                           };

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_REMOVE_TEAM_MEMBER);

            // Assert
            Assert.False(authorizationResult.Succeeded);
        }

        [Fact]
        public async Task MemberCanRemoveHimselfTest()
        {
            // Arrange
            SetCurrentUser(MemberTeam1);
            var resource = new CanRemoveTeamMemberResource
                           {
                               UserId = MemberTeam1.Id,
                               // ReSharper disable once PossibleInvalidOperationException
                               TeamId = MemberTeam1.TeamId.Value
                           };

            // Act
            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(GetCurrentUser(), GetAuthorizationRequest(resource), AuthorizationConstants.Policies.CAN_REMOVE_TEAM_MEMBER);

            // Assert
            Assert.True(authorizationResult.Succeeded);
        }
    }
}