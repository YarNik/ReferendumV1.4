import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'

Vue.use(VueRouter)

  const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },  
  {
    path: '/logIn',
    name: 'LogIn',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: function () {
      return import(/* webpackChunkName: "logIn" */ '../views/LogIn.vue')
    }
  },
  {
    path: '/signUp',
    name: 'SignUp',    
    component: function () {
      return import(/* webpackChunkName: "signUp" */ '../views/SignUp.vue')
    }
  },
 
  {
    path: '/SingleReferendum/:id',
    name: 'SingleReferendum',       
    component: function () {      
      return import(/* webpackChunkName: "SingleReferendum" */ '../views/SingleReferendum.vue')      
    },
    props: true
  }
  
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
