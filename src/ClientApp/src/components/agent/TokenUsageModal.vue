<!-- TokenUsageModal.vue - Modal de exibição de uso de tokens -->
<script setup lang="ts">
interface Props {
  usage: {
    input?: number;
    output?: number;
    total?: number;
  } | null;
}

defineProps<Props>();

const emit = defineEmits<{
  close: [];
}>();
</script>

<template>
  <div class="modal d-block" tabindex="-1" style="background-color: rgba(0,0,0,0.5);">
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content border-0 shadow-lg">
        <div class="modal-header border-0 bg-purple text-white">
          <h5 class="modal-title fw-bold">
            <i class="bi bi-lightning-charge-fill me-2"></i>
            Consumo de Tokens
          </h5>
          <button type="button" class="btn-close btn-close-white" @click="emit('close')"></button>
        </div>
        <div class="modal-body p-4">
          <div v-if="usage">
            <!-- Cards de Tokens -->
            <div class="row g-3 mb-4">
              <!-- Token de Entrada -->
              <div class="col-12">
                <div class="card border-0 shadow-sm">
                  <div class="card-body p-3">
                    <div class="d-flex align-items-center justify-content-between">
                      <div class="d-flex align-items-center">
                        <div class="rounded-circle bg-primary p-2 me-3">
                          <i class="bi bi-arrow-down-circle text-white fs-4"></i>
                        </div>
                        <div>
                          <div class="text-muted small mb-1">Tokens de Entrada</div>
                          <div class="fw-bold fs-4 text-primary">{{ usage.input?.toLocaleString() ?? 0 }}</div>
                        </div>
                      </div>
                      <span class="badge bg-primary rounded-pill px-3 py-2 fs-6">
                        Input
                      </span>
                    </div>
                  </div>
                </div>
              </div>

              <!-- Token de Saída -->
              <div class="col-12">
                <div class="card border-0 shadow-sm">
                  <div class="card-body p-3">
                    <div class="d-flex align-items-center justify-content-between">
                      <div class="d-flex align-items-center">
                        <div class="rounded-circle bg-success p-2 me-3">
                          <i class="bi bi-arrow-up-circle text-white fs-4"></i>
                        </div>
                        <div>
                          <div class="text-muted small mb-1">Tokens de Saída</div>
                          <div class="fw-bold fs-4 text-success">{{ usage.output?.toLocaleString() ?? 0 }}</div>
                        </div>
                      </div>
                      <span class="badge bg-success rounded-pill px-3 py-2 fs-6">
                        Output
                      </span>
                    </div>
                  </div>
                </div>
              </div>

              <!-- Total -->
              <div class="col-12">
                <div class="card border-0 shadow-sm">
                  <div class="card-body p-3">
                    <div class="d-flex align-items-center justify-content-between">
                      <div class="d-flex align-items-center">
                        <div class="rounded-circle bg-warning p-2 me-3">
                          <i class="bi bi-calculator-fill text-white fs-4"></i>
                        </div>
                        <div>
                          <div class="text-muted small mb-1">Total de Tokens</div>
                          <div class="fw-bold fs-3 text-warning">{{ usage.total?.toLocaleString() ?? 0 }}</div>
                        </div>
                      </div>
                      <span class="badge bg-warning rounded-pill px-3 py-2 fs-6">
                        Total
                      </span>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="modal-footer border-0">
          <button type="button" class="btn btn-secondary px-4" @click="emit('close')">
            <i class="bi bi-x-circle me-2"></i>Fechar
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

