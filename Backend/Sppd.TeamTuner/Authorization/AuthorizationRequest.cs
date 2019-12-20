using System;
using System.Security.Claims;

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
    ///         cref="IAuthorizationService.AuthorizeAsync(ClaimsPrincipal,object,System.Collections.Generic.IEnumerable{IAuthorizationRequirement})" />
    ///     uses this object as a resource.
    /// </summary>
    public class AuthorizationRequest<TResource>
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
        public TResource Resource { get; }

        public AuthorizationRequest(IServiceProvider serviceProvider, TResource resource)
        {
            ServiceProvider = serviceProvider;
            CurrentUser = serviceProvider.GetService<ITeamTunerUserProvider>().CurrentUser;
            RepositoryResolver = serviceProvider.GetService<IRepositoryResolver>();
            Resource = resource;
        }
    }
}