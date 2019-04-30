using System;

using Microsoft.AspNetCore.Authorization;

using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Domain.Interfaces;

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
                    // TODO: extend rights
                    return IsAdmin(user) || IsCurrentUser(user, userId);
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
        private static bool IsAdmin(ITeamTunerUser user)
        {
            return Equals(CoreConstants.Auth.Roles.ADMIN, user.ApplicationRole);
        }

        // User
        private static bool IsCurrentUser(ITeamTunerUser user, Guid? userId)
        {
            return Equals(user.Id, userId);
        }

        // Team
        private static bool IsInTeam(ITeamTunerUser user, Guid? teamId)
        {
            return Equals(user.TeamId, teamId);
        }

        private static bool IsTeamLeader(ITeamTunerUser user, Guid? teamId)
        {
            return IsInTeam(user, teamId) && Equals(CoreConstants.Auth.Roles.LEADER, user.TeamRole);
        }

        private static bool IsTeamCoLeader(ITeamTunerUser user, Guid? teamId)
        {
            return IsInTeam(user, teamId) && Equals(CoreConstants.Auth.Roles.CO_LEADER, user.TeamRole);
        }

        // Federation
        private static bool IsInFederation(ITeamTunerUser user, Guid? federationId)
        {
            return Equals(user.FederationId, federationId);
        }

        private static bool IsFederationLeader(ITeamTunerUser user, Guid? federationId)
        {
            return IsInFederation(user, federationId) && Equals(CoreConstants.Auth.Roles.LEADER, user.FederationRole);
        }

        private static bool IsFederationCoLeader(ITeamTunerUser user, Guid? federationId)
        {
            return IsInFederation(user, federationId) && Equals(CoreConstants.Auth.Roles.CO_LEADER, user.FederationRole);
        }
    }
}