// Character Types
export interface Character {
  id: string
  faction: string
  race: string
  class: string
  gender: string
  profession1?: string | null
  profession2?: string | null
  subProfession1?: string | null
  subProfession2?: string | null
  generatedAt: string
  savedAt?: string | null
}

// API Request/Response Types
export interface GenerateRandomRequest {
  // No parameters - fully random generation
}

export interface GenerateCustomRequest {
  faction?: string
  race?: string
  class?: string
}

export interface CharacterResponse extends Character {}

export interface CharactersListResponse {
  characters: Character[]
}

// Game Data Types
export interface Faction {
  id: number
  name: string
}

export interface Race {
  id: number
  name: string
  factionName: string
}

export interface Class {
  id: number
  name: string
}

export interface Profession {
  id: number
  name: string
  isPrimary: boolean
}

// API Response Types
export interface FactionsResponse {
  factions: Faction[]
}

export interface RacesResponse {
  races: Race[]
}

export interface ClassesResponse {
  classes: Class[]
}

export interface ProfessionsResponse {
  professions: Profession[]
}
