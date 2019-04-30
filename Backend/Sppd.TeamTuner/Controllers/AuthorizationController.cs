using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Sppd.TeamTuner.Authorization;
using Sppd.TeamTuner.Core.Domain.Interfaces;
using Sppd.TeamTuner.Core.Providers;

namespace Sppd.TeamTuner.Controllers
{
    /// <summary>
    ///     Contains functionality for authorization. Extend this class for controllers having authorization needs.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    public abstract class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly Lazy<ITeamTunerUser> _currentUser;

        protected AuthorizationController(ITeamTunerUserProvider userProvider, IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
            _currentUser = new Lazy<ITeamTunerUser>(() => userProvider.CurrentUser);
        }

        /// <summary>
        ///     Authorizes using the specified policy and parameter.
        /// </summary>
        /// <param name="policyName">Name of the policy.</param>
        /// <param name="parameter">The parameter.</param>
        protected async Task<AuthorizationResult> AuthorizeAsync(string policyName, object parameter)
        {
            return await _authorizationService.AuthorizeAsync(User, GetAuthorizationRequest(parameter), policyName);
        }

        private AuthorizationRequest GetAuthorizationRequest(object parameter)
        {
            return new AuthorizationRequest(_currentUser.Value, parameter);
        }
    }
}