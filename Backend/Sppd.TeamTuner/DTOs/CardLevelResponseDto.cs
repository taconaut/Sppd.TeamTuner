using System;
using System.ComponentModel.DataAnnotations;

namespace Sppd.TeamTuner.DTOs
{
    public class CardLevelResponseDto
    {
        /// <summary>
        ///     The card level identifier
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        ///     The user identifier
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        ///     The card identifier
        /// </summary>
        [Required]
        public Guid CardId { get; set; }

        /// <summary>
        ///     The level of the card for the user
        /// </summary>
        [Required, Range(1, 7)]
        public int Level { get; set; }

        /// <summary>
        ///     When the level has been last modified.
        /// </summary>
        public DateTime? LevelLastModified { get; set; }
    }
}