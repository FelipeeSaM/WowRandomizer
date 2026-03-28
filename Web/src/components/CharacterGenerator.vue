<script setup lang="ts">
import { ref } from 'vue'
import { useCharacterStore } from '@/stores'

const characterStore = useCharacterStore()
const isGenerating = ref(false)

async function handleGenerateRandom() {
  isGenerating.value = true
  try {
    await characterStore.generateRandom()
  } catch (error) {
    console.error('Erro ao gerar personagem:', error)
  } finally {
    isGenerating.value = false
  }
}
</script>

<template>
  <div class="bg-white dark:bg-gray-800 rounded-lg shadow-lg p-6">
    <!-- Error Display -->
    <div 
      v-if="characterStore.error" 
      class="mb-4 p-4 bg-red-50 dark:bg-red-900/20 border border-red-300 dark:border-red-700 rounded-md"
    >
      <p class="text-red-800 dark:text-red-300 text-sm">
        ❌ {{ characterStore.error }}
      </p>
      <button
        @click="characterStore.clearError"
        class="mt-2 text-sm text-red-600 dark:text-red-400 hover:underline"
      >
        Fechar
      </button>
    </div>

    <!-- Generate Button -->
    <div class="text-center">
      <button
        @click="handleGenerateRandom"
        :disabled="isGenerating || characterStore.loading"
        class="relative px-8 py-4 bg-gradient-to-r from-blue-600 to-purple-600 hover:from-blue-700 hover:to-purple-700 disabled:from-gray-400 disabled:to-gray-500 text-white font-bold rounded-lg shadow-lg transform hover:scale-105 active:scale-95 disabled:scale-100 transition-all duration-200 disabled:cursor-not-allowed"
      >
        <!-- Loading Spinner -->
        <span v-if="isGenerating || characterStore.loading" class="flex items-center justify-center">
          <svg 
            class="animate-spin -ml-1 mr-3 h-5 w-5 text-white" 
            xmlns="http://www.w3.org/2000/svg" 
            fill="none" 
            viewBox="0 0 24 24"
          >
            <circle 
              class="opacity-25" 
              cx="12" 
              cy="12" 
              r="10" 
              stroke="currentColor" 
              stroke-width="4"
            />
            <path 
              class="opacity-75" 
              fill="currentColor" 
              d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
            />
          </svg>
          Gerando...
        </span>
        
        <!-- Normal State -->
        <span v-else class="flex items-center justify-center">
          🎲 Gerar Personagem Aleatório
        </span>
      </button>

      <p class="text-sm text-gray-500 dark:text-gray-400 mt-4">
        Clique para gerar um personagem completamente aleatório
      </p>
    </div>

    <!-- Stats (opcional) -->
    <div 
      v-if="characterStore.totalCharacters > 0" 
      class="mt-6 pt-6 border-t border-gray-200 dark:border-gray-700 text-center"
    >
      <p class="text-sm text-gray-600 dark:text-gray-400">
        Você já gerou <strong class="text-blue-600 dark:text-blue-400">{{ characterStore.totalCharacters }}</strong> personagem(ns)!
      </p>
    </div>
  </div>
</template>
