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
        private readonly ICardLevelRepository _cardLevelRepository;

        public CardService(IRepository<Card> cardRepository, ICardLevelRepository cardLevelRepository, IUnitOfWork unitOfWork)
            : base(cardRepository, unitOfWork)
        {
            _cardLevelRepository = cardLevelRepository;
        }

        public async Task<IDictionary<Card, int?>> GetForUserAsync(Guid userId)
        {
            var cards = await Repository.GetAllAsync();
            var cardLevels = await _cardLevelRepository.GetAllForUserAsync(userId);
            return cards.ToDictionary(card => card, card => cardLevels.SingleOrDefault(cl => cl.CardId == card.Id)?.Level);
        }
    }
}