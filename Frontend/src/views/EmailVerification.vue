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

<script>
import { authorizationService } from '@/_services'

export default {
  name: 'EmailVerification',
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
  mehtods: {
    async resendVerificationEmail() {
      await authorizationService.resend.resendVerificationEmail(this.code)
      // TODO: notify user in toast or similar
    }
  }
}
</script>

<style scoped>
* {
  text-align: center;
}
</style>
