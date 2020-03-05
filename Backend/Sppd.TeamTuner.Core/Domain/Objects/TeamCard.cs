using System.Collections.Generic;

using Sppd.TeamTuner.Core.Domain.Entities;

namespace Sppd.TeamTuner.Core.Domain.Objects
{
    public class TeamCard : BaseCard
    {
        /// <summary>
        ///     Gets or sets the levels. Key=level (1-7); Value=list of users having card at this level.
        /// </summary>
        /// <value>
        ///     The levels.
        /// </value>
        public IDictionary<int, IEnumerable<TeamTunerUser>> Levels { get; set; }
    }
}