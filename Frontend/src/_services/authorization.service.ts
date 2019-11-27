import { stringHelper, axiosConfigurator } from '@/_helpers'
import { storageKeys } from '@/_constants'
import { UsersClient, AuthorizationRequestDto, UserAuthorizationResponseDto, UserCreateRequestDto, EmailVerificationClient } from '@/api'
import { BehaviorSubject } from 'rxjs'

class AuthorizationService {
  private currentUserSubject: BehaviorSubject<UserAuthorizationResponseDto | null>

  public constructor() {
    var currentUserJson = this.getCurrentUserFromLocalStorage()
    var currentUser = currentUserJson == null ? null : JSON.parse(currentUserJson) as UserAuthorizationResponseDto
    axiosConfigurator.setCurrentUser(currentUser)

    this.currentUserSubject = new BehaviorSubject<UserAuthorizationResponseDto | null>(currentUser)
  }

  public get currentUser() {
    return this.currentUserSubject.asObservable()
  }

  public get currentUserValue() {
    return this.currentUserSubject.value
  }

  public get isAuthorized() {
    return this.currentUserSubject.value != null
  }

  public async createUser(userName: string, email: string, password: string) {
    const userCreateRequest = new UserCreateRequestDto()
    userCreateRequest.name = userName
    userCreateRequest.sppdName = userName
    userCreateRequest.email = email
    userCreateRequest.passwordMd5 = stringHelper.md5hash(password)

    const usersClient = new UsersClient()
    const userCreateResponse = await usersClient.registerUser(userCreateRequest)

    return userCreateResponse
  }

  public async login(userName: string, password: string) {
    const authorizationRequest = new AuthorizationRequestDto()
    authorizationRequest.name = userName
    authorizationRequest.passwordMd5 = stringHelper.md5hash(password)

    const usersClient = new UsersClient()
    const authorizationResponse = await usersClient.authorizeUser(authorizationRequest)

    localStorage.setItem(storageKeys.currentUser, JSON.stringify(authorizationResponse))
    this.currentUserSubject.next(authorizationResponse)

    axiosConfigurator.setCurrentUser(authorizationResponse)

    return authorizationResponse
  }

  public async verifyEmail(code: string) {
    const emailVerificationClient = new EmailVerificationClient()
    return emailVerificationClient.verifyEmail(code)
  }

  public async resendVerificationEmail(code: string) {
    const emailVerificationClient = new EmailVerificationClient()
    return emailVerificationClient.resendVerificationEmail(code)
  }

  public logout() {
    localStorage.removeItem(storageKeys.currentUser)
    this.currentUserSubject.next(null)

    axiosConfigurator.setCurrentUser(null)
  }

  public isCurrentUserInApplicationRoles(roles: string[]) {
    for (var i = 0; i < roles.length; i++) {
      var role = roles[i]
      if (this.isCurrentUserInApplicationRole(role)) {
        return true
      }
    }

    return false
  }

  public isCurrentUserInApplicationRole(role: string) {
    return this.currentUserValue !== null && this.currentUserValue.applicationRole === role
  }

  public isCurrentUserInTeamRoles(roles: string[]) {
    for (var i = 0; i < roles.length; i++) {
      var role = roles[i]
      if (this.isCurrentUserInTeamRole(role)) {
        return true
      }
    }

    return false
  }

  public isCurrentUserInTeamRole(role: string) {
    return this.currentUserValue !== null && this.currentUserValue.teamRole === role
  }

  private getCurrentUserFromLocalStorage() {
    return localStorage.getItem(storageKeys.currentUser)
  }
}

export const authorizationService = new AuthorizationService()
