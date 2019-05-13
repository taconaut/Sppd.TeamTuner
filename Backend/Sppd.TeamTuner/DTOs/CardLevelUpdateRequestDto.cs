using System;
using System.ComponentModel.DataAnnotations;

namespace Sppd.TeamTuner.DTOs
{
    public class CardLevelUpdateRequestDto
    {
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
        ///     The level of the card for the user (1-7).
        /// </summary>
        [Required, Range(1, 7)]
        public int Level { get; set; }
    }
}