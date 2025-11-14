<script setup lang="ts">
import { ref, onMounted } from 'vue';
import type { MessageResult } from '@/models/messageResult';
import { Role } from '@/models/messageResult';
import type { Thread } from '@/models/thread';
import agentService from '@/services/agent';
import MarkdownIt from 'markdown-it';
import ThreadsListModal from './ThreadsListModal.vue';
import TokenUsageModal from './TokenUsageModal.vue';
import MultiAgentSettingsModal from './MultiAgentSettingsModal.vue';

const md = new MarkdownIt();

interface SpecializedAgent {
  name: string;
  instructions: string;
  featureId: number;
}

interface Props {
  featureId: number;
  title: string;
  welcomeMessage: string;
  defaultOrchestratorName: string;
  defaultOrchestratorInstructions: string;
  defaultSpecializedAgents: SpecializedAgent[];
}

const props = defineProps<Props>();

// Estados
const isLoading = ref(false);
const isWaitingResponse = ref(false);
const userInput = ref('');
const messages = ref<MessageResult[]>([]);
const currentThreadId = ref<string>('');
const threads = ref<Thread[]>([]);

// Configurações do orquestrador e agentes
const orchestratorSettings = ref({
  name: props.defaultOrchestratorName,
  instructions: props.defaultOrchestratorInstructions
});

const specializedAgents = ref<SpecializedAgent[]>([...props.defaultSpecializedAgents]);

// Modais
const showSettingsModal = ref(false);
const showThreadsModal = ref(false);
const showUsageModal = ref(false);
const selectedUsage = ref<{ input?: number; output?: number; total?: number } | null>(null);

// Estilos das mensagens
const userStyle = 'max-width: 75%; background-color: #0d6efd; color: white; margin-left: auto;';
const assistantStyle = 'max-width: 75%; background-color: #e6e6e6; color: black;';

// Função para criar nova thread
async function createNewThread() {
  try {
    isLoading.value = true;
    const thread = await agentService.createThread({ feature: props.featureId });
    currentThreadId.value = thread.id;
    messages.value = [];

    // Mensagem de boas-vindas
    messages.value.push({
      role: Role.Agent,
      content: props.welcomeMessage
    });
  } catch (error) {
    console.error('Erro ao criar thread:', error);
    alert('Erro ao criar thread. Tente novamente.');
  } finally {
    isLoading.value = false;
  }
}

// Função para construir as instruções completas do orquestrador
function buildOrchestratorInstructions(): string {
  let instructions = orchestratorSettings.value.instructions + '\n\n';
  instructions += '## AGENTES ESPECIALIZADOS DISPONÍVEIS:\n\n';

  specializedAgents.value.forEach((agent, index) => {
    instructions += `### ${index + 1}. ${agent.name}\n`;
    instructions += `${agent.instructions}\n\n`;
  });

  instructions += '\n---\n';
  instructions += 'Você deve decidir qual agente especializado usar ou se você mesmo pode responder diretamente.';

  return instructions;
}

// Função para montar o array de AgentRequest
function buildAgentsRequest() {
  return specializedAgents.value.map(agent => ({
    agentName: agent.name,
    agentInstructions: agent.instructions,
    feature: agent.featureId
  }));
}

// Função para enviar mensagem
async function sendMessage() {
  if (!userInput.value.trim() || !currentThreadId.value) return;

  const input = userInput.value;
  messages.value.push({ role: Role.User, content: input });
  userInput.value = '';
  isWaitingResponse.value = true;

  try {
    // Monta o request completo com orquestrador e agentes especializados
    const response = await agentService.runMultiAgents({
      feature: props.featureId,
      agentName: orchestratorSettings.value.name,
      agentInstructions: buildOrchestratorInstructions(),
      threadId: currentThreadId.value,
      message: input,
      agents: buildAgentsRequest()
    });

    messages.value.push(response);
  } catch (error) {
    console.error('Erro ao enviar mensagem:', error);
    messages.value.push({
      role: Role.Agent,
      content: 'Desculpe, ocorreu um erro ao processar sua mensagem. Tente novamente.'
    });
  } finally {
    isWaitingResponse.value = false;
  }
}

// Função para resetar thread
async function resetThread() {
  if (isLoading.value) return;
  await createNewThread();
}

// Função para abrir modal de threads
async function openThreadsModal() {
  try {
    const result = await agentService.listThreads(props.featureId);
    threads.value = result || [];
    showThreadsModal.value = true;
  } catch (error) {
    console.error('Erro ao listar threads:', error);
    alert('Erro ao listar threads. Tente novamente.');
  }
}

// Função para carregar thread específica
async function loadThread(threadId: string) {
  try {
    isLoading.value = true;
    showThreadsModal.value = false;

    const threadMessages = await agentService.getMessages(threadId);
    currentThreadId.value = threadId;
    messages.value = threadMessages;
  } catch (error) {
    console.error('Erro ao carregar mensagens da thread:', error);
    alert('Erro ao carregar mensagens. Tente novamente.');
  } finally {
    isLoading.value = false;
  }
}

