using System.Collections.Generic;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core.Domain.Entities;

namespace Sppd.TeamTuner.Core.Services
{
    /// <summary>
    ///     Services for <see cref="CoreData" /> handling.
    /// </summary>
    public interface ICoreDataService
    {
        /// <summary>
        ///     Gets all card types (Spell, Fighter...)
        /// </summary>
        Task<IEnumerable<CardType>> GetCardTypesAsync();

        /// <summary>
        ///     Gets all rarities (Common, Rare...)
        /// </summary>
        Task<IEnumerable<Rarity>> GetRaritiesAsync();

        /// <summary>
        ///     Gets all themes (Sci-fy, Fantasy...)
        /// </summary>
        Task<IEnumerable<Theme>> GetThemesAsync();
    }
}