using System;
using System.ComponentModel.DataAnnotations;

using Sppd.TeamTuner.Core;

namespace Sppd.TeamTuner.DTOs
{
    public class CardResponseDto : NamedDto
    {
        /// <summary>
        ///     The card description.
        /// </summary>
        [StringLength(CoreConstants.StringLength.Card.DESCRIPTION)]
        public string Description { get; set; }

        [Required]
        public int ManaCost { get; set; }

        /// <summary>
        ///     The external identifier
        /// </summary>
        /// <remarks>For https://sppd.feinwaru.com/ </remarks>
        [StringLength(CoreConstants.StringLength.Card.EXTERNAL_ID)]
        public string ExternalId { get; set; }

        /// <summary>
        ///     The theme identifier
        /// </summary>
        [Required]
        public Guid ThemeId { get; set; }

        /// <summary>
        ///     The rarity identifier.
        /// </summary>
        [Required]
        public Guid RarityId { get; set; }

        /// <summary>
        ///     The type identifier.
        /// </summary>
        [Required]
        public Guid TypeId { get; set; }

        /// <summary>
        ///     The character type identifier.
        /// </summary>
        public Guid? CharacterTypeId { get; set; }
    }
}