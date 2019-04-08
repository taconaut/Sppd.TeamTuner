using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sppd.TeamTuner.Core.Domain.Validation;

namespace Sppd.TeamTuner.Core.Exceptions
{
    /// <summary>
    ///     Exception thrown when entity validation has failed.
    /// </summary>
    /// <seealso cref="BusinessException" />
    [Serializable]
    public class EntityValidationException : BusinessException
    {
        private IReadOnlyCollection<EntityValidationResult> _validationResults;

        public IReadOnlyCollection<EntityValidationResult> ValidationResults => _validationResults ?? (_validationResults = new List<EntityValidationResult>());

        public override string Message
        {
            get
            {
                if (ValidationResults.All(vr => vr.IsValid))
                {
                    return base.Message;
                }

                var message = new StringBuilder($"Entity validation errors occured:{Environment.NewLine}");
                var invalidResults = ValidationResults.Where(vr => !vr.IsValid).ToList();
                for (var i = 0; i < invalidResults.Count; i++)
                {
                    var postFix = i == invalidResults.Count - 1 ? string.Empty : Environment.NewLine;
                    message.Append($"{invalidResults[i]}{postFix}");
                }

                return message.ToString();
            }
        }

        public string DisplayMessage => base.Message;

        public EntityValidationException(IReadOnlyCollection<EntityValidationResult> validationValidationResults)
        {
            _validationResults = validationValidationResults;
        }
    }
}