// Função para lidar com tecla Enter
function handleKeyDown(event: KeyboardEvent) {
  if (event.key === 'Enter' && !event.shiftKey && !isLoading.value && !isWaitingResponse.value) {
    event.preventDefault();
    sendMessage();
  }
}

// Função para abrir modal de usage
function openUsageModal(usage: { input?: number; output?: number; total?: number }) {
  selectedUsage.value = usage;
  showUsageModal.value = true;
}

// Função para salvar configurações
function saveSettings(settings: {
  orchestrator: { name: string; instructions: string };
  agents: SpecializedAgent[];
}) {
  orchestratorSettings.value = settings.orchestrator;
  specializedAgents.value = settings.agents;
  showSettingsModal.value = false;
}

// Inicialização
onMounted(async () => {
  await createNewThread();
});
</script>

<template>
  <div class="d-flex flex-column p-3" style="height: 95vh;">
    <h4 class="mb-3 text-black s-h3">
      <slot name="icon"></slot>
      {{ title }}
    </h4>

    <!-- Info do Orquestrador -->
    <div class="mb-2 p-2 rounded border bg-light">
      <div class="d-flex justify-content-between align-items-center">
        <div>
          <strong>🎯 Orquestrador:</strong>
          <span class="badge bg-primary ms-2">
            {{ orchestratorSettings.name }}
          </span>
        </div>
        <div>
          <strong>🤖 Agentes:</strong>
          <span class="badge bg-secondary ms-2">
            {{ specializedAgents.length }} especializado(s)
          </span>
        </div>
      </div>
    </div>

    <div v-if="isLoading" class="text-muted small mb-2">
      <span class="spinner-border spinner-border-sm me-2" role="status"></span>
      Carregando agentes...
    </div>

    <div v-else-if="isWaitingResponse" class="text-muted small mb-2">
      <span class="spinner-border spinner-border-sm me-2" role="status"></span>
      {{ orchestratorSettings.name }} está processando...
    </div>

    <!-- Área de Mensagens -->
    <div class="flex-grow-1 border rounded p-3 mb-3 overflow-auto" style="min-height: 0;">
      <div v-for="(msg, i) in messages" :key="i"
           class="mb-2 p-2 rounded"
           :style="msg.role === Role.User ? userStyle : assistantStyle">
        <div class="d-flex gap-2">
          <div class="flex-grow-1">
            <div v-html="md.render(msg.content)"></div>
          </div>
          <div v-if="msg.usage" class="flex-shrink-0">
            <button
              @click="openUsageModal(msg.usage)"
              class="btn btn-warning btn-sm shadow-sm rounded-circle p-0"
              style="width: 32px; height: 32px; min-width: 32px; min-height: 32px;"
              title="💡 Ver consumo de tokens"
            >
              <i class="bi bi-lightning-charge-fill"></i>
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Barra de Ações -->
    <div class="d-flex gap-2">
      <input
        class="form-control"
        v-model="userInput"
        @keydown="handleKeyDown"
        :disabled="isLoading || isWaitingResponse"
        placeholder="Digite sua mensagem..."
      />
      <button
        class="btn btn-secondary btn-sm d-flex align-items-center gap-2"
        @click="resetThread"
        :disabled="isLoading"
        title="Criar nova thread e resetar chat"
      >
        <i class="bi bi-arrow-clockwise"></i> Resetar
      </button>
      <button
        class="btn btn-warning btn-sm d-flex align-items-center gap-2"
        @click="showSettingsModal = true"
        :disabled="isLoading"
        title="Configurar orquestrador e agentes"
      >
        <i class="bi bi-gear"></i> Configurar
      </button>
      <button
        class="btn btn-info btn-sm d-flex align-items-center gap-2"
        @click="openThreadsModal"
        :disabled="isLoading"
        title="Ver threads anteriores"
      >
        <i class="bi bi-list-ul"></i> Threads
      </button>
      <button
        class="btn btn-neutral btn-sm d-flex align-items-center gap-2"
        @click="sendMessage"
        :disabled="isLoading || isWaitingResponse"
      >
        <i class="bi bi-send"></i> Enviar
      </button>
    </div>

<!-- Modais -->
    <MultiAgentSettingsModal
      v-if="showSettingsModal"
      :orchestrator-name="orchestratorSettings.name"
      :orchestrator-instructions="orchestratorSettings.instructions"
      :specialized-agents="specializedAgents"
      @close="showSettingsModal = false"
      @save="saveSettings"
    />

    <ThreadsListModal
      v-if="showThreadsModal"
      :threads="threads"
      :current-thread-id="currentThreadId"
      @close="showThreadsModal = false"
      @load-thread="loadThread"
    />

    <TokenUsageModal
      v-if="showUsageModal"
      :usage="selectedUsage"
      @close="showUsageModal = false"
    />
  </div>
</template>
