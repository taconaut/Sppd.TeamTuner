using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Hangfire;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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
            ConfigureServicesOnStartupRegistrators(services);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = _ => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Use AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            RegisterSwagger(services);
            RegisterAuthentication(services);

            // Add resource based policies used to authorize in controller methods
            services.AddAuthorization(options => options.AddPolicies());

            // Support injecting any constructor parameter as lazy
            services.AddScoped(typeof(Lazy<>));

            services.AddMvc(options => options.EnableEndpointRouting = false);

            _logger.LogInformation("Services have been configured");
        }

        /// <summary>
        ///     Configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The hosting environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            _logger.LogDebug("Start configuring application");

            var generalConfig = app.ApplicationServices.GetConfig<GeneralConfig>();

            ConfigureOnStartupRegistrators(app.ApplicationServices);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Allow CORS
            // TODO: Check if this is really what we want
            app.UseCors(builder => builder.AllowAnyOrigin()
                                          .AllowAnyHeader()
                                          .AllowAnyMethod());

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

            if (generalConfig.EnableHangfire)
            {
                app.UseHangfireDashboard();
            }

            app.UseMvc();

            _logger.LogInformation("Application is ready");
        }

        private static void RegisterSwagger(IServiceCollection services)
        {
            var generalConfig = services.BuildServiceProvider().GetConfig<GeneralConfig>();
            if (generalConfig.EnableSwaggerUI)
            {
                // Make SwaggerUI available
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Sppd.TeamTuner", Version = "v1" });

                    // Set the path to the XML file containing comments for the Swagger JSON and UI.
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    options.IncludeXmlComments(xmlPath);

                    // Below two options are being set to allow specifying the bearer token in SwaggerUI. This allows to authenticate using SwaggerUI.
                    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                                                            {
                                                                Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                                                                In = ParameterLocation.Header,
                                                                Name = "Authorization",
                                                                Type = SecuritySchemeType.ApiKey
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
            var authConfig = services.BuildServiceProvider().GetConfig<AuthConfig>();
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
                                           var userService = context.HttpContext.RequestServices.GetService<ITeamTunerUserService>();
                                           var userId = Guid.Parse(context.Principal!.Claims.Single(c => c.Type == ClaimTypes.Name).Value);

                                           TeamTunerUser user;
                                           try
                                           {
                                               var userTask = userService!.GetByIdAsync(userId, new[]
                                                                                                {
                                                                                                    // Include all properties required to authorize access
                                                                                                    nameof(TeamTunerUser.Team),
                                                                                                    string.Join(".", nameof(TeamTunerUser.Federation), nameof(Federation.Teams))
                                                                                                });
                                               userTask.Wait();
                                               user = userTask.Result;

                                               if (!user.IsEmailVerified)
                                               {
                                                   throw new SecurityException("Email not verified");
                                               }
                                           }
                                           catch (SecurityException)
                                           {
                                               throw;
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
        }

        /// <summary>
        ///     Loads the application assemblies (Sppd.TeamTuner*.dll) found in the base directory.
        /// </summary>
        private void LoadApplicationAssemblies()
        {
            _logger.LogDebug("Start dynamic assembly loading");

            using (var directoryCatalog = new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory, $"{CoreConstants.Application.APP_DLL_PREFIX}*.dll"))
            {
                var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).ToList();
                foreach (var assemblyFilePath in directoryCatalog.LoadedFiles)
                {
                    var isAssemblyRegistered = loadedAssemblies.Any(a => string.Equals(a.GetFilePath(), assemblyFilePath, StringComparison.InvariantCultureIgnoreCase));
                    if (!isAssemblyRegistered)
                    {
                        try
                        {
                            // Load the application assembly if it hasn't already been loaded
                            Assembly.Load(assemblyFilePath);
                            _logger.LogInformation($"Dynamically loaded assembly '{assemblyFilePath}'");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Failed to load assembly '{assemblyFilePath}' ");
                        }
                    }
                }
            }

            _logger.LogInformation("Finished dynamically loading assemblies");
        }

        /// <summary>
        ///     Calls <see cref="IStartupRegistrator.ConfigureServices" /> on all non-abstract implementations of
        ///     <see cref="IStartupRegistrator" />
        /// </summary>
        /// <param name="services">The service collection to register the services on.</param>
        private static void ConfigureServicesOnStartupRegistrators(IServiceCollection services)
        {
            var startupRegistratorInstances = GetInstances<IStartupRegistrator>();
            foreach (var startupRegistratorInstance in startupRegistratorInstances.OrderBy(s => s.Priority))
            {
                startupRegistratorInstance.ConfigureServices(services);
            }
        }

        /// <summary>
        ///     Calls <see cref="IStartupRegistrator.Configure" /> on all non-abstract implementations of
        ///     <see cref="IStartupRegistrator" />
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        private static void ConfigureOnStartupRegistrators(IServiceProvider serviceProvider)
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
                yield return (T)Activator.CreateInstance(typeToInstantiate);
            }
        }
    }
}