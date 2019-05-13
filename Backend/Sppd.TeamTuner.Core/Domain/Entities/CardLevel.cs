using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Sppd.TeamTuner.Core.Domain.Validation;
using Sppd.TeamTuner.Core.Validation;

namespace Sppd.TeamTuner.Core.Domain.Entities
{
    /// <summary>
    ///     Specify the level of a card for a user.
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public class CardLevel : BaseEntity
    {
        public Card Card { get; set; }

        public Guid CardId { get; set; }

        public TeamTunerUser User { get; set; }

        public Guid UserId { get; set; }

        [Required, Range(1, 7)]
        public int Level { get; set; }

        public override IEnumerable<EntityValidationError> Validate(IValidationContext context)
        {
            if (Level < 1 || Level > 7)
            {
                yield return new EntityValidationError("Must be greater than 0 and smaller than 8", nameof(Level));
            }
        }
    }
}