using System;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Common;

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

        public Task SeedAsync()
        {
            _cardThemeRepository.Add(new Theme
                                     {
                                         Id = new Guid(TestingConstants.Theme.ADVENTURE_ID),
                                         Name = "Adventure"
                                     });
            _cardThemeRepository.Add(new Theme
                                     {
                                         Id = new Guid(TestingConstants.Theme.SCIFI_ID),
                                         Name = "Sci-Fy"
                                     });
            _cardThemeRepository.Add(new Theme
                                     {
                                         Id = new Guid(TestingConstants.Theme.FANTASY_ID),
                                         Name = "Fantasy"
                                     });
            _cardThemeRepository.Add(new Theme
                                     {
                                         Id = new Guid(TestingConstants.Theme.MYSTICAL_ID),
                                         Name = "Mystical"
                                     });
            _cardThemeRepository.Add(new Theme
                                     {
                                         Id = new Guid(TestingConstants.Theme.NEUTRAL_ID),
                                         Name = "Neutral"
                                     });

            return Task.CompletedTask;
        }
    }
}