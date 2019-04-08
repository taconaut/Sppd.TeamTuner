using System;
using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;

namespace Sppd.TeamTuner.Authorization
{
    internal static class PoliciesBuilder
    {
        /// <summary>
        ///     Adds the policies required for authorization to <see cref="options" />.
        /// </summary>
        /// <param name="options">The authorization options.</param>
        public static void AddPolicies(this AuthorizationOptions options)
        {
            options.AddPolicy(AuthorizationConstants.Policies.IS_ADMIN,
                policy => policy.RequireAssertion(ctx =>
                {
                    var federationId = ctx.Resource as Guid?;
                    return IsAdmin(ctx.User);
                }));

            options.AddPolicy(AuthorizationConstants.Policies.IS_OWNER,
                policy => policy.RequireAssertion(ctx =>
                {
                    var userId = ctx.Resource as Guid?;
                    return IsAdmin(ctx.User) || IsOwner(ctx.User, userId);
                }));

            options.AddPolicy(AuthorizationConstants.Policies.IS_IN_TEAM,
                policy => policy.RequireAssertion(ctx =>
                {
                    var teamId = ctx.Resource as Guid?;
                    return IsAdmin(ctx.User) || IsInTeam(ctx.User, teamId);
                }));

            options.AddPolicy(AuthorizationConstants.Policies.IS_IN_FEDERATION,
                policy => policy.RequireAssertion(ctx =>
                {
                    var federationId = ctx.Resource as Guid?;
                    return IsAdmin(ctx.User) || IsInFederation(ctx.User, federationId);
                }));
        }

        private static bool IsOwner(ClaimsPrincipal user, Guid? userId)
        {
            return user.HasClaim(claim => Equals(claim.Type, AuthorizationConstants.ClaimTypes.USER_ID) && Equals(claim.Value, userId?.ToString()));
        }

        private static bool IsAdmin(ClaimsPrincipal user)
        {
            return user.HasClaim(claim => Equals(claim.Type, AuthorizationConstants.ClaimTypes.USER_ID) && Equals(claim.Value, Core.CoreConstants.Auth.Roles.ADMIN));
        }

        private static bool IsInTeam(ClaimsPrincipal user, Guid? teamId)
        {
            return user.HasClaim(claim => Equals(claim.Type, AuthorizationConstants.ClaimTypes.TEAM_ID) && Equals(claim.Value, teamId?.ToString()));
        }

        private static bool IsInFederation(ClaimsPrincipal user, Guid? federationId)
        {
            return user.HasClaim(claim => Equals(claim.Type, AuthorizationConstants.ClaimTypes.FEDERATION_ID) && Equals(claim.Value, federationId?.ToString()));
        }
    }
}