<template>
  <div class="d-flex justify-content-center align-items-center mt-4">
    <img :src="logoAzBr" alt="Logo Azure Brasil" class="azure-logo-hover" style="max-height: 120px; cursor: pointer;" @click="openAzureBrasil">
  </div>
</template>

<script setup lang="ts">
const logoAzBr = `${window.location.origin}/images/byAzbr.png`

const openAzureBrasil = () => {
  window.open('https://azurebrasil.cloud/', '_blank')
}
</script>

<style scoped>
.azure-logo-hover {
  transition: transform 0.3s ease, filter 0.3s ease;
}

.azure-logo-hover:hover {
  transform: scale(1.1);
  filter: brightness(1.2);
}
</style>

