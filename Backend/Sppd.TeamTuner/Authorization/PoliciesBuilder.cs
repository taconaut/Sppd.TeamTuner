using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;

using Sppd.TeamTuner.Authorization.Resources;
using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Domain.Enumerations;
using Sppd.TeamTuner.Core.Domain.Interfaces;
using Sppd.TeamTuner.Core.Repositories;

namespace Sppd.TeamTuner.Authorization
{
    internal static class PoliciesBuilder
    {
        /// <summary>
        ///     Adds the policies required for authorization.
        /// </summary>
        /// <param name="options">The authorization options.</param>
        public static void AddPolicies(this AuthorizationOptions options)
        {
            // User
            options.AddPolicy(AuthorizationConstants.Policies.CAN_READ_USER,
                policy => policy.RequireAssertion(async ctx =>
                {
                    var authorizationRequest = (AuthorizationRequest<CanReadUserResource>) ctx.Resource;
                    var currentUser = authorizationRequest.CurrentUser;
                    var userId = authorizationRequest.Resource.UserId;
                    var userRepository = new Lazy<IRepository<TeamTunerUser>>(() => authorizationRequest.RepositoryResolver.ResolveFor<TeamTunerUser>());
                    return IsAdmin(currentUser) || IsCurrentUser(currentUser, userId) || await IsProfileVisibleAsync(currentUser, userId, userRepository);
                }));
            options.AddPolicy(AuthorizationConstants.Policies.CAN_UPDATE_USER,
                policy => policy.RequireAssertion(ctx =>
                {
                    var authorizationRequest = (AuthorizationRequest<CanUpdateUserResource>) ctx.Resource;
                    var currentUser = authorizationRequest.CurrentUser;
                    var userId = authorizationRequest.Resource.UserId;
                    return IsAdmin(currentUser) || IsCurrentUser(currentUser, userId);
                }));
            options.AddPolicy(AuthorizationConstants.Policies.CAN_DELETE_USER,
                policy => policy.RequireAssertion(ctx =>
                {
                    var authorizationRequest = (AuthorizationRequest<CanDeleteUserResource>) ctx.Resource;
                    var currentUser = authorizationRequest.CurrentUser;
                    var userId = authorizationRequest.Resource.UserId;
                    return IsAdmin(currentUser) || IsCurrentUser(currentUser, userId);
                }));

            // Team
            options.AddPolicy(AuthorizationConstants.Policies.CAN_READ_TEAM,
                policy => policy.RequireAssertion(ctx =>
                {
                    var authorizationRequest = (AuthorizationRequest<CanReadTeamResource>) ctx.Resource;
                    var currentUser = authorizationRequest.CurrentUser;
                    var teamId = authorizationRequest.Resource.TeamId;
                    return IsAdmin(currentUser) || IsInTeam(currentUser, teamId);
                }));
            options.AddPolicy(AuthorizationConstants.Policies.CAN_UPDATE_TEAM,
                policy => policy.RequireAssertion(ctx =>
                {
                    var authorizationRequest = (AuthorizationRequest<CanUpdateTeamResource>) ctx.Resource;
                    var currentUser = authorizationRequest.CurrentUser;
                    var teamId = authorizationRequest.Resource.TeamId;
                    return IsAdmin(currentUser) || IsTeamLeader(currentUser, teamId) || IsTeamCoLeader(currentUser, teamId);
                }));
            options.AddPolicy(AuthorizationConstants.Policies.CAN_DELETE_TEAM,
                policy => policy.RequireAssertion(ctx =>
                {
                    var authorizationRequest = (AuthorizationRequest<CanDeleteTeamResource>) ctx.Resource;
                    var currentUser = authorizationRequest.CurrentUser;
                    var teamId = authorizationRequest.Resource.TeamId;
                    return IsAdmin(currentUser) || IsTeamLeader(currentUser, teamId);
                }));
            options.AddPolicy(AuthorizationConstants.Policies.CAN_UPDATE_USER_TEAM_ROLE,
                policy => policy.RequireAssertion(async ctx =>
                {
                    var authorizationRequest = (AuthorizationRequest<CanUpdateMemberTeamRoleResource>) ctx.Resource;
                    var currentUser = authorizationRequest.CurrentUser;
                    var resource = authorizationRequest.Resource;
                    var userRepository = new Lazy<IRepository<TeamTunerUser>>(() => authorizationRequest.RepositoryResolver.ResolveFor<TeamTunerUser>());
                    return IsAdmin(currentUser) || await CanUpdateUserTeamRoleAsync(currentUser, resource, userRepository);
                }));
            options.AddPolicy(AuthorizationConstants.Policies.CAN_REMOVE_TEAM_MEMBER,
                policy => policy.RequireAssertion(async ctx =>
                {
                    var authorizationRequest = (AuthorizationRequest<CanRemoveTeamMemberResource>) ctx.Resource;
                    var currentUser = authorizationRequest.CurrentUser;
                    var resource = authorizationRequest.Resource;
                    var userRepository = new Lazy<IRepository<TeamTunerUser>>(() => authorizationRequest.RepositoryResolver.ResolveFor<TeamTunerUser>());
                    return IsAdmin(currentUser) || await CanRemoveTeamMemberAsync(currentUser, resource, userRepository);
                }));

            // Team membership requests
            options.AddPolicy(AuthorizationConstants.Policies.CAN_MANAGE_TEAM_MEMBERSHIP_REQUESTS,
                policy => policy.RequireAssertion(ctx =>
                {
                    var authorizationRequest = (AuthorizationRequest<CanManageTeamMembershipRequestsResource>) ctx.Resource;
                    var currentUser = authorizationRequest.CurrentUser;
                    var teamId = authorizationRequest.Resource.TeamId;
                    return IsAdmin(currentUser) || IsTeamLeader(currentUser, teamId) || IsTeamCoLeader(currentUser, teamId);
                }));
            options.AddPolicy(AuthorizationConstants.Policies.CAN_ABORT_TEAM_MEMBERSHIP_REQUESTS,
                policy => policy.RequireAssertion(ctx =>
                {
                    var authorizationRequest = (AuthorizationRequest<CanAbortTeamMembershipResource>) ctx.Resource;
                    var currentUser = authorizationRequest.CurrentUser;
                    var userId = authorizationRequest.Resource.UserId;
                    return IsAdmin(currentUser) || IsCurrentUser(currentUser, userId);
                }));
        }

