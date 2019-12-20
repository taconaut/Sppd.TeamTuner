<template>
  <sidebar-menu :menu="menu" :show-one-child="true" :hideToggle="true" />
</template>

<script lang="ts">
import Vue from 'vue'

// @ts-ignore
import { SidebarMenu } from 'vue-sidebar-menu'
// @ts-ignore
import 'vue-sidebar-menu/dist/vue-sidebar-menu.css'
// @ts-ignore
import { authorizationService } from '@/_services'

export default Vue.extend({
  components: {
    SidebarMenu
  },
  data: function() {
    return {
      menu: [
        {
          title: authorizationService.currentUserValue.name,
          icon: 'fa fa-user',
          child: [
            {
              href:
                '/user/' + authorizationService.currentUserValue.id + '/cards',
              title: 'Cards',
              icon: 'fa fa-hat-wizard'
            }
          ]
        },
        {
          title: 'Team',
          icon: 'fa fa-users',
          child: [
            {
              href: '#',
              title: 'Cards',
              icon: 'fa fa-hat-wizard'
            },
            {
              href: '#',
              title: 'Team war',
              icon: 'fa fa-dove'
            }
          ]
        }
      ]
    }
  }
})
</script>
