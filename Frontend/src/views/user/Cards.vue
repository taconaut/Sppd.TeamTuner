<template>
  <div>
    <CardsFilter @filter="applyFilter" />
    <Cards :filter="cardsFilter" />
  </div>
</template>

<script>
import CardsFilter from '@/components/cards/Filter.vue'
import Cards from '@/components/user/Cards.vue'

export default {
  name: 'UserCards',
  components: {
    CardsFilter,
    Cards
  },
  data: function() {
    return {
      cardsFilter: null
    }
  },
  methods: {
    applyFilter(cardsFilter) {
      this.cardsFilter = cardsFilter
    }
  }
}
</script>
