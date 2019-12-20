<template>
  <div>
    <b-card title="Team" class="m-2">
      <TeamDetail />
    </b-card>
    <b-card title="Requests" class="m-2">
      <TeamPendingMembershipRequests />
    </b-card>
    <b-card title="Members" class="m-2">
      <TeamMembers />
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
