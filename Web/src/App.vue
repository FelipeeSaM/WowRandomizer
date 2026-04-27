<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <div class="app">
    <!-- Navigation -->
    <nav class="bg-white dark:bg-gray-800 shadow-lg sticky top-0 z-50">
      <div class="container mx-auto px-4">
        <div class="flex items-center justify-between h-16">
          <!-- Logo -->
          <RouterLink 
            to="/" 
            class="text-2xl font-bold bg-gradient-to-r from-blue-600 to-purple-600 bg-clip-text text-transparent hover:from-blue-500 hover:to-purple-500 transition-all"
          >
            ⚔️ WowRandomizer
          </RouterLink>

          <!-- Navigation Links -->
          <div class="flex space-x-4">
            <RouterLink
              to="/"
              class="px-4 py-2 rounded-md text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-gray-700 transition-colors font-medium"
              active-class="bg-blue-100 dark:bg-blue-900 text-blue-700 dark:text-blue-300"
            >
              🏠 Home
            </RouterLink>
            <RouterLink
              to="/generate"
              class="px-4 py-2 rounded-md text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-gray-700 transition-colors font-medium"
              active-class="bg-blue-100 dark:bg-blue-900 text-blue-700 dark:text-blue-300"
            >
              🎲 Generate
            </RouterLink>
            <RouterLink
              to="/characters"
              class="px-4 py-2 rounded-md text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-gray-700 transition-colors font-medium"
              active-class="bg-blue-100 dark:bg-blue-900 text-blue-700 dark:text-blue-300"
            >
              📜 Characters
            </RouterLink>
            <RouterLink
              to="/custom"
              class="px-4 py-2 rounded-md text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-gray-700 transition-colors font-medium"
              active-class="bg-blue-100 dark:bg-blue-900 text-blue-700 dark:text-blue-300"
            >
              🛠️ Customize
            </RouterLink>
          </div>
        </div>
      </div>
    </nav>

    <!-- Main Content -->
    <main>
      <RouterView />
    </main>

    <!-- Footer -->
    <footer class="bg-gray-800 text-gray-300 py-6">
      <div class="container mx-auto px-4 text-center">
        <p class="text-sm">
          ⚔️ WowRandomizer - Developed by 
          <a 
            href="https://github.com/FelipeeSaM" 
            target="_blank"
            class="text-blue-400 hover:text-blue-300 font-medium"
          >
            FelipeeSaM
          </a>
        </p>
      </div>
    </footer>
  </div>
</template>

<style scoped>
.app {
  height: 100vh;
  overflow: hidden;
  display: flex;
  flex-direction: column;
}

main {
  flex: 1;
  overflow-y: auto;
}
</style>
