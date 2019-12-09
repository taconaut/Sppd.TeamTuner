type Filter = {
  theme: ThemeFilter
  type: TypeFilter
  rarity: RarityFilter
}

type ThemeFilter = {
  adventure: boolean
  fantasy: boolean
  scifi: boolean
  mystical: boolean
  neutral: boolean
  superheroe: boolean
}

type TypeFilter = {
  assassin: boolean
  melee: boolean
  ranged: boolean
  tank: boolean
  totem: boolean
  spell: boolean
  trap: boolean
}

type RarityFilter = {
  common: boolean
  rare: boolean
  epic: boolean
  legendary: boolean
}