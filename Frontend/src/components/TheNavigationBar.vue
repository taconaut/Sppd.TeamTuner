<template>
  <b-navbar type="dark" variant="dark" fixed="top">
    <router-link class="navbar-brand" to="/">Sppd.TeamTuner</router-link>
    <div class="collapse navbar-collapse" />
    <span class="navbar-button">
      <b-button v-if="!isAuthorized" class="btn-success" @click="emitShowLoginDialogEvent">Login</b-button>
      <b-button
        v-if="!isAuthorized"
        class="ml-1 btn-secondary"
        @click="emitShowRegisterDialogEvent"
      >Register</b-button>
      <b-dropdown v-if="isAuthorized" toggle-class="text-decoration-none" variant="link" no-caret>
        <template v-slot:button-content>
          <img class="avatar" :src="currentUserAvatar" />
        </template>
        <b-dropdown-item :to="this.currentUserProfileUrl">Profile</b-dropdown-item>
        <b-dropdown-divider></b-dropdown-divider>
        <b-dropdown-item @click="logout">Logout</b-dropdown-item>
      </b-dropdown>
    </span>
  </b-navbar>
</template>

<script>
import { eventBus } from '@/_helpers'
import { authorizationService } from '@/_services'
import { eventIdentifiers } from '@/_constants'

export default {
  name: 'TheNavigationBar',
  data: function() {
    return {
      currentUser: null
    }
  },
  computed: {
    isAuthorized() {
      return this.currentUser != null
    },
    currentUserAvatar() {
      // TODO: use the user avatar instead of this.
      return (
        'https://api.adorable.io/avatars/36/' + this.currentUser.email + '.png'
      )
    },
    currentUserProfileUrl() {
      return '/user/' + this.currentUser.id + '/profile'
    }
  },
  methods: {
    emitShowLoginDialogEvent() {
      eventBus.$emit(eventIdentifiers.showLoginDialog, true)
    },
    emitShowRegisterDialogEvent() {
      eventBus.$emit(eventIdentifiers.showRegisterDialog, true)
    },
    logout() {
      authorizationService.logout()
    },
    setCurrentUser(user) {
      this.currentUser = user
    }
  },
  mounted() {
    authorizationService.currentUser.subscribe(user =>
      this.setCurrentUser(user)
    )
  }
}
</script>

<style scoped>
.avatar {
  height: 30px;
  width: 30px;
}
</style>
