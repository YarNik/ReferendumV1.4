<template>
  <div class="home">    
    <Referendum/>
    <span>test new branch2 17.05</span>
  </div>
</template>

<script>
// @ is an alias to /src

import Referendum from '@/components/Referendum.vue'

export default {
  name: 'Home',
  components: {
    Referendum
  }
}
</script>
