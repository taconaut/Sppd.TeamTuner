import { CardLevelsClient, CardLevelUpdateRequestDto, CardLevelResponseDto } from '@/api'

class CardLevelService {
  private cardLevelsClient: CardLevelsClient | undefined

  async setCardLevel(userId: string, cardId: string, level: number): Promise<CardLevelResponseDto> {
    var cardLevelUpdateRequest = new CardLevelUpdateRequestDto()
    cardLevelUpdateRequest.userId = userId
    cardLevelUpdateRequest.cardId = cardId
    cardLevelUpdateRequest.level = level

    return this.getCardLevelsClient().setCardLevel(cardLevelUpdateRequest)
  }

  private getCardLevelsClient(): CardLevelsClient {
    if (!this.cardLevelsClient) {
      this.cardLevelsClient = new CardLevelsClient()
    }
    return this.cardLevelsClient
  }
}

export const cardLevelService = new CardLevelService()
