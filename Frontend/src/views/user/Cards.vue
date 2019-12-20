<template>
  <div>
    <CardsFilter @filter="applyFilter" />
    <UserCards :filter="cardsFilter" />
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import CardsFilter from '@/components/cards/Filter.vue'
import UserCards from '@/components/user/Cards.vue'

export default Vue.extend({
  name: 'UserCardsView',
  components: {
    CardsFilter,
    UserCards
  },
  data: function() {
    return {
      cardsFilter: null as Filter
    }
  },
  methods: {
    applyFilter(cardsFilter: Filter) {
      this.cardsFilter = cardsFilter
    }
  }
})
</script>
