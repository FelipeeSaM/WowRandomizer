<template>
  <div class="min-h-screen bg-gray-50 dark:bg-gray-900 py-8">
    <div class="container mx-auto px-4 max-w-3xl">

      <!-- Header -->
      <div class="mb-8">
        <h1 class="text-4xl font-bold text-gray-900 dark:text-white mb-2">⚙️ Custom Character</h1>
        <p class="text-gray-600 dark:text-gray-400">Choose your preferences and generate a tailored character.</p>
      </div>

      <form @submit.prevent="onSubmit" class="space-y-5">

        <!-- Faction -->
        <div class="bg-white dark:bg-gray-800 rounded-xl p-5 shadow-sm border border-gray-200 dark:border-gray-700">
          <p class="font-semibold text-gray-700 dark:text-gray-300 mb-3">Faction</p>
          <div class="grid grid-cols-2 gap-4">
            <!-- Alliance -->
            <label class="cursor-pointer">
              <input type="radio" value="Alliance" v-model="form.faction" @change="onFactionChange" class="sr-only" />
              <div :class="['rounded-xl border-2 p-4 flex items-center gap-3 transition-all',
                form.faction === 'Alliance'
                  ? 'border-blue-500 bg-blue-50 dark:bg-blue-900/30'
                  : 'border-gray-200 dark:border-gray-600 hover:border-blue-300 dark:hover:border-blue-700']">
                <span class="text-3xl">🦁</span>
                <div>
                  <p :class="['font-bold text-lg transition-colors',
                    form.faction === 'Alliance' ? 'text-blue-600 dark:text-blue-400' : 'text-gray-700 dark:text-gray-300']">Alliance</p>
                  <p class="text-xs text-gray-500 dark:text-gray-400">For the king!</p>
                </div>
                <div class="ml-auto">
                  <div :class="['w-5 h-5 rounded-full border-2 flex items-center justify-center transition-all',
                    form.faction === 'Alliance' ? 'border-blue-500 bg-blue-500' : 'border-gray-300 dark:border-gray-500']">
                    <div v-if="form.faction === 'Alliance'" class="w-2 h-2 rounded-full bg-white"></div>
                  </div>
                </div>
              </div>
            </label>
            <!-- Horde -->
            <label class="cursor-pointer">
              <input type="radio" value="Horde" v-model="form.faction" @change="onFactionChange" class="sr-only" />
              <div :class="['rounded-xl border-2 p-4 flex items-center gap-3 transition-all',
                form.faction === 'Horde'
                  ? 'border-red-500 bg-red-50 dark:bg-red-900/30'
                  : 'border-gray-200 dark:border-gray-600 hover:border-red-300 dark:hover:border-red-700']">
                <span class="text-3xl">🐺</span>
                <div>
                  <p :class="['font-bold text-lg transition-colors',
                    form.faction === 'Horde' ? 'text-red-600 dark:text-red-400' : 'text-gray-700 dark:text-gray-300']">Horde</p>
                  <p class="text-xs text-gray-500 dark:text-gray-400">Lok'tar Ogar!</p>
                </div>
                <div class="ml-auto">
                  <div :class="['w-5 h-5 rounded-full border-2 flex items-center justify-center transition-all',
                    form.faction === 'Horde' ? 'border-red-500 bg-red-500' : 'border-gray-300 dark:border-gray-500']">
                    <div v-if="form.faction === 'Horde'" class="w-2 h-2 rounded-full bg-white"></div>
                  </div>
                </div>
              </div>
            </label>
          </div>
        </div>

        <!-- Race & Class -->
        <div class="grid grid-cols-1 md:grid-cols-2 gap-5">
          <!-- Race -->
          <div class="bg-white dark:bg-gray-800 rounded-xl p-5 shadow-sm border border-gray-200 dark:border-gray-700">
            <label class="block font-semibold text-gray-700 dark:text-gray-300 mb-3">Race</label>
            <div class="relative">
              <select
                v-model="form.race"
                @change="onRaceChange"
                class="w-full appearance-none bg-gray-50 dark:bg-gray-700 border border-gray-300 dark:border-gray-600 text-gray-900 dark:text-white rounded-lg px-4 py-3 pr-10 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent transition cursor-pointer"
              >
                <option value="" disabled>— Select a race —</option>
                <option v-for="race in races" :key="race.name" :value="race.name">{{ getRaceEmoji(race.name) }} {{ race.name }}</option>
              </select>
              <div class="pointer-events-none absolute inset-y-0 right-0 flex items-center px-3 text-gray-400">
                <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
                </svg>
              </div>
            </div>
          </div>
          <!-- Class -->
          <div class="bg-white dark:bg-gray-800 rounded-xl p-5 shadow-sm border border-gray-200 dark:border-gray-700">
            <label class="block font-semibold text-gray-700 dark:text-gray-300 mb-3">Class</label>
            <div class="relative">
              <select
                v-model="form.class"
                class="w-full appearance-none bg-gray-50 dark:bg-gray-700 border border-gray-300 dark:border-gray-600 text-gray-900 dark:text-white rounded-lg px-4 py-3 pr-10 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent transition cursor-pointer"
              >
                <option value="" disabled>— Select a class —</option>
                <option v-for="cls in classes" :key="cls.name" :value="cls.name">{{ getClassEmoji(cls.name) }} {{ cls.name }}</option>
              </select>
              <div class="pointer-events-none absolute inset-y-0 right-0 flex items-center px-3 text-gray-400">
                <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
                </svg>
              </div>
            </div>
          </div>
        </div>

        <!-- Primary Professions -->
        <div class="bg-white dark:bg-gray-800 rounded-xl p-5 shadow-sm border border-gray-200 dark:border-gray-700">
          <div class="flex items-center justify-between mb-3">
            <p class="font-semibold text-gray-700 dark:text-gray-300">🔨 Primary Professions</p>
            <span :class="['text-sm font-medium px-2 py-0.5 rounded-full transition-colors',
              form.primaryProfessions.length === 2
                ? 'bg-amber-100 text-amber-700 dark:bg-amber-900/40 dark:text-amber-300'
                : 'bg-gray-100 text-gray-500 dark:bg-gray-700 dark:text-gray-400']">
              {{ form.primaryProfessions.length }}/2
            </span>
          </div>
          <div class="flex flex-wrap gap-2">
            <label
              v-for="prof in primaryProfessions"
              :key="prof.name"
              :class="['select-none', isDisabledPrimary(prof.name) ? 'cursor-not-allowed opacity-40' : 'cursor-pointer']"
            >
              <input
                type="checkbox"
                :value="prof.name"
                v-model="form.primaryProfessions"
                :disabled="isDisabledPrimary(prof.name)"
                class="sr-only"
              />
              <span :class="['inline-flex items-center gap-1.5 px-3 py-2 rounded-lg border-2 text-sm font-medium transition-all',
                form.primaryProfessions.includes(prof.name)
                  ? 'border-amber-500 bg-amber-50 text-amber-700 dark:bg-amber-900/30 dark:text-amber-300 dark:border-amber-500'
                  : 'border-gray-200 dark:border-gray-600 text-gray-600 dark:text-gray-400 hover:border-amber-300 dark:hover:border-amber-600']">
                {{ getProfessionEmoji(prof.name) }} {{ prof.name }}
              </span>
            </label>
          </div>
        </div>

        <!-- Secondary Professions -->
        <div class="bg-white dark:bg-gray-800 rounded-xl p-5 shadow-sm border border-gray-200 dark:border-gray-700">
          <div class="flex items-center justify-between mb-3">
            <p class="font-semibold text-gray-700 dark:text-gray-300">🎣 Secondary Professions</p>
            <span :class="['text-sm font-medium px-2 py-0.5 rounded-full transition-colors',
              form.secondaryProfessions.length === 2
                ? 'bg-green-100 text-green-700 dark:bg-green-900/40 dark:text-green-300'
                : 'bg-gray-100 text-gray-500 dark:bg-gray-700 dark:text-gray-400']">
              {{ form.secondaryProfessions.length }}/2
            </span>
          </div>
          <div class="flex flex-wrap gap-2">
            <label
              v-for="prof in secondaryProfessions"
              :key="prof.name"
              :class="['select-none', isDisabledSecondary(prof.name) ? 'cursor-not-allowed opacity-40' : 'cursor-pointer']"
            >
              <input
                type="checkbox"
                :value="prof.name"
                v-model="form.secondaryProfessions"
                :disabled="isDisabledSecondary(prof.name)"
                class="sr-only"
              />
              <span :class="['inline-flex items-center gap-1.5 px-3 py-2 rounded-lg border-2 text-sm font-medium transition-all',
                form.secondaryProfessions.includes(prof.name)
                  ? 'border-green-500 bg-green-50 text-green-700 dark:bg-green-900/30 dark:text-green-300 dark:border-green-500'
                  : 'border-gray-200 dark:border-gray-600 text-gray-600 dark:text-gray-400 hover:border-green-300 dark:hover:border-green-600']">
                {{ getProfessionEmoji(prof.name) }} {{ prof.name }}
              </span>
            </label>
          </div>
        </div>

        <!-- Actions -->
        <div class="flex gap-4">
          <button
            type="submit"
            class="flex-1 bg-gradient-to-r from-blue-600 to-purple-600 hover:from-blue-700 hover:to-purple-700 text-white font-bold py-3 rounded-xl shadow-md transition-all transform hover:scale-[1.02] active:scale-[0.98]"
          >
            ⚙️ Generate Character
          </button>
          <button
            type="button"
            @click="onRandom"
            class="flex-1 bg-gradient-to-r from-gray-500 to-gray-600 hover:from-gray-600 hover:to-gray-700 text-white font-bold py-3 rounded-xl shadow-md transition-all transform hover:scale-[1.02] active:scale-[0.98]"
          >
            🎲 Generate Random
          </button>
        </div>

        <!-- Warning -->
        <div v-if="warning" class="p-4 bg-yellow-50 dark:bg-yellow-900/20 border border-yellow-300 dark:border-yellow-700 rounded-xl text-sm text-yellow-700 dark:text-yellow-300">
          ⚠️ {{ warning }}
        </div>

      </form>

      <!-- Character Result -->
      <div v-if="character" class="mt-8">
        <CharacterDisplay :character="character" />
      </div>

    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'
