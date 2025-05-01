<template>
  <div class="soccer-component">
    <h1 class="text-3xl font-bold mb-6">Today's Soccer Games</h1>

    <!-- Loading State -->
    <div v-if="loading" class="flex flex-col items-center py-8">
      <div class="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-blue-500"></div>
      <p class="mt-4 text-gray-600">Loading matches...</p>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="bg-red-100 border-l-4 border-red-500 text-red-700 p-4 mb-6">
      <p>{{ error }}</p>
    </div>

    <!-- No Matches Found -->
    <div v-else-if="games.length === 0" class="text-center p-8 bg-gray-50 rounded-lg">
      <p class="text-gray-500">No matches scheduled for today.</p>
    </div>

    <!-- Soccer Games Display -->
    <div v-else class="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
      <div v-for="(game, index) in games" :key="index" class="soccer-game-card">
        <div class="p-4">
          <div class="mb-2">
            <strong>{{ game.leagueData.name }}</strong> - {{ new Date(game.fixtureData.date).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }) }}
          </div>

          <div class="mb-2">
            <strong>Home Team:</strong> {{ game.teams.home.name }} (Rank: {{ game.teamInfo[0].rank }}), Pts: {{ game.teamInfo[0].points }}, GD: {{ game.teamInfo[0].goalsDiff }}, Form: {{ formatForm(game.teamInfo[0].form) }}
          </div>

          <div class="mb-2">
            <strong>Away Team:</strong> {{ game.teams.away.name }} (Rank: {{ game.teamInfo[1].rank }}), Pts: {{ game.teamInfo[1].points }}, GD: {{ game.teamInfo[1].goalsDiff }}, Form: {{ formatForm(game.teamInfo[1].form) }}
          </div>

          <div class="mb-2">
            <strong>Status:</strong> {{ game.fixtureData.status.long }}
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
  /* Flexbox container for soccer component */
  .soccer-component {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: flex-start;
    width: 100%;
    gap: 16px;
  }

  .soccer-game-card {
    background-color: white;
    border-radius: 8px;
    padding: 20px;
    margin-bottom: 16px;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    width: 100%;
    max-width: 100%;
    margin: 0 auto;
  }

  /* For grid layout */
  .grid {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
    gap: 20px;
    width: 100%; /* Ensure grid takes the full width */
  }

  .soccer-game-card h2 {
    font-size: 1.5rem;
    margin-bottom: 8px;
    font-weight: 600;
  }

  .soccer-game-card p {
    margin: 5px 0;
  }
</style>

<script>
  import { defineComponent } from 'vue';

  export default defineComponent({
    data() {
      return {
        loading: false,
        games: [],
        error: null,
      };
    },
    created() {
      this.fetchGames();
    },
    methods: {
      async fetchGames() {
        this.loading = true;
        this.error = null;

        try {
          const response = await fetch('/api/soccer/today');
          if (!response.ok) {
            throw new Error(`Failed to fetch games: ${response.status} ${response.statusText}`);
          }

          const responseData = await response.json();
          this.games = responseData.fixtures || [];
        } catch (err) {
          this.error = err.message;
        } finally {
          this.loading = false;
        }
      },

      formatForm(form) {
        return form.split('').map(result => {
          switch (result.toUpperCase()) {
            case 'W': return 'W';
            case 'D': return 'D';
            case 'L': return 'L';
            default: return result;
          }
        }).join('');
      }
    },
  });
</script>
