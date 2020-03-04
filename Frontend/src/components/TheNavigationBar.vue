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

<script lang="ts">
import Vue from 'vue'

// @ts-ignore
import { eventBus } from '@/_helpers'
// @ts-ignore
import { authorizationService, administrationService } from '@/_services'
// @ts-ignore
import { eventIdentifiers, roles } from '@/_constants'
// @ts-ignore
import {
  UserResponseDto,
  SystemInfoDto,
  UserAuthorizationResponseDto
} from '../api'

export default Vue.extend({
  name: 'TheNavigationBar',
  data: function() {
    return {
      currentUser: null as UserResponseDto,
      systemInfoText: null as string,
      canEditTeam: false
    }
  },
  computed: {
    isAuthorized(): boolean {
      return this.currentUser != null
    },
    currentUserAvatar(): string {
      // TODO: use the user avatar instead of this.
      return (
        'https://api.adorable.io/avatars/36/' + this.currentUser.id + '.png'
      )
    },
    currentUserProfileUrl(): string {
      return '/user/' + this.currentUser.id + '/profile'
    },
    currentTeamProfileUrl(): string {
      return '/team/' + this.currentUser.teamId + '/profile'
    }
  },
  methods: {
    emitShowLoginDialogEvent(): void {
      eventBus.$emit(eventIdentifiers.showLoginDialog, true)
    },
    emitShowRegisterDialogEvent(): void {
      eventBus.$emit(eventIdentifiers.showRegisterDialog, true)
    },
    logout(): void {
      authorizationService.logout()
    },
    setCurrentUser(user: UserResponseDto): void {
      this.currentUser = user
      const teamId = user !== null ? user.teamId : null
      this.canEditTeam = authorizationService.canEditTeam(teamId)
    },
    setSystemInfo(systemInfo: SystemInfoDto): void {
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
  async mounted(): Promise<void> {
    authorizationService.currentUser.subscribe(
      (user: UserAuthorizationResponseDto) =>
      // TODO: remove this fishy cast to unknown, once object hierarchy issue has been solved
        this.setCurrentUser((user as unknown) as UserResponseDto)
    )
    this.setSystemInfo(await administrationService.getSystemInfo())
  }
})
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
