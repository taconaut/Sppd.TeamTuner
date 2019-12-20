<template>
  <div class="m-5">
    <h1>Email verification</h1>
    <div v-if="isEmailVerifying">Your email is being verified.</div>
    <div v-else>
      <div v-if="isEmailVerified">Your email has been verified.</div>
      <div v-else>
        <div>Your email could not be verified.</div>
        <b-button @click="resendVerificationEmail">Re-send email</b-button>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'

// @ts-ignore
import { authorizationService } from '@/_services'

export default Vue.extend({
  name: 'EmailVerificationView',
  data: function() {
    return {
      isEmailVerifying: true,
      isEmailVerified: false
    }
  },
  computed: {
    code: function() {
      return this.$route.params.code
    }
  },
  async mounted() {
    this.isEmailVerified = await authorizationService.verifyEmail(this.code)
    this.isEmailVerifying = false
  },
  methods: {
    async resendVerificationEmail() {
      await authorizationService.resendVerificationEmail(this.code)
      this.$toasted.show(
        "We've sent you an email. Please check your inbox and click on the link to confirm it.",
        {
          type: 'success',
          position: 'top-center',
          duration: 5000
        }
      )
    }
  }
})
</script>

<style scoped>
* {
  text-align: center;
}
</style>
