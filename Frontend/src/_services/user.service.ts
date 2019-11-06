import { UsersClient } from '@/api'

class UserService {
  async getCards(userId: string) {
    const usersClient = new UsersClient()
    return usersClient.getCardsWithUserLevels(userId)
  }
}

export const userService = new UserService()
