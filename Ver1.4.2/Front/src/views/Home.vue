<template>
  <div class="home">    
    <Referendum/>
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
