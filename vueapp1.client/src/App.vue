<script setup>
  import SoccerGames from './components/SoccerGames.vue'
  import PostsList from './components/PostsList.vue'
</script>

<template>
  <div class="min-h-screen bg-gray-100 text-gray-800">
    <!-- Main Content Section - Using full viewport width -->
    <main class="w-screen px-4 py-8">
      <!-- Side by side container with explicit full width -->
      <div class="flex w-full gap-8">
        <!-- Soccer Games Section - Force expanded width -->
        <div class="w-1/2 pr-4">
          <div class="w-full">
            <SoccerGames />
          </div>
        </div>

        <!-- Posts Section - Force expanded width with max width limit -->
        <div class="w-1/2 pl-4 max-w-[700px]">
          <div class="w-full">
            <PostsList />
          </div>
        </div>
      </div>
    </main>
  </div>
</template>

<style>
  main {
    width: 75vw;
    max-width: 75vw;
    margin: 0;
    box-sizing: border-box;
  }

  .flex {
    display: flex;
    width: 100%;
    max-width: 100%;
  }

  .w-1/2 {
    width: 50%;
    max-width: 50%;
  }

  .gap-8 {
    gap: 2rem;
  }
</style>
