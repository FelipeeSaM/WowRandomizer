import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { Character, GenerateCustomRequest } from '@/types'
import { characterApi } from '@/services'

export const useCharacterStore = defineStore('character', () => {
  // State
  const characters = ref<Character[]>([])
  const currentCharacter = ref<Character | null>(null)
  const loading = ref(false)
  const error = ref<string | null>(null)

  // Getters
  const hasCharacters = computed(() => characters.value.length > 0)
  const totalCharacters = computed(() => characters.value.length)
  
  const charactersByFaction = computed(() => {
    const grouped = characters.value.reduce((acc: Record<string, Character[]>, char: Character) => {
      if (!acc[char.faction]) {
        acc[char.faction] = []
      }
      acc[char.faction].push(char)
      return acc
    }, {} as Record<string, Character[]>)
    return grouped
  })

  // Actions
  async function generateRandom() {
    loading.value = true
    error.value = null
    try {
      const character = await characterApi.generateRandom()
      currentCharacter.value = character
      // Adiciona à lista local (opcional, depende se quer manter histórico)
      characters.value.unshift(character)
      return character
    } catch (err: any) {
      error.value = err.response?.data?.message || err.message || 'Erro ao gerar personagem'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function generateCustom(params: GenerateCustomRequest) {
    loading.value = true
    error.value = null
    try {
      const character = await characterApi.generateCustom(params)
      currentCharacter.value = character
      characters.value.unshift(character)
      return character
    } catch (err: any) {
      error.value = err.response?.data?.message || err.message || 'Erro ao gerar personagem customizado'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function fetchAll() {
    loading.value = true
    error.value = null
    try {
      const fetchedCharacters = await characterApi.getAll()
      characters.value = fetchedCharacters
    } catch (err: any) {
      error.value = err.response?.data?.message || err.message || 'Erro ao buscar personagens'
      throw err
    } finally {
      loading.value = false
    }
  }

  function clearError() {
    error.value = null
  }

  function clearCurrent() {
    currentCharacter.value = null
  }

  function clearAll() {
    characters.value = []
    currentCharacter.value = null
    error.value = null
  }

  return {
    // State
    characters,
    currentCharacter,
    loading,
    error,
    // Getters
    hasCharacters,
    totalCharacters,
    charactersByFaction,
    // Actions
    generateRandom,
    generateCustom,
    fetchAll,
    clearError,
    clearCurrent,
    clearAll,
  }
})
