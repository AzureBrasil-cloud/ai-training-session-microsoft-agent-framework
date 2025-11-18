<template>
  <div class="d-flex align-items-center mt-4" :class="alignmentClass">
    <img :src="logoSrc" alt="Logo Azure Brasil" class="azure-logo-hover" style="max-height: 90px; cursor: pointer;" @click="openAzureBrasil">
  </div>
</template>

<script setup lang="ts">
import { computed, ref, onMounted, onBeforeUnmount } from 'vue'

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

const theme = ref('');

const logoAzBr = `${window.location.origin}/images/byAzbr.png`
const logoAzBrLight = `${window.location.origin}/images/byAzbrLight.png`

const logoSrc = computed(() => {
  return theme.value === 'dark' ? logoAzBr : logoAzBrLight
})

let observer: MutationObserver | null = null;

onMounted(() => {
  const savedTheme = sessionStorage.getItem('theme');
  if (savedTheme) {
    theme.value = savedTheme;
  } else {
    theme.value = 'dark'; // Default theme
  }
  document.documentElement.setAttribute('data-bs-theme', theme.value);


  observer = new MutationObserver(mutations => {
    mutations.forEach(mutation => {
      if (mutation.attributeName === 'data-bs-theme') {
        theme.value = document.documentElement.getAttribute('data-bs-theme')!;
      }
    });
  });

  observer.observe(document.documentElement, {
    attributes: true
  });
});

onBeforeUnmount(() => {
  if (observer) {
    observer.disconnect();
  }
});

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

