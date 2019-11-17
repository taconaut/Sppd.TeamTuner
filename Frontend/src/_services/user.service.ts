import { UsersClient, UserUpdateRequestDto, UserResponseDto } from '@/api'
import { stringHelper } from '@/_helpers'

class UserService {
  async getCards(userId: string) {
    const usersClient = new UsersClient()
    return usersClient.getCardsWithUserLevels(userId)
  }

  async getUser(userId: string) {
    const usersClient = new UsersClient()
    return usersClient.getUserByUserId(userId)
  }

  async leaveTeam(userId: string) {
    const usersClient = new UsersClient()
    return usersClient.leaveTeam(userId)
  }

  async updateUser(user: UserResponseDto, password: string | null) {
    var propertiesToUpdate = ['Name', 'SppdName', 'Description', 'Email']

    var request = new UserUpdateRequestDto()
    request.id = user.id
    request.version = user.version
    request.name = user.name
    request.sppdName = user.name
    request.description = user.description
    request.email = user.email
    if (password) {
      request.passwordMd5 = stringHelper.md5hash(password)
      propertiesToUpdate.push('PasswordMd5')
    }

    request.propertiesToUpdate = propertiesToUpdate

    const usersClient = new UsersClient()
    return usersClient.updateUser(request)
  }
}

export const userService = new UserService()
