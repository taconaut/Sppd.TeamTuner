import Vue from 'vue'
import App from '@/App.vue'
import router from '@/router'

import '@babel/polyfill'
import 'mutationobserver-shim'

// Plugins
import '@/plugins/bootstrap-vue'
import '@/plugins/vue-toasted'
import '@/plugins/vue-bootstrap-typeahead'

Vue.config.productionTip = false

new Vue({
  router,
  render: h => h(App)
}).$mount('#app')
