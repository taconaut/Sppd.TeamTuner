using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Core.Services;

namespace Sppd.TeamTuner.Infrastructure.Services
{
    internal class CardService : ServiceBase<Card>, ICardService
    {
        private readonly ICardRepository _cardRepository;
        private readonly ICardLevelRepository _cardLevelRepository;

        public CardService(ICardRepository cardRepository, ICardLevelRepository cardLevelRepository, IUnitOfWork unitOfWork)
            : base(cardRepository, unitOfWork)
        {
            _cardRepository = cardRepository;
            _cardLevelRepository = cardLevelRepository;
        }

        public async Task<IDictionary<Card, CardLevel>> GetForUserAsync(Guid userId)
        {
            var cards = await Repository.GetAllAsync();
            var cardLevels = await _cardLevelRepository.GetAllForUserAsync(userId);
            return cards.ToDictionary(card => card, card => cardLevels.SingleOrDefault(cl => cl.CardId == card.Id));
        }

        public Task<Card> GetByExternalIdAsync(string externalId)
        {
            return _cardRepository.GetByExternalIdAsync(externalId);
        }
    }
}