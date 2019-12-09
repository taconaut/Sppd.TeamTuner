<template>
  <div id="user-cards">
    <div class="filterContainer row no-gutters">
      <div class="col-sm-3 col-md-auto m-1">
        <b-button
          :pressed.sync="filter.rarity.common"
          @click="emitFilterChanged"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/theme-stones/common/' + cardIdentifiers.themeNeutralId + '.png'"
          />
        </b-button>
        <b-button
          :pressed.sync="filter.rarity.rare"
          @click="emitFilterChanged"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/theme-stones/special/' + cardIdentifiers.rarityRareId + '.png'"
          />
        </b-button>
        <b-button
          :pressed.sync="filter.rarity.epic"
          @click="emitFilterChanged"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/theme-stones/special/' + cardIdentifiers.rarityEpicId + '.png'"
          />
        </b-button>
        <b-button
          :pressed.sync="filter.rarity.legendary"
          @click="emitFilterChanged"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/theme-stones/special/' + cardIdentifiers.rarityLegendaryId + '.png'"
          />
        </b-button>
      </div>
      <div class="col-sm-3 col-md-auto m-1">
        <b-button
          :pressed.sync="filter.theme.adventure"
          @click="emitFilterChanged"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/theme-icons/' + cardIdentifiers.themeAdventureId + '.png'"
          />
        </b-button>
        <b-button
          :pressed.sync="filter.theme.fantasy"
          @click="emitFilterChanged"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/theme-icons/' + cardIdentifiers.themeFantasyId + '.png'"
          />
        </b-button>
        <b-button
          :pressed.sync="filter.theme.scifi"
          @click="emitFilterChanged"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/theme-icons/' + cardIdentifiers.themeScifiId + '.png'"
          />
        </b-button>
        <b-button
          :pressed.sync="filter.theme.mystical"
          @click="emitFilterChanged"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/theme-icons/' + cardIdentifiers.themeMysticalId + '.png'"
          />
        </b-button>
        <b-button
          :pressed.sync="filter.theme.superhero"
          @click="emitFilterChanged"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/theme-icons/' + cardIdentifiers.themeSuperheroId + '.png'"
          />
        </b-button>
        <b-button
          :pressed.sync="filter.theme.neutral"
          @click="emitFilterChanged"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/theme-icons/' + cardIdentifiers.themeNeutralId + '.png'"
          />
        </b-button>
      </div>
      <div class="col-sm-3 col-md-auto m-1">
        <b-button
          :pressed.sync="filter.type.tank"
          @click="emitFilterChanged"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/type-icons/' + cardIdentifiers.characterTypeTankId + '/special/' + cardIdentifiers.rarityEpicId + '.png'"
          />
        </b-button>
        <b-button
          :pressed.sync="filter.type.melee"
          @click="emitFilterChanged"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/type-icons/' + cardIdentifiers.characterTypeMeleeId + '/special/' + cardIdentifiers.rarityEpicId + '.png'"
          />
        </b-button>
        <b-button
          :pressed.sync="filter.type.assassin"
          @click="emitFilterChanged"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/type-icons/' + cardIdentifiers.characterTypeAssassinId + '/special/' + cardIdentifiers.rarityEpicId + '.png'"
          />
        </b-button>
        <b-button
          :pressed.sync="filter.type.ranged"
          @click="emitFilterChanged"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/type-icons/' + cardIdentifiers.characterTypeRangedId + '/special/' + cardIdentifiers.rarityEpicId + '.png'"
          />
        </b-button>
        <b-button
          :pressed.sync="filter.type.totem"
          @click="emitFilterChanged"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/type-icons/' + cardIdentifiers.characterTypeTotemId + '/special/' + cardIdentifiers.rarityEpicId + '.png'"
          />
        </b-button>
        <b-button
          :pressed.sync="filter.type.spell"
          @click="emitFilterChanged"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/type-icons/' + cardIdentifiers.cardTypeSpellId + '/special/' + cardIdentifiers.rarityEpicId + '.png'"
          />
        </b-button>
        <b-button
          :pressed.sync="filter.type.trap"
          @click="emitFilterChanged"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/type-icons/' + cardIdentifiers.cardTypeTrapId + '/special/' + cardIdentifiers.rarityEpicId + '.png'"
          />
        </b-button>
      </div>
      <div class="col-sm-3 col-md-auto m-1">
        <b-button @click="resetFilter" variant="outline-dark" shadow-none>All</b-button>
        <b-button @click="clearFilter" variant="outline-dark" shadow-none>None</b-button>
      </div>
    </div>
  </div>
</template>

<script>
import { cardIdentifiers } from '@/_constants'

export default {
  name: 'CardsFilter',
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
      },
      cardIdentifiers: cardIdentifiers
    }
  },
  mounted()
  {
    this.emitFilterChanged()
  },
  methods: {
    resetFilter() {
      this.setAllFilters(true)
    },
    clearFilter() {
      this.setAllFilters(false)
    },
    setAllFilters(isEnabled) {
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
    emitFilterChanged() {
      this.$emit('filter', { ...this.filter })
    }
  }
}
</script>

<style>
.filterContainer {
  background: lightgray;
}
</style>
