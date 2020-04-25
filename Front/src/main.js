import Vue from 'vue'
import Vuex from 'vuex';
import App from './App.vue'
import router from './router'

Vue.use(Vuex);

Vue.config.productionTip = false

const store = new Vuex.Store({
  state: {
    activeUserName: 'Guest',
    activeUserId: null
  },
  mutations: {
    updateActiveUserName: function(state, newActiveUser) { 
      state.activeUserName = newActiveUser.userName;
      state.activeUserId = newActiveUser.id;
    }
  }
});


new Vue({
  router,
  store,
  
  render: function (h) { return h(App) }
}).$mount('#app')
