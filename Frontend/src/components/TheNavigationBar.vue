<template>
  <b-navbar type="dark" variant="dark" fixed="top">
    <router-link class="navbar-brand" to="/">Sppd.TeamTuner</router-link>
    <div class="collapse navbar-collapse" />
    <span class="navbar-button">
      <b-button v-if="!isAuthorized" variant="success" @click="emitShowLoginDialogEvent">Login</b-button>
      <b-button
        v-if="!isAuthorized"
        class="ml-1"
        variant="secondary"
        @click="emitShowRegisterDialogEvent"
      >Register</b-button>
      <b-dropdown v-if="isAuthorized" toggle-class="text-decoration-none" variant="link" no-caret>
        <template v-slot:button-content>
          <img class="avatar" :src="currentUserAvatar" />
        </template>
        <b-dropdown-item :to="this.currentUserProfileUrl">My profile</b-dropdown-item>
        <b-dropdown-item :to="this.currentTeamProfileUrl" v-if="canEditTeam">Team profile</b-dropdown-item>
        <b-dropdown-divider></b-dropdown-divider>
        <b-dropdown-item @click="logout">Logout</b-dropdown-item>
        <b-dropdown-divider></b-dropdown-divider>
        <b-dropdown-text class="system-info">{{systemInfoText}}</b-dropdown-text>
      </b-dropdown>
    </span>
  </b-navbar>
</template>

<script>
import { eventBus } from '@/_helpers'
import { authorizationService, administrationService } from '@/_services'
import { eventIdentifiers, roles } from '@/_constants'

export default {
  name: 'TheNavigationBar',
  data: function() {
    return {
      currentUser: null,
      systemInfoText: null,
      canEditTeam: false
    }
  },
  computed: {
    isAuthorized() {
      return this.currentUser != null
    },
    currentUserAvatar() {
      // TODO: use the user avatar instead of this.
      return (
        'https://api.adorable.io/avatars/36/' + this.currentUser.id + '.png'
      )
    },
    currentUserProfileUrl() {
      return '/user/' + this.currentUser.id + '/profile'
    },
    currentTeamProfileUrl() {
      return '/team/' + this.currentUser.teamId + '/profile'
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
      this.canEditTeam = authorizationService.isCurrentUserInTeamRoles([roles.teamLeader, roles.teamCoLeader])
    },
    setSystemInfo(systemInfo) {
      var systemInfoText = systemInfo.version
      if (systemInfo.gitCommitHash) {
        systemInfoText += '-' + systemInfo.gitCommitHash
      }
      if (systemInfo.buildTimeUtc) {
        systemInfoText += '\n' + systemInfo.buildTimeUtc.toLocaleDateString()
      }

      this.systemInfoText = systemInfoText
    }
  },
  async mounted() {
    authorizationService.currentUser.subscribe(user =>
      this.setCurrentUser(user)
    )
    this.setSystemInfo(await administrationService.getSystemInfo())
  }
}
</script>

<style scoped>
.avatar {
  height: 30px;
  width: 30px;
}
.system-info {
  text-align: right;
  font-size: 10px;
}
</style>
