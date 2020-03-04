import { stringHelper, axiosConfigurator } from '@/_helpers'
import { storageKeys, roles } from '@/_constants'
import { UsersClient, AuthorizationRequestDto, UserAuthorizationResponseDto, UserCreateRequestDto, EmailVerificationClient, UserResponseDto } from '@/api'
import { BehaviorSubject, Observable } from 'rxjs'

class AuthorizationService {
  private currentUserSubject: BehaviorSubject<UserAuthorizationResponseDto | null>

  private usersClient: UsersClient | undefined
  private emailVerificationClient: EmailVerificationClient | undefined

  public constructor() {
    var currentUserJson = this.getCurrentUserFromLocalStorage()
    var currentUser = currentUserJson == null ? null : JSON.parse(currentUserJson) as UserAuthorizationResponseDto
    axiosConfigurator.setCurrentUser(currentUser)

    this.currentUserSubject = new BehaviorSubject<UserAuthorizationResponseDto | null>(currentUser)
  }

  public get currentUser(): Observable<UserAuthorizationResponseDto> {
    return this.currentUserSubject.asObservable()
  }

  public get currentUserValue(): UserAuthorizationResponseDto {
    return this.currentUserSubject.value
  }

  public get isAuthorized(): boolean {
    return this.currentUserValue != null
  }

  public canEditTeam(teamId: string): boolean {
    return this.currentUserValue.applicationRole === roles.appAdmin ||
      (this.currentUserValue.teamId === teamId && this.isCurrentUserInTeamRoles([roles.teamLeader, roles.teamCoLeader]))
  }

  public canEditUser(userId: string): boolean {
    return this.currentUserValue.applicationRole === roles.appAdmin || this.currentUserValue.id === userId
  }

  public async createUser(userName: string, email: string, password: string): Promise<UserResponseDto> {
    const userCreateRequest = new UserCreateRequestDto()
    userCreateRequest.name = userName
    userCreateRequest.sppdName = userName
    userCreateRequest.email = email
    userCreateRequest.passwordMd5 = stringHelper.md5hash(password)

    return this.getUsersClient().registerUser(userCreateRequest)
  }

  public async login(userName: string, password: string): Promise<UserAuthorizationResponseDto> {
    const authorizationRequest = new AuthorizationRequestDto()
    authorizationRequest.name = userName
    authorizationRequest.passwordMd5 = stringHelper.md5hash(password)

    const authorizationResponse = await this.getUsersClient().authorizeUser(authorizationRequest)

    localStorage.setItem(storageKeys.currentUser, JSON.stringify(authorizationResponse))
    this.currentUserSubject.next(authorizationResponse)

    axiosConfigurator.setCurrentUser(authorizationResponse)

    return authorizationResponse
  }

  public async verifyEmail(code: string): Promise<boolean> {
    return this.getEmailVerificationClient().verifyEmail(code)
  }

  public async resendVerificationEmail(code: string): Promise<void> {
    return this.getEmailVerificationClient().resendVerificationEmail(code)
  }

  public logout(): void {
    localStorage.removeItem(storageKeys.currentUser)
    this.currentUserSubject.next(null)

    // TODO: Log out on the server

    axiosConfigurator.setCurrentUser(null)
  }

  public isCurrentUserInApplicationRoles(roles: string[]): boolean {
    for (var i = 0; i < roles.length; i++) {
      var role = roles[i]
      if (this.isCurrentUserInApplicationRole(role)) {
        return true
      }
    }

    return false
  }

  public isCurrentUserInApplicationRole(role: string): boolean {
    return this.currentUserValue !== null && this.currentUserValue.applicationRole === role
  }

  public isCurrentUserInTeamRoles(roles: string[]): boolean {
    for (var i = 0; i < roles.length; i++) {
      var role = roles[i]
      if (this.isCurrentUserInTeamRole(role)) {
        return true
      }
    }

    return false
  }

  public isCurrentUserInTeamRole(role: string): boolean {
    return this.currentUserValue !== null && this.currentUserValue.teamRole === role
  }

  private getCurrentUserFromLocalStorage(): string {
    return localStorage.getItem(storageKeys.currentUser)
  }

  private getUsersClient(): UsersClient {
    if (!this.usersClient) {
      this.usersClient = new UsersClient()
    }
    return this.usersClient
  }

  private getEmailVerificationClient(): EmailVerificationClient {
    if (!this.emailVerificationClient) {
      this.emailVerificationClient = new EmailVerificationClient()
    }
    return this.emailVerificationClient
  }
}

export const authorizationService = new AuthorizationService()
