using Sppd.TeamTuner.Core.Domain.Validation;

namespace Sppd.TeamTuner.Core.Services
{
    /// <summary>
    ///     Offers validation functionality
    /// </summary>
    public interface IValidationService
    {
        /// <summary>
        ///     Validates all changed entities.
        /// </summary>
        /// <returns>A result collection containing all validation errors (if any).</returns>
        EntityValidationResultCollection ValidateAllChangedEntities();
    }
}