<script setup lang="ts">
import { RouterLink } from 'vue-router'
</script>

<template>
  <div class="min-h-screen bg-gradient-to-br from-blue-900 via-purple-900 to-red-900">
    <div class="container mx-auto px-4 py-16">
      <div class="text-center space-y-8">
        <!-- Hero Section -->
        <div class="space-y-4">
          <h1 class="text-6xl font-bold text-white drop-shadow-lg">
            ⚔️ WowRandomizer
          </h1>
          <p class="text-xl text-gray-200 max-w-2xl mx-auto">
            Gere personagens aleatórios de World of Warcraft e descubra novas combinações de raças, classes e profissões!
          </p>
        </div>

        <!-- CTA Buttons -->
        <div class="flex flex-col sm:flex-row gap-4 justify-center items-center">
          <RouterLink
            to="/generate"
            class="px-8 py-4 bg-blue-600 hover:bg-blue-700 text-white font-bold rounded-lg shadow-lg transform hover:scale-105 transition-all duration-200"
          >
            🎲 Gerar Personagem
          </RouterLink>
          
          <RouterLink
            to="/characters"
            class="px-8 py-4 bg-purple-600 hover:bg-purple-700 text-white font-bold rounded-lg shadow-lg transform hover:scale-105 transition-all duration-200"
          >
            📜 Ver Personagens
          </RouterLink>
        </div>

        <!-- Features -->
        <div class="grid grid-cols-1 md:grid-cols-3 gap-6 mt-16 max-w-4xl mx-auto">
          <div class="bg-white/10 backdrop-blur-sm rounded-lg p-6 text-white">
            <div class="text-4xl mb-2">🎲</div>
            <h3 class="text-lg font-bold mb-2">Totalmente Aleatório</h3>
            <p class="text-sm text-gray-300">Deixe a sorte decidir tudo: facção, raça, classe e profissões!</p>
          </div>

          <div class="bg-white/10 backdrop-blur-sm rounded-lg p-6 text-white">
            <div class="text-4xl mb-2">⚙️</div>
            <h3 class="text-lg font-bold mb-2">Customizável</h3>
            <p class="text-sm text-gray-300">Escolha sua facção ou classe favorita e randomize o resto.</p>
          </div>

          <div class="bg-white/10 backdrop-blur-sm rounded-lg p-6 text-white">
            <div class="text-4xl mb-2">💾</div>
            <h3 class="text-lg font-bold mb-2">Salvo Automaticamente</h3>
            <p class="text-sm text-gray-300">Todos os personagens são salvos e você pode visualizá-los depois.</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