        // Application
        private static bool IsAdmin(ITeamTunerUser currentUser)
        {
            return Equals(CoreConstants.Authorization.Roles.ADMIN, currentUser.ApplicationRole);
        }

        // User
        private static bool IsCurrentUser(ITeamTunerUser currentUser, Guid? userId)
        {
            return Equals(currentUser.Id, userId);
        }

        private static async Task<bool> IsProfileVisibleAsync(ITeamTunerUser currentUser, Guid? userId, Lazy<IRepository<TeamTunerUser>> userRepository)
        {
            if (!userId.HasValue)
            {
                return false;
            }

            var user = await userRepository.Value.GetAsync(userId.Value);
            if (user == null)
            {
                return false;
            }

            switch (user.ProfileVisibility)
            {
                case UserProfileVisibility.Team:
                    return Equals(currentUser.TeamId, user.TeamId);

                case UserProfileVisibility.Federation:
                    return Equals(currentUser.FederationId, user.FederationId);

                default:
                    return false;
            }
        }

        // Team
        private static bool IsInTeam(ITeamTunerUser currentUser, Guid? teamId)
        {
            return Equals(currentUser.TeamId, teamId);
        }

        private static bool IsTeamLeader(ITeamTunerUser currentUser, Guid? teamId)
        {
            return IsInTeam(currentUser, teamId)
                   && Equals(CoreConstants.Authorization.Roles.LEADER, currentUser.TeamRole);
        }

        private static bool IsTeamCoLeader(ITeamTunerUser currentUser, Guid? teamId)
        {
            return IsInTeam(currentUser, teamId)
                   && Equals(CoreConstants.Authorization.Roles.CO_LEADER, currentUser.TeamRole);
        }

        private static async Task<bool> CanUpdateUserTeamRoleAsync(ITeamTunerUser currentUser, CanUpdateMemberTeamRoleResource resource,
            Lazy<IRepository<TeamTunerUser>> userRepository)
        {
            var user = await userRepository.Value.GetAsync(resource.UserId);

            // Do not allow to update the role for a team the user is not in
            if (!IsInTeam(user, resource.TeamId))
            {
                return false;
            }

            // The team leader is not allowed to change his role
            if (IsTeamLeader(user, resource.TeamId))
            {
                return false;
            }

            switch (resource.Role)
            {
                case CoreConstants.Authorization.Roles.LEADER:
                case CoreConstants.Authorization.Roles.CO_LEADER:
                    // The team leader is allowed to grant leader and co-leader roles for everyone except himself
                    return IsTeamLeader(currentUser, resource.TeamId);

                case CoreConstants.Authorization.Roles.MEMBER:
                    // The user himself as well as the leader and co-leaders are allowed to grant user the member role
                    return IsCurrentUser(currentUser, resource.UserId)
                           || IsTeamLeader(currentUser, resource.TeamId)
                           || IsTeamCoLeader(currentUser, resource.TeamId);

                default:
                    throw new NotSupportedException($"Role '{resource.Role}' is not supported.");
            }
        }

        private static async Task<bool> CanRemoveTeamMemberAsync(ITeamTunerUser currentUser, CanRemoveTeamMemberResource resource, Lazy<IRepository<TeamTunerUser>> userRepository)
        {
            // A user can only be removed by a member of the same team
            if (!IsInTeam(currentUser, resource.TeamId))
            {
                return false;
            }

            var user = await userRepository.Value.GetAsync(resource.UserId);

            // A user can't be removed from a team he is not part of
            if (!IsInTeam(user, resource.TeamId))
            {
                return false;
            }

            // The leader is not allowed to be removed from the team
            if (IsTeamLeader(user, resource.TeamId))
            {
                return false;
            }

            // A user can leave a team at any time if he isn't the team leader
            if (currentUser.Id == resource.UserId
                && currentUser.TeamRole != CoreConstants.Authorization.Roles.LEADER)
            {
                return true;
            }

            // A user with a higher role can remove other users
            return IsTeamRole1GreaterThanRole2(currentUser.TeamRole, user.TeamRole);
        }

        private static bool IsTeamRole1GreaterThanRole2(string role1, string role2)
        {
            switch (role1)
            {
                case CoreConstants.Authorization.Roles.LEADER:
                    return role2 == CoreConstants.Authorization.Roles.CO_LEADER
                           || role2 == CoreConstants.Authorization.Roles.MEMBER;

                case CoreConstants.Authorization.Roles.CO_LEADER:
                    return role2 == CoreConstants.Authorization.Roles.MEMBER;

                default:
                    return false;
            }
        }
    }
}