using System.Collections.Generic;

using Sppd.TeamTuner.Core.Domain.Entities;

namespace Sppd.TeamTuner.Core.Domain.Objects
{
    public class TeamCards
    {
        /// <summary>
        ///     Gets or sets the team.
        /// </summary>
        /// <value>
        ///     The team.
        /// </value>
        public Team Team { get; set; }

        /// <summary>
        ///     Gets or sets the card.
        /// </summary>
        /// <value>
        ///     The card.
        /// </value>
        public IEnumerable<TeamCard> Cards { get; set; }
    }
}