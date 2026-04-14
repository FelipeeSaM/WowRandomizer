<script setup lang="ts">
import { onMounted } from 'vue'
import { useCharacterStore } from '@/stores'
import CharacterCard from '@/components/CharacterCard.vue'

const characterStore = useCharacterStore()

onMounted(async () => {
  try {
    await characterStore.fetchAll()
  } catch (error) {
    console.error('Error loading characters:', error)
  }
})
</script>

<template>
  <div class="min-h-screen bg-gray-50 dark:bg-gray-900 py-8">
    <div class="container mx-auto px-4 max-w-6xl">
      <!-- Header -->
      <div class="mb-8">
        <h1 class="text-4xl font-bold text-gray-900 dark:text-white mb-2">
          📜 Generated Characters
        </h1>
        <p class="text-gray-600 dark:text-gray-400">
          Total: {{ characterStore.totalCharacters }} characters
        </p>
      </div>

      <!-- Loading -->
      <div v-if="characterStore.loading" class="text-center py-16">
        <div class="animate-spin rounded-full h-16 w-16 border-b-4 border-blue-600 mx-auto"></div>
        <p class="text-gray-600 dark:text-gray-400 mt-4">Loading characters...</p>
      </div>

      <!-- Error -->
      <div v-else-if="characterStore.error" class="bg-red-50 dark:bg-red-900/20 border border-red-300 dark:border-red-700 rounded-lg p-4">
        <p class="text-red-800 dark:text-red-300">❌ {{ characterStore.error }}</p>
      </div>

      <!-- Empty State -->
      <div 
        v-else-if="!characterStore.hasCharacters" 
        class="text-center py-16 bg-white dark:bg-gray-800 rounded-lg shadow-sm"
      >
        <div class="text-6xl mb-4">📭</div>
        <p class="text-gray-600 dark:text-gray-400 text-lg">
          No characters found
        </p>
        <p class="text-gray-500 dark:text-gray-500 text-sm mt-2">
          Generate your first character!
        </p>
      </div>

      <!-- Characters List -->
      <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        <CharacterCard
          v-for="character in characterStore.characters"
          :key="character.id"
          :character="character"
        />
      </div>
    </div>
  </div>
</template>
