<template>
  <div class="app-container">
    <h1>Agenda</h1>
    <AgendaList />
    <Toast position="top-right" />
  </div>
</template>

<script setup lang="ts">
import AgendaList from "./components/AgendaList.vue";
import { Toast } from "primevue";
</script>

<style scoped>
.app-container {
  max-width: 800px;
  margin: 2rem auto;
  padding: 1rem;
}
</style>
