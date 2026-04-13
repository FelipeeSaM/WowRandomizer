<script setup lang="ts">
import { RouterLink } from 'vue-router'
</script>

<template>
  <div class="min-h-screen bg-gray-50 dark:bg-gray-900 flex items-center justify-center p-4">
    <div class="text-center space-y-6">
      <div class="text-9xl">😕</div>
      <h1 class="text-4xl font-bold text-gray-900 dark:text-white">
        404 - Page Not Found
      </h1>
      <p class="text-gray-600 dark:text-gray-400 text-lg">
        The page you are looking for does not exist.
      </p>
      <RouterLink
        to="/"
        class="inline-block px-6 py-3 bg-blue-600 hover:bg-blue-700 text-white font-medium rounded-lg shadow-md transform hover:scale-105 transition-all duration-200"
      >
        🏠 Back to Home
      </RouterLink>
    </div>
  </div>
</template>
