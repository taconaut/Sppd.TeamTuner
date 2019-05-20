using System;
using System.ComponentModel.DataAnnotations;

using Sppd.TeamTuner.Core;

namespace Sppd.TeamTuner.DTOs
{
    public class UserCardResponseDto
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
        ///     The user identifier
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

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
        ///     The level (NULL if not set)
        /// </summary>
        [Range(1, 7)]
        public int? Level { get; set; }
    }
}