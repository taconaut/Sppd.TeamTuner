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
              v-model="passwordFirst"
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
              v-model="passwordSecond"
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

<script lang="ts">
import Vue from 'vue'

// @ts-ignore
import { userService, authorizationService } from '@/_services'
// @ts-ignore
import { roles } from '@/_constants'
// @ts-ignore
import { UserResponseDto } from '@/api'

export default Vue.extend({
  name: 'UserDetailComponent',
  data: function() {
    return {
      show: false,
      editedUser: null as UserResponseDto,
      originalUser: null as UserResponseDto,
      passwordFirst: null as string,
      passwordSecond: null as string,
      isChangePassword: false,
      isCurrentUserAdmin: false
    }
  },
  computed: {
    userId: function(): string {
      return this.$route.params.userId
    }
  },
  async created(): Promise<void> {
    this.originalUser = await userService.getUser(this.userId)
    this.refreshOriginalUser()
  },
  methods: {
    async onSubmit(): Promise<void> {
      // TODO: validate password
      var password = this.isChangePassword ? this.passwordFirst : null

      this.originalUser = await userService.updateUser(
        this.editedUser,
        password
      )
    },
    onReset(): void {
      this.setOriginalUserAsEditedUser()
    },
    setOriginalUserAsEditedUser(): void {
      // deep clone the user
      this.editedUser = JSON.parse(JSON.stringify(this.originalUser))
    },
    updateIsCurrentUserAdmin(): void {
      this.isCurrentUserAdmin = authorizationService.isCurrentUserInApplicationRole(
        roles.appAdmin
      )
    },
    refreshOriginalUser(): void {
      this.show = false

      this.setOriginalUserAsEditedUser()
      this.updateIsCurrentUserAdmin()

      this.show = true
    }
  }
})
</script>
