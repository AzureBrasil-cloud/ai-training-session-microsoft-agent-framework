<script setup lang="ts">
import {onMounted, ref} from "vue";
import AzureBrasilLogo from '@/components/common/AzureBrasilLogo.vue';

let userRole = ref("");

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
                <div class="mt-4 w-100">
                  <AzureBrasilLogo align="start" />
                </div>
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
                            <p class="text-muted small mb-0">Solicite descontos especiais</p>
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
                            <h5 class="card-title fw-semibold mb-2">Cadastro de Carros</h5>
                            <p class="text-muted small mb-0">Agente com tools para cadastrar veículos</p>
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
                            <h5 class="card-title fw-semibold mb-2">Políticas Clientes</h5>
                            <p class="text-muted small mb-0">Agente com RAG sobre políticas privadas</p>
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
                            <h5 class="card-title fw-semibold mb-2">Feedbacks</h5>
                            <p class="text-muted small mb-0">Classificador de sentimento com few-shot</p>
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
                            <h5 class="card-title fw-semibold mb-2">Anúncios de Carros</h5>
                            <p class="text-muted small mb-0">Agente que analisa anúncios de veículos</p>
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
                            <h5 class="card-title fw-semibold mb-2">Produtos</h5>
                            <p class="text-muted small mb-0">Consulta produtos via MCP</p>
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
                            <h5 class="card-title fw-semibold mb-2">Preços</h5>
                            <p class="text-muted small mb-0">Consulta preços via MCP</p>
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
                            <h5 class="card-title fw-semibold mb-2">Estoque</h5>
                            <p class="text-muted small mb-0">Consulta estoque via MCP</p>
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
                            <h5 class="card-title fw-semibold mb-2">Peças</h5>
                            <p class="text-muted small mb-0">Agente orquestrador multi-MCP</p>
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
                            <h5 class="card-title fw-semibold mb-2">Desconto</h5>
                            <p class="text-muted small mb-0">Agente de aprovação de descontos</p>
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
.hover-shadow-lg {
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  cursor: pointer;
}

.hover-shadow-lg:hover {
  transform: translateY(-10px) scale(1.05);
  box-shadow: 0 20px 40px rgba(0, 0, 0, 0.3) !important;
}

.hover-shadow-lg:hover .card-body i {
  transform: scale(1.1);
  transition: transform 0.3s cubic-bezier(0.4, 0, 0.2, 1);
}

.transition-all {
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
}

.card {
  min-height: 180px;
  border: 2px solid transparent;
}

.card:hover {
  border-color: rgba(109, 40, 217, 0.2);
}

.card-body {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
}

.card-body i {
  transition: transform 0.3s cubic-bezier(0.4, 0, 0.2, 1);
}
</style>
