<!-- FeedbackClassifierView.vue - Página de Classificação de Feedbacks -->
<script setup lang="ts">
import { ref } from 'vue';
import aiInferenceService from '@/services/aiInference';
import AgentSettingsModal from '@/components/agent/AgentSettingsModal.vue';
import TokenUsageModal from '@/components/agent/TokenUsageModal.vue';
import HelpButton from '@/components/common/HelpButton.vue';

interface FeedbackItem {
  id: number;
  text: string;
  classification: string;
  isLoading: boolean;
  usage: {
    input?: number;
    output?: number;
    total?: number;
  } | null;
}

// Lista de feedbacks
const feedbacks = ref<FeedbackItem[]>([
  { id: 1, text: 'Prometeram um retorno que nunca aconteceu', classification: '', isLoading: false, usage: null },
  { id: 2, text: 'Foi bom em partes, mas pode melhorar.', classification: '', isLoading: false, usage: null },
  { id: 3, text: 'A pior experiência que já tive com suporte técnico.', classification: '', isLoading: false, usage: null },
  { id: 4, text: 'Equipe fantástica, resolveram tudo em minutos.', classification: '', isLoading: false, usage: null },
  { id: 5, text: 'Boa experiência, voltaria a contratar.', classification: '', isLoading: false, usage: null }
]);

// Configurações do agente (instruções)
const showSettingsModal = ref(false);
const showTokenModal = ref(false);
const selectedUsage = ref<{ input?: number; output?: number; total?: number } | null>(null);

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
    feedback.usage = null;

    const result = await aiInferenceService.complete({
      instructions: agentSettings.value.instructions,
      message: feedback.text
    });

    feedback.classification = result.content;
    feedback.usage = result.usage || null;
  } catch (error) {
    console.error('Erro ao classificar feedback:', error);
    feedback.classification = 'Erro ao classificar';
  } finally {
    feedback.isLoading = false;
  }
}

// Função para mostrar modal de tokens
function showTokenUsage(usage: { input?: number; output?: number; total?: number } | null) {
  selectedUsage.value = usage;
  showTokenModal.value = true;
}

// Função para salvar configurações
function saveSettings(settings: { name: string; instructions: string }) {
  agentSettings.value = settings;
  showSettingsModal.value = false;
}
</script>

