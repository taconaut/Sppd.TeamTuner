using System;
using System.ComponentModel.DataAnnotations;

namespace Sppd.TeamTuner.DTOs
{
    /// <summary>
    ///     Contains information about a card on user level.
    /// </summary>
    /// <seealso cref="CardResponseBaseDto" />
    public class UserCardResponseDto : CardResponseBaseDto
    {
        /// <summary>
        ///     The level (NULL if not set)
        /// </summary>
        [Range(1, 7)]
        public int? Level { get; set; }

        /// <summary>
        ///     When the level has been last modified.
        /// </summary>
        public DateTime? LevelLastModified { get; set; }
    }
}