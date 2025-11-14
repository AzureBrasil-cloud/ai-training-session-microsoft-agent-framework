<!-- WorkflowSettingsModal.vue - Modal para configurar orquestrador e agentes -->
<script setup lang="ts">
import { ref } from 'vue';

interface SpecializedAgent {
  name: string;
  instructions: string;
  featureId: number;
}

interface Props {
  orchestratorName: string;
  orchestratorInstructions: string;
  specializedAgents: SpecializedAgent[];
}

const props = defineProps<Props>();

const emit = defineEmits<{
  close: [];
  save: [settings: {
    orchestrator: { name: string; instructions: string };
    agents: SpecializedAgent[];
  }];
}>();

// Estados locais
const localOrchestratorName = ref(props.orchestratorName);
const localOrchestratorInstructions = ref(props.orchestratorInstructions);
const localAgents = ref<SpecializedAgent[]>(JSON.parse(JSON.stringify(props.specializedAgents)));


// Salvar configurações
function save() {
  if (!localOrchestratorName.value.trim()) {
    alert('O nome do orquestrador é obrigatório.');
    return;
  }

  if (!localOrchestratorInstructions.value.trim()) {
    alert('As instruções do orquestrador são obrigatórias.');
    return;
  }

  const hasInvalidAgent = localAgents.value.some(
    agent => !agent.name.trim() || !agent.instructions.trim()
  );

  if (hasInvalidAgent) {
    alert('Todos os agentes devem ter nome e instruções preenchidos.');
    return;
  }

  emit('save', {
    orchestrator: {
      name: localOrchestratorName.value,
      instructions: localOrchestratorInstructions.value
    },
    agents: localAgents.value
  });
}
</script>

<template>
  <div class="modal d-block" style="background-color: rgba(0,0,0,0.5);">
    <div class="modal-dialog modal-xl modal-dialog-scrollable">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">
            <i class="bi bi-gear"></i> Configurar Workflow
          </h5>
          <button type="button" class="btn-close" @click="emit('close')"></button>
        </div>

        <div class="modal-body">
          <!-- Configuração do Orquestrador -->
          <div class="mb-4 p-3 border rounded bg-light">
            <h6 class="mb-3">
              <i class="bi bi-diagram-3"></i> Agente Orquestrador
            </h6>

            <div class="mb-3">
              <label class="form-label fw-bold">Nome do Orquestrador:</label>
              <input
                v-model="localOrchestratorName"
                type="text"
                class="form-control"
                placeholder="Ex: Orquestrador Principal"
              />
            </div>

            <div class="mb-0">
              <label class="form-label fw-bold">Instruções do Orquestrador:</label>
              <textarea
                v-model="localOrchestratorInstructions"
                class="form-control"
                rows="6"
                placeholder="Descreva o papel do orquestrador e como ele deve coordenar os agentes especializados..."
              ></textarea>
              <small class="text-muted">
                O orquestrador receberá automaticamente as instruções de todos os agentes especializados.
              </small>
            </div>
          </div>

          <!-- Configuração dos Agentes Especializados -->
<div class="mb-4 p-3 border rounded bg-light">
  <h6 class="mb-3">
    <i class="bi bi-robot"></i> Agentes Especializados
  </h6>

  <div
    v-for="(agent, index) in localAgents"
    :key="index"
    class="border rounded bg-white p-3 mb-3"
  >
    <h6 class="fw-bold">Agente {{ index + 1 }} — {{ agent.name }}</h6>

    <label class="form-label fw-bold mt-2">Nome:</label>
    <input
      v-model="agent.name"
      type="text"
      class="form-control"
      placeholder="Nome do agente"
    />

    <label class="form-label fw-bold mt-3">Instruções:</label>
    <textarea
      v-model="agent.instructions"
      rows="5"
      class="form-control"
      placeholder="Prompt / Instruções do agente"
    ></textarea>
  </div>
</div>

        </div>

        <div class="modal-footer">
          <button type="button" class="btn btn-secondary" @click="emit('close')">
            Cancelar
          </button>
          <button type="button" class="btn btn-primary" @click="save">
            <i class="bi bi-check-circle"></i> Salvar Configurações
          </button>
        </div>
      </div>
    </div>
  </div>
</template>
