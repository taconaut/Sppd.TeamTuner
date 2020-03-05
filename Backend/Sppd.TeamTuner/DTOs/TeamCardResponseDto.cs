using System.Collections.Generic;

namespace Sppd.TeamTuner.DTOs
{
    /// <summary>
    ///     Contains information about a card on team level.
    /// </summary>
    /// <seealso cref="CardResponseBaseDto" />
    public class TeamCardResponseDto : CardResponseBaseDto
    {
        /// <summary>
        ///     Gets or sets the levels. Key=level (1-7); Value=number of members having the card at this level.
        /// </summary>
        /// <value>
        ///     The levels.
        /// </value>
        public IDictionary<int, int> Levels { get; set; }
    }
}