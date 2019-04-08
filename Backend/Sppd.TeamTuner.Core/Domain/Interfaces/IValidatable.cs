using System.Collections.Generic;

using Sppd.TeamTuner.Core.Domain.Validation;
using Sppd.TeamTuner.Core.Validation;

namespace Sppd.TeamTuner.Core.Domain.Interfaces
{
    /// <summary>
    ///     Objects implementing this interface can be validated
    /// </summary>
    public interface IValidatable
    {
        /// <summary>
        ///     Validates.
        /// </summary>
        /// <param name="context">The context (containing a repository resolver).</param>
        /// <returns>
        ///     Encountered <see cref="EntityValidationError" />s if any; otherwise and empty
        ///     <see cref=" IEnumerable{EntityValidationError} " />
        /// </returns>
        IEnumerable<EntityValidationError> Validate(IValidationContext context);
    }
}