<template>
  <div>
    <b-table
      v-if="filter && filteredCards && filteredCards.length > 0"
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
import { cardsHelper } from "@/_helpers";

export default {
  name: 'Cards',
  props: {
    filter: {
      type: Object
    }
  },
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
      ]
    }
  },
  watch: {
    filter: function() {
      this.applyFilter()
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
      if (card.rarityId.toUpperCase() === cardIdentifiers.rarityCommonId) {
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
        card.typeId.toUpperCase() === cardIdentifiers.cardTypeSpellId ||
        card.typeId.toUpperCase() === cardIdentifiers.cardTypeTrapId
          ? card.typeId
          : card.characterTypeId
      if (card.rarityId.toUpperCase() === cardIdentifiers.rarityCommonId) {
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
    applyFilter() {
      if (this.filter && this.userCards) {
        this.filteredCards = this.userCards.filter(this.cardMatchesFilter)
      }
    },
    cardMatchesFilter(card) {
      return cardsHelper.cardMatchesFilter(card, this.filter)
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
</style>
