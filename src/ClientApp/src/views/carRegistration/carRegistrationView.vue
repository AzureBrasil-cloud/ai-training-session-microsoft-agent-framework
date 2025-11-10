<script setup lang="ts">
import { ref, onMounted } from 'vue';
import HelpButton from "@/components/common/HelpButton.vue";
import agentService from '@/services/agent';
import type { MessageResult } from '@/models/messageResult';
import type { Thread } from '@/models/thread';
import MarkdownIt from 'markdown-it';

const md = new MarkdownIt();

// Feature ID para cadastro de carros
const FEATURE_CAR_REGISTRATION = 1;

// Estados
const isLoading = ref(false);
const isWaitingResponse = ref(false);
const userInput = ref('');
const messages = ref<MessageResult[]>([]);
const currentThreadId = ref<string>('');
const threads = ref<Thread[]>([]);

// Configurações do agente
const agentSettings = ref({
  name: 'Agente de Cadastro de Carros',
  instructions: 'Você é um assistente que ajuda a cadastrar e consultar carros, solicitando informações como modelo, marca, ano e placa.'
});

// Modais
const showInstructionsModal = ref(false);
const showThreadsModal = ref(false);

// Cópias temporárias para edição no modal
const tempAgentName = ref('');
const tempAgentInstructions = ref('');

// Estilos das mensagens
const userStyle = 'max-width: 75%; background-color: #0d6efd; color: white; margin-left: auto;';
const assistantStyle = 'max-width: 75%; background-color: #e6e6e6; color: black;';

// Função para criar nova thread
async function createNewThread() {
  try {
    isLoading.value = true;
    const thread = await agentService.createThread({ feature: FEATURE_CAR_REGISTRATION });
    currentThreadId.value = thread.id;
    messages.value = [];

    // Mensagem de boas-vindas
    messages.value.push({
      role: 'Agent',
      content: '👋 Olá! Sou o Agente de Cadastro de Carros. Como posso ajudar você hoje? Posso auxiliar no cadastro, consulta ou atualização de informações de veículos.'
    });
  } catch (error) {
    console.error('Erro ao criar thread:', error);
    alert('Erro ao criar thread. Tente novamente.');
  } finally {
    isLoading.value = false;
  }
}

