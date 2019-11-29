<template>
  <div>
    <UserDetail />
    <UserTeamSelection />
  </div>
</template>

<script>
import UserDetail from '@/components/user/Detail.vue'
import UserTeamSelection from '@/components/user/TeamSelection.vue'

export default {
  name: 'UserProfile',
  components: {
    UserDetail,
    UserTeamSelection
  }
}
</script>
