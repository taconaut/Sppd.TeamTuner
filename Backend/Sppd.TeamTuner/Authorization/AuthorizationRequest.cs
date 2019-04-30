using System;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

using Sppd.TeamTuner.Core.Domain.Interfaces;
using Sppd.TeamTuner.Core.Providers;
using Sppd.TeamTuner.Core.Repositories;

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
        ///     The service provider which can be used to resolve any required service while authorizing.
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        ///     Gets the current user.
        /// </summary>
        public ITeamTunerUser CurrentUser { get; }

        /// <summary>
        ///     Gets the repository resolver.
        /// </summary>
        public IRepositoryResolver RepositoryResolver { get; }

        /// <summary>
        ///     Gets the resource.
        /// </summary>
        public object Resource { get; }

        public AuthorizationRequest(IServiceProvider serviceProvider, object resource)
        {
            ServiceProvider = serviceProvider;
            CurrentUser = serviceProvider.GetService<ITeamTunerUserProvider>().CurrentUser;
            RepositoryResolver = serviceProvider.GetService<IRepositoryResolver>();
            Resource = resource;
        }
    }
}