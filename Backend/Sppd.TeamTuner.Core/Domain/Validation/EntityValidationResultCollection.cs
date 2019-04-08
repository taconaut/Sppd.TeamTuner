using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Sppd.TeamTuner.Core.Exceptions;

namespace Sppd.TeamTuner.Core.Domain.Validation
{
    /// <summary>
    ///     Holds a set of <see cref="EntityValidationResult" />
    /// </summary>
    /// <seealso cref="IEnumerable" />
    public class EntityValidationResultCollection : IEnumerable
    {
        private readonly IReadOnlyCollection<EntityValidationResult> _validationResults;

        /// <summary>
        ///     The number of errors
        /// </summary>
        public int Count => _validationResults.Count;

        public EntityValidationResultCollection(IReadOnlyCollection<EntityValidationResult> validationResults)
        {
            _validationResults = validationResults;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Throws an <see cref="EntityValidationException" /> if any error is contained.
        /// </summary>
        /// <exception cref="EntityValidationException">Thrown when at least one error is part of the collection.</exception>
        public void ThrowIfHasInvalid()
        {
            var invalidValidationResults = _validationResults.Where(vr => !vr.IsValid).ToArray();
            if (invalidValidationResults.Length != 0)
            {
                throw new EntityValidationException(new ReadOnlyCollection<EntityValidationResult>(invalidValidationResults));
            }
        }

        public IEnumerator<EntityValidationResult> GetEnumerator()
        {
            return _validationResults.GetEnumerator();
        }
    }
}