using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sppd.TeamTuner.DTOs
{
    /// <summary>
    ///     Response for team cards.
    /// </summary>
    public class TeamCardsResponseDto
    {
        /// <summary>
        ///     Gets or sets the team identifier.
        /// </summary>
        /// <value>
        ///     The team identifier.
        /// </value>
        [Required]
        public Guid TeamId { get; set; }

        /// <summary>
        ///     Gets or sets the cards.
        /// </summary>
        /// <value>
        ///     The cards.
        /// </value>
        [Required]
        public IEnumerable<TeamCardResponseDto> Cards { get; set; }
    }
}