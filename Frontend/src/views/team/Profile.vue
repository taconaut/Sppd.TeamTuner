<template>
  <div>
    <b-card title="Team" class="m-2">
      <team-detail />
    </b-card>
    <b-card title="Requests" class="m-2">
      <team-pending-membership-requests />
    </b-card>
    <b-card title="Members" class="m-2">
      <team-members />
    </b-card>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'

import TeamDetail from '@/components/team/Detail.vue'
import TeamPendingMembershipRequests from '@/components/team/PendingMembershipRequests.vue'
import TeamMembers from '@/components/team/Members.vue'

export default Vue.extend({
  name: 'TeamProfileView',
  components: {
    TeamDetail,
    TeamPendingMembershipRequests,
    TeamMembers
  }
})
</script>
