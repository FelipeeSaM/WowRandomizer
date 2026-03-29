<template>
  <div class="max-w-xl mx-auto p-6 bg-white dark:bg-gray-800 rounded-lg shadow-md">
    <h2 class="text-2xl font-bold mb-4">Gerar Personagem Customizado</h2>
    <form @submit.prevent="onSubmit" class="space-y-4">
      <!-- Facção -->
      <div>
        <label class="block font-medium mb-1">Facção</label>
        <div class="flex gap-4">
          <label class="flex items-center gap-1">
            <input type="radio" value="Alliance" v-model="form.faction" @change="onFactionChange" /> Aliança
          </label>
          <label class="flex items-center gap-1">
            <input type="radio" value="Horde" v-model="form.faction" @change="onFactionChange" /> Horda
          </label>
        </div>
      </div>
      <!-- Raça -->
      <div>
        <label class="block font-medium mb-1">Raça</label>
        <select v-model="form.race" @change="onRaceChange" class="w-full p-2 rounded border">
          <option value="" disabled>Selecione uma raça</option>
          <option v-for="race in races" :key="race.name" :value="race.name">{{ race.name }}</option>
        </select>
      </div>
      <!-- Classe -->
      <div>
        <label class="block font-medium mb-1">Classe</label>
        <select v-model="form.class" class="w-full p-2 rounded border">
          <option value="" disabled>Selecione uma classe</option>
          <option v-for="cls in classes" :key="cls.name" :value="cls.name">{{ cls.name }}</option>
        </select>
      </div>
      <!-- Profissões Primárias -->
      <div>
        <label class="block font-medium mb-1">Profissões Primárias (até 2)</label>
        <select v-model="form.primaryProfessions" multiple class="w-full p-2 rounded border">
          <option v-for="prof in primaryProfessions" :key="prof.name" :value="prof.name">{{ prof.name }}</option>
        </select>
        <small v-if="form.primaryProfessions.length > 2" class="text-red-500">Selecione no máximo 2 profissões primárias.</small>
      </div>
      <!-- Profissões Secundárias -->
      <div>
        <label class="block font-medium mb-1">Profissões Secundárias (até 2)</label>
        <select v-model="form.secondaryProfessions" multiple class="w-full p-2 rounded border">
          <option v-for="prof in secondaryProfessions" :key="prof.name" :value="prof.name">{{ prof.name }}</option>
        </select>
        <small v-if="form.secondaryProfessions.length > 2" class="text-red-500">Selecione no máximo 2 profissões secundárias.</small>
      </div>
      <div class="flex gap-4 mt-6">
        <button type="submit" class="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700">Gerar Personagem</button>
        <button type="button" @click="onRandom" class="bg-gray-400 text-white px-4 py-2 rounded hover:bg-gray-500">Gerar Aleatório</button>
      </div>
      <div v-if="warning" class="mt-4 text-yellow-700 bg-yellow-100 border border-yellow-300 rounded p-2">
        {{ warning }}
      </div>
    </form>
    <div v-if="character" class="mt-8">
      <CharacterDisplay :character="character" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'
import { gameDataApi, characterApi } from '@/services/api'
import CharacterDisplay from '@/components/CharacterDisplay.vue'

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

function onFactionChange() {
  loadRaces()
}
function onRaceChange() {
  loadClasses()
}

async function onSubmit() {
  warning.value = ''
  if (!form.value.faction && !form.value.race && !form.value.class && form.value.primaryProfessions.length === 0 && form.value.secondaryProfessions.length === 0) {
    warning.value = 'Selecione ao menos um parâmetro ou clique em "Gerar Aleatório".'
    return
  }
  if (form.value.primaryProfessions.length > 2 || form.value.secondaryProfessions.length > 2) {
    warning.value = 'Selecione no máximo 2 profissões primárias e 2 secundárias.'
    return
  }
  const params: any = {}
  if (form.value.faction) params.factionName = form.value.faction
  if (form.value.race) params.raceName = form.value.race
  if (form.value.class) params.className = form.value.class
  if (form.value.primaryProfessions.length > 0) params.profession1 = form.value.primaryProfessions[0]
  if (form.value.primaryProfessions.length > 1) params.profession2 = form.value.primaryProfessions[1]
  if (form.value.secondaryProfessions.length > 0) params.subProfession1 = form.value.secondaryProfessions[0]
  if (form.value.secondaryProfessions.length > 1) params.subProfession2 = form.value.secondaryProfessions[1]
  console.log('Enviando parâmetros para geração:', params)
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
