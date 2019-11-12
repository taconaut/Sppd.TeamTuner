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

<script>
import TheNavigationBar from '@/components/TheNavigationBar'
import TheSideBar from '@/components/TheSideBar'
import LoginDialogModal from '@/components/LoginDialogModal'
import RegisterDialogModal from '@/components/RegisterDialogModal'
import { authorizationService } from '@/_services'
import { eventBus, axiosHelper } from '@/_helpers'
import { eventIdentifiers } from '@/_constants'

export default {
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
  methods: {
    showLoginDialog() {
      this.isLoginDialogVisible = true
    },
    hideLoginDialog() {
      this.isLoginDialogVisible = false
    },
    showRegisterDialog() {
      this.isRegisterDialogVisible = true
    },
    hideRegisterDialog() {
      this.isRegisterDialogVisible = false
    },
    updateContentLeftPadding() {
      this.contentLeftPadding = this.isAuthorized ? 250 : 0
    },
    setCurrentUser(user) {
      this.isAuthorized = user != null
      if (
        !this.isAuthorized &&
        this.$router.currentRoute.name !== 'home' &&
        this.$router.currentRoute.name !== 'email-confirmation'
      ) {
        // Redirect to home if the user has logged out
        this.$router.push({ name: 'home' })
      }
      this.updateContentLeftPadding()
    }
  },
  created() {
    axiosHelper.configureDefaults()
  },
  mounted() {
    eventBus.$on(eventIdentifiers.showLoginDialog, isVisible => {
      if (isVisible) {
        this.showLoginDialog()
      } else {
        this.hideLoginDialog()
      }
    })
    eventBus.$on(eventIdentifiers.showRegisterDialog, isVisible => {
      if (isVisible) {
        this.showRegisterDialog()
      } else {
        this.hideRegisterDialog()
      }
    })

    authorizationService.currentUser.subscribe(user => {
      this.setCurrentUser(user)
    })
  },
  destroyed() {
    eventBus.$off(eventIdentifiers.showLoginDialog)
    eventBus.$off(eventIdentifiers.showRegisterDialog)
  }
}
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
