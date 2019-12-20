using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Sppd.TeamTuner.Authorization;

namespace Sppd.TeamTuner.Controllers
{
    /// <summary>
    ///     Contains functionality for authorization. Extend this class for controllers having authorization needs.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    public abstract class AuthorizationController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAuthorizationService _authorizationService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AuthorizationController" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="authorizationService">The authorization service.</param>
        protected AuthorizationController(IServiceProvider serviceProvider, IAuthorizationService authorizationService)
        {
            _serviceProvider = serviceProvider;
            _authorizationService = authorizationService;
        }

        /// <summary>
        ///     Authorizes using the specified policy and parameter.
        /// </summary>
        /// <param name="policyName">Name of the policy.</param>
        /// <param name="parameter">The parameter.</param>
        protected async Task<AuthorizationResult> AuthorizeAsync<TResource>(string policyName, TResource parameter)
        {
            return await _authorizationService.AuthorizeAsync(User, GetAuthorizationRequest(parameter), policyName);
        }

        private AuthorizationRequest<TResource> GetAuthorizationRequest<TResource>(TResource parameter)
        {
            return new AuthorizationRequest<TResource>(_serviceProvider, parameter);
        }
    }
}