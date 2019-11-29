<template>
  <b-form v-if="show" @submit="onSubmit" @reset="onReset">
    <b-card title="User" class="m-2">
      <!-- Name -->
      <div class="form-group row">
        <label class="col-sm-2 col-form-label">Name</label>
        <div class="col-sm-10">
          <b-input type="text" v-model="editedUser.name" placeholder="Enter your name." />
        </div>
      </div>

      <!-- Description -->
      <div class="form-group row">
        <label class="col-sm-2 col-form-label">Description</label>
        <div class="col-sm-10">
          <b-textarea
            type="text"
            v-model="editedUser.description"
            placeholder="What is this all about?"
          />
        </div>
      </div>

      <!-- Email -->
      <div class="form-group row">
        <label class="col-sm-2 col-form-label">Email</label>
        <div class="col-sm-10">
          <b-input type="text" v-model="editedUser.email" placeholder="Enter email" />
        </div>
      </div>

      <!-- Application role -->
      <div class="form-group row" v-if="isCurrentUserAdmin">
        <label class="col-sm-2 col-form-label">Application role</label>
        <div class="col-sm-10">
          <label>{{editedUser.applicationRole}}</label>
        </div>
      </div>

      <!-- Password -->
      <b-checkbox v-model="isChangePassword">Change password</b-checkbox>
      <div v-if="isChangePassword" class="mt-2">
        <div class="form-group row">
          <label class="col-sm-2 col-form-label">New Password</label>
          <div class="col-sm-10">
            <b-input
              type="password"
              v-model="editedUser.passwordFirst"
              autocomplete="new-password"
              placeholder="Enter password"
            />
          </div>
        </div>
        <div class="form-group row">
          <label class="col-sm-2 col-form-label">Confirm password</label>
          <div class="col-sm-10">
            <b-input
              type="password"
              v-model="editedUser.passwordSecond"
              autocomplete="new-password"
              placeholder="Confirm password"
            />
          </div>
        </div>
      </div>

      <div class="mt-4">
        <b-button type="submit" variant="outline-success">Save</b-button>
        <b-button type="reset" variant="outline-secondary" class="ml-2">Reset</b-button>
      </div>
    </b-card>
  </b-form>
</template>

<script>
import { userService, authorizationService } from '@/_services'
import { roles } from '@/_constants'

export default {
  name: 'UserDetail',
  data: function() {
    return {
      show: false,
      editedUser: {},
      originalUser: null,
      isChangePassword: false,
      isCurrentUserAdmin: false
    }
  },
  computed: {
    userId: function() {
      return this.$route.params.userId
    }
  },
  async created() {
    this.originalUser = await userService.getUser(this.userId)
    this.refreshOriginalUser()
  },
  methods: {
    async onSubmit(evt) {
      evt.preventDefault()

      // TODO: validate before submit

      var password = this.isChangePassword
        ? this.editedUser.passwordFirst
        : null

      this.originalUser = await userService.updateUser(
        this.editedUser,
        password
      )
    },
    onReset(evt) {
      evt.preventDefault()

      this.setOriginalUserAsEditedUser()

      // Trick to reset/clear native browser form validation state
      this.show = false
      this.$nextTick(() => {
        this.show = true
      })
    },
    setOriginalUserAsEditedUser() {
      // deep clone the user
      this.editedUser = JSON.parse(JSON.stringify(this.originalUser))
    },
    updateIsCurrentUserAdmin() {
      this.isCurrentUserAdmin = authorizationService.isCurrentUserInApplicationRole(
        roles.Admin
      )
    },
    async refreshOriginalUser() {
      this.show = false

      this.setOriginalUserAsEditedUser()
      this.updateIsCurrentUserAdmin()

      this.show = true
    }
  }
}
</script>
