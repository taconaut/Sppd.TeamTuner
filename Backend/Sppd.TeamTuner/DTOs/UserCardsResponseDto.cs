using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sppd.TeamTuner.DTOs
{
    /// <summary>
    ///     Response for user cards request.
    /// </summary>
    public class UserCardsResponseDto
    {
        /// <summary>
        ///     The user identifier
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        ///     Gets or sets the cards.
        /// </summary>
        /// <value>
        ///     The cards.
        /// </value>
        [Required]
        public IEnumerable<UserCardResponseDto> Cards { get; set; }
    }
}