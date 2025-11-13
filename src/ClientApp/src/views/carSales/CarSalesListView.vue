<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'
import carSalesService, { type CarSale } from '@/services/carSales'

const carSales = ref<CarSale[]>([])
const loading = ref(false)
const error = ref('')
const selectedImage = ref<{ url: string; model: string } | null>(null)
const selectedCar = ref<CarSale | null>(null)

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

const formatPrice = (price: number) => {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  }).format(price)
}

const openImageModal = (imageUrl: string, model: string) => {
  selectedImage.value = { url: imageUrl, model }
}

const closeImageModal = () => {
  selectedImage.value = null
}

const openDetailsModal = (car: CarSale) => {
  selectedCar.value = car
}

const closeDetailsModal = () => {
  selectedCar.value = null
}

const handleEscape = (e: KeyboardEvent) => {
  if (e.key === 'Escape') {
    if (selectedImage.value) {
      closeImageModal()
    } else if (selectedCar.value) {
      closeDetailsModal()
    }
  }
}

onMounted(() => {
  loadCarSales()
  window.addEventListener('keydown', handleEscape)
})

onUnmounted(() => {
  window.removeEventListener('keydown', handleEscape)
})
</script>

<template>
  <div class="container-fluid py-4">
    <div class="row mb-4">
      <div class="col-12">
        <div class="d-flex justify-content-between align-items-center">
          <h2>
            <i class="bi bi-car-front-fill me-2"></i>
            Anúncios de carros
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
      <p class="mt-3 text-muted">Carregando carros...</p>
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
                    <th scope="col">Imagem</th>
                    <th scope="col" class="col-modelo">Modelo</th>
                    <th scope="col">Placa</th>
                    <th scope="col">Cor</th>
                    <th scope="col">Preço</th>
                    <th scope="col">Preço Referência</th>
                    <th scope="col">Detalhes</th>
                    <th scope="col">Data de Cadastro</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="car in carSales" :key="car.id">
                    <td>
                      <div class="image-cell">
                        <img
                          :src="car.imageUrl"
                          :alt="car.model"
                          class="car-image"
                        />
                        <button
                          class="btn btn-sm btn-outline-primary mt-1 w-100"
                          @click="openImageModal(car.imageUrl, car.model)"
                        >
                          Ver
                        </button>
                      </div>
                    </td>
                    <td class="col-modelo">
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
                      <span v-if="car.referencePrice" class="text-muted">
                        {{ formatPrice(car.referencePrice) }}
                      </span>
                      <span v-else class="text-muted small">-</span>
                    </td>
                    <td>
                      <button
                        class="btn btn-sm btn-outline-info"
                        @click="openDetailsModal(car)"
                      >
                        <i class="bi bi-info-circle me-1"></i>
                        Detalhes
                      </button>
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
    </div>

    <!-- Image Modal -->
    <div v-if="selectedImage" class="modal d-block" tabindex="-1" style="background-color: rgba(0,0,0,0.5);">
      <div class="modal-dialog modal-dialog-centered modal-xl">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">
              <i class="bi bi-image me-2"></i>
              {{ selectedImage.model }}
            </h5>
            <button type="button" class="btn-close" @click="closeImageModal" aria-label="Fechar"></button>
          </div>
          <div class="modal-body text-center">
            <img
              :src="selectedImage.url"
              :alt="selectedImage.model"
              class="img-fluid"
              style="max-height: 70vh;"
            />
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" @click="closeImageModal">
              <i class="bi bi-x-circle me-2"></i>
              Fechar
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Details Modal -->
    <div v-if="selectedCar" class="modal d-block" tabindex="-1" style="background-color: rgba(0,0,0,0.5);">
      <div class="modal-dialog modal-dialog-centered modal-lg">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">
              <i class="bi bi-info-circle me-2"></i>
              Detalhes do Veículo
            </h5>
            <button type="button" class="btn-close" @click="closeDetailsModal" aria-label="Fechar"></button>
          </div>
          <div class="modal-body">
            <div class="row mb-3">
              <div class="col-12">
                <h6 class="fw-bold">{{ selectedCar.model }}</h6>
                <p class="text-muted mb-0">{{ selectedCar.description }}</p>
              </div>
            </div>

            <div class="row">
              <!-- Agent Consideration -->
              <div v-if="selectedCar.agentConsideration" class="col-12 mb-3">
                <div class="card border-info">
                  <div class="card-header bg-info text-white">
                    <i class="bi bi-lightbulb-fill me-2"></i>
                    Considerações do Agente
                  </div>
                  <div class="card-body">
                    <p class="mb-0">{{ selectedCar.agentConsideration }}</p>
                  </div>
                </div>
              </div>

              <div class="col-md-6">
                <div class="card border-success mb-3">
                  <div class="card-header bg-success text-white">
                    <i class="bi bi-check-circle-fill me-2"></i>
                    Pontos Fortes
                  </div>
                  <div class="card-body">
                    <ul v-if="selectedCar.strengths && selectedCar.strengths.length > 0" class="mb-0">
                      <li v-for="(strength, idx) in selectedCar.strengths" :key="idx" class="text-success">
                        {{ strength }}
                      </li>
                    </ul>
                    <p v-else class="text-muted mb-0">Nenhum ponto forte registrado.</p>
                  </div>
                </div>
              </div>

              <div class="col-md-6">
                <div class="card border-warning mb-3">
                  <div class="card-header bg-warning text-dark">
                    <i class="bi bi-exclamation-triangle-fill me-2"></i>
                    Pontos Fracos
                  </div>
                  <div class="card-body">
                    <ul v-if="selectedCar.weaknesses && selectedCar.weaknesses.length > 0" class="mb-0">
                      <li v-for="(weakness, idx) in selectedCar.weaknesses" :key="idx" class="text-warning">
                        {{ weakness }}
                      </li>
                    </ul>
                    <p v-else class="text-muted mb-0">Nenhum ponto fraco registrado.</p>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" @click="closeDetailsModal">
              <i class="bi bi-x-circle me-2"></i>
              Fechar
            </button>
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

.image-cell {
  width: 120px;
}

.car-image {
  width: 120px;
  height: auto;
  object-fit: contain;
  border-radius: 4px;
  display: block;
}

.col-modelo {
  max-width: 200px;
  word-wrap: break-word;
  white-space: normal;
}
</style>
