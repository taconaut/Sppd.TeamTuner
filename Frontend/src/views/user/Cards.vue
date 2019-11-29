<template>
  <div id="user-cards">
    <div class="filterContainer row no-gutters">
      <div class="col-sm-3 col-md-auto m-1">
        <b-button
          :pressed.sync="filters.rarity.common"
          @click="applyFilter"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/theme-stones/common/' + cardIdentifiers.themeNeutralId + '.png'"
          />
        </b-button>
        <b-button
          :pressed.sync="filters.rarity.rare"
          @click="applyFilter"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/theme-stones/special/' + cardIdentifiers.rarityRareId + '.png'"
          />
        </b-button>
        <b-button
          :pressed.sync="filters.rarity.epic"
          @click="applyFilter"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/theme-stones/special/' + cardIdentifiers.rarityEpicId + '.png'"
          />
        </b-button>
        <b-button
          :pressed.sync="filters.rarity.legendary"
          @click="applyFilter"
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
          :pressed.sync="filters.theme.adventure"
          @click="applyFilter"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/theme-icons/' + cardIdentifiers.themeAdventureId + '.png'"
          />
        </b-button>
        <b-button
          :pressed.sync="filters.theme.fantasy"
          @click="applyFilter"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/theme-icons/' + cardIdentifiers.themeFantasyId + '.png'"
          />
        </b-button>
        <b-button
          :pressed.sync="filters.theme.scifi"
          @click="applyFilter"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/theme-icons/' + cardIdentifiers.themeScifiId + '.png'"
          />
        </b-button>
        <b-button
          :pressed.sync="filters.theme.mystical"
          @click="applyFilter"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/theme-icons/' + cardIdentifiers.themeMysticalId + '.png'"
          />
        </b-button>
        <b-button
          :pressed.sync="filters.theme.superhero"
          @click="applyFilter"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/theme-icons/' + cardIdentifiers.themeSuperheroId + '.png'"
          />
        </b-button>
        <b-button
          :pressed.sync="filters.theme.neutral"
          @click="applyFilter"
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
          :pressed.sync="filters.type.tank"
          @click="applyFilter"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/type-icons/' + cardIdentifiers.characterTypeTankId + '/special/' + cardIdentifiers.rarityEpicId + '.png'"
          />
        </b-button>
        <b-button
          :pressed.sync="filters.type.melee"
          @click="applyFilter"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/type-icons/' + cardIdentifiers.characterTypeMeleeId + '/special/' + cardIdentifiers.rarityEpicId + '.png'"
          />
        </b-button>
        <b-button
          :pressed.sync="filters.type.assassin"
          @click="applyFilter"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/type-icons/' + cardIdentifiers.characterTypeAssassinId + '/special/' + cardIdentifiers.rarityEpicId + '.png'"
          />
        </b-button>
        <b-button
          :pressed.sync="filters.type.ranged"
          @click="applyFilter"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/type-icons/' + cardIdentifiers.characterTypeRangedId + '/special/' + cardIdentifiers.rarityEpicId + '.png'"
          />
        </b-button>
        <b-button
          :pressed.sync="filters.type.totem"
          @click="applyFilter"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/type-icons/' + cardIdentifiers.characterTypeTotemId + '/special/' + cardIdentifiers.rarityEpicId + '.png'"
          />
        </b-button>
        <b-button
          :pressed.sync="filters.type.spell"
          @click="applyFilter"
          variant="outline-dark"
          shadow-none
        >
          <img
            height="24px"
            :src="'/img/cards/type-icons/' + cardIdentifiers.cardTypeSpellId + '/special/' + cardIdentifiers.rarityEpicId + '.png'"
          />
        </b-button>
        <b-button
          :pressed.sync="filters.type.trap"
          @click="applyFilter"
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

    <b-table
      v-if="filteredCards && filteredCards.length > 0"
      :items="filteredCards"
      :fields="fields"
      :sort-by.sync="sortBy"
      striped
      sort-icon-left
    >
      <template v-slot:cell(rarityId)="data">
        <img height="20px" :src="getRarityIcon(data.item)" />
      </template>
      <template v-slot:cell(characterTypeId)="data">
        <img height="20px" :src="getTypeIcon(data.item)" />
      </template>
      <template v-slot:cell(themeId)="data">
        <img height="20px" :src="getThemeIcon(data.item)" />
      </template>
      <template v-slot:cell(cardName)="data">{{ data.item.cardName }}</template>
      <template v-slot:cell(level)="data">
        <b-dropdown
          size="sm"
          :text="data.item.level !== null ? data.item.level.toString() : 'not set'"
        >
          <b-dropdown-item @click="updateLevel(data.item, 1)">1</b-dropdown-item>
          <b-dropdown-item @click="updateLevel(data.item, 2)">2</b-dropdown-item>
          <b-dropdown-item @click="updateLevel(data.item, 3)">3</b-dropdown-item>
          <b-dropdown-item @click="updateLevel(data.item, 4)">4</b-dropdown-item>
          <b-dropdown-item @click="updateLevel(data.item, 5)">5</b-dropdown-item>
          <b-dropdown-item @click="updateLevel(data.item, 6)">6</b-dropdown-item>
          <b-dropdown-item @click="updateLevel(data.item, 7)">7</b-dropdown-item>
        </b-dropdown>
      </template>
      <template
        v-slot:cell(levelLastModified)="data"
      >{{ data.item.levelLastModified == null ? 'never' : data.item.levelLastModified.toLocaleString() }}</template>
    </b-table>
  </div>
