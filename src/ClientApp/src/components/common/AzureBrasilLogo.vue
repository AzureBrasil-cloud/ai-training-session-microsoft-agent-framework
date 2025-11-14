<template>
  <div class="d-flex align-items-center mt-4" :class="alignmentClass">
    <img :src="logoAzBr" alt="Logo Azure Brasil" class="azure-logo-hover" style="max-height: 120px; cursor: pointer;" @click="openAzureBrasil">
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'

interface Props {
  align?: 'start' | 'center' | 'end'
}

const props = withDefaults(defineProps<Props>(), {
  align: 'center'
})

const alignmentClass = computed(() => {
  switch (props.align) {
    case 'start':
      return 'justify-content-start'
    case 'end':
      return 'justify-content-end'
    default:
      return 'justify-content-center'
  }
})

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

