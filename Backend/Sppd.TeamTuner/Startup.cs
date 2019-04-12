using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using Sppd.TeamTuner.Authorization;
using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Config;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Exceptions;
using Sppd.TeamTuner.Core.Providers;
using Sppd.TeamTuner.Core.Services;
using Sppd.TeamTuner.Core.Utils.Extensions;
using Sppd.TeamTuner.Core.Utils.Helpers;
using Sppd.TeamTuner.Extensions;

using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;

namespace Sppd.TeamTuner
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;

        public Startup(ILogger<Startup> logger, ILoggerFactory loggerFactory)
        {
            _logger = logger;

            // Set the logger factory for future, static, usage
            LoggerHelper.LoggerFactory = loggerFactory;
        }

        /// <summary>
        ///     Add services to the container
        /// </summary>
        /// <param name="services">The service collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            _logger.LogDebug("Start configuring services");

            LoadApplicationAssemblies();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Use AutoMapper
            services.AddAutoMapper();

            RegisterServicesOnStartupRegistries(services);

            RegisterSwagger(services);
            RegisterAuthentication(services);

            // Support injecting any constructor parameter as lazy
            services.AddScoped(typeof(Lazy<>));

            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            _logger.LogInformation("Services have been configured");
        }

        /// <summary>
        ///     Configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The hosting environment.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            _logger.LogDebug("Start configuring application");

            var generalConfig = GetConfig<GeneralConfig>(app.ApplicationServices);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            if (generalConfig.EnableSwaggerUI)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1"); });
                _logger.LogInformation("SwaggerUI has been enabled");
            }

            app.UseGlobalExceptionLogger();
            app.UseErrorHandlingMiddleware();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc();

            ConfigureServicesOnStartupRegistries(app.ApplicationServices);

            _logger.LogInformation("Application is ready");
        }

        private static void RegisterSwagger(IServiceCollection services)
        {
            var generalConfig = GetConfig<GeneralConfig>(services.BuildServiceProvider());
            if (generalConfig.EnableSwaggerUI)
            {
                // Make SwaggerUI available
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new Info {Title = "Sppd.TeamTuner", Version = "v1"});
                    // Below two options are being set to allow specifying the bearer token in SwaggerUI. This allows to authenticate using SwaggerUI.
                    options.AddSecurityDefinition("oauth2", new ApiKeyScheme
                                                            {
                                                                Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                                                                In = "header",
                                                                Name = "Authorization",
                                                                Type = "apiKey"
                                                            });
                    options.OperationFilter<SecurityRequirementsOperationFilter>();
                });
            }
        }

        /// <summary>
        ///     Registers the authentication.
        /// </summary>
        /// <param name="services">The services.</param>
        private static void RegisterAuthentication(IServiceCollection services)
        {
            var authConfig = GetConfig<AuthConfig>(services.BuildServiceProvider());
            var key = Encoding.ASCII.GetBytes(authConfig.Secret);
            services.AddAuthentication(x =>
                    {
                        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(x =>
                    {
                        x.Events = new JwtBearerEvents
                                   {
                                       OnTokenValidated = context =>
                                       {
                                           var userService = context.HttpContext.RequestServices.GetRequiredService<ITeamTunerUserService>();
                                           var userId = Guid.Parse(context.Principal.Claims.Single(c => c.Type == AuthorizationConstants.ClaimTypes.USER_ID).Value);

                                           TeamTunerUser user;
                                           try
                                           {
                                               var userTask = userService.GetByIdAsync(userId);
                                               userTask.Wait();
                                               user = userTask.Result;
                                           }
                                           catch (Exception ex)
                                           {
                                               var logger = LoggerHelper.CreateLogger<JwtBearerEvents>();
                                               logger.LogError(ex, "Authorized user not found in DB. Access forbidden");
                                               throw new SecurityException("User not found");
                                           }

                                           // Prepare the user provider so it can provide the currently logged in user though DI
                                           var userProvider = context.HttpContext.RequestServices.GetRequiredService<ITeamTunerUserProvider>();
                                           userProvider.CurrentUser = user;

                                           return Task.CompletedTask;
                                       }
                                   };
                        x.RequireHttpsMetadata = false;
                        x.SaveToken = true;
                        x.TokenValidationParameters = new TokenValidationParameters
                                                      {
                                                          ValidateIssuerSigningKey = true,
                                                          IssuerSigningKey = new SymmetricSecurityKey(key),
                                                          ValidateIssuer = false,
                                                          ValidateAudience = false
                                                      };
                    });

            // The token provider is being used to set the bearer token in the HTTP authorization header when authenticating
            services.AddSingleton<ITokenProvider, JwtTokenProvider>();

            // Add resource based policies used to authorize in controller methods
            services.AddAuthorization(options => options.AddPolicies());
        }

        /// <summary>
        ///     Loads the application assemblies (Sppd.TeamTuner*.dll) found in the base directory.
        /// </summary>
        private void LoadApplicationAssemblies()
        {
            _logger.LogDebug("Start dynamic assembly loading");

            var directoryCatalog = new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory, $"{Core.CoreConstants.Application.APP_DLL_PREFIX}*.dll");
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).ToList();
            foreach (var assemblyFilePath in directoryCatalog.LoadedFiles)
            {
                var cleanAssemblyFilePath = FileHelper.GetCleanFilePath(assemblyFilePath);
                var isAssemblyRegistered = loadedAssemblies.Any(a => string.Equals(a.GetFilePath(), cleanAssemblyFilePath, StringComparison.InvariantCultureIgnoreCase));
                if (!isAssemblyRegistered)
                {
                    // Load the application assembly if it hasn't already been loaded
                    Assembly.Load(assemblyFilePath);
                    _logger.LogInformation($"Dynamically loaded assembly '{assemblyFilePath}'");
                }
            }

            _logger.LogInformation("Finished dynamically loading assemblies");
        }

        /// <summary>
        ///     Calls <see cref="IStartupRegistrator.Register" /> on all non-abstract implementations of
        ///     <see cref="IStartupRegistrator" />
        /// </summary>
        /// <param name="services">The service collection to register the services on.</param>
        private static void RegisterServicesOnStartupRegistries(IServiceCollection services)
        {
            var startupRegistratorInstances = GetInstances<IStartupRegistrator>();
            foreach (var startupRegistratorInstance in startupRegistratorInstances.OrderBy(s => s.Priority))
            {
                startupRegistratorInstance.Register(services);
            }
        }

        /// <summary>
        ///     Calls <see cref="IStartupRegistrator.Configure" /> on all non-abstract implementations of
        ///     <see cref="IStartupRegistrator" />
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        private static void ConfigureServicesOnStartupRegistries(IServiceProvider serviceProvider)
        {
            var startupRegistratorInstances = GetInstances<IStartupRegistrator>();
            foreach (var startupRegistratorInstance in startupRegistratorInstances.OrderBy(s => s.Priority))
            {
                startupRegistratorInstance.Configure(serviceProvider);
            }
        }

        /// <summary>
        ///     Gets an instance of every concrete type, specified by <c>T</c>.
        /// </summary>
        /// <typeparam name="T">Type to instantiate</typeparam>
        /// <returns>A list of instances for all concrete implementations of type <c>T</c></returns>
        private static IEnumerable<T> GetInstances<T>()
        {
            var serviceRegistryType = typeof(T);
            var typesToInstantiate = AppDomain.CurrentDomain.GetAssemblies()
                                              .SelectMany(s => s.GetTypes())
                                              .Where(p => p.IsClass && !p.IsAbstract && serviceRegistryType.IsAssignableFrom(p));

            foreach (var typeToInstantiate in typesToInstantiate)
            {
                yield return (T) Activator.CreateInstance(typeToInstantiate);
            }
        }

        /// <summary>
        ///     Gets the general configuration.
        /// </summary>
        private static TConfig GetConfig<TConfig>(IServiceProvider serviceProvider)
            where TConfig : class, IConfig, new()
        {
            return serviceProvider.GetService<IConfigProvider<TConfig>>().Config;
        }
    }
}