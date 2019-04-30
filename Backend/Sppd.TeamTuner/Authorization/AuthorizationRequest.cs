using System.Collections.Generic;

using Microsoft.AspNetCore.Authorization;

using Sppd.TeamTuner.Core.Domain.Interfaces;

namespace Sppd.TeamTuner.Authorization
{
    /// <summary>
    ///     Used for every call to
    ///     <see
    ///         cref="IAuthorizationService.AuthorizeAsync(System.Security.Claims.ClaimsPrincipal,object,System.Collections.Generic.IEnumerable{Microsoft.AspNetCore.Authorization.IAuthorizationRequirement})" />
    ///     uses this object as a resource.
    /// </summary>
    public class AuthorizationRequest
    {
        /// <summary>
        ///     Gets the current user.
        /// </summary>
        public ITeamTunerUser CurrentUser { get; }

        /// <summary>
        ///     Gets the resource.
        /// </summary>
        public object Resource { get; }

        public AuthorizationRequest(ITeamTunerUser currentUser, object resource)
        {
            CurrentUser = currentUser;
            Resource = resource;
        }
    }
}