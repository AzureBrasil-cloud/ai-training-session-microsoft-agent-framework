<!-- FeedbackClassifierView.vue - Página de Classificação de Feedbacks -->
<script setup lang="ts">
import { ref } from 'vue';
import aiInferenceService from '@/services/aiInference';
import AgentSettingsModal from '@/components/agent/AgentSettingsModal.vue';

interface FeedbackItem {
  id: number;
  text: string;
  classification: string;
  isLoading: boolean;
}

// Lista de feedbacks
const feedbacks = ref<FeedbackItem[]>([
  { id: 1, text: 'Prometeram um retorno que nunca aconteceu', classification: '', isLoading: false },
  { id: 2, text: 'Foi bom em partes, mas pode melhorar.', classification: '', isLoading: false },
  { id: 3, text: 'A pior experiência que já tive com suporte técnico.', classification: '', isLoading: false },
  { id: 4, text: 'Equipe fantástica, resolveram tudo em minutos.', classification: '', isLoading: false },
  { id: 5, text: 'Boa experiência, voltaria a contratar.', classification: '', isLoading: false }
]);

// Configurações do agente (instruções)
const showSettingsModal = ref(false);
const agentSettings = ref({
  name: 'Classificador de Sentimentos',
  instructions: 'Você é um classificador de sentimento de atendimento ao cliente da Contoso AutoTech. Sua tarefa é receber uma avaliação de serviço e retornar uma nota de 1 a 5, onde: 1 = MUITO RUIM, 2 = RUIM, 3 = MÉDIO, 4 = BOM e 5 = MUITO BOM.\n' +
    '\n' +
    'Exemplo 1\n' +
    '- Entrada: Serviço péssimo! Nunca mais volto!\n' +
    '- Saída: 1\n' +
    '\n' +
    'Exemplo 2\n' +
    '- Entrada: Não gostei!\n' +
    '- Saída: 2\n' +
    '\n' +
    'Exemplo 3\n' +
    '- Entrada: Resolveram meu problema, mas tive dificuldades de me comunicar com eles.\n' +
    '- Saída: 3\n' +
    '\n' +
    'Exemplo 4\n' +
    '- Entrada: Gostei! Me atenderam bem\n' +
    '- Saída: 4\n' +
    '\n' +
    'Exemplo 5\n' +
    '- Entrada: Ótima equipe, com certeza vou voltar!\n' +
    '- Saída: 5\n' +
    '\n' +
    'Retorne APENAS o número de 1 a 5, sem texto adicional.'
});

// Função para mapear número para nome da classificação
function getClassificationName(classification: string): string {
  const trimmed = classification.trim();

  // Mapeia números para nomes
  const classificationMap: Record<string, string> = {
    '1': '⭐ MUITO RUIM',
    '2': '⭐⭐ RUIM',
    '3': '⭐⭐⭐ MÉDIO',
    '4': '⭐⭐⭐⭐ BOM',
    '5': '⭐⭐⭐⭐⭐ MUITO BOM'
  };

  // Retorna o nome correspondente ou o texto original se não for um número de 1 a 5
  return classificationMap[trimmed] || classification;
}

// Função para classificar um feedback
async function classifyFeedback(feedback: FeedbackItem) {
  try {
    feedback.isLoading = true;
    feedback.classification = '';

    const result = await aiInferenceService.complete({
      instructions: agentSettings.value.instructions,
      message: feedback.text
    });

    feedback.classification = result.content;
  } catch (error) {
    console.error('Erro ao classificar feedback:', error);
    feedback.classification = 'Erro ao classificar';
  } finally {
    feedback.isLoading = false;
  }
}

// Função para salvar configurações
function saveSettings(settings: { name: string; instructions: string }) {
  agentSettings.value = settings;
  showSettingsModal.value = false;
}
</script>

<template>
  <div class="container-fluid p-4">
    <!-- Cabeçalho -->
    <div class="d-flex justify-content-between align-items-center mb-4">
      <h2 class="s-h2">
        <i class="bi bi-chat-square-text me-2"></i>
        Classificador de Feedback de Atendimentos
      </h2>
      <button
        class="btn btn-warning"
        @click="showSettingsModal = true"
        title="Configurar instruções do classificador"
      >
        <i class="bi bi-gear"></i> Instruções
      </button>
    </div>

    <!-- Tabela de Feedbacks -->
    <div class="card shadow-sm">
      <div class="card-body p-0">
        <div class="table-responsive">
          <table class="table table-hover mb-0">
            <thead class="table-light">
              <tr>
                <th scope="col" style="width: 5%;">#</th>
                <th scope="col" style="width: 50%;">Feedback</th>
                <th scope="col" style="width: 15%;">Ação</th>
                <th scope="col" style="width: 30%;">Classificação</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="feedback in feedbacks" :key="feedback.id">
                <th scope="row" class="align-middle">{{ feedback.id }}</th>
                <td class="align-middle">{{ feedback.text }}</td>
                <td class="align-middle">
                  <button
                    class="btn btn-purple btn-sm"
                    @click="classifyFeedback(feedback)"
                    :disabled="feedback.isLoading"
                  >
                    <span v-if="feedback.isLoading" class="spinner-border spinner-border-sm me-1" role="status"></span>
                    <i v-else class="bi bi-stars me-1"></i>
                    Classificar
                  </button>
                </td>
                <td class="align-middle">
                  <div v-if="feedback.isLoading" class="text-muted">
                    <span class="spinner-border spinner-border-sm me-2" role="status"></span>
                    Classificando...
                  </div>
                  <div v-else-if="feedback.classification" class="fw-semibold text-success">
                    {{ getClassificationName(feedback.classification) }}
                  </div>
                  <div v-else class="text-muted fst-italic">
                    Aguardando classificação
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>

    <!-- Modal de Configurações -->
    <AgentSettingsModal
      v-if="showSettingsModal"
      :agent-name="agentSettings.name"
      :agent-instructions="agentSettings.instructions"
      @close="showSettingsModal = false"
      @save="saveSettings"
    />
  </div>
</template>

<style scoped>
.table th {
  font-weight: 600;
  color: #495057;
}

.table td {
  vertical-align: middle;
}

.table-hover tbody tr:hover {
  background-color: rgba(102, 126, 234, 0.05);
}

.card {
  border: none;
  border-radius: 0.5rem;
}

.s-h2 {
  font-size: 1.75rem;
  font-weight: 600;
}
</style>

