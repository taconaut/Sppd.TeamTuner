using System.Collections.Generic;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Core.Services;

namespace Sppd.TeamTuner.Infrastructure.Services
{
    internal class CoreDataService : ICoreDataService
    {
        private readonly IRepository<CardType> _cardTypeRepository;
        private readonly IRepository<CharacterType> _characterTypeRepository;
        private readonly IRepository<Rarity> _rarityRepository;
        private readonly IRepository<Theme> _themeRepository;

        public CoreDataService(IRepository<CardType> cardTypeRepository, IRepository<CharacterType> characterTypeRepository, IRepository<Rarity> rarityRepository, IRepository<Theme> themeRepository)
        {
            _cardTypeRepository = cardTypeRepository;
            _characterTypeRepository = characterTypeRepository;
            _rarityRepository = rarityRepository;
            _themeRepository = themeRepository;
        }

        public async Task<IEnumerable<CardType>> GetCardTypesAsync()
        {
            return await _cardTypeRepository.GetAllAsync();
        }

        public async Task<IEnumerable<CharacterType>> GetCharacterTypesAsync()
        {
            return await _characterTypeRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Rarity>> GetRaritiesAsync()
        {
            return await _rarityRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Theme>> GetThemesAsync()
        {
            return await _themeRepository.GetAllAsync();
        }
    }
}