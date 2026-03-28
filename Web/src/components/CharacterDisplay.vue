<script setup lang="ts">
import { computed } from 'vue'
import type { Character } from '@/types'

const props = defineProps<{
  character: Character
}>()

// Emojis e cores por facção
const factionConfig = computed(() => {
  const faction = props.character.faction.toLowerCase()
  if (faction.includes('alliance')) {
    return {
      emoji: '🦁',
      color: 'text-blue-600 dark:text-blue-400',
      bgColor: 'bg-blue-50 dark:bg-blue-900/20',
      borderColor: 'border-blue-300 dark:border-blue-700',
    }
  } else {
    return {
      emoji: '🐺',
      color: 'text-red-600 dark:text-red-400',
      bgColor: 'bg-red-50 dark:bg-red-900/20',
      borderColor: 'border-red-300 dark:border-red-700',
    }
  }
})

// Formatação da data
const formattedDate = computed(() => {
  const date = new Date(props.character.generatedAt)
  return date.toLocaleString('pt-BR', {
    dateStyle: 'short',
    timeStyle: 'short',
  })
})
</script>

<template>
  <div class="bg-white dark:bg-gray-800 rounded-lg shadow-xl overflow-hidden animate-slide-up">
    <!-- Header with Faction -->
    <div 
      class="px-6 py-4 border-b-4"
      :class="[factionConfig.bgColor, factionConfig.borderColor]"
    >
      <div class="flex items-center justify-between">
        <div class="flex items-center space-x-3">
          <span class="text-4xl">{{ factionConfig.emoji }}</span>
          <div>
            <h2 class="text-2xl font-bold" :class="factionConfig.color">
              {{ character.faction }}
            </h2>
            <p class="text-sm text-gray-600 dark:text-gray-400">
              Gerado em {{ formattedDate }}
            </p>
          </div>
        </div>
      </div>
    </div>

    <!-- Character Details -->
    <div class="p-6 space-y-4">
      <!-- Race & Class -->
      <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
        <div class="bg-gray-50 dark:bg-gray-700 rounded-lg p-4">
          <p class="text-sm text-gray-500 dark:text-gray-400 mb-1">Raça</p>
          <p class="text-xl font-bold text-gray-900 dark:text-white">
            {{ character.race }}
          </p>
        </div>

        <div class="bg-gray-50 dark:bg-gray-700 rounded-lg p-4">
          <p class="text-sm text-gray-500 dark:text-gray-400 mb-1">Classe</p>
          <p class="text-xl font-bold text-gray-900 dark:text-white">
            {{ character.class }}
          </p>
        </div>
      </div>

      <!-- Gender -->
      <div class="bg-gray-50 dark:bg-gray-700 rounded-lg p-4">
        <p class="text-sm text-gray-500 dark:text-gray-400 mb-1">Gênero</p>
        <p class="text-xl font-bold text-gray-900 dark:text-white">
          {{ character.gender === 'Male' ? '♂️ Masculino' : '♀️ Feminino' }}
        </p>
      </div>

      <!-- Professions -->
      <div v-if="character.profession1 || character.profession2" class="space-y-2">
        <p class="text-sm font-semibold text-gray-700 dark:text-gray-300">
          🔨 Profissões Principais
        </p>
        <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div v-if="character.profession1" class="bg-amber-50 dark:bg-amber-900/20 rounded-lg p-3 border border-amber-200 dark:border-amber-800">
            <p class="text-amber-900 dark:text-amber-200 font-medium">
              {{ character.profession1 }}
            </p>
          </div>
          <div v-if="character.profession2" class="bg-amber-50 dark:bg-amber-900/20 rounded-lg p-3 border border-amber-200 dark:border-amber-800">
            <p class="text-amber-900 dark:text-amber-200 font-medium">
              {{ character.profession2 }}
            </p>
          </div>
        </div>
      </div>

      <!-- Sub Professions -->
      <div v-if="character.subProfession1 || character.subProfession2" class="space-y-2">
        <p class="text-sm font-semibold text-gray-700 dark:text-gray-300">
          🎣 Profissões Secundárias
        </p>
        <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div v-if="character.subProfession1" class="bg-green-50 dark:bg-green-900/20 rounded-lg p-3 border border-green-200 dark:border-green-800">
            <p class="text-green-900 dark:text-green-200 font-medium">
              {{ character.subProfession1 }}
            </p>
          </div>
          <div v-if="character.subProfession2" class="bg-green-50 dark:bg-green-900/20 rounded-lg p-3 border border-green-200 dark:border-green-800">
            <p class="text-green-900 dark:text-green-200 font-medium">
              {{ character.subProfession2 }}
            </p>
          </div>
        </div>
      </div>

      <!-- Character ID (for debugging) -->
      <div class="pt-4 border-t border-gray-200 dark:border-gray-700">
        <p class="text-xs text-gray-400 dark:text-gray-500 font-mono">
          ID: {{ character.id }}
        </p>
      </div>
    </div>
  </div>
</template>

<style scoped>
@keyframes slide-up {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.animate-slide-up {
  animation: slide-up 0.5s ease-out;
}
</style>