import { gameDataApi, characterApi } from '@/services/api'
import CharacterDisplay from '@/components/CharacterDisplay.vue'
import { getProfessionEmoji, getRaceEmoji, getClassEmoji } from '@/utils/imageHelpers'

const form = ref({
  faction: '',
  race: '',
  class: '',
  primaryProfessions: [] as string[],
  secondaryProfessions: [] as string[],
})

const races = ref<{ name: string }[]>([])
const classes = ref<{ name: string }[]>([])
const professions = ref<{ name: string, isPrimary: boolean }[]>([])
const primaryProfessions = ref<{ name: string }[]>([])
const secondaryProfessions = ref<{ name: string }[]>([])
const character = ref<any>(null)
const warning = ref('')

async function loadRaces() {
  if (!form.value.faction) { races.value = []; return }
  const res = await gameDataApi.getRaces(form.value.faction)
  races.value = res.races
  form.value.race = ''
  classes.value = []
  form.value.class = ''
}

async function loadClasses() {
  if (!form.value.race) { classes.value = []; return }
  const res = await gameDataApi.getClasses(form.value.race)
  classes.value = res.classes
  form.value.class = ''
}

async function loadProfessions() {
  const res = await gameDataApi.getProfessions()
  professions.value = res.professions
  primaryProfessions.value = professions.value.filter(p => p.isPrimary)
  secondaryProfessions.value = professions.value.filter(p => !p.isPrimary)
}

