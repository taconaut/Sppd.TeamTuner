using Sppd.TeamTuner.Core.Domain.Entities;

namespace Sppd.TeamTuner.Core.Domain.Objects
{
    /// <summary>
    ///     Specifies the level of a card for a user.
    /// </summary>
    public class UserCard
    {
        /// <summary>
        ///     Gets or sets the card.
        /// </summary>
        /// <value>
        ///     The card.
        /// </value>
        public Card Card { get; set; }

        /// <summary>
        ///     Gets or sets the level.
        /// </summary>
        /// <value>
        ///     The level.
        /// </value>
        public CardLevel Level { get; set; }

        /// <summary>
        ///     Gets or sets the user.
        /// </summary>
        /// <value>
        ///     The user.
        /// </value>
        public TeamTunerUser User { get; set; }
    }
}