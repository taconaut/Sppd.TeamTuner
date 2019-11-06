import axios from 'axios'
import { config } from '@/config'
import { UserAuthorizationResponseDto } from '@/api'

function configureDefaults() {
  axios.defaults.baseURL = config.apiUrl

  axios.interceptors.response.use(
    function (response) {
      return response;
    },
    function (error) {
      // handle error
      if (error.response) {
        alert(error.response.data.message);
      }
    });
}

function setCurrentUser(currentUser: UserAuthorizationResponseDto | null) {
  var authorizationHeaderValue = null
  if (currentUser != null) {
    authorizationHeaderValue = `Bearer ${currentUser.token}`
  }

  axios.defaults.headers.common['Authorization'] = authorizationHeaderValue
}

export const axiosHelper = {
  configureDefaults,
  setCurrentUser
}
