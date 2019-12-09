<template>
  <b-card v-if="show" title="Team" class="m-2">
    <div v-if="isInTeam">
      <!-- User is in a team -->
      <div class="form-group row">
        <label class="col-sm-2 col-label">Team</label>
        <div class="col-sm-8">
          <label>{{user.teamName}}</label>
        </div>
        <b-button class="col-sm-2" variant="outline-secondary" @click="leaveTeam">Leave team</b-button>
      </div>

      <div class="form-group row">
        <label class="col-sm-2 col-label">Role</label>
        <div class="col-sm-10">
          <label>{{user.teamRole}}</label>
        </div>
      </div>
    </div>

    <div v-else>
      <!-- User has a pending team membership request -->
      <div v-if="pendingTeamMembershipRequest" class="form-group row">
        <label class="col-sm-2 col-label">Pending request</label>
        <label
          class="col-sm-3 col-label"
        >{{pendingTeamMembershipRequest.teamName}} ({{pendingTeamMembershipRequest.requestDateUtc.toLocaleString()}})</label>
        <b-button variant="outline-danger" @click="abortTeamMembershipRequest">Abort request</b-button>
      </div>

      <!-- User can join or create a team -->
      <div v-else>
        <!-- Join existing -->
        <div class="form-group row">
          <label class="col-sm-2 col-label">Join existing</label>
          <vue-bootstrap-typeahead
            v-model="teamSearch"
            :data="teams"
            :serializer="team => team.name"
            :minMatchingChars="3"
            @hit="setSelectedTeam($event)"
            @input="clearSelectedTeam"
            class="col-sm-8"
            placeholder="Search existing teams"
          />
          <b-button
            variant="outline-success"
            class="col-sm-2"
            :disabled="!selectedTeam"
            @click="sendTeamMembershipRequest"
          >Send request</b-button>
        </div>

        <!-- Create new -->
        <div class="form-group row">
          <label class="col-sm-2 col-label">Create new</label>
          <div class="col-sm-8">
            <b-input type="text" v-model="newTeamName" placeholder="New team name" />
          </div>
          <b-button variant="outline-danger" class="col-sm-2" @click="createTeam">Create team</b-button>
        </div>
      </div>
    </div>
  </b-card>
</template>

<script>
import {
  userService,
  teamService,
  teamMembershipRequestService
} from '@/_services'
import { roles } from '@/_constants'

export default {
  name: 'UserTeamSelection',
  data: function() {
    return {
      show: false,
      user: {},
      teams: [],
      teamSearch: '',
      selectedTeam: null,
      pendingTeamMembershipRequest: null,
      newTeamName: ''
    }
  },
  computed: {
    userId: function() {
      return this.$route.params.userId
    },
    isInTeam: function() {
      return this.user !== null && this.user.teamId !== null
    }
  },
  async created() {
    this.refresh()
  },
  watch: {
    teamSearch(teamName) {
      if (this.teamSearch.length < 3) {
        // Require a minimum of 3 chars to execute a query
        return
      }

      teamService.searchTeamByName(teamName).then(teams => {
        this.teams = teams
      })
    }
  },
  methods: {
    async refresh() {
      this.show = false

      this.user = await userService.getUser(this.userId)
      await this.updatePendingTeamMembershipRequest()

      this.show = true
    },
    async updatePendingTeamMembershipRequest() {
      if (!this.isInTeam) {
        this.pendingTeamMembershipRequest = await teamMembershipRequestService.getPendingTeamMembershipRequest(
          this.userId
        )
      }
    },
    async createTeam() {
      if (this.newTeamName == null || this.newTeamName.length < 3) {
        // TODO: show validation error
        return
      }

      await teamService.createTeam(this.newTeamName)

      this.user = await userService.getUser(this.userId)
    },
    async leaveTeam() {
      await userService.leaveTeam(this.userId).then(
        updatedUser => (this.user = updatedUser),
        error => {
          this.$toasted.show(error.response.data.error, {
            type: 'error',
            position: 'top-center',
            duration: 5000
          })
        }
      )
    },
    abortTeamMembershipRequest() {
      teamMembershipRequestService
        .abortTeamMembershipRequest(this.pendingTeamMembershipRequest.id)
        .then(() => (this.pendingTeamMembershipRequest = null))
    },
    sendTeamMembershipRequest() {
      // TODO: add comment
      teamMembershipRequestService
        .sendTeamMembershipRequest(this.selectedTeam.id, this.userId)
        .then(() => {
          this.selectedTeam = null
          this.updatePendingTeamMembershipRequest()
        })
    },
    clearSelectedTeam(team) {
      this.selectedTeam = null
    },
    setSelectedTeam(team) {
      this.selectedTeam = team
    }
  }
}
</script>
