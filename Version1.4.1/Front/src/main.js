import Vue from 'vue'
import App from './App.vue'
import router from './router'

Vue.config.productionTip = false


let globalData = new Vue({
  data: { 
    $activeUserName: 'Guest',
    $activeUserId: 0
  }
});
Vue.mixin({
  computed: {
    $activeUserName: {
      get: function () { return globalData.$data.$activeUserName },
      set: function (newactiveUserName) { globalData.$data.$activeUserName = newactiveUserName; }
    },
    $activeUserId: {
      get: function () { return globalData.$data.$activeUserId },
      set: function (newActiveUserId) { globalData.$data.$activeUserId = newActiveUserId; }
    }
  }
})


new Vue({
  router,  
  render: function (h) { return h(App) }
}).$mount('#app')
