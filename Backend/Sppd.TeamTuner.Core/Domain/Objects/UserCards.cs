using System.Collections.Generic;

using Sppd.TeamTuner.Core.Domain.Entities;

namespace Sppd.TeamTuner.Core.Domain.Objects
{
    public class UserCards
    {
        /// <summary>
        ///     Gets or sets the user.
        /// </summary>
        /// <value>
        ///     The user.
        /// </value>
        public TeamTunerUser User { get; set; }

        /// <summary>
        ///     Gets or sets the cards.
        /// </summary>
        /// <value>
        ///     The cards.
        /// </value>
        public IEnumerable<UserCard> Cards { get; set; }
    }
}