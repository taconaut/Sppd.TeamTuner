<template>
  <transition name="modal">
    <div class="modal-mask">
      <div class="modal-wrapper">
        <div class="modal-container">
          <h2>Login</h2>
          <form @submit.prevent="handleSubmit">
            <div class="form-group">
              <label for="username">Username</label>
              <input
                type="text"
                v-model="username"
                name="username"
                autocomplete="username"
                class="form-control"
                :class="{ 'is-invalid': !isUsernameValid }"
              />
              <div v-show="!isUsernameValid" class="invalid-feedback">Username is required</div>
            </div>
            <div class="form-group">
              <label for="password">Password</label>
              <input
                type="password"
                v-model="password"
                name="password"
                autocomplete="current-password"
                class="form-control"
                :class="{ 'is-invalid': !isPasswordValid }"
              />
              <div v-show="!isPasswordValid" class="invalid-feedback">Password is required</div>
            </div>
            <div v-if="error" class="alert alert-danger">{{error}}</div>
            <div class="form-group d-flex align-items-center">
              <button class="btn btn-primary" :disabled="loading" @click="doLogin">Login</button>
              <button class="btn btn-secondary ml-2" @click="close">Cancel</button>
              <div class="spinner-border text-dark ml-2" role="status" v-show="loading">
                <span class="sr-only">Loading...</span>
              </div>
            </div>
          </form>
        </div>
      </div>
    </div>
  </transition>
</template>

<script lang="ts">
import Vue from 'vue'

// @ts-ignore
import { authorizationService } from '@/_services'

export default Vue.extend({
  name: 'LoginDialogModal',
  data: function() {
    return {
      username: '',
      password: '',
      submitted: false,
      loading: false,
      error: '',
      isUsernameValid: true,
      isPasswordValid: true,
      isDoLogin: false
    }
  },
  methods: {
    close(): void {
      this.$emit('close')
    },
    doLogin(): void {
      this.isDoLogin = true
    },
    updateIsUsernameValid(): void {
      this.isUsernameValid = this.username !== ''
    },
    updateIsPasswordValid(): void {
      this.isPasswordValid = this.password !== ''
    },
    async handleSubmit(): Promise<void> {
      if (!this.isDoLogin) {
        return
      }

      this.submitted = true

      this.updateIsUsernameValid()
      this.updateIsPasswordValid()
      if (!(this.username && this.password)) {
        return
      }

      this.loading = true

      await authorizationService.login(this.username, this.password).then(
        () => {
          this.$emit('close')
        },
        error => {
          this.error = error.response.data.error
        }
      )

      this.loading = false
    }
  }
})
</script>

<style scoped>
.modal-mask {
  position: fixed;
  z-index: 9998;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: table;
  transition: opacity 0.3s ease;
}

.modal-wrapper {
  display: table-cell;
  vertical-align: middle;
}

.modal-container {
  width: 300px;
  margin: 0px auto;
  padding: 20px 30px;
  background-color: #fff;
  border-radius: 2px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.33);
  transition: all 0.3s ease;
  font-family: Helvetica, Arial, sans-serif;
}

.modal-header h3 {
  margin-top: 0;
  color: #42b983;
}

.modal-body {
  margin: 20px 0;
}

.modal-default-button {
  float: right;
}

/*
 * The following styles are auto-applied to elements with
 * transition="modal" when their visibility is toggled
 * by Vue.js.
 *
 * You can easily play with the modal transition by editing
 * these styles.
 */

.modal-enter {
  opacity: 0;
}

.modal-leave-active {
  opacity: 0;
}

.modal-enter .modal-container,
.modal-leave-active .modal-container {
  -webkit-transform: scale(1.1);
  transform: scale(1.1);
}
</style>
