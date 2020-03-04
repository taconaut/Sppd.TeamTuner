import { CardResponseDto } from '@/api'
import { cardIdentifiers } from '@/_constants'

export const cardsHelper = {
  cardMatchesFilter,
  getRarityIcon,
  getThemeIcon,
  getTypeIcon
}

function getRarityIcon(card: CardResponseDto): string {
  if (card.rarityId === cardIdentifiers.rarityCommonId) {
    return '/img/cards/theme-stones/common/' + card.themeId + '.png'
  } else {
    return '/img/cards/theme-stones/special/' + card.rarityId + '.png'
  }
}

function getThemeIcon(card: CardResponseDto): string {
  return '/img/cards/theme-icons/' + card.themeId + '.png'
}

function getTypeIcon(card: CardResponseDto): string {
  var typeId =
    card.typeId === cardIdentifiers.cardTypeSpellId ||
      card.typeId === cardIdentifiers.cardTypeTrapId
      ? card.typeId
      : card.characterTypeId
  if (card.rarityId === cardIdentifiers.rarityCommonId) {
    return ''.concat('/img/cards/type-icons/', typeId, '/common/', card.themeId, '.png')
  } else {
    return ''.concat('/img/cards/type-icons/', typeId, '/special/', card.rarityId, '.png')
  }
}

function cardMatchesFilter(card: CardResponseDto, filter: Filter): boolean {
  return (
    matchesThemeFilter(card, filter) &&
    matchesTypeFilter(card, filter) &&
    matchesRarityFilter(card, filter)
  )
}

function matchesThemeFilter(card: CardResponseDto, filter: Filter): boolean {
  if (filter.theme.adventure && card.themeId === cardIdentifiers.themeAdventureId) {
    return true
  }
  if (filter.theme.fantasy && card.themeId === cardIdentifiers.themeFantasyId) {
    return true
  }
  if (filter.theme.scifi && card.themeId === cardIdentifiers.themeScifiId) {
    return true
  }
  if (filter.theme.mystical && card.themeId === cardIdentifiers.themeMysticalId) {
    return true
  }
  if (filter.theme.superhero && card.themeId === cardIdentifiers.themeSuperheroId) {
    return true
  }
  if (filter.theme.neutral && card.themeId === cardIdentifiers.themeNeutralId) {
    return true
  }
  return false
}

function matchesTypeFilter(card: CardResponseDto, filter: Filter): boolean {
  if (
    filter.type.tank &&
    card.characterTypeId != null &&
    card.characterTypeId ===
    cardIdentifiers.characterTypeTankId
  ) {
    return true
  }
  if (
    filter.type.melee &&
    card.characterTypeId != null &&
    card.characterTypeId ===
    cardIdentifiers.characterTypeMeleeId
  ) {
    return true
  }
  if (
    filter.type.assassin &&
    card.characterTypeId != null &&
    card.characterTypeId ===
    cardIdentifiers.characterTypeAssassinId
  ) {
    return true
  }
  if (
    filter.type.ranged &&
    card.characterTypeId != null &&
    card.characterTypeId ===
    cardIdentifiers.characterTypeRangedId
  ) {
    return true
  }
  if (
    filter.type.totem &&
    card.characterTypeId != null &&
    card.characterTypeId ===
    cardIdentifiers.characterTypeTotemId
  ) {
    return true
  }
  if (filter.type.spell && card.typeId === cardIdentifiers.cardTypeSpellId) {
    return true
  }
  if (filter.type.trap && card.typeId === cardIdentifiers.cardTypeTrapId) {
    return true
  }
  return false
}

function matchesRarityFilter(card: CardResponseDto, filter: Filter): boolean {
  if (filter.rarity.common && card.rarityId === cardIdentifiers.rarityCommonId) {
    return true
  }
  if (filter.rarity.rare && card.rarityId === cardIdentifiers.rarityRareId) {
    return true
  }
  if (filter.rarity.epic && card.rarityId === cardIdentifiers.rarityEpicId) {
    return true
  }
  if (filter.rarity.legendary && card.rarityId === cardIdentifiers.rarityLegendaryId) {
    return true
  }
  return false
}
