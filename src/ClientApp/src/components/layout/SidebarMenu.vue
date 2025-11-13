<script setup lang="ts">
import { auth } from '@/utils/auth';
import UserMenu from './UserMenu.vue';
import {onBeforeMount, ref} from 'vue';
import { useRouter } from 'vue-router';

const userIsAdmin = auth.userIsAdmin();

let userRole = ref("");
const router = useRouter();

onBeforeMount(() => {
  const loggedUser = sessionStorage.getItem("loggedUser");

  userRole.value = loggedUser ? JSON.parse(loggedUser)?.role : "";
});

const logo = `${window.location.origin}/images/logo-acai.png`;
</script>

<template>

  <div class="d-flex justify-content-between align-items-center d-lg-none p-3" @click="router.push('/')">

    <img :src="logo" alt="Logo" width="85" />

    <button
      class="btn btn-purple"
      type="button"
      data-bs-toggle="offcanvas"
      data-bs-target="#sidebarOffcanvas"
      aria-controls="sidebarOffcanvas"
    >
      <i class="bi bi-list fs-3"></i>
    </button>
  </div>
  <div class="offcanvas-lg offcanvas-start w-rem-80 w-lg-auto border-end-lg" data-bs-scroll="true"
       tabindex="-1"
       id="sidebarOffcanvas" aria-labelledby="sidebarOffcanvasLabel">
    <nav
      class="sidebar w-100 w-rem-lg-64 d-flex flex-column flex-shrink-0 position-relative z-2 h-100">

      <div class="dropdown-center px-4 py-2 mx-n2 position-relative">
        <div
          class="w-100 px-2 py-2 text-start border-0 bg-transparent shadow-none bg-accent-hover rounded d-flex gap-3 align-items-center"
          @click="router.push('/')"
          style="cursor: pointer"
        >
          <img :src="logo" alt="..." width="85" />
          <div class="d-grid flex-grow-1 ls-tight text-sm">
            <span :class="[userIsAdmin ? 'text-white' : 'text-black', 'fw-semibold']">Contoso AutoTech</span>
            <span class="text-truncate text-xs text-body-secondary mt-n1">Web app</span>
          </div>
        </div>
      </div>

      <div class="px-4 py-2 flex-fill overflow-y-auto scrollbar">
        <div v-if="userRole === 'user'" class="vstack gap-5">
          <div>
            <div class="d-flex align-items-center px-3 px-lg-0 mb-3">
      <span class="d-block text-sm item-purple fw-semibold bg-light px-3 py-2 rounded-3 w-100">
        USUÁRIO
      </span>
            </div>
            <ul class="navbar-nav navbar-vertical-nav d-flex flex-column mx-lg-n2">
               <li class="nav-item">
                <RouterLink to="/discounts-client" class="dropdown-item">
                 <i class="bi bi-robot px-3"></i>
                  <span>Assistente de Desconto</span>
                </RouterLink>
              </li>
            </ul>
          </div>
        </div>


        <div v-else-if="userRole === 'admin'" class="vstack gap-5">
          <div>
            <div class="d-flex align-items-center px-3 px-lg-0 mb-3">
      <span class="d-block text-sm item-purple fw-semibold bg-light px-3 py-2 rounded-3 w-100">
        ADMIN
      </span>
            </div>
            <ul class="navbar-nav navbar-vertical-nav d-flex flex-column mx-lg-n2">
              <li class="nav-item">
                <RouterLink to="/car-registration" class="dropdown-item">
                  <i class="bi bi-car-front px-3"></i>
                  <span>Cadastro de Carros</span>
                </RouterLink>
              </li>
              <li class="nav-item">
                <RouterLink to="/customer-policies" class="dropdown-item">
                  <i class="bi bi-shield-check px-3"></i>
                  <span>Políticas clientes</span>
                </RouterLink>
              </li>
              <li class="nav-item">
                <RouterLink to="/feedback-classifier" class="dropdown-item">
                  <i class="bi bi-chat-square-text px-3"></i>
                  <span>Feedbacks</span>
                </RouterLink>
              </li>
              <li class="nav-item">
                <RouterLink to="/car-sales" class="dropdown-item">
                  <i class="bi bi-car-front-fill px-3"></i>
                  <span>Anúncios de Carros</span>
                </RouterLink>
              </li>
              <li class="nav-item">
                <RouterLink to="/car-parts-product" class="dropdown-item">
                  <i class="bi bi-robot px-3"></i>
                  <span>Agente de Consulta de Produtos</span>
                </RouterLink>
              </li>
              <li class="nav-item">
                <RouterLink to="/car-parts-price" class="dropdown-item">
                  <i class="bi bi-robot px-3"></i>
                  <span>Agente de Consulta de Preços</span>
                </RouterLink>
              </li>
              <li class="nav-item">
                <RouterLink to="/car-parts-stock" class="dropdown-item">
                  <i class="bi bi-robot px-3"></i>
                  <span>Agente de Consulta de Estoque</span>
                </RouterLink>
              </li>
              <li class="nav-item">
                <RouterLink to="/car-parts" class="dropdown-item">
                 <i class="bi bi-robot px-3"></i>
                  <span>Multi Agentes</span>
                </RouterLink>
              </li>
              <li class="nav-item">
                <RouterLink to="/discounts-manager" class="dropdown-item">
                 <i class="bi bi-robot px-3"></i>
                  <span>Desconto Gerente</span>
                </RouterLink>
              </li>
            </ul>
          </div>
        </div>


      </div>

      <UserMenu/>
    </nav>
  </div>
</template>

<style scoped></style>
