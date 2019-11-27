<template>
  <div v-if="show">
    <b-form @submit="onSubmit" @reset="onReset">
      <b-card title="User" class="m-2">
        <!-- Name -->
        <div class="form-group row">
          <label class="col-sm-2 col-form-label">Name</label>
          <div class="col-sm-10">
            <b-input type="text" v-model="editedUser.name" placeholder="Enter your name." />
          </div>
        </div>

        <!-- Description -->
        <div class="form-group row">
          <label class="col-sm-2 col-form-label">Description</label>
          <div class="col-sm-10">
            <b-textarea
              type="text"
              v-model="editedUser.description"
              placeholder="What is this all about?"
            />
          </div>
        </div>

        <!-- Email -->
        <div class="form-group row">
          <label class="col-sm-2 col-form-label">Email</label>
          <div class="col-sm-10">
            <b-input type="text" v-model="editedUser.email" placeholder="Enter email" />
          </div>
        </div>

        <!-- Application role -->
        <div class="form-group row" v-if="isCurrentUserAdmin">
          <label class="col-sm-2 col-form-label">Application role</label>
          <div class="col-sm-10">
            <label>{{editedUser.applicationRole}}</label>
          </div>
        </div>

        <!-- Password -->
        <b-checkbox v-model="isChangePassword">Change password</b-checkbox>
        <div v-if="isChangePassword" class="mt-2">
          <div class="form-group row">
            <label class="col-sm-2 col-form-label">New Password</label>
            <div class="col-sm-10">
              <b-input
                type="password"
                v-model="editedUser.passwordFirst"
                autocomplete="new-password"
                placeholder="Enter password"
              />
            </div>
          </div>
          <div class="form-group row">
            <label class="col-sm-2 col-form-label">Confirm password</label>
            <div class="col-sm-10">
              <b-input
                type="password"
                v-model="editedUser.passwordSecond"
                autocomplete="new-password"
                placeholder="Confirm password"
              />
            </div>
          </div>
        </div>

        <div class="mt-4">
          <b-button type="submit" variant="outline-success">Save</b-button>
          <b-button type="reset" variant="outline-secondary" class="ml-2">Reset</b-button>
        </div>
      </b-card>
    </b-form>

    <b-card title="Team" class="m-2">
      <div v-if="isInTeam">
        <!-- User is in a team -->
        <div class="form-group row">
          <label class="col-sm-2 col-label">Team</label>
          <div class="col-sm-8">
            <label>{{editedUser.teamName}}</label>
          </div>
          <b-button class="col-sm-2" variant="outline-secondary" @click="leaveTeam">Leave team</b-button>
        </div>

        <div class="form-group row">
          <label class="col-sm-2 col-label">Role</label>
          <div class="col-sm-10">
            <label>{{editedUser.teamRole}}</label>
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
  </div>
</template>

<script>
import {
  userService,
  teamService,
  authorizationService,
  teamMembershipRequestService
} from '@/_services'
import { roles } from '@/_constants'

export default {
  name: 'UserProfile',
  data: function() {
    return {
      show: false,
      editedUser: {},
      originalUser: null,
      isChangePassword: false,
      isCurrentUserAdmin: false,
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
      return this.originalUser !== null && this.originalUser.teamId !== null
    }
  },
  async created() {
    this.originalUser = await userService.getUser(this.userId)
    this.refreshOriginalUser()
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
    async onSubmit(evt) {
      evt.preventDefault()

      // TODO: validate before submit

      var password = this.isChangePassword
        ? this.editedUser.passwordFirst
        : null

      this.originalUser = await userService.updateUser(
        this.editedUser,
        password
      )
    },
    onReset(evt) {
      evt.preventDefault()

      this.setOriginalUserAsEditedUser()

      // Trick to reset/clear native browser form validation state
      this.show = false
      this.$nextTick(() => {
        this.show = true
      })
    },
    setOriginalUserAsEditedUser() {
      // deep clone the user
      this.editedUser = JSON.parse(JSON.stringify(this.originalUser))
    },
    async updatePendingTeamMembershipRequest() {
      this.pendingTeamMembershipRequest = await teamMembershipRequestService.getPendingTeamMembershipRequest(
        this.userId
      )
    },
    updateIsCurrentUserAdmin() {
      this.isCurrentUserAdmin = authorizationService.isCurrentUserInApplicationRole(
        roles.Admin
      )
    },
    async refreshOriginalUser() {
      this.show = false

      this.setOriginalUserAsEditedUser()
      this.updateIsCurrentUserAdmin()

      if (!this.isInTeam) {
        await this.updatePendingTeamMembershipRequest()
      }

      this.show = true
    },

    // Team operations
    async createTeam() {
      if (this.newTeamName == null || this.newTeamName.length < 3) {
        // TODO: show validation error
        return
      }

      await teamService.createTeam(this.newTeamName)

      this.originalUser = await userService.getUser(this.userId)
      this.refreshOriginalUser()
    },
    async leaveTeam() {
      this.originalUser = await userService.leaveTeam(this.userId)
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
