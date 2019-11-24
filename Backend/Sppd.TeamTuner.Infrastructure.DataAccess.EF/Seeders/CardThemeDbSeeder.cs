using System;
using System.Threading.Tasks;

using Sppd.TeamTuner.Common;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Infrastructure.DataAccess.EF.Config;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Seeders
{
    internal class CardThemeDbSeeder : IDbSeeder
    {
        private readonly IRepository<Theme> _cardThemeRepository;

        public CardThemeDbSeeder(IRepository<Theme> cardThemeRepository)
        {
            _cardThemeRepository = cardThemeRepository;
        }

        public int Priority => SeederConstants.Priority.BASE_DATA;

        public Task SeedAsync(SeedMode seedMode)
        {
            if (seedMode == SeedMode.None)
            {
                return Task.CompletedTask;
            }

            _cardThemeRepository.Add(new Theme
                                     {
                                         Id = new Guid(CoreDataConstants.Theme.ADVENTURE_ID),
                                         Name = "Adventure"
                                     });
            _cardThemeRepository.Add(new Theme
                                     {
                                         Id = new Guid(CoreDataConstants.Theme.SCIFI_ID),
                                         Name = "Sci-Fy"
                                     });
            _cardThemeRepository.Add(new Theme
                                     {
                                         Id = new Guid(CoreDataConstants.Theme.FANTASY_ID),
                                         Name = "Fantasy"
                                     });
            _cardThemeRepository.Add(new Theme
                                     {
                                         Id = new Guid(CoreDataConstants.Theme.MYSTICAL_ID),
                                         Name = "Mystical"
                                     });
            _cardThemeRepository.Add(new Theme
                                     {
                                         Id = new Guid(CoreDataConstants.Theme.NEUTRAL_ID),
                                         Name = "Neutral"
                                     });
            _cardThemeRepository.Add(new Theme
                                     {
                                         Id = new Guid(CoreDataConstants.Theme.SUPERHERO_ID),
                                         Name = "Superhero"
                                     });

            return Task.CompletedTask;
        }
    }
}