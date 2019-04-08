using System;

namespace Sppd.TeamTuner.Core.Domain.Entities
{
    /// <summary>
    ///     Specify the level of a card for a user.
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public class CardLevel : BaseEntity
    {
        public Card Card { get; set; }

        public Guid CardId { get; set; }

        public TeamTunerUser User { get; set; }

        public Guid UserId { get; set; }

        public int Level { get; set; }
    }
}