<template>
  <HelpButton>
    <h2 class="mb-5 mt-8"><i class="bi bi-chat-square-text px-2"></i> Descritivo da Página de Classificador de Feedback de Atendimentos</h2>
    <p>
      Esta página apresenta um <strong>classificador de sentimento automático</strong> especializado em avaliar feedbacks de atendimento ao cliente. O sistema utiliza inferência de IA para analisar avaliações textuais e atribuir uma nota de 1 a 5, permitindo quantificar a satisfação dos clientes de forma rápida e consistente.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-list-task px-2"></i> Funcionalidades</h5>
    <ul>
      <li><strong>Classificação Automática:</strong> Analisa textos de feedback e atribui uma nota de 1 (MUITO RUIM) a 5 (MUITO BOM).</li>
      <li><strong>Processamento em Lote:</strong> Permite classificar múltiplos feedbacks individualmente através de botões de ação.</li>
      <li><strong>Visualização de Resultados:</strong> Exibe a classificação com estrelas e descrição textual para fácil compreensão.</li>
      <li><strong>Monitoramento de Tokens:</strong> Rastreia o consumo de tokens de cada classificação para análise de custos.</li>
      <li><strong>Personalização de Instruções:</strong> Permite ajustar as instruções do modelo através do botão "Instruções".</li>
      <li><strong>Feedback Visual:</strong> Indicadores de carregamento durante o processamento de cada item.</li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-database px-2"></i> Abordagens de Classificação</h5>
    <p>
      Este classificador pode operar em <strong>dois modos diferentes</strong>, controlados pela configuração <code>UseClassificationFineTunedModel</code> no <code>appsettings.json</code>:
    </p>

    <h6 class="mt-4 mb-2"><i class="bi bi-1-circle px-2"></i> Modo Few-Shot Learning (UseClassificationFineTunedModel = false)</h6>
    <p>
      Utiliza a técnica de <strong>Few-Shot Learning (Aprendizado com Poucos Exemplos)</strong>, onde o modelo recebe exemplos demonstrativos nas instruções para entender o padrão de classificação esperado.
    </p>

    <p><strong>Como funciona:</strong></p>
    <ol>
      <li><strong>Exemplos de Treinamento:</strong> O prompt contém 5 exemplos (um para cada nota) que ensinam o modelo sobre o comportamento esperado.</li>
      <li><strong>Aprendizado Contextual:</strong> O modelo aprende o padrão através dos exemplos fornecidos, sem necessidade de fine-tuning.</li>
      <li><strong>Inferência Consistente:</strong> Com base nos exemplos, o modelo classifica novos feedbacks de forma consistente.</li>
    </ol>

    <p><strong>Estrutura dos Exemplos:</strong></p>
    <ul>
      <li><strong>Exemplo 1 (Nota 1):</strong> "Serviço péssimo! Nunca mais volto!" → Sentimento: MUITO RUIM</li>
      <li><strong>Exemplo 2 (Nota 2):</strong> "Não gostei!" → Sentimento: RUIM</li>
      <li><strong>Exemplo 3 (Nota 3):</strong> "Resolveram meu problema, mas tive dificuldades..." → Sentimento: MÉDIO</li>
      <li><strong>Exemplo 4 (Nota 4):</strong> "Gostei! Me atenderam bem" → Sentimento: BOM</li>
      <li><strong>Exemplo 5 (Nota 5):</strong> "Ótima equipe, com certeza vou voltar!" → Sentimento: MUITO BOM</li>
    </ul>

    <p><strong>Vantagens:</strong></p>
    <ul>
      <li>✅ Rápido de implementar - não requer treinamento do modelo</li>
      <li>✅ Flexível - fácil de ajustar mudando os exemplos nas instruções</li>
      <li>✅ Custo-efetivo - utiliza modelos pré-treinados sem necessidade de fine-tuning</li>
      <li>✅ Interpretável - os exemplos tornam o comportamento transparente</li>
    </ul>

    <h6 class="mt-4 mb-2"><i class="bi bi-2-circle px-2"></i> Modo Fine-Tuned Model (UseClassificationFineTunedModel = true)</h6>
    <p>
      Utiliza um <strong>modelo Azure OpenAI customizado via Fine-Tuning</strong>, treinado especificamente para classificar feedbacks de atendimento da Contoso AutoTech.
    </p>

    <p><strong>Arquivos de Treinamento:</strong></p>
    <ul>
      <li><code>training_data.jsonl</code> - Dataset de treinamento com 100+ exemplos classificados</li>
      <li><code>validation_data.jsonl</code> - Dataset de validação para avaliar a performance do modelo</li>
    </ul>

    <p><strong>Estrutura dos Dados de Treinamento:</strong></p>
    <p>Cada linha do arquivo JSONL contém uma conversação completa no formato:</p>
    <pre class="bg-light p-3 rounded" style="font-size: 0.85rem;"><code class="text-dark">{
  "messages": [
    {
      "role": "system",
      "content": "Você é um classificador de sentimento de atendimento ao cliente da Contoso Tech..."
    },
    {
      "role": "user",
      "content": "Equipe fantástica, resolveram tudo em minutos."
    },
    {
      "role": "assistant",
      "content": "5"
    }
  ]
}</code></pre>

    <p><strong>Como funciona o Fine-Tuning:</strong></p>
    <ol>
      <li><strong>Preparação de Dados:</strong> Criação de datasets estruturados (training_data.jsonl e validation_data.jsonl) com exemplos rotulados.</li>
      <li><strong>Upload no Azure OpenAI:</strong> Envio dos arquivos para o serviço de Fine-Tuning do Azure OpenAI.</li>
      <li><strong>Treinamento:</strong> O modelo base é ajustado especificamente para a tarefa de classificação de sentimentos usando os exemplos fornecidos.</li>
      <li><strong>Validação:</strong> O modelo é testado com o dataset de validação para garantir precisão.</li>
      <li><strong>Deploy:</strong> O modelo customizado é implantado e configurado no <code>appsettings.json</code> com o deployment name específico.</li>
      <li><strong>Inferência:</strong> Chamadas de classificação utilizam o modelo fine-tuned ao invés do modelo base.</li>
    </ol>

    <p><strong>Vantagens do Fine-Tuning:</strong></p>
    <ul>
      <li>✅ <strong>Maior Precisão:</strong> Modelo treinado especificamente no domínio da Contoso AutoTech</li>
      <li>✅ <strong>Consistência Superior:</strong> Aprende padrões específicos do negócio através de 100+ exemplos</li>
      <li>✅ <strong>Menor Dependência de Prompts:</strong> Não precisa de exemplos nas instruções a cada chamada</li>
      <li>✅ <strong>Tokens Reduzidos:</strong> Economiza tokens por não incluir exemplos em cada requisição</li>
      <li>✅ <strong>Performance Melhorada:</strong> Respostas mais rápidas e previsíveis</li>
      <li>✅ <strong>Adaptação ao Contexto:</strong> Entende nuances específicas de feedbacks automotivos</li>
    </ul>

    <p><strong>Comparação:</strong></p>
    <table class="table table-bordered table-sm mt-3">
      <thead class="table-light">
        <tr>
          <th>Aspecto</th>
          <th>Few-Shot Learning</th>
          <th>Fine-Tuned Model</th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td><strong>Setup</strong></td>
          <td>Imediato</td>
          <td>Requer treinamento prévio</td>
        </tr>
        <tr>
          <td><strong>Precisão</strong></td>
          <td>Boa</td>
          <td>Excelente (otimizada para o domínio)</td>
        </tr>
        <tr>
          <td><strong>Custo por Chamada</strong></td>
          <td>Maior (exemplos em cada request)</td>
          <td>Menor (sem exemplos necessários)</td>
        </tr>
        <tr>
          <td><strong>Flexibilidade</strong></td>
          <td>Alta (ajustes via prompt)</td>
          <td>Média (requer retreinamento)</td>
        </tr>
        <tr>
          <td><strong>Quantidade de Dados</strong></td>
          <td>5 exemplos no prompt</td>
          <td>100+ exemplos de treinamento</td>
        </tr>
        <tr>
          <td><strong>Ideal Para</strong></td>
          <td>Prototipagem e testes rápidos</td>
          <td>Produção em escala</td>
        </tr>
      </tbody>
    </table>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-diagram-3 px-2"></i> Fluxo de Classificação</h5>
    <ol>
      <li><strong>Entrada:</strong> O usuário clica no botão "Classificar" de um feedback específico.</li>
      <li><strong>Processamento:</strong> O texto do feedback é enviado ao serviço de AI Inference junto com as instruções contendo os exemplos.</li>
      <li><strong>Análise:</strong> O modelo analisa o sentimento do texto com base nos padrões aprendidos dos exemplos.</li>
      <li><strong>Resposta:</strong> O modelo retorna apenas um número de 1 a 5.</li>
      <li><strong>Visualização:</strong> O sistema converte o número em uma classificação visual com estrelas e descrição.</li>
      <li><strong>Rastreamento:</strong> Os dados de consumo de tokens são armazenados e podem ser visualizados através do ícone de raio.</li>
    </ol>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-lightning-charge px-2"></i> Monitoramento de Tokens</h5>
    <p>
      Cada classificação rastreia o consumo de tokens (entrada, saída e total), permitindo análise de custos e otimização do uso do modelo. O ícone de raio (⚡) aparece após a classificação e abre um modal com os detalhes do consumo.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-gear px-2"></i> Personalização</h5>
    <p>
      As instruções do classificador podem ser personalizadas através do botão "Instruções". Isso permite:
    </p>
    <ul>
      <li>Ajustar os critérios de classificação</li>
      <li>Modificar os exemplos de few-shot learning</li>
      <li>Adaptar o sistema para diferentes contextos de negócio</li>
      <li>Refinar a sensibilidade do classificador</li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-bullseye px-2"></i> Objetivo</h5>
    <p>
      O objetivo deste classificador é <strong>automatizar a análise de satisfação do cliente</strong>, transformando feedbacks qualitativos em métricas quantitativas que podem ser facilmente agregadas, analisadas e utilizadas para tomada de decisões estratégicas sobre qualidade de atendimento.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-link-45deg px-2"></i> Links Úteis</h5>
    <ul>
      <li>
        <a href="https://learn.microsoft.com/pt-br/agent-framework/overview/agent-framework-overview" target="_blank" rel="noopener">
          Visão Geral do Microsoft Agent Framework
        </a>
      </li>
      <li>
        <a href="https://learn.microsoft.com/en-us/azure/ai-foundry/openai/concepts/prompt-engineering#examples" target="_blank" rel="noopener">
          Prompt Engineering - Examples (Few-Shot Learning)
        </a>
      </li>
      <li>
        <a href="https://learn.microsoft.com/en-us/azure/ai-foundry/concepts/fine-tuning-overview" target="_blank" rel="noopener">
          Fine-Tuning Overview - Azure AI Foundry
        </a>
      </li>
      <li>
        <a href="https://www.azurebrasil.cloud/blog/customizando-modelos-do-azure-open-ai-com-fine-tuning-2/" target="_blank" rel="noopener">
          Customizando Modelos do Azure OpenAI com Fine-Tuning - Azure Brasil
        </a>
      </li>
    </ul>
  </HelpButton>

  <div class="container-fluid p-4">
    <!-- Cabeçalho -->
    <div class="d-flex justify-content-between align-items-center mb-4">
      <h2 class="s-h2">
        <i class="bi bi-chat-square-text me-2"></i>
        Classificador de Feedback de Atendimentos
      </h2>
      <button
        class="btn btn-warning btn-sm"
        @click="showSettingsModal = true"
        title="Configurar instruções do classificador"
      >
        <i class="bi bi-gear me-1"></i> Instruções
      </button>
    </div>

    <!-- Tabela de Feedbacks -->
    <div class="card shadow-sm">
      <div class="card-body p-0">
        <div class="table-responsive">
          <table class="table table-hover mb-0">
            <thead class="table-light">
              <tr>
                <th scope="col" class="text-muted" style="width: 5%;">#</th>
                <th scope="col" class="text-muted" style="width: 45%;">Feedback</th>
                <th scope="col" class="text-muted" style="width: 15%;">Ação</th>
                <th scope="col" class="text-muted" style="width: 25%;">Classificação</th>
                <th scope="col" class="text-muted" style="width: 10%;">Tokens</th>
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
                <td class="align-middle text-center">
                  <button
                    v-if="feedback.usage"
                    class="btn btn-sm btn-outline-info"
                    @click="showTokenUsage(feedback.usage)"
                    title="Ver consumo de tokens"
                  >
                    <i class="bi bi-lightning-charge-fill"></i>
                  </button>
                  <span v-else class="text-muted">-</span>
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

    <!-- Modal de Token Usage -->
    <TokenUsageModal
      v-if="showTokenModal"
      :usage="selectedUsage"
      @close="showTokenModal = false"
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

