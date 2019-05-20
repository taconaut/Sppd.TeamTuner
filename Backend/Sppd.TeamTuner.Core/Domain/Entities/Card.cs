using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Sppd.TeamTuner.Core.Domain.Validation;
using Sppd.TeamTuner.Core.Validation;

namespace Sppd.TeamTuner.Core.Domain.Entities
{
    /// <summary>
    ///     A card.
    /// </summary>
    /// <seealso cref="NamedEntity" />
    public class Card : NamedEntity
    {
        [StringLength(CoreConstants.StringLength.Card.EXTERNAL_ID)]
        public string ExternalId { get; set; }

        [StringLength(CoreConstants.StringLength.Card.DESCRIPTION)]
        public string Description { get; set; }

        public int ManaCost { get; set; }

        public Theme Theme { get; set; }

        public Guid ThemeId { get; set; }

        public Rarity Rarity { get; set; }

        public Guid RarityId { get; set; }

        public CardType Type { get; set; }

        public Guid TypeId { get; set; }

        public override IEnumerable<EntityValidationError> Validate(IValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return new EntityValidationError("Must be set", nameof(Name));
            }

            if (ThemeId == default)
            {
                yield return new EntityValidationError("Must be set", nameof(Theme));
            }

            if (RarityId == default)
            {
                yield return new EntityValidationError("Must be set", nameof(Rarity));
            }

            if (TypeId == default)
            {
                yield return new EntityValidationError("Must be set", nameof(Type));
            }

            if (ManaCost > 10 || ManaCost < 1)
            {
                yield return new EntityValidationError("Must be between 1-10", nameof(ManaCost));
            }
        }
    }
}