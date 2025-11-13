<!-- WorkflowSettingsModal.vue - Modal para configurar orquestrador e agentes -->
<script setup lang="ts">
import { ref } from 'vue';

interface SpecializedAgent {
  name: string;
  instructions: string;
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

// Adicionar novo agente
function addAgent() {
  localAgents.value.push({
    name: `Agente ${localAgents.value.length + 1}`,
    instructions: 'Instruções do agente...'
  });
}

// Remover agente
function removeAgent(index: number) {
  if (localAgents.value.length > 1) {
    localAgents.value.splice(index, 1);
  } else {
    alert('É necessário ter pelo menos um agente especializado.');
  }
}

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

          <!-- Agentes Especializados -->
          <div>
            <div class="d-flex justify-content-between align-items-center mb-3">
              <h6 class="mb-0">
                <i class="bi bi-robot"></i> Agentes Especializados
              </h6>
              <button
                @click="addAgent"
                class="btn btn-success btn-sm"
              >
                <i class="bi bi-plus-circle"></i> Adicionar Agente
              </button>
            </div>

            <div
              v-for="(agent, index) in localAgents"
              :key="index"
              class="mb-3 p-3 border rounded"
            >
              <div class="d-flex justify-content-between align-items-center mb-2">
                <h6 class="mb-0">Agente {{ index + 1 }}</h6>
                <button
                  @click="removeAgent(index)"
                  class="btn btn-danger btn-sm"
                  :disabled="localAgents.length === 1"
                >
                  <i class="bi bi-trash"></i> Remover
                </button>
              </div>

              <div class="mb-2">
                <label class="form-label">Nome:</label>
                <input
                  v-model="agent.name"
                  type="text"
                  class="form-control"
                  placeholder="Ex: Agente de Estoque"
                />
              </div>

              <div>
                <label class="form-label">Instruções:</label>
                <textarea
                  v-model="agent.instructions"
                  class="form-control"
                  rows="4"
                  placeholder="Descreva as responsabilidades e capacidades deste agente..."
                ></textarea>
              </div>
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
