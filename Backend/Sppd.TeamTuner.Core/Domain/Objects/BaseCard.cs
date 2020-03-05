using Sppd.TeamTuner.Core.Domain.Entities;

namespace Sppd.TeamTuner.Core.Domain.Objects
{
    /// <summary>
    ///     Base class holding a card.
    /// </summary>
    public abstract class BaseCard
    {
        /// <summary>
        ///     Gets or sets the card.
        /// </summary>
        /// <value>
        ///     The card.
        /// </value>
        public Card Card { get; set; }
    }
}