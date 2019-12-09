import { CardResponseDto } from '@/api'
import { cardIdentifiers } from '@/_constants'

export const cardsHelper = {
  cardMatchesFilter,
  getRarityIcon,
  getThemeIcon,
  getTypeIcon
}

function getRarityIcon(card: CardResponseDto) {
  if (card.rarityId.toUpperCase() === cardIdentifiers.rarityCommonId) {
    return '/img/cards/theme-stones/common/' + card.themeId + '.png'
  } else {
    return '/img/cards/theme-stones/special/' + card.rarityId + '.png'
  }
}

function getThemeIcon(card: CardResponseDto) {
  return '/img/cards/theme-icons/' + card.themeId + '.png'
}

function getTypeIcon(card: CardResponseDto) {
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
}

function cardMatchesFilter(card: CardResponseDto, filter: Filter) {
  return (
    matchesThemeFilter(card, filter) &&
    matchesTypeFilter(card, filter) &&
    matchesRarityFilter(card, filter)
  )
}

function matchesThemeFilter(card: CardResponseDto, filter: Filter) {
  if (
    filter.theme.adventure &&
    card.themeId.toUpperCase() === cardIdentifiers.themeAdventureId
  ) {
    return true
  }
  if (
    filter.theme.fantasy &&
    card.themeId.toUpperCase() === cardIdentifiers.themeFantasyId
  ) {
    return true
  }
  if (
    filter.theme.scifi &&
    card.themeId.toUpperCase() === cardIdentifiers.themeScifiId
  ) {
    return true
  }
  if (
    filter.theme.mystical &&
    card.themeId.toUpperCase() === cardIdentifiers.themeMysticalId
  ) {
    return true
  }
  if (
    filter.theme.superhero &&
    card.themeId.toUpperCase() === cardIdentifiers.themeSuperheroId
  ) {
    return true
  }
  if (
    filter.theme.neutral &&
    card.themeId.toUpperCase() === cardIdentifiers.themeNeutralId
  ) {
    return true
  }
  return false
}

function matchesTypeFilter(card: CardResponseDto, filter: Filter) {
  if (
    filter.type.tank &&
    card.characterTypeId != null &&
    card.characterTypeId.toUpperCase() ===
    cardIdentifiers.characterTypeTankId
  ) {
    return true
  }
  if (
    filter.type.melee &&
    card.characterTypeId != null &&
    card.characterTypeId.toUpperCase() ===
    cardIdentifiers.characterTypeMeleeId
  ) {
    return true
  }
  if (
    filter.type.assassin &&
    card.characterTypeId != null &&
    card.characterTypeId.toUpperCase() ===
    cardIdentifiers.characterTypeAssassinId
  ) {
    return true
  }
  if (
    filter.type.ranged &&
    card.characterTypeId != null &&
    card.characterTypeId.toUpperCase() ===
    cardIdentifiers.characterTypeRangedId
  ) {
    return true
  }
  if (
    filter.type.totem &&
    card.characterTypeId != null &&
    card.characterTypeId.toUpperCase() ===
    cardIdentifiers.characterTypeTotemId
  ) {
    return true
  }
  if (
    filter.type.spell &&
    card.typeId.toUpperCase() === cardIdentifiers.cardTypeSpellId
  ) {
    return true
  }
  if (
    filter.type.trap &&
    card.typeId.toUpperCase() === cardIdentifiers.cardTypeTrapId
  ) {
    return true
  }
  return false
}

function matchesRarityFilter(card: CardResponseDto, filter: Filter) {
  if (
    filter.rarity.common &&
    card.rarityId.toUpperCase() === cardIdentifiers.rarityCommonId
  ) {
    return true
  }
  if (
    filter.rarity.rare &&
    card.rarityId.toUpperCase() === cardIdentifiers.rarityRareId
  ) {
    return true
  }
  if (
    filter.rarity.epic &&
    card.rarityId.toUpperCase() === cardIdentifiers.rarityEpicId
  ) {
    return true
  }
  if (
    filter.rarity.legendary &&
    card.rarityId.toUpperCase() === cardIdentifiers.rarityLegendaryId
  ) {
    return true
  }
  return false
}
