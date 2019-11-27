<template>
  <div v-if="show">
    <b-form @submit="onSubmit" @reset="onReset">
      <b-card title="Team" class="m-2">
        <div class="form-group row">
          <label class="col-sm-2 col-form-label">Name</label>
          <div class="col-sm-10">
            <b-input type="text" v-model="editedTeam.name" placeholder="Enter the team name." />
          </div>
        </div>

        <div class="form-group row">
          <label class="col-sm-2 col-form-label">Description</label>
          <div class="col-sm-10">
            <b-textarea type="text" v-model="editedTeam.description" placeholder="..." />
          </div>
        </div>

        <div class="mt-4">
          <b-button type="submit" variant="outline-success">Save</b-button>
          <b-button type="reset" variant="outline-secondary" class="ml-2">Reset</b-button>
        </div>
      </b-card>
    </b-form>
  </div>
</template>

<script>
import { teamService } from '@/_services'

export default {
  name: 'TeamDetail',
  data: function() {
    return {
      show: false,
      editedTeam: {},
      originalTeam: null
    }
  },
  computed: {
    teamId: function() {
      return this.$route.params.teamId
    }
  },
  async created() {
    this.originalTeam = await teamService.getTeam(this.teamId)
    this.refreshOriginalTeam()
  },
  methods: {
    async onSubmit(evt) {
      evt.preventDefault()

      // TODO: validate before submit

      this.originalTeam = await teamService.updateTeam(this.editedTeam)
    },
    onReset(evt) {
      evt.preventDefault()

      this.setOriginalTeamAsEditedTeam()

      // Trick to reset/clear native browser form validation state
      this.show = false
      this.$nextTick(() => {
        this.show = true
      })
    },
    setOriginalTeamAsEditedTeam() {
      // deep clone the team
      this.editedTeam = JSON.parse(JSON.stringify(this.originalTeam))
    },
    async refreshOriginalTeam() {
      this.show = false

      this.setOriginalTeamAsEditedTeam()

      this.show = true
    }
  }
}
</script>