// Função para enviar mensagem
async function sendMessage() {
  if (!userInput.value.trim() || !currentThreadId.value) return;

  const input = userInput.value;
  messages.value.push({ role: 'User', content: input });
  userInput.value = '';
  isWaitingResponse.value = true;

  try {
    const response = await agentService.run({
      threadId: currentThreadId.value,
      message: input,
      agentName: agentSettings.value.name,
      agentInstructions: agentSettings.value.instructions
    });

    messages.value.push(response);
  } catch (error) {
    console.error('Erro ao enviar mensagem:', error);
    messages.value.push({
      role: 'Agent',
      content: '❌ Desculpe, ocorreu um erro ao processar sua mensagem. Tente novamente.'
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

// Função para abrir modal de instruções
function openInstructionsModal() {
  tempAgentName.value = agentSettings.value.name;
  tempAgentInstructions.value = agentSettings.value.instructions;
  showInstructionsModal.value = true;
}

// Função para salvar instruções
function saveInstructions() {
  agentSettings.value.name = tempAgentName.value;
  agentSettings.value.instructions = tempAgentInstructions.value;
  showInstructionsModal.value = false;
}

// Função para abrir modal de threads
async function openThreadsModal() {
  try {
    const result = await agentService.listThreads(FEATURE_CAR_REGISTRATION);
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

// Inicialização
onMounted(async () => {
  await createNewThread();
});

const videoUrl = `${window.location.origin}/videos/car-agent.mp4`;
</script>

<template>
  <HelpButton>
    <div class="d-flex justify-content-center my-4">
      <video
          ref="player"
          :src="videoUrl"
          controls
          loop
          autoplay
          muted
          playsinline
          style="width: 100%;"
      ></video>
    </div>

    <h2 class="mb-5 mt-8"><i class="bi bi-car-front px-2"></i> Descritivo da Página do Assistente de Cadastro de Carros</h2>
    <p>
      Esta página apresenta um <strong>assistente virtual especializado em cadastro e gerenciamento de veículos</strong>. Ele auxilia o usuário a cadastrar, consultar e atualizar informações de carros como marca, modelo, ano, placa e outros dados relevantes.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-list-task px-2"></i> Funcionalidades</h5>
    <ul>
      <li><strong>Cadastro de Veículos:</strong> O assistente guia o usuário no processo de cadastro, solicitando informações necessárias.</li>
      <li><strong>Consulta de Informações:</strong> Permite buscar dados de veículos já cadastrados.</li>
      <li><strong>Atualização de Dados:</strong> Possibilita modificar informações de veículos existentes.</li>
      <li><strong>Interface Conversacional:</strong> Toda interação via chat em linguagem natural.</li>
      <li><strong>Gerenciamento de Threads:</strong> Mantém histórico de conversas organizadas em threads.</li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-gear px-2"></i> Personalização</h5>
    <p>
      O agente permite personalizar suas instruções e comportamento através do botão "Instruções", possibilitando ajustar o contexto e a forma como ele interage com o usuário.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-bullseye px-2"></i> Objetivo</h5>
    <p>
      O objetivo deste agente é <strong>simplificar o processo de cadastro e gerenciamento de veículos</strong>, oferecendo uma interface intuitiva e conversacional que elimina a necessidade de formulários complexos.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-link-45deg px-2"></i> Links Úteis</h5>
    <ul>
      <li>
        <a href="https://learn.microsoft.com/en-us/azure/ai-services/agents/overview" target="_blank" rel="noopener">
          Azure AI Agent Service – Visão Geral
        </a>
      </li>
      <li>
        <a href="https://tallesvaliatti.com/criando-uma-aplica%C3%A7%C3%A3o-com-o-azure-ai-agent-service-parte-1-1d4fef7901a4" target="_blank" rel="noopener">
          Azure AI Agent Service – Overview
        </a>
      </li>
    </ul>
  </HelpButton>

  <!-- Chat Window -->
  <div class="d-flex flex-column p-3" style="height: 95vh;">
    <h4 class="mb-3 item-purple s-h3"><i class="bi bi-car-front px-3"></i>Assistente de Cadastro de Carros</h4>

    <div v-if="isLoading" class="text-muted small mb-2">
      <span class="spinner-border spinner-border-sm me-2" role="status"></span>
      Carregando agente...
    </div>

    <div v-else-if="isWaitingResponse" class="text-muted small mb-2">
      <span class="spinner-border spinner-border-sm me-2" role="status"></span>
      Pensando...
    </div>

    <!-- Área de Mensagens -->
    <div class="flex-grow-1 border rounded p-3 mb-3 overflow-auto" style="min-height: 0;">
      <div v-for="(msg, i) in messages" :key="i"
           class="mb-2 p-2 rounded"
           :style="msg.role === 'User' ? userStyle : assistantStyle">
        <div v-html="md.render(msg.content)"></div>
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
        class="btn btn-secondary"
        @click="resetThread"
        :disabled="isLoading"
        title="Criar nova thread e resetar chat"
      >
        <i class="bi bi-arrow-clockwise"></i> Resetar
      </button>
      <button
        class="btn btn-warning"
        @click="openInstructionsModal"
        :disabled="isLoading"
        title="Configurar instruções do agente"
      >
        <i class="bi bi-gear"></i> Instruções
      </button>
      <button
        class="btn btn-info"
        @click="openThreadsModal"
        :disabled="isLoading"
        title="Ver threads anteriores"
      >
        <i class="bi bi-list-ul"></i> Threads
      </button>
      <button
        class="btn btn-purple"
        @click="sendMessage"
        :disabled="isLoading || isWaitingResponse"
      >
        <i class="bi bi-send"></i> Enviar
      </button>
    </div>
  </div>

  <!-- Modal de Instruções -->
  <div v-if="showInstructionsModal" class="modal d-block" tabindex="-1" style="background-color: rgba(0,0,0,0.5);">
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title"><i class="bi bi-gear"></i> Configurações do Agente</h5>
          <button type="button" class="btn-close" @click="showInstructionsModal = false"></button>
        </div>
        <div class="modal-body">
          <div class="mb-3">
            <label for="agentName" class="form-label">Nome do Agente</label>
            <input
              type="text"
              class="form-control"
              id="agentName"
              v-model="tempAgentName"
              placeholder="Ex: Agente de Cadastro de Carros"
            />
          </div>
          <div class="mb-3">
            <label for="agentInstructions" class="form-label">Instruções do Agente</label>
            <textarea
              class="form-control"
              id="agentInstructions"
              rows="5"
              v-model="tempAgentInstructions"
              placeholder="Descreva como o agente deve se comportar..."
            ></textarea>
          </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-secondary" @click="showInstructionsModal = false">Cancelar</button>
          <button type="button" class="btn btn-primary" @click="saveInstructions">Salvar</button>
        </div>
      </div>
    </div>
  </div>

  <!-- Modal de Threads -->
  <div v-if="showThreadsModal" class="modal d-block" tabindex="-1" style="background-color: rgba(0,0,0,0.5);">
    <div class="modal-dialog modal-dialog-centered modal-lg">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title"><i class="bi bi-list-ul"></i> Threads Anteriores</h5>
          <button type="button" class="btn-close" @click="showThreadsModal = false"></button>
        </div>
        <div class="modal-body">
          <div v-if="threads.length === 0" class="text-muted text-center py-4">
            Nenhuma thread encontrada.
          </div>
          <div v-else class="table-responsive">
            <table class="table table-hover">
              <thead>
                <tr>
                  <th scope="col" style="width: 40%;">ID da Thread</th>
                  <th scope="col" style="width: 60%;">Primeira Mensagem</th>
                </tr>
              </thead>
              <tbody>
                <tr
                  v-for="thread in threads"
                  :key="thread.id"
                  @click="loadThread(thread.id)"
                  style="cursor: pointer;"
                  :class="{ 'table-active': thread.id === currentThreadId }"
                >
                  <td>
                    <i class="bi bi-chat-dots me-2"></i>
                    <code>{{ thread.id }}</code>
                  </td>
                  <td>
                    <span v-if="thread.firstTruncatedMessage && thread.firstTruncatedMessage.trim()" class="text-muted">
                      {{ thread.firstTruncatedMessage }}
                    </span>
                    <span v-else class="text-muted fst-italic">
                      Sem mensagem
                    </span>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-secondary" @click="showThreadsModal = false">Fechar</button>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.btn-purple {
  background-color: #6f42c1;
  border-color: #6f42c1;
  color: white;
}

.btn-purple:hover {
  background-color: #5a32a3;
  border-color: #5a32a3;
}

.item-purple {
  color: #6f42c1;
}

.s-h3 {
  font-weight: 600;
}

.bck-h {
  background-color: #f8f9fa;
}

.modal.d-block {
  display: block !important;
}
</style>

