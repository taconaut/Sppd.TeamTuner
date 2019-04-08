using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sppd.TeamTuner.Core.Domain.Entities;

namespace Sppd.TeamTuner.Core.Domain.Validation
{
    /// <summary>
    ///     Validation of an entire entity; containing the entity along with all validation errors
    /// </summary>
    public class EntityValidationResult
    {
        private IReadOnlyCollection<EntityValidationError> _validationErrors;

        public BaseEntity Entity { get; set; }

        public bool IsValid => ValidationErrors.Count == 0;

        public IReadOnlyCollection<EntityValidationError> ValidationErrors => _validationErrors ?? (_validationErrors = new List<EntityValidationError>());

        public EntityValidationResult(BaseEntity entity, IReadOnlyCollection<EntityValidationError> validationErrors)
        {
            Entity = entity;
            _validationErrors = validationErrors;
        }

        public override string ToString()
        {
            if (IsValid)
            {
                return base.ToString();
            }

            var entityType = Entity?.GetType().Name ?? "NULL";
            var entityId = Entity?.Id.ToString();

            var message = new StringBuilder($"Entity of type='{entityType}' with Id='{entityId}' is not valid:{Environment.NewLine}");
            for (var i = 0; i < ValidationErrors.Count; i++)
            {
                var postFix = i == ValidationErrors.Count - 1 ? string.Empty : Environment.NewLine;
                message.Append($"{ValidationErrors.ElementAt(i)}{postFix}");
            }

            return message.ToString();
        }
    }
}