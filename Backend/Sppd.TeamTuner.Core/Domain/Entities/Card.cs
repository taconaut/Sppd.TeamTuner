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
        [Required, StringLength(CoreConstants.StringLength.Card.FRIENDLY_NAME)]
        public string FriendlyName { get; set; }

        public int ExternalId { get; set; }

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

            if (string.IsNullOrWhiteSpace(FriendlyName))
            {
                yield return new EntityValidationError("Must be set", nameof(FriendlyName));
            }

            if (ExternalId <= 0)
            {
                yield return new EntityValidationError("Must be greater than 0", nameof(ExternalId));
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
        }
    }
}