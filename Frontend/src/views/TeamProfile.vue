<template>
  <div>
    <TeamDetail />
    <TeamPendingMembershipRequests />
  </div>
</template>

<script>
import TeamDetail from '@/components/team/Detail.vue'
import TeamPendingMembershipRequests from '@/components/team/PendingMembershipRequests.vue'

export default {
  name: 'TeamProfile',
  components: {
    TeamDetail,
    TeamPendingMembershipRequests
  }
}
</script>
