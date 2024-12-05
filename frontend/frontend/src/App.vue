<template>
  <v-app>
    <v-main>
      <CoffeeMachine />
    </v-main>
  </v-app>
</template>

<script>
import CoffeeMachine from './components/CoffeeMachine.vue';

export default {
  name: 'App',
  components: {
    CoffeeMachine,
  },
};
</script>
