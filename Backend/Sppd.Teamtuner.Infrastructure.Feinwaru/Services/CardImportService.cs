using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Config;
using Sppd.TeamTuner.Core.Services;
using Sppd.TeamTuner.Infrastructure.Feinwaru.Config;
using Sppd.TeamTuner.Infrastructure.Feinwaru.Domain.Enumerations;
using Sppd.TeamTuner.Infrastructure.Feinwaru.Domain.Objects;

using Card = Sppd.TeamTuner.Core.Domain.Entities.Card;

namespace Sppd.TeamTuner.Infrastructure.Feinwaru.Services
{
    internal class CardImportService : ICardImportService
    {
        private const string ID_PLACEHOLDER = "{id}";

        private readonly ICardService _cardService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CardImportService> _logger;
        private readonly Lazy<FeinwaruConfig> _feinwaruConfig;

        public CardImportService(IConfigProvider<FeinwaruConfig> feinwaruConfigProvider, ICardService cardService, IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<CardImportService> logger)
        {
            _cardService = cardService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _feinwaruConfig = new Lazy<FeinwaruConfig>(() => feinwaruConfigProvider.Config);
        }

        public async Task DoImportAsync()
        {
            _logger.LogInformation($"Start importing cards from {_feinwaruConfig.Value.ApiUrl}");

            var cardsList = await GetCardsListAsync();
            if (!cardsList.Success)
            {
                throw new Exception($"The cards list could not be retrieved. Code={cardsList.Code}; Error={cardsList.Error}");
            }

            _logger.LogDebug("Successfully retrieved card list.");

            await UpdateCardsAsync(cardsList);

            _logger.LogInformation("Finished importing cards");
        }

        private async Task UpdateCardsAsync(CardListResponse cardsList)
        {
            var cardsToUpdate = await GetCardsToUpdateAsync(cardsList);

            // Convert the Feinwaru cards to team tuner entities
            var cardAddEntities = _mapper.Map<IEnumerable<Card>>(cardsToUpdate.ToAdd).ToList();
            var cardUpdateEntities = _mapper.Map<IEnumerable<Card>>(cardsToUpdate.ToUpdate).ToList();
            // The update entities haven't got the correct entity IDs. Set the correct ones
            await SetIdsAsync(cardUpdateEntities);
            var cardDeleteEntityIds = cardsToUpdate.IdsToDelete.ToList();

            // Save the cards
            await _cardService.CreateAsync(cardAddEntities, false);
            await _cardService.UpdateAsync(cardUpdateEntities, commitChanges: false);
            await _cardService.DeleteAsync(cardDeleteEntityIds, false);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation(
                $"Feinwaru returned {cardsList.Data.Count} cards. Added {cardAddEntities.Count}, updated {cardUpdateEntities.Count} and deleted {cardDeleteEntityIds.Count}");
        }

        private async Task SetIdsAsync(IEnumerable<Card> cardUpdateEntities)
        {
            var allStoredCards = (await _cardService.GetAllAsync()).ToList();
            foreach (var cardEntity in cardUpdateEntities)
            {
                cardEntity.Id = allStoredCards.Single(e => e.ExternalId == cardEntity.ExternalId).Id;
            }
        }

