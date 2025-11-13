<script setup lang="ts">
import { ref, onMounted } from 'vue'
import carSalesService, { type CarSale } from '@/services/carSales'

const carSales = ref<CarSale[]>([])
const loading = ref(false)
const error = ref('')

const loadCarSales = async () => {
  loading.value = true
  error.value = ''

  try {
    carSales.value = await carSalesService.getAll()
  } catch (err: any) {
    error.value = err?.message || 'Erro ao carregar vendas de carros'
    console.error('Error loading car sales:', err)
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  loadCarSales()
})

const formatPrice = (price: number) => {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  }).format(price)
}
</script>

<template>
  <div class="container-fluid py-4">
    <div class="row mb-4">
      <div class="col-12">
        <div class="d-flex justify-content-between align-items-center">
          <h2>
            <i class="bi bi-car-front-fill me-2"></i>
            Vendas de Carros
          </h2>
          <button
            class="btn btn-primary"
            @click="loadCarSales"
            :disabled="loading"
          >
            <i class="bi bi-arrow-clockwise me-2"></i>
            Atualizar
          </button>
        </div>
      </div>
    </div>

    <!-- Loading State -->
    <div v-if="loading" class="text-center py-5">
      <div class="spinner-border text-primary" role="status">
        <span class="visually-hidden">Carregando...</span>
      </div>
      <p class="mt-3 text-muted">Carregando vendas de carros...</p>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="alert alert-danger" role="alert">
      <i class="bi bi-exclamation-triangle-fill me-2"></i>
      {{ error }}
    </div>

    <!-- Empty State -->
    <div v-else-if="carSales.length === 0" class="text-center py-5">
      <i class="bi bi-inbox display-1 text-muted"></i>
      <p class="mt-3 text-muted">Nenhuma venda de carro encontrada.</p>
    </div>

    <!-- Data Table -->
    <div v-else class="row">
      <div class="col-12">
        <div class="card shadow-sm">
          <div class="card-body">
            <div class="table-responsive">
              <table class="table table-hover">
                <thead>
                  <tr>
                    <th scope="col">Modelo</th>
                    <th scope="col">Placa</th>
                    <th scope="col">Cor</th>
                    <th scope="col">Preço</th>
                    <th scope="col">Pontos Fortes</th>
                    <th scope="col">Pontos Fracos</th>
                    <th scope="col">Data de Cadastro</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="car in carSales" :key="car.id">
                    <td>
                      <strong>{{ car.model }}</strong>
                    </td>
                    <td>
                      <span class="badge bg-secondary">{{ car.licensePlate }}</span>
                    </td>
                    <td>{{ car.color }}</td>
                    <td>
                      <strong class="text-success">{{ formatPrice(car.price) }}</strong>
                    </td>
                    <td>
                      <ul class="mb-0 ps-3" v-if="car.strengths && car.strengths.length > 0">
                        <li v-for="(strength, idx) in car.strengths" :key="idx" class="small text-success">
                          {{ strength }}
                        </li>
                      </ul>
                      <span v-else class="text-muted small">-</span>
                    </td>
                    <td>
                      <ul class="mb-0 ps-3" v-if="car.weaknesses && car.weaknesses.length > 0">
                        <li v-for="(weakness, idx) in car.weaknesses" :key="idx" class="small text-danger">
                          {{ weakness }}
                        </li>
                      </ul>
                      <span v-else class="text-muted small">-</span>
                    </td>
                    <td class="small text-muted">
                      {{ new Date(car.createdAt).toLocaleString('pt-BR') }}
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>

      <!-- Detailed Cards View (Optional) -->
      <div class="col-12 mt-4">
        <h4 class="mb-3">Detalhes</h4>
        <div class="row">
          <div v-for="car in carSales" :key="car.id" class="col-md-6 col-lg-4 mb-4">
            <div class="card h-100 shadow-sm">
              <div class="card-header bg-primary text-white">
                <h5 class="mb-0">{{ car.model }}</h5>
              </div>
              <div class="card-body">
                <p class="mb-2">
                  <strong>Placa:</strong>
                  <span class="badge bg-secondary ms-2">{{ car.licensePlate }}</span>
                </p>
                <p class="mb-2"><strong>Cor:</strong> {{ car.color }}</p>
                <p class="mb-3">
                  <strong>Preço:</strong>
                  <span class="text-success fs-5">{{ formatPrice(car.price) }}</span>
                </p>

                <p class="mb-2"><strong>Descrição:</strong></p>
                <p class="text-muted small">{{ car.description }}</p>

                <div v-if="car.strengths && car.strengths.length > 0" class="mt-3">
                  <strong class="text-success">
                    <i class="bi bi-check-circle-fill me-1"></i>
                    Pontos Fortes:
                  </strong>
                  <ul class="mt-2">
                    <li v-for="(strength, idx) in car.strengths" :key="idx" class="small text-success">
                      {{ strength }}
                    </li>
                  </ul>
                </div>

                <div v-if="car.weaknesses && car.weaknesses.length > 0" class="mt-3">
                  <strong class="text-danger">
                    <i class="bi bi-x-circle-fill me-1"></i>
                    Pontos Fracos:
                  </strong>
                  <ul class="mt-2">
                    <li v-for="(weakness, idx) in car.weaknesses" :key="idx" class="small text-danger">
                      {{ weakness }}
                    </li>
                  </ul>
                </div>
              </div>
              <div class="card-footer text-muted small">
                Cadastrado em: {{ new Date(car.createdAt).toLocaleString('pt-BR') }}
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.table-responsive {
  max-height: 600px;
  overflow-y: auto;
}

.card {
  transition: transform 0.2s;
}

.card:hover {
  transform: translateY(-2px);
}
</style>