function isDisabledPrimary(name: string): boolean {
  return !form.value.primaryProfessions.includes(name) && form.value.primaryProfessions.length >= 2
}

function isDisabledSecondary(name: string): boolean {
  return !form.value.secondaryProfessions.includes(name) && form.value.secondaryProfessions.length >= 2
}

function onFactionChange() {
  loadRaces()
}
function onRaceChange() {
  loadClasses()
}

async function onSubmit() {
  warning.value = ''
  if (!form.value.faction && !form.value.race && !form.value.class && form.value.primaryProfessions.length === 0 && form.value.secondaryProfessions.length === 0) {
    warning.value = 'Select at least one parameter or click "Generate Random".'
    return
  }
  const params: any = {  }
  if (form.value.faction) params.factionName = form.value.faction
  if (form.value.race) params.raceName = form.value.race
  if (form.value.class) params.className = form.value.class
    params.mainProfession = form.value.primaryProfessions.map(name => {
    const p = professions.value.find(pr => pr.name === name)
    return {
      name: p?.name ?? name,
      isPrimary: true
    }
  })

  params.subProfession = form.value.secondaryProfessions.map(name => {
    const p = professions.value.find(pr => pr.name === name)
    return {
      name: p?.name ?? name,
      isPrimary: false
    }
  })
  console.log('Sending generation parameters:', params)
  const res = await characterApi.generateCustom(params)
  character.value = res
}

async function onRandom() {
  warning.value = ''
  const res = await characterApi.generateRandom()
  character.value = res
}

watch(() => form.value.faction, loadRaces)
watch(() => form.value.race, loadClasses)

loadProfessions()
</script>
