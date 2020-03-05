using System;
using System.ComponentModel.DataAnnotations;

using Sppd.TeamTuner.Core;

namespace Sppd.TeamTuner.DTOs
{
    /// <summary>
    ///     Base class holding card information.
    /// </summary>
    public abstract class CardResponseBaseDto
    {
        /// <summary>
        ///     The card identifier
        /// </summary>
        [Required]
        public Guid CardId { get; set; }

        /// <summary>
        ///     The name of the card
        /// </summary>
        [Required, StringLength(CoreConstants.StringLength.Named.NAME_MAX, MinimumLength = CoreConstants.StringLength.Named.NAME_MIN)]
        public string CardName { get; set; }

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