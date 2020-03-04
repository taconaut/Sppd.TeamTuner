import { UsersClient, UserUpdateRequestDto, UserResponseDto, TeamsClient, UserCardResponseDto } from '@/api'
import { stringHelper } from '@/_helpers'

class UserService {
  private usersClient: UsersClient | undefined
  private teamsClient: TeamsClient | undefined

  async getCards(userId: string): Promise<UserCardResponseDto[]> {
    return this.getUsersClient().getCardsWithUserLevels(userId)
  }

  async getUser(userId: string): Promise<UserResponseDto> {
    return this.getUsersClient().getUserByUserId(userId)
  }

  async leaveTeam(teamId: string, userId: string): Promise<void> {
    return this.getTeamsClient().removeMember(teamId, userId)
  }

  async updateUser(user: UserResponseDto, password: string | null): Promise<UserResponseDto> {
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

    return this.getUsersClient().updateUser(request)
  }

  private getUsersClient(): UsersClient {
    if (!this.usersClient) {
      this.usersClient = new UsersClient()
    }
    return this.usersClient
  }

  private getTeamsClient(): TeamsClient {
    if (!this.teamsClient) {
      this.teamsClient = new TeamsClient()
    }
    return this.teamsClient
  }
}

export const userService = new UserService()
