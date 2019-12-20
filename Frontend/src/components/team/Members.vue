<template>
  <b-table :items="teamMembers" :fields="fields" :sort-by.sync="sortBy" striped sort-icon-left>
    <template v-slot:cell(name)="data">{{ data.item.name }}</template>
    <template v-slot:cell(description)="data">{{ data.item.description }}</template>
    <template v-slot:cell(teamRole)="data">
      <b-dropdown size="sm" :text="data.item.teamRole">
        <b-dropdown-item @click="updateRole(data.item, roles.teamLeader)">{{roles.teamLeader}}</b-dropdown-item>
        <b-dropdown-item @click="updateRole(data.item, roles.teamCoLeader)">{{roles.teamCoLeader}}</b-dropdown-item>
        <b-dropdown-item @click="updateRole(data.item, roles.teamMember)">{{roles.teamMember}}</b-dropdown-item>
      </b-dropdown>
    </template>
    <template v-slot:cell(removeMember)="data">
      <b-button
        variant="outline-danger"
        @click="kickMember(data.item)"
        v-if="canKickMember(data.item)"
      >Kick</b-button>
    </template>
  </b-table>
</template>

<script lang="ts">
import Vue from 'vue'

// @ts-ignore
import { teamService, authorizationService } from '@/_services'
// @ts-ignore
import { eventBus, rolesHelper } from '@/_helpers'
// @ts-ignore
import { eventIdentifiers, roles } from '@/_constants'
// @ts-ignore
import { UserResponseDto, UserAuthorizationResponseDto } from '@/api.ts'

export default Vue.extend({
  name: 'TeamMembersComponent',
  data: function() {
    return {
      teamMembers: [],
      sortBy: 'name',
      fields: [
        {
          key: 'name',
          label: 'Name',
          sortable: true
        },
        {
          key: 'description',
          label: 'Description',
          sortable: false
        },
        {
          key: 'teamRole',
          label: 'Role',
          sortable: true
        },
        {
          key: 'removeMember',
          label: '',
          sortable: false
        }
      ],
      roles: roles
    }
  },
  computed: {
    teamId: function(): string {
      return this.$route.params.teamId
    },
    currentUser: function(): UserAuthorizationResponseDto {
      return authorizationService.currentUserValue
    }
  },
  async mounted() {
    await this.refreshMembers()

    eventBus.$on(eventIdentifiers.teamMembersChanged, () => {
      this.refreshMembers()
    })
  },
  methods: {
    async refreshMembers() {
      this.teamMembers = await teamService.getMembers(this.teamId)
    },
    async updateRole(user: UserResponseDto, role: string) {
      await teamService.updateUserTeamRole(this.teamId, user.id, role).then(
        () => this.refreshMembers(),
        error => {
          this.$toasted.show(error.response.data.error, {
            type: 'error',
            position: 'top-center',
            duration: 5000
          })
        }
      )
    },
    async kickMember(user: UserResponseDto) {
      teamService.removeMember(this.teamId, user.id).then(
        () => this.refreshMembers(),
        error => {
          this.$toasted.show(error.response.data.error, {
            type: 'error',
            position: 'top-center',
            duration: 5000
          })
        }
      )
    },
    canKickMember(user: UserResponseDto) {
      if(this.currentUser.id === user.id){
        return false
      }
      
      if (this.currentUser.applicationRole === this.roles.appAdmin) {
        return true
      }

      if (user.teamRole === this.roles.teamLeader) {
        return false
      }

      return rolesHelper.isTeamRole1GreaterThanRole2(
        this.currentUser.teamRole,
        user.teamRole
      )
    }
  }
})
</script>
