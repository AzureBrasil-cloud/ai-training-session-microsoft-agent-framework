<script setup lang="ts">
import { RouterLink } from 'vue-router'
//import { useUserStore } from '@/stores/user';
import { useRouter } from 'vue-router';
import {onBeforeMount, onMounted, ref} from 'vue';

let userEmail = ref("");
const router = useRouter();
const theme = ref('dark'); // default theme

onBeforeMount(() => {
  const loggedUser = sessionStorage.getItem("loggedUser");
  if (loggedUser) {
    userEmail.value = JSON.parse(loggedUser)?.email;
  }
});

onMounted(() => {
  const savedTheme = sessionStorage.getItem('theme') || 'dark';
  theme.value = savedTheme;
  document.documentElement.setAttribute('data-bs-theme', savedTheme);
});

const toggleTheme = () => {
  const newTheme = theme.value === 'light' ? 'dark' : 'light';
  theme.value = newTheme;
  sessionStorage.setItem('theme', newTheme);
  document.documentElement.setAttribute('data-bs-theme', newTheme);
}

const handleLogout = () => {
  sessionStorage.removeItem("loggedUser");
  router.push({ name: 'signin' });
};

</script>

<template>
  <div class="d-flex flex-column gap-2 px-4 pb-4">
    <div class="dropend mx-n2">
      <button type="button"
        class="w-100 px-2 py-2 text-start border-0 bg-transparent shadow-none bg-accent-hover rounded d-flex gap-3 align-items-center"
        data-bs-toggle="dropdown">
        <div class="avatar">
          <img src="https://assets.webpixels.io/img/memoji/memoji-2.svg" alt="..." />
        </div>
        <div class="flex-fill">
          <div class="d-flex align-items-center gap-2">
            <span class="text-sm text-heading fw-semibold" :title="userEmail">{{ userEmail.length > 22 ? userEmail.slice(0, 22) + '...' : userEmail }}</span>
          </div>
          <!-- <span class="d-block text-xs text-muted">{{ $store.user?.email }}</span> -->
        </div>
      </button>


      <div class="dropdown-menu ">
        <div class="dropdown-header">
          <span class="d-block text-xs text-muted mb-1">Logado(a) como:</span>
          <span class="d-block text-heading fw-semibold">{{ userEmail }}</span>
        </div>
        <div class="dropdown-divider"></div>
        <a @click="toggleTheme" class="dropdown-item" style="cursor: pointer;">
          <i :class="theme === 'dark' ? 'bi bi-sun mx-3' : 'bi bi-moon mx-3'"></i>
          <span>{{ theme === 'dark' ? 'Light Mode' : 'Dark Mode' }}</span>
        </a>
        <div class="dropdown-divider"></div>
        <a @click="handleLogout" class="dropdown-item" style="cursor: pointer;">
          <i class="bi bi-person mx-3"></i>Sair
        </a>
      </div>

    </div>
  </div>
</template>

<style scoped></style>