</template>

<script>
import { userService, cardLevelService } from '@/_services'
import { cardIdentifiers } from '@/_constants'

export default {
  data: function() {
    return {
      userCards: null,
      filteredCards: null,
      sortBy: 'themeId',
      fields: [
        {
          key: 'rarityId',
          label: '',
          sortable: true,
          tdClass: 'card-row-slim'
        },
        {
          key: 'characterTypeId',
          label: '',
          sortable: true,
          tdClass: 'card-row-slim'
        },
        { key: 'themeId', label: '', sortable: true, tdClass: 'card-row-slim' },
        {
          key: 'cardName',
          label: 'Card',
          sortable: true,
          tdClass: 'card-row-maxWidth'
        },
        { key: 'level', sortable: true, tdClass: 'card-row-slim' },
        { key: 'levelLastModified', label: 'Last updated', sortable: true }
      ],
      filters: {
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
  async mounted() {
    var userId = this.$route.params.userId
    this.userCards = await userService.getCards(userId)
    this.applyFilter()
  },
  methods: {
    async updateLevel(card, level) {
      var userId = this.$route.params.userId
      var updateResult = await cardLevelService.setCardLevel(
        userId,
        card.cardId,
        level
      )

      card.level = updateResult.level
      card.levelLastModified = updateResult.levelLastModified
    },
    getRarityIcon(card) {
      if (card.rarityId.toUpperCase() === this.cardIdentifiers.rarityCommonId) {
        return '/img/cards/theme-stones/common/' + card.themeId + '.png'
      } else {
        return '/img/cards/theme-stones/special/' + card.rarityId + '.png'
      }
    },
    getThemeIcon(card) {
      return '/img/cards/theme-icons/' + card.themeId + '.png'
    },
    getTypeIcon(card) {
      var typeId =
        card.typeId.toUpperCase() === this.cardIdentifiers.cardTypeSpellId ||
        card.typeId.toUpperCase() === this.cardIdentifiers.cardTypeTrapId
          ? card.typeId
          : card.characterTypeId

      if (card.rarityId.toUpperCase() === this.cardIdentifiers.rarityCommonId) {
        return (
          '/img/cards/type-icons/' + typeId + '/common/' + card.themeId + '.png'
        )
      } else {
        return (
          '/img/cards/type-icons/' +
          typeId +
          '/special/' +
          card.rarityId +
          '.png'
        )
      }
    },
    resetFilter() {
      this.setAllFilters(true)
    },
    clearFilter() {
      this.setAllFilters(false)
    },
    setAllFilters(isEnabled) {
      // Rarity
      this.filters.rarity.common = isEnabled
      this.filters.rarity.rare = isEnabled
      this.filters.rarity.epic = isEnabled
      this.filters.rarity.legendary = isEnabled

      // Theme
      this.filters.theme.adventure = isEnabled
      this.filters.theme.fantasy = isEnabled
      this.filters.theme.scifi = isEnabled
      this.filters.theme.mystical = isEnabled
      this.filters.theme.superhero = isEnabled
      this.filters.theme.neutral = isEnabled

      // Type
      this.filters.type.tank = isEnabled
      this.filters.type.melee = isEnabled
      this.filters.type.assassin = isEnabled
      this.filters.type.ranged = isEnabled
      this.filters.type.totem = isEnabled
      this.filters.type.spell = isEnabled
      this.filters.type.trap = isEnabled

      this.applyFilter()
    },
    applyFilter() {
      this.filteredCards = this.userCards.filter(this.cardMatchesFilter)
    },
    cardMatchesFilter(card) {
      return (
        this.matchesThemeFilter(card) &&
        this.matchesTypeFilter(card) &&
        this.matchesRarityFilter(card)
      )
    },
    matchesThemeFilter(card) {
      if (
        this.filters.theme.adventure &&
        card.themeId.toUpperCase() === cardIdentifiers.themeAdventureId
      ) {
        return true
      }
      if (
        this.filters.theme.fantasy &&
        card.themeId.toUpperCase() === cardIdentifiers.themeFantasyId
      ) {
        return true
      }
      if (
        this.filters.theme.scifi &&
        card.themeId.toUpperCase() === cardIdentifiers.themeScifiId
      ) {
        return true
      }
      if (
        this.filters.theme.mystical &&
        card.themeId.toUpperCase() === cardIdentifiers.themeMysticalId
      ) {
        return true
      }
      if (
        this.filters.theme.superhero &&
        card.themeId.toUpperCase() === cardIdentifiers.themeSuperheroId
      ) {
        return true
      }
      if (
        this.filters.theme.neutral &&
        card.themeId.toUpperCase() === cardIdentifiers.themeNeutralId
      ) {
        return true
      }

      return false
    },
    matchesTypeFilter(card) {
      if (
        this.filters.type.tank &&
        card.characterTypeId !== null &&
        card.characterTypeId.toUpperCase() ===
          cardIdentifiers.characterTypeTankId
      ) {
        return true
      }
      if (
        this.filters.type.melee &&
        card.characterTypeId !== null &&
        card.characterTypeId.toUpperCase() ===
          cardIdentifiers.characterTypeMeleeId
      ) {
        return true
      }
      if (
        this.filters.type.assassin &&
        card.characterTypeId !== null &&
        card.characterTypeId.toUpperCase() ===
          cardIdentifiers.characterTypeAssassinId
      ) {
        return true
      }
      if (
        this.filters.type.ranged &&
        card.characterTypeId !== null &&
        card.characterTypeId.toUpperCase() ===
          cardIdentifiers.characterTypeRangedId
      ) {
        return true
      }
      if (
        this.filters.type.totem &&
        card.characterTypeId !== null &&
        card.characterTypeId.toUpperCase() ===
          cardIdentifiers.characterTypeTotemId
      ) {
        return true
      }
      if (
        this.filters.type.spell &&
        card.typeId.toUpperCase() === cardIdentifiers.cardTypeSpellId
      ) {
        return true
      }
      if (
        this.filters.type.trap &&
        card.typeId.toUpperCase() === cardIdentifiers.cardTypeTrapId
      ) {
        return true
      }

      return false
    },
    matchesRarityFilter(card) {
      if (
        this.filters.rarity.common &&
        card.rarityId.toUpperCase() === cardIdentifiers.rarityCommonId
      ) {
        return true
      }
      if (
        this.filters.rarity.rare &&
        card.rarityId.toUpperCase() === cardIdentifiers.rarityRareId
      ) {
        return true
      }
      if (
        this.filters.rarity.epic &&
        card.rarityId.toUpperCase() === cardIdentifiers.rarityEpicId
      ) {
        return true
      }
      if (
        this.filters.rarity.legendary &&
        card.rarityId.toUpperCase() === cardIdentifiers.rarityLegendaryId
      ) {
        return true
      }

      return false
    }
  }
}
</script>

<style>
td.card-row-slim {
  width: 1px;
}
td.card-row-maxWidth {
  max-width: 100px;
}
.filterContainer {
  background: lightgray;
}
</style>
