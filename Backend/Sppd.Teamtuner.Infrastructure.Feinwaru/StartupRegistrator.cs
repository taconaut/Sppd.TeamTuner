using System;

using Microsoft.Extensions.DependencyInjection;

using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Services;
using Sppd.TeamTuner.Infrastructure.Feinwaru.Services;

namespace Sppd.TeamTuner.Infrastructure.Feinwaru
{
    public class StartupRegistrator : IStartupRegistrator
    {
        public int Priority => 150;

        public void Register(IServiceCollection services)
        {
            services.AddScoped<ICardImportService, CardImportService>();
        }

        public void Configure(IServiceProvider serviceProvider)
        {
        }
    }
}