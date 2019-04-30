using System;

using Microsoft.AspNetCore.Authorization;

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
            // Application
            options.AddPolicy(AuthorizationConstants.Policies.IS_ADMIN,
                policy => policy.RequireAssertion(ctx =>
                {
                    var authorizationRequest = (AuthorizationRequest) ctx.Resource;
                    var user = authorizationRequest.CurrentUser;
                    return IsAdmin(user);
                }));

            // User
            options.AddPolicy(AuthorizationConstants.Policies.CAN_READ_USER,
                policy => policy.RequireAssertion(ctx =>
                {
                    var authorizationRequest = (AuthorizationRequest) ctx.Resource;
                    var user = authorizationRequest.CurrentUser;
                    var userId = authorizationRequest.Resource as Guid?;
                    var userRepository = new Lazy<IRepository<TeamTunerUser>>(() => authorizationRequest.RepositoryResolver.ResolveFor<TeamTunerUser>());
                    return IsAdmin(user) || IsCurrentUser(user, userId) || IsProfileVisible(user, userId, userRepository);
                }));
            options.AddPolicy(AuthorizationConstants.Policies.CAN_UPDATE_USER,
                policy => policy.RequireAssertion(ctx =>
                {
                    var authorizationRequest = (AuthorizationRequest) ctx.Resource;
                    var user = authorizationRequest.CurrentUser;
                    var userId = authorizationRequest.Resource as Guid?;
                    return IsAdmin(user) || IsCurrentUser(user, userId);
                }));
            options.AddPolicy(AuthorizationConstants.Policies.CAN_DELETE_USER,
                policy => policy.RequireAssertion(ctx =>
                {
                    var authorizationRequest = (AuthorizationRequest) ctx.Resource;
                    var user = authorizationRequest.CurrentUser;
                    var userId = authorizationRequest.Resource as Guid?;
                    return IsAdmin(user) || IsCurrentUser(user, userId);
                }));

            // Team
            options.AddPolicy(AuthorizationConstants.Policies.CAN_READ_TEAM,
                policy => policy.RequireAssertion(ctx =>
                {
                    var authorizationRequest = (AuthorizationRequest) ctx.Resource;
                    var user = authorizationRequest.CurrentUser;
                    var teamId = authorizationRequest.Resource as Guid?;
                    return IsAdmin(user) || IsInTeam(user, teamId);
                }));
            options.AddPolicy(AuthorizationConstants.Policies.CAN_UPDATE_TEAM,
                policy => policy.RequireAssertion(ctx =>
                {
                    var authorizationRequest = (AuthorizationRequest) ctx.Resource;
                    var user = authorizationRequest.CurrentUser;
                    var teamId = authorizationRequest.Resource as Guid?;
                    return IsAdmin(user) || IsTeamLeader(user, teamId) || IsTeamCoLeader(user, teamId);
                }));
            options.AddPolicy(AuthorizationConstants.Policies.CAN_DELETE_TEAM,
                policy => policy.RequireAssertion(ctx =>
                {
                    var authorizationRequest = (AuthorizationRequest) ctx.Resource;
                    var user = authorizationRequest.CurrentUser;
                    var teamId = authorizationRequest.Resource as Guid?;
                    return IsAdmin(user) || IsTeamLeader(user, teamId) || IsTeamCoLeader(user, teamId);
                }));

            // Team membership requests
            options.AddPolicy(AuthorizationConstants.Policies.CAN_ACCEPT_TEAM_MEMBERSHIP_REQUESTS,
                policy => policy.RequireAssertion(ctx =>
                {
                    var authorizationRequest = (AuthorizationRequest) ctx.Resource;
                    var user = authorizationRequest.CurrentUser;
                    var teamId = authorizationRequest.Resource as Guid?;
                    return IsAdmin(user) || IsTeamLeader(user, teamId) || IsTeamCoLeader(user, teamId);
                }));

            // Federation
            options.AddPolicy(AuthorizationConstants.Policies.CAN_READ_FEDERATION,
                policy => policy.RequireAssertion(ctx =>
                {
                    var authorizationRequest = (AuthorizationRequest) ctx.Resource;
                    var user = authorizationRequest.CurrentUser;
                    var federationId = authorizationRequest.Resource as Guid?;
                    return IsAdmin(user) || IsInFederation(user, federationId);
                }));
            options.AddPolicy(AuthorizationConstants.Policies.CAN_UPDATE_FEDERATION,
                policy => policy.RequireAssertion(ctx =>
                {
                    var authorizationRequest = (AuthorizationRequest) ctx.Resource;
                    var user = authorizationRequest.CurrentUser;
                    var federationId = authorizationRequest.Resource as Guid?;
                    return IsAdmin(user) || IsFederationLeader(user, federationId) || IsFederationCoLeader(user, federationId);
                }));
            options.AddPolicy(AuthorizationConstants.Policies.CAN_DELETE_FEDERATION,
                policy => policy.RequireAssertion(ctx =>
                {
                    var authorizationRequest = (AuthorizationRequest) ctx.Resource;
                    var user = authorizationRequest.CurrentUser;
                    var federationId = authorizationRequest.Resource as Guid?;
                    return IsAdmin(user) || IsFederationLeader(user, federationId) || IsFederationCoLeader(user, federationId);
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

        private static bool IsProfileVisible(ITeamTunerUser currentUser, Guid? userId, Lazy<IRepository<TeamTunerUser>> userRepository)
        {
            if (!userId.HasValue)
            {
                return false;
            }

            var getUserTask = userRepository.Value.GetAsync(userId.Value);
            getUserTask.Wait();
            var user = getUserTask.Result;

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

        // Federation
        private static bool IsInFederation(ITeamTunerUser currentUser, Guid? federationId)
        {
            return Equals(currentUser.FederationId, federationId);
        }

        private static bool IsFederationLeader(ITeamTunerUser currentUser, Guid? federationId)
        {
            return IsInFederation(currentUser, federationId)
                   && Equals(CoreConstants.Authorization.Roles.LEADER, currentUser.FederationRole);
        }

        private static bool IsFederationCoLeader(ITeamTunerUser currentUser, Guid? federationId)
        {
            return IsInFederation(currentUser, federationId)
                   && Equals(CoreConstants.Authorization.Roles.CO_LEADER, currentUser.FederationRole);
        }
    }
}