        private async Task<CardsToUpdateResult> GetCardsToUpdateAsync(CardListResponse cardsList)
        {
            var cardsToAdd = new List<Domain.Objects.Card>();
            var cardsToUpdate = new List<Domain.Objects.Card>();

            // Get all the cards having to be added or updated from Feinwaru
            var cardResponseTasks = new Dictionary<Task<CardResponse>, CardUpdateOperation>();
            foreach (var cardListCard in cardsList.Data)
            {
                // The identifier used by Feinwaru
                var externalId = cardListCard.Id;

                if (await _cardService.ExternalIdExistsAsync(externalId))
                {
                    // TODO: update condition once Feinwaru has added the updated_at property
                    if (_feinwaruConfig.Value.ImportMode == CardImportMode.UpdateAll || false)
                    {
                        cardResponseTasks.Add(GetCardAsync(externalId), CardUpdateOperation.Update);
                    }
                    else
                    {
                        _logger.LogTrace($"Do not retrieve card {cardListCard.Name} with externalId={externalId} because it already exists");
                    }
                }
                else
                {
                    cardResponseTasks.Add(GetCardAsync(externalId), CardUpdateOperation.Add);
                }
            }

            // ReSharper disable once CoVariantArrayConversion
            Task.WaitAll(cardResponseTasks.Select(kv => kv.Key).ToArray());

            foreach (var cardResponseTask in cardResponseTasks)
            {
                var cardResponse = cardResponseTask.Key.Result;
                var cardUpdateOperation = cardResponseTask.Value;

                if (!cardResponse.Success)
                {
                    _logger.LogError(
                        $"Failed to import card '{cardResponse.Card.Name}' with externalId={cardResponse.Card.Id} from Feinwaru. Code={cardResponse.Code}; Error={cardResponse.Error}");
                }
                else
                {
                    switch (cardUpdateOperation)
                    {
                        case CardUpdateOperation.Add:
                            cardsToAdd.Add(cardResponse.Card);
                            break;

                        case CardUpdateOperation.Update:
                            cardsToUpdate.Add(cardResponse.Card);
                            break;

                        default:
                            throw new NotSupportedException($"CardUpdateOperation '{cardUpdateOperation}' is not supported");
                    }
                }
            }

            // Get cards to delete
            var allStoredCards = await _cardService.GetAllAsync();
            var cardIdsToDelete = allStoredCards.Where(c => !cardsList.Data.Select(cl => cl.Id).Contains(c.ExternalId))
                                                .Select(c => c.Id);

            return new CardsToUpdateResult(cardsToAdd, cardsToUpdate, cardIdsToDelete);
        }

        private async Task<CardListResponse> GetCardsListAsync()
        {
            return await GetResponseAsync<CardListResponse>(_feinwaruConfig.Value.CardsListEndpoint);
        }

        private async Task<CardResponse> GetCardAsync(string cardId)
        {
            return await GetResponseAsync<CardResponse>(_feinwaruConfig.Value.CardEndpoint.Replace(ID_PLACEHOLDER, cardId));
        }

        /// <summary>
        ///     Gets the response from the specified URI and deserializes it as object of type <see cref="TResponse" />
        /// </summary>
        /// <typeparam name="TResponse">The type of deserialized object.</typeparam>
        /// <param name="relativeUri">The relative URI.</param>
        /// <returns>The deserialized response object.</returns>
        private async Task<TResponse> GetResponseAsync<TResponse>(string relativeUri)
        {
            var absoluteUri = GetAbsoluteUri(relativeUri);
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync(absoluteUri);
                _logger.LogTrace($"Successfully got response from {absoluteUri}");
                return JsonConvert.DeserializeObject<TResponse>(response);
            }
        }

        /// <summary>
        ///     Gets the absolute URI by concatenating the .
        /// </summary>
        /// <param name="relativeUri">The relative URI.</param>
        /// <returns>The absolute URI</returns>
        private Uri GetAbsoluteUri(string relativeUri)
        {
            return new Uri(new Uri(_feinwaruConfig.Value.ApiUrl), relativeUri);
        }

        private class CardsToUpdateResult
        {
            public IEnumerable<Domain.Objects.Card> ToAdd { get; }

            public IEnumerable<Domain.Objects.Card> ToUpdate { get; }

            public IEnumerable<Guid> IdsToDelete { get; }

            public CardsToUpdateResult(IEnumerable<Domain.Objects.Card> toAdd, IEnumerable<Domain.Objects.Card> toUpdate, IEnumerable<Guid> idsToDelete)
            {
                ToAdd = toAdd;
                ToUpdate = toUpdate;
                IdsToDelete = idsToDelete;
            }
        }

        private enum CardUpdateOperation
        {
            Add = 0,
            Update = 1
        }
    }
}