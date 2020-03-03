<template>
  <div id="user-cards">
    <div class="filterContainer row no-gutters">
      <!-- Rarities -->
      <div class="col-sm-3 col-md-auto m-1">
        <filter-button
          :isActive="filter.rarity.common"
          :imgSrc="'/img/cards/theme-stones/common/' + cardIdentifiers.themeNeutralId + '.png'"
          v-on:isActive="filter.rarity.common = $event; emitFilterChanged()"
        />
        <filter-button
          :isActive="filter.rarity.rare"
          :imgSrc="'/img/cards/theme-stones/special/' + cardIdentifiers.rarityRareId + '.png'"
          v-on:isActive="filter.rarity.rare = $event; emitFilterChanged()"
        />
        <filter-button
          :isActive="filter.rarity.epic"
          :imgSrc="'/img/cards/theme-stones/special/' + cardIdentifiers.rarityEpicId + '.png'"
          v-on:isActive="filter.rarity.epic = $event; emitFilterChanged()"
        />
        <filter-button
          :isActive="filter.rarity.legendary"
          :imgSrc="'/img/cards/theme-stones/special/' + cardIdentifiers.rarityLegendaryId + '.png'"
          v-on:isActive="filter.rarity.legendary = $event; emitFilterChanged()"
        />
      </div>

      <!-- Themes -->
      <div class="col-sm-3 col-md-auto m-1">
        <filter-button
          :isActive="filter.theme.adventure"
          :imgSrc="'/img/cards/theme-icons/' + cardIdentifiers.themeAdventureId + '.png'"
          v-on:isActive="filter.theme.adventure = $event; emitFilterChanged()"
        />
        <filter-button
          :isActive="filter.theme.fantasy"
          :imgSrc="'/img/cards/theme-icons/' + cardIdentifiers.themeFantasyId + '.png'"
          v-on:isActive="filter.theme.fantasy = $event; emitFilterChanged()"
        />
        <filter-button
          :isActive="filter.theme.scifi"
          :imgSrc="'/img/cards/theme-icons/' + cardIdentifiers.themeScifiId + '.png'"
          v-on:isActive="filter.theme.scifi = $event; emitFilterChanged()"
        />
        <filter-button
          :isActive="filter.theme.mystical"
          :imgSrc="'/img/cards/theme-icons/' + cardIdentifiers.themeMysticalId + '.png'"
          v-on:isActive="filter.theme.mystical = $event; emitFilterChanged()"
        />
        <filter-button
          :isActive="filter.theme.superhero"
          :imgSrc="'/img/cards/theme-icons/' + cardIdentifiers.themeSuperheroId + '.png'"
          v-on:isActive="filter.theme.superhero = $event; emitFilterChanged()"
        />
        <filter-button
          :isActive="filter.theme.neutral"
          :imgSrc="'/img/cards/theme-icons/' + cardIdentifiers.themeNeutralId + '.png'"
          v-on:isActive="filter.theme.neutral = $event; emitFilterChanged()"
        />
      </div>

      <!-- Types -->
      <div class="col-sm-3 col-md-auto m-1">
        <filter-button
          :isActive="filter.type.tank"
          :imgSrc="'/img/cards/type-icons/' + cardIdentifiers.characterTypeTankId + '/special/' + cardIdentifiers.rarityEpicId + '.png'"
          v-on:isActive="filter.type.tank = $event; emitFilterChanged()"
        />
        <filter-button
          :isActive="filter.type.melee"
          :imgSrc="'/img/cards/type-icons/' + cardIdentifiers.characterTypeMeleeId + '/special/' + cardIdentifiers.rarityEpicId + '.png'"
          v-on:isActive="filter.type.melee = $event; emitFilterChanged()"
        />
        <filter-button
          :isActive="filter.type.assassin"
          :imgSrc="'/img/cards/type-icons/' + cardIdentifiers.characterTypeAssassinId + '/special/' + cardIdentifiers.rarityEpicId + '.png'"
          v-on:isActive="filter.type.assassin = $event; emitFilterChanged()"
        />
        <filter-button
          :isActive="filter.type.ranged"
          :imgSrc="'/img/cards/type-icons/' + cardIdentifiers.characterTypeRangedId + '/special/' + cardIdentifiers.rarityEpicId + '.png'"
          v-on:isActive="filter.type.ranged = $event; emitFilterChanged()"
        />
        <filter-button
          :isActive="filter.type.totem"
          :imgSrc="'/img/cards/type-icons/' + cardIdentifiers.characterTypeTotemId + '/special/' + cardIdentifiers.rarityEpicId + '.png'"
          v-on:isActive="filter.type.totem = $event; emitFilterChanged()"
        />
        <filter-button
          :isActive="filter.type.spell"
          :imgSrc="'/img/cards/type-icons/' + cardIdentifiers.cardTypeSpellId + '/special/' + cardIdentifiers.rarityEpicId + '.png'"
          v-on:isActive="filter.type.spell = $event; emitFilterChanged()"
        />
        <filter-button
          :isActive="filter.type.trap"
          :imgSrc="'/img/cards/type-icons/' + cardIdentifiers.cardTypeTrapId + '/special/' + cardIdentifiers.rarityEpicId + '.png'"
          v-on:isActive="filter.type.trap = $event; emitFilterChanged()"
        />
      </div>
      <div class="col-sm-3 col-md-auto m-1">
        <b-button @click="resetFilter" variant="outline-dark" shadow-none>All</b-button>
        <b-button @click="clearFilter" variant="outline-dark" shadow-none>None</b-button>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'

// @ts-ignore
import { cardIdentifiers } from '@/_constants'
// @ts-ignore
import FilterButton from './FilterButton'

export default Vue.extend({
  name: 'CardsFilterComponent',
  components: {
    FilterButton
  },
  data: function() {
    return {
      filter: {
        theme: {
          adventure: true,
          fantasy: true,
          scifi: true,
          mystical: true,
          neutral: true,
          superhero: true
        },
        type: {
          assassin: true,
          melee: true,
          ranged: true,
          tank: true,
          totem: true,
          spell: true,
          trap: true
        },
        rarity: {
          common: true,
          rare: true,
          epic: true,
          legendary: true
        }
      } as Filter,
      cardIdentifiers: cardIdentifiers
    }
  },
  mounted() {
    this.emitFilterChanged()
  },
  methods: {
    resetFilter() {
      this.setAllFilters(true)
    },
    clearFilter() {
      this.setAllFilters(false)
    },
    setAllFilters(isEnabled: boolean) {
      // Rarity
      this.filter.rarity.common = isEnabled
      this.filter.rarity.rare = isEnabled
      this.filter.rarity.epic = isEnabled
      this.filter.rarity.legendary = isEnabled

      // Theme
      this.filter.theme.adventure = isEnabled
      this.filter.theme.fantasy = isEnabled
      this.filter.theme.scifi = isEnabled
      this.filter.theme.mystical = isEnabled
      this.filter.theme.superhero = isEnabled
      this.filter.theme.neutral = isEnabled

      // Type
      this.filter.type.tank = isEnabled
      this.filter.type.melee = isEnabled
      this.filter.type.assassin = isEnabled
      this.filter.type.ranged = isEnabled
      this.filter.type.totem = isEnabled
      this.filter.type.spell = isEnabled
      this.filter.type.trap = isEnabled

      this.emitFilterChanged()
    },
    updateFilter(filterProperty: boolean, isActive: boolean) {
      filterProperty = isActive
      this.emitFilterChanged()
    },
    emitFilterChanged() {
      this.$emit('filter', { ...this.filter })
    }
  }
})
</script>

<style scoped>
.filterContainer {
  background: lightgray;
}
</style>
