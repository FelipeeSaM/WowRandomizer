<script setup lang="ts">
import CharacterGenerator from '@/components/CharacterGenerator.vue'
import CharacterDisplay from '@/components/CharacterDisplay.vue'
import { useCharacterStore } from '@/stores'

const characterStore = useCharacterStore()
</script>

<template>
  <div class="min-h-screen bg-gray-50 dark:bg-gray-900 py-8">
    <div class="container mx-auto px-4 max-w-4xl">
      <!-- Header -->
      <div class="text-center mb-8">
        <h1 class="text-4xl font-bold text-gray-900 dark:text-white mb-2">
          🎲 Gerador de Personagens
        </h1>
        <p class="text-gray-600 dark:text-gray-400">
          Clique no botão abaixo para gerar um novo personagem aleatório
        </p>
      </div>

      <!-- Generator Component -->
      <div class="mb-8">
        <CharacterGenerator />
      </div>

      <!-- Character Display -->
      <div v-if="characterStore.currentCharacter" class="animate-fade-in">
        <CharacterDisplay :character="characterStore.currentCharacter" />
      </div>

      <!-- Empty State -->
      <div 
        v-else 
        class="text-center py-16 bg-white dark:bg-gray-800 rounded-lg shadow-sm border-2 border-dashed border-gray-300 dark:border-gray-700"
      >
        <div class="text-6xl mb-4">🎭</div>
        <p class="text-gray-600 dark:text-gray-400 text-lg">
          Nenhum personagem gerado ainda
        </p>
        <p class="text-gray-500 dark:text-gray-500 text-sm mt-2">
          Clique no botão acima para começar
        </p>
      </div>
    </div>
  </div>
</template>

<style scoped>
@keyframes fade-in {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.animate-fade-in {
  animation: fade-in 0.5s ease-out;
}
</style>
