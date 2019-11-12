import { stringHelper, axiosHelper } from '@/_helpers'
import { storageKeys } from '@/_constants'
import { UsersClient, AuthorizationRequestDto, UserAuthorizationResponseDto, UserCreateRequestDto } from '@/api'
import { BehaviorSubject } from 'rxjs'

class AuthorizationService {
  private currentUserSubject: BehaviorSubject<UserAuthorizationResponseDto | null>

  public constructor() {
    var currentUserJson = this.getCurrentUserFromLocalStorage()
    var currentUser = currentUserJson == null ? null : JSON.parse(currentUserJson) as UserAuthorizationResponseDto
    axiosHelper.setCurrentUser(currentUser)

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

    axiosHelper.setCurrentUser(authorizationResponse)

    return authorizationResponse
  }

  public async verifyEmail(code: string) {
    const usersClient = new UsersClient()
    return usersClient.verifyEmail(code)
  }

  public async resendVerificationEmail(code: string) {
    const usersClient = new UsersClient()
    return usersClient.resendVerificationMail(code)
  }

  public logout() {
    localStorage.removeItem(storageKeys.currentUser)
    this.currentUserSubject.next(null)

    axiosHelper.setCurrentUser(null)
  }

  private getCurrentUserFromLocalStorage() {
    return localStorage.getItem(storageKeys.currentUser)
  }
}

export const authorizationService = new AuthorizationService()
