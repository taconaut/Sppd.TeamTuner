using System;

using Sppd.TeamTuner.Core.Validation;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Validation
{
    public class ValidationContext : IValidationContext
    {
        public ValidationContext(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }
    }
}