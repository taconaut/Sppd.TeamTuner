<template>
  <div>
    <UserDetail />
    <UserTeamSelection />
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import UserDetail from '@/components/user/Detail.vue'
import UserTeamSelection from '@/components/user/TeamSelection.vue'
export default Vue.extend({
  name: 'UserProfileView',
  components: {
    UserDetail,
    UserTeamSelection
  }
})
</script>
