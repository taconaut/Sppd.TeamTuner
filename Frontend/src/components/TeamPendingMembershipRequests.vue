<template>
  <b-card title="Membership Requests" class="m-2">
    <div v-if="!hasPendingRequests" class="m-2">No pending requests.</div>
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
  </b-card>
</template>

<script>
import { teamService, teamMembershipRequestService } from '@/_services'

export default {
  name: 'TeamPendingMembershipRequests',
  data: function() {
    return {
      pendingMembershipRequests: null,
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
    teamId: function() {
      return this.$route.params.teamId
    }
  },
  async created() {
    this.refreshPendingMembershipRequests()
  },
  methods: {
    async acceptMembershipRequest(membershipRequestId) {
      await teamMembershipRequestService.acceptTeamMembershipRequest(
        membershipRequestId
      )
      await this.refreshPendingMembershipRequests()
    },
    async rejectMembershipRequest(membershipRequestId) {
      await teamMembershipRequestService.rejectTeamMembershipRequest(
        membershipRequestId
      )
      await this.refreshPendingMembershipRequests()
    },
    async refreshPendingMembershipRequests() {
      this.showTable = false
      this.pendingMembershipRequests = await teamService.getPendingMembershipRequests(
        this.teamId
      )
      this.hasPendingRequests = this.pendingMembershipRequests.length > 0
      this.showTable = true
    }
  }
}
</script>

<style>
td.team-memebership-request-row-slim {
  width: 1px;
}
td.team-memebership-request-row-maxWidth {
  max-width: 100px;
}
</style>
