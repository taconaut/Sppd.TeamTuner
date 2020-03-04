<template>
  <div>
    <div v-if="!hasPendingRequests" class="m-2">No pending membership requests.</div>
    <b-table v-else-if="showTable" :items="pendingMembershipRequests" :fields="tableFields" striped>
      <template v-slot:cell(userName)="data">{{ data.item.userName }}</template>
      <template v-slot:cell(comment)="data">{{ data.item.comment }}</template>
      <template v-slot:cell(requestDateUtc)="data">{{ data.item.requestDateUtc.toLocaleString() }}</template>
      <template v-slot:cell(accept)="data">
        <b-button variant="outline-success" @click="acceptMembershipRequest(data.item.id)">Accept</b-button>
      </template>
      <template v-slot:cell(reject)="data">
        <b-button variant="outline-secondary" @click="rejectMembershipRequest(data.item.id)">Reject</b-button>
      </template>
    </b-table>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'

// @ts-ignore
import { teamService, teamMembershipRequestService } from '@/_services'
// @ts-ignore
import { eventBus } from '@/_helpers'
// @ts-ignore
import { eventIdentifiers } from '@/_constants'
// @ts-ignore
import { TeamMembershipRequestResponseDto } from '@/api'

export default Vue.extend({
  name: 'TeamPendingMembershipRequestsComponent',
  data: function() {
    return {
      pendingMembershipRequests: null as TeamMembershipRequestResponseDto[],
      hasPendingRequests: false,
      showTable: false,
      tableFields: [
        {
          key: 'userName',
          label: 'User',
          sortable: true
        },
        {
          key: 'comment',
          label: 'Comment',
          sortable: false
        },
        {
          key: 'requestDateUtc',
          label: 'Date',
          sortable: true
        },
        {
          key: 'accept',
          label: '',
          sortable: false,
          tdClass: 'card-row-slim'
        },
        {
          key: 'reject',
          label: '',
          sortable: false,
          tdClass: 'card-row-slim'
        }
      ]
    }
  },
  computed: {
    teamId: function(): string {
      return this.$route.params.teamId
    }
  },
  async created(): Promise<void> {
    this.refreshPendingMembershipRequests()
  },
  methods: {
    async acceptMembershipRequest(membershipRequestId: string): Promise<void> {
      await teamMembershipRequestService.acceptTeamMembershipRequest(
        membershipRequestId
      )
      await this.refreshPendingMembershipRequests()
      eventBus.$emit(eventIdentifiers.teamMembersChanged)
    },
    async rejectMembershipRequest(membershipRequestId: string): Promise<void> {
      await teamMembershipRequestService.rejectTeamMembershipRequest(
        membershipRequestId
      )
      await this.refreshPendingMembershipRequests()
    },
    async refreshPendingMembershipRequests(): Promise<void> {
      this.showTable = false
      this.pendingMembershipRequests = await teamService.getPendingMembershipRequests(
        this.teamId
      )
      this.hasPendingRequests = this.pendingMembershipRequests.length > 0
      this.showTable = true
    }
  }
})
</script>

<style>
td.team-memebership-request-row-slim {
  width: 1px;
}
td.team-memebership-request-row-maxWidth {
  max-width: 100px;
}
</style>
