<template>
  <div id="app">
    <TheNavigationBar class="navbar" />
    <TheSideBar class="sidebar" v-if="isAuthorized" />
    <div class="content" :style="{ paddingLeft: contentLeftPadding + 'px' }">
      <router-view />
    </div>

    <LoginDialogModal v-if="isLoginDialogVisible" @close="hideLoginDialog" />
    <RegisterDialogModal v-if="isRegisterDialogVisible" @close="hideRegisterDialog" />
  </div>
</template>

<script lang="ts">
import Vue from 'vue'

// @ts-ignore
import TheNavigationBar from '@/components/TheNavigationBar'
// @ts-ignore
import TheSideBar from '@/components/TheSideBar'
// @ts-ignore
import LoginDialogModal from '@/components/LoginDialogModal'
// @ts-ignore
import RegisterDialogModal from '@/components/RegisterDialogModal'
// @ts-ignore
import { authorizationService } from '@/_services'
// @ts-ignore
import { eventBus, axiosConfigurator } from '@/_helpers'
// @ts-ignore
import { eventIdentifiers } from '@/_constants'
// @ts-ignore
import { UserResponseDto, UserAuthorizationResponseDto } from '@/api'

export default Vue.extend({
  name: 'App',
  components: {
    TheNavigationBar,
    TheSideBar,
    LoginDialogModal,
    RegisterDialogModal
  },
  data: function() {
    return {
      isLoginDialogVisible: false,
      isRegisterDialogVisible: false,
      isAuthorized: false,
      contentLeftPadding: 0
    }
  },
  created(): void {
    axiosConfigurator.configureDefaults()
  },
  mounted(): void {
    eventBus.$on(eventIdentifiers.showLoginDialog, (isVisible: boolean) => {
      if (isVisible) {
        this.showLoginDialog()
      } else {
        this.hideLoginDialog()
      }
    })
    eventBus.$on(eventIdentifiers.showRegisterDialog, (isVisible: boolean) => {
      if (isVisible) {
        this.showRegisterDialog()
      } else {
        this.hideRegisterDialog()
      }
    })

    authorizationService.currentUser.subscribe(user => {
      this.setIsAuthorized(user != null)
    })
  },
  destroyed(): void {
    eventBus.$off(eventIdentifiers.showLoginDialog)
    eventBus.$off(eventIdentifiers.showRegisterDialog)
  },
  methods: {
    showLoginDialog(): void {
      this.isLoginDialogVisible = true
    },
    hideLoginDialog(): void {
      this.isLoginDialogVisible = false
    },
    showRegisterDialog(): void {
      this.isRegisterDialogVisible = true
    },
    hideRegisterDialog(): void {
      this.isRegisterDialogVisible = false
    },
    updateContentLeftPadding(): void {
      this.contentLeftPadding = this.isAuthorized ? 250 : 0
    },
    setIsAuthorized(isAuthorized: boolean): void {
      this.isAuthorized = isAuthorized
      if (
        !this.isAuthorized &&
        this.$router.currentRoute.name !== 'home' &&
        this.$router.currentRoute.name !== 'email-verification'
      ) {
        // Redirect to home if the user has logged out
        this.$router.push({ name: 'home' })
      }
      this.updateContentLeftPadding()
    }
  }
})
</script>

<style>
:root {
  --navbar-height: 50px;
  --sidebar-width: 250px;
}

#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}

.content {
  padding-top: var(--navbar-height);
}
.sidebar {
  position: fixed;
  top: var(--navbar-height);
  width: var(--sidebar-width);
  height: calc(100% - var(--navbar-height));
}
.navbar {
  height: var(--navbar-height);
}
</style>
