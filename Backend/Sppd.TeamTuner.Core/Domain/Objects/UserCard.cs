﻿using Sppd.TeamTuner.Core.Domain.Entities;

namespace Sppd.TeamTuner.Core.Domain.Objects
{
    /// <summary>
    ///     Specifies the level of a card for a user.
    /// </summary>
    public class UserCard : BaseCard
    {
        /// <summary>
        ///     Gets or sets the level.
        /// </summary>
        /// <value>
        ///     The level.
        /// </value>
        public CardLevel Level { get; set; }
    }
}