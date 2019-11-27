import Vue from 'vue'
import Router from 'vue-router'

// Views
import Home from '@/views/Home.vue'
import UserCards from '@/views/UserCards.vue'
import UserProfile from '@/views/UserProfile.vue'
import TeamProfile from '@/views/TeamProfile.vue'
import EmailVerification from '@/views/EmailVerification.vue'

Vue.use(Router)

export default new Router({
  routes: [
    // Home
    {
      path: '/',
      name: 'home',
      component: Home
    },

    // User
    {
      path: '/user/:userId/cards',
      name: 'user-cards',
      component: UserCards
    },
    {
      path: '/user/:userId/profile',
      name: 'user-profile',
      component: UserProfile
    },
    {
      path: '/user/email-verification/:code',
      name: 'email-verification',
      component: EmailVerification
    },

    // Team,
    {
      path: '/team/:teamId/profile',
      name: 'team-profile',
      component: TeamProfile
    }
  ]
})
