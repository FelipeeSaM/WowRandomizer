import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { Faction, Race, Class, Profession } from '@/types'
import { gameDataApi } from '@/services'

export const useGameDataStore = defineStore('gameData', () => {
  // State
  const factions = ref<Faction[]>([])
  const races = ref<Race[]>([])
  const classes = ref<Class[]>([])
  const professions = ref<Profession[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)

  // Actions
  async function fetchFactions() {
    if (factions.value.length > 0) return // Cache simples
    
    loading.value = true
    error.value = null
    try {
      const response = await gameDataApi.getFactions()
      factions.value = response.factions
    } catch (err: any) {
      error.value = err.response?.data?.message || err.message || 'Erro ao buscar facções'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function fetchRaces(factionName?: string) {
    loading.value = true
    error.value = null
    try {
      const response = await gameDataApi.getRaces(factionName)
      races.value = response.races
    } catch (err: any) {
      error.value = err.response?.data?.message || err.message || 'Erro ao buscar raças'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function fetchClasses(raceName?: string) {
    loading.value = true
    error.value = null
    try {
      const response = await gameDataApi.getClasses(raceName)
      classes.value = response.classes
    } catch (err: any) {
      error.value = err.response?.data?.message || err.message || 'Erro ao buscar classes'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function fetchProfessions() {
    if (professions.value.length > 0) return // Cache simples
    
    loading.value = true
    error.value = null
    try {
      const response = await gameDataApi.getProfessions()
      professions.value = response.professions
    } catch (err: any) {
      error.value = err.response?.data?.message || err.message || 'Erro ao buscar profissões'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function loadAllGameData() {
    await Promise.all([
      fetchFactions(),
      fetchRaces(),
      fetchClasses(),
      fetchProfessions(),
    ])
  }

  function clearError() {
    error.value = null
  }

  return {
    // State
    factions,
    races,
    classes,
    professions,
    loading,
    error,
    // Actions
    fetchFactions,
    fetchRaces,
    fetchClasses,
    fetchProfessions,
    loadAllGameData,
    clearError,
  }
})
