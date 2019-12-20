<template>
  <div v-if="show">
    <b-form @submit="onSubmit" @reset="onReset">
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
    </b-form>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'

// @ts-ignore
import { teamService } from '@/_services'
// @ts-ignore
import { TeamResponseDto } from '@/api'

export default Vue.extend({
  name: 'TeamDetailComponent',
  data: function() {
    return {
      show: false,
      editedTeam: null as TeamResponseDto,
      originalTeam: null as TeamResponseDto
    }
  },
  computed: {
    teamId: function(): string {
      return this.$route.params.teamId
    }
  },
  async created() {
    this.originalTeam = await teamService.getTeam(this.teamId)
    this.refreshOriginalTeam()
  },
  methods: {
    async onSubmit() {
      // TODO: validate before submit
      this.originalTeam = await teamService.updateTeam(this.editedTeam)
    },
    onReset() {
      this.setOriginalTeamAsEditedTeam()
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
})
</script>
