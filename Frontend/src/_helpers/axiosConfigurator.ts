import axios from 'axios'
import { configurationService } from '@/_services/configuration.service'
import { UserAuthorizationResponseDto } from '@/api'

function configureDefaults() {
  axios.defaults.baseURL = configurationService.apiUrl
}

function setCurrentUser(currentUser: UserAuthorizationResponseDto | null) {
  var authorizationHeaderValue = null
  if (currentUser != null) {
    authorizationHeaderValue = `Bearer ${currentUser.token}`
  }

  axios.defaults.headers.common['Authorization'] = authorizationHeaderValue
}

export const axiosConfigurator = {
  configureDefaults,
  setCurrentUser
}
