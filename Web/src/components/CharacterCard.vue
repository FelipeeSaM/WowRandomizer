<script setup lang="ts">
import { computed } from 'vue'
import type { Character } from '@/types'
import { getProfessionEmoji, getClassEmoji, getRaceEmoji } from '../utils/imageHelpers'

const props = defineProps<{
  character: Character
}>()

// Faction configuration
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

const formattedDate = computed(() => {
  const date = new Date(props.character.generatedAt)
  return date.toLocaleDateString('en-US', { dateStyle: 'short' })
})
</script>

<template>
  <div class="bg-white dark:bg-gray-800 rounded-lg shadow-md overflow-hidden hover:shadow-xl transition-shadow duration-300">
    <!-- Header -->
    <div 
      class="px-4 py-3 border-l-4"
      :class="[factionConfig.bgColor, factionConfig.borderColor]"
    >
      <div class="flex items-center space-x-2">
        <span class="text-2xl">{{ factionConfig.emoji }}</span>
        <h3 class="font-bold text-lg" :class="factionConfig.color">
          {{ character.faction }}
        </h3>
      </div>
    </div>

    <!-- Content -->
    <div class="p-4 space-y-3">
      <!-- Race & Class -->
      <div class="flex justify-between items-center">
        <div>
          <p class="text-xs text-gray-500 dark:text-gray-400">Race</p>
          <p class="font-semibold text-gray-900 dark:text-white">{{ character.race }} {{ getRaceEmoji(character.race) }}</p>
        </div>
        <div class="text-right">
          <p class="text-xs text-gray-500 dark:text-gray-400">Class</p>
          <p class="font-semibold text-gray-900 dark:text-white">{{ character.class }} {{ getClassEmoji(character.class) }}</p>
        </div>
      </div>

      <!-- Gender -->
      <div>
        <p class="text-xs text-gray-500 dark:text-gray-400">Gender</p>
        <p class="text-sm text-gray-700 dark:text-gray-300">
          {{ character.gender === 'Male' ? '♂️ Male' : '♀️ Female' }}
        </p>
      </div>

      <!-- Professions -->
      <div v-if="character.profession1 || character.profession2" class="space-y-1">
        <p class="text-xs text-gray-500 dark:text-gray-400">🔨 Professions</p>
        <div class="flex flex-wrap gap-2">
          <span 
            v-if="character.profession1"
            class="px-2 py-1 bg-amber-100 dark:bg-amber-900/30 text-amber-800 dark:text-amber-300 text-xs rounded-full"
          >
            {{ character.profession1 }} {{ getProfessionEmoji(character.profession1) }}
          </span>
          <span 
            v-if="character.profession2"
            class="px-2 py-1 bg-amber-100 dark:bg-amber-900/30 text-amber-800 dark:text-amber-300 text-xs rounded-full"
          >
            {{ character.profession2 }} {{ getProfessionEmoji(character.profession2) }}
          </span>
        </div>
      </div>

      <!-- Date -->
      <div class="pt-2 border-t border-gray-200 dark:border-gray-700">
        <p class="text-xs text-gray-400 dark:text-gray-500">
          📅 {{ formattedDate }}
        </p>
      </div>
    </div>
  </div>
</template>
