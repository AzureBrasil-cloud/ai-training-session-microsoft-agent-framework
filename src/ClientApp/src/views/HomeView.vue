<script setup lang="ts">
import {onMounted, ref} from "vue";
import {useRouter} from "vue-router";
import AzureBrasilLogo from '@/components/common/AzureBrasilLogo.vue';

let userRole = ref("");
const router = useRouter();

onMounted(async () => {
  const loggedUser = sessionStorage.getItem("loggedUser");
  userRole.value = loggedUser ? JSON.parse(loggedUser)?.role : "";
});
</script>

<template>

  <div class="flex-fill h-100 d-flex flex-column">
    <header class="px-4 py-3">
      <div class="d-flex align-items-center justify-content-between flex-wrap gap-4 ">
        <div>
          <h6 style="color: gray">
            AzureBrasil.cloud apresenta Contoso AutoTech
          </h6>
        </div>
      </div>
    </header>
    <div
      class="flex-fill overflow-y-lg-auto scrollbar bg-body rounded-top-3 rounded-top-lg-4 border-top"
      data-x-inject="page">

      <main class="h-100">
        <div class="container-fluid h-100">
          <div class="row g-0 align-items-center justify-content-center" style="height: 100%;">
            <div class="col-12 d-flex flex-column flex-md-row h-100">

              <div class="w-100 w-md-50 d-flex flex-column justify-content-center align-items-start px-5 py-5 py-md-0 mb-4 mb-md-0 end-border">
                <h1 class="home-text-light text-start fw-bold lh-1 display-6 display-md-3 display-lg-1">
                  Seja muito <br> bem-vindo(a) ao <u>Contoso AutoTech</u>
                </h1>
              </div>

              <div class="w-100 w-md-50 d-flex flex-column justify-content-center px-5">
                <div v-if="userRole === 'user'">
                  <div class="row g-3 ps-0 ps-sm-4">
                    <div class="col-12 col-md-6 col-lg-4">
                      <RouterLink to="/discounts-client" class="text-decoration-none">
                        <div class="card h-100 border-0 shadow-sm hover-shadow-lg transition-all">
                          <div class="card-body text-center p-4">
                            <div class="mb-3">
                              <i class="bi bi-percent text-danger" style="font-size: 3rem;"></i>
                            </div>
                            <h5 class="card-title fw-semibold mb-2">Desconto</h5>
                          </div>
                        </div>
                      </RouterLink>
                    </div>
                  </div>
                </div>

                <div v-else-if="userRole === 'admin'">
                  <div class="row g-3 ps-0 ps-sm-4">
                    <!-- Card: Cadastro de Carros -->
                    <div class="col-12 col-md-6 col-lg-4">
                      <RouterLink to="/car-registration" class="text-decoration-none">
                        <div class="card h-100 border-0 shadow-sm hover-shadow-lg transition-all">
                          <div class="card-body text-center p-4">
                            <div class="mb-3">
                              <i class="bi bi-car-front text-primary" style="font-size: 3rem;"></i>
                            </div>
                            <h5 class="card-title fw-semibold mb-0">Cadastro de Carros</h5>
                          </div>
                        </div>
                      </RouterLink>
                    </div>

                    <!-- Card: Políticas Clientes -->
                    <div class="col-12 col-md-6 col-lg-4">
                      <RouterLink to="/customer-policies" class="text-decoration-none">
                        <div class="card h-100 border-0 shadow-sm hover-shadow-lg transition-all">
                          <div class="card-body text-center p-4">
                            <div class="mb-3">
                              <i class="bi bi-shield-check text-success" style="font-size: 3rem;"></i>
                            </div>
                            <h5 class="card-title fw-semibold mb-0">Políticas Clientes</h5>
                          </div>
                        </div>
                      </RouterLink>
                    </div>

                    <!-- Card: Feedbacks -->
                    <div class="col-12 col-md-6 col-lg-4">
                      <RouterLink to="/feedback-classifier" class="text-decoration-none">
                        <div class="card h-100 border-0 shadow-sm hover-shadow-lg transition-all">
                          <div class="card-body text-center p-4">
                            <div class="mb-3">
                              <i class="bi bi-chat-square-text text-info" style="font-size: 3rem;"></i>
                            </div>
                            <h5 class="card-title fw-semibold mb-0">Feedbacks</h5>
                          </div>
                        </div>
                      </RouterLink>
                    </div>

                    <!-- Card: Anúncios de Carros -->
                    <div class="col-12 col-md-6 col-lg-4">
                      <RouterLink to="/car-sales" class="text-decoration-none">
                        <div class="card h-100 border-0 shadow-sm hover-shadow-lg transition-all">
                          <div class="card-body text-center p-4">
                            <div class="mb-3">
                              <i class="bi bi-megaphone text-warning" style="font-size: 3rem;"></i>
                            </div>
                            <h5 class="card-title fw-semibold mb-0">Anúncios de Carros</h5>
                          </div>
                        </div>
                      </RouterLink>
                    </div>

                    <!-- Card: Produtos -->
                    <div class="col-12 col-md-6 col-lg-4">
                      <RouterLink to="/car-parts-product" class="text-decoration-none">
                        <div class="card h-100 border-0 shadow-sm hover-shadow-lg transition-all">
                          <div class="card-body text-center p-4">
                            <div class="mb-3">
                              <i class="bi bi-box-seam text-danger" style="font-size: 3rem;"></i>
                            </div>
                            <h5 class="card-title fw-semibold mb-0">Produtos</h5>
                          </div>
                        </div>
                      </RouterLink>
                    </div>

                    <!-- Card: Preços -->
                    <div class="col-12 col-md-6 col-lg-4">
                      <RouterLink to="/car-parts-price" class="text-decoration-none">
                        <div class="card h-100 border-0 shadow-sm hover-shadow-lg transition-all">
                          <div class="card-body text-center p-4">
                            <div class="mb-3">
                              <i class="bi bi-tags text-primary" style="font-size: 3rem;"></i>
                            </div>
                            <h5 class="card-title fw-semibold mb-0">Preços</h5>
                          </div>
                        </div>
                      </RouterLink>
                    </div>

                    <!-- Card: Estoque -->
                    <div class="col-12 col-md-6 col-lg-4">
                      <RouterLink to="/car-parts-stock" class="text-decoration-none">
                        <div class="card h-100 border-0 shadow-sm hover-shadow-lg transition-all">
                          <div class="card-body text-center p-4">
                            <div class="mb-3">
                              <i class="bi bi-boxes text-success" style="font-size: 3rem;"></i>
                            </div>
                            <h5 class="card-title fw-semibold mb-0">Estoque</h5>
                          </div>
                        </div>
                      </RouterLink>
                    </div>

                    <!-- Card: Peças -->
                    <div class="col-12 col-md-6 col-lg-4">
                      <RouterLink to="/car-parts" class="text-decoration-none">
                        <div class="card h-100 border-0 shadow-sm hover-shadow-lg transition-all">
                          <div class="card-body text-center p-4">
                            <div class="mb-3">
                              <i class="bi bi-gear text-info" style="font-size: 3rem;"></i>
                            </div>
                            <h5 class="card-title fw-semibold mb-0">Peças</h5>
                          </div>
                        </div>
                      </RouterLink>
                    </div>

                    <!-- Card: Desconto -->
                    <div class="col-12 col-md-6 col-lg-4">
                      <RouterLink to="/discounts-manager" class="text-decoration-none">
                        <div class="card h-100 border-0 shadow-sm hover-shadow-lg transition-all">
                          <div class="card-body text-center p-4">
                            <div class="mb-3">
                              <i class="bi bi-percent text-danger" style="font-size: 3rem;"></i>
                            </div>
                            <h5 class="card-title fw-semibold mb-0">Desconto</h5>
                          </div>
                        </div>
                      </RouterLink>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </main>
    </div>
  </div>

</template>

<style scoped>
.text-left {
  text-align: left;
}

.text-right {
  text-align: right;
}

.hover-shadow-lg {
  transition: all 0.3s ease;
  cursor: pointer;
}

.hover-shadow-lg:hover {
  transform: translateY(-5px);
  box-shadow: 0 1rem 3rem rgba(0, 0, 0, 0.175) !important;
}

.transition-all {
  transition: all 0.3s ease;
}

.card {
  min-height: 150px;
}

.card-body {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
}
</style>
