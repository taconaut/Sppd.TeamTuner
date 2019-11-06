import { CardLevelsClient, CardLevelUpdateRequestDto } from '@/api'

class CardLevelService {
  async setCardLevel(userId: string, cardId: string, level: number) {
    const cardLevelsClient = new CardLevelsClient()

    var cardLevelUpdateRequest = new CardLevelUpdateRequestDto()
    cardLevelUpdateRequest.userId = userId
    cardLevelUpdateRequest.cardId = cardId
    cardLevelUpdateRequest.level = level

    return cardLevelsClient.setCardLevel(cardLevelUpdateRequest)
  }
}

export const cardLevelService = new CardLevelService()
