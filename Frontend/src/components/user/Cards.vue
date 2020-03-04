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
        <img height="20px" :src="cardsHelper.getRarityIcon(data.item)" />
      </template>
      <template v-slot:cell(characterTypeId)="data">
        <img height="20px" :src="cardsHelper.getTypeIcon(data.item)" />
      </template>
      <template v-slot:cell(themeId)="data">
        <img height="20px" :src="cardsHelper.getThemeIcon(data.item)" />
      </template>
      <template v-slot:cell(cardName)="data">{{ data.item.cardName }}</template>
      <template v-slot:cell(level)="data">
        <!-- Editable card levels -->
        <b-dropdown
          v-if="canCurrentUserEdit"
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
        <!-- Read only card levels -->
        <div v-else>{{ data.item.level !== null ? data.item.level : '-' }}</div>
      </template>
      <template
        v-slot:cell(levelLastModified)="data"
      >{{ data.item.levelLastModified == null ? 'never' : data.item.levelLastModified.toLocaleString() }}</template>
    </b-table>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'

// @ts-ignore
import { userService, cardLevelService, authorizationService } from '@/_services'
// @ts-ignore
import { cardIdentifiers } from '@/_constants'
// @ts-ignore
import { cardsHelper } from '@/_helpers'
// @ts-ignore
import { UserResponseCardDto } from '@/api'

export default Vue.extend({
  name: 'UserCardsComponent',
  props: {
    filter: {
      type: Object as () => Filter
    }
  },
  data: function() {
    return {
      userCards: null as UserResponseCardDto[],
      filteredCards: null as UserResponseCardDto[],
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
      cardsHelper: cardsHelper
    }
  },
  computed: {
    userId: function(): string {
      return this.$route.params.userId
    },
    canCurrentUserEdit: function(): boolean {
      return authorizationService.canEditUser(this.userId)
    }
  },
  watch: {
    filter: function(): void {
      this.applyFilter()
    }
  },
  async mounted(): Promise<void> {
    this.userCards = await userService.getCards(this.userId)
    this.applyFilter()
  },
  methods: {
    async updateLevel(card: UserResponseCardDto, level: number): Promise<void> {
      var updateResult = await cardLevelService.setCardLevel(
        this.userId,
        card.cardId,
        level
      )
      card.level = updateResult.level
      card.levelLastModified = updateResult.levelLastModified
    },
    applyFilter(): void {
      if (this.filter && this.userCards) {
        this.filteredCards = this.userCards.filter(this.cardMatchesFilter)
      }
    },
    cardMatchesFilter(card: UserResponseCardDto): boolean {
      return cardsHelper.cardMatchesFilter(card, this.filter)
    }
  }
})
</script>

<style>
td.card-row-slim {
  width: 1px;
}
td.card-row-maxWidth {
  max-width: 100px;
}
</style>
