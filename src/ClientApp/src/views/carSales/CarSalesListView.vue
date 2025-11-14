<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'
import carSalesService, { type CarSale } from '@/services/carSales'
import HelpButton from '@/components/common/HelpButton.vue'

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
  <HelpButton>
    <h2 class="mb-5 mt-8"><i class="bi bi-megaphone px-2"></i> Descritivo da Página de Anúncios de Carros</h2>
    <p>
      Esta página apresenta um <strong>sistema inteligente de análise de anúncios de veículos</strong> que utiliza um agente autônomo para processar, enriquecer e avaliar automaticamente anúncios de carros disponíveis para venda. O agente extrai informações de uma página web externa, analisa cada veículo e gera insights valiosos para tomada de decisão.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-list-task px-2"></i> Funcionalidades</h5>
    <ul>
      <li><strong>Visualização de Anúncios:</strong> Lista todos os veículos analisados com informações detalhadas (modelo, placa, cor, preço).</li>
      <li><strong>Imagens Ampliadas:</strong> Permite visualizar imagens dos veículos em tamanho completo através de modal.</li>
      <li><strong>Análise Detalhada:</strong> Exibe pontos fortes, pontos fracos, preço de referência e considerações do agente.</li>
      <li><strong>Atualização Manual:</strong> Botão para reprocessar os anúncios e obter dados atualizados.</li>
      <li><strong>Interface Responsiva:</strong> Layout adaptável para diferentes tamanhos de tela.</li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-robot px-2"></i> Agente Autônomo de Análise</h5>
    <p>
      Este sistema utiliza um <strong>Agente Autônomo Inteligente</strong> que executa automaticamente quando a configuração <code>ExecuteCarsSalesAgent</code> está definida como <code>true</code> no <code>appsettings.json</code>.
    </p>

    <h6 class="mt-4 mb-2"><i class="bi bi-gear-fill px-2"></i> Configuração de Execução</h6>
    <p>
      O agente é controlado pela configuração:
    </p>
    <pre class="bg-light p-3 rounded" style="font-size: 0.85rem;"><code class="text-dark">"Application": {
  "ExecuteCarsSalesAgent": true,  // Ativa a execução do agente autônomo
  "CarSalesRemoteUrl": "https://app-cars-sales.../",  // URL da página de anúncios
  ...
}</code></pre>

    <p>
      <strong>Quando ativado (true):</strong> O agente executa automaticamente na inicialização da aplicação através do <code>CarSalesHostedService</code>, processando todos os anúncios disponíveis na página remota.
    </p>
    <p>
      <strong>Quando desativado (false):</strong> O agente não executa automaticamente. Os dados só são carregados se já existirem no banco de dados.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-cpu px-2"></i> Como o Agente Funciona</h5>
    <p>
      O agente autônomo executa um processo em 4 etapas principais, utilizando <strong>Function Tools</strong> para interagir com sistemas externos e realizar operações complexas:
    </p>

    <ol>
      <li class="mb-3">
        <strong>Etapa 1: Extração de Dados da Página Remota</strong>
        <p>O agente acessa a URL configurada em <code>CarSalesRemoteUrl</code> e extrai o HTML completo da página de anúncios de carros. Esta página contém cards com informações sobre veículos disponíveis para venda.</p>
        <p><strong>Tool Utilizada:</strong> <code>GetCarSalesPageContent()</code></p>
        <ul>
          <li>Retorna o HTML bruto da página de anúncios</li>
          <li>Não possui parâmetros de entrada</li>
          <li>Dados retornados servem como input para análise do LLM</li>
        </ul>
      </li>

      <li class="mb-3">
        <strong>Etapa 2: Análise e Criação de Registros</strong>
        <p>Para cada veículo identificado na página, o agente invoca a tool de criação de registro no banco de dados. O LLM extrai automaticamente as informações estruturadas do HTML (modelo, placa, cor, preço, descrição, imagem URL) e passa para a tool.</p>
        <p><strong>Tool Utilizada:</strong> <code>CreateCarSale()</code></p>
        <ul>
          <li><strong>Parâmetros:</strong>
            <ul>
              <li><code>imageUrl</code> - URL da imagem do veículo</li>
              <li><code>model</code> - Modelo do veículo (ex: "Toyota Corolla XEi 2.0")</li>
              <li><code>color</code> - Cor do veículo</li>
              <li><code>licensePlate</code> - Placa do veículo</li>
              <li><code>price</code> - Preço anunciado</li>
              <li><code>fullDescription</code> - Descrição completa extraída do anúncio</li>
            </ul>
          </li>
          <li>Esta tool persiste o registro no banco de dados</li>
          <li>Chamada automaticamente pelo agente para cada carro encontrado</li>
        </ul>
      </li>

      <li class="mb-3">
        <strong>Etapa 3: Análise de Pontos Fortes e Fracos</strong>
        <p>O agente analisa a descrição completa de cada veículo e identifica aspectos positivos (pontos fortes) e negativos (pontos fracos). Esta análise considera fatores como estado de conservação, histórico de manutenção, equipamentos, e possíveis defeitos mencionados.</p>
        <p><strong>Técnica Utilizada:</strong> <strong>Structured Output</strong> - O LLM retorna um objeto JSON estruturado garantindo que os dados sigam um schema definido com listas de pontos fortes e fracos que são persistidos no banco de dados.</p>
      </li>

      <li class="mb-3">
        <strong>Etapa 4: Geração de Considerações do Agente</strong>
        <p>Com base no preço anunciado, preço de referência (quando disponível), pontos fortes e fracos, o agente gera uma análise crítica sobre se o valor do veículo é justo, caro ou barato. Inclui recomendações e considerações sobre a oportunidade de compra.</p>
        <p><strong>Técnica Utilizada:</strong> <strong>Structured Output</strong> - Garante que as considerações do agente sejam retornadas em formato estruturado e previsível, facilitando a persistência e exibição dos dados.</p>
      </li>
    </ol>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-tools px-2"></i> Ferramentas (Tools) Disponíveis</h5>
    <p>
      O agente autônomo utiliza <strong>2 Function Tools principais</strong> para executar suas tarefas:
    </p>

    <div class="card mb-3">
      <div class="card-body">
        <h6 class="card-title"><i class="bi bi-download px-2"></i> 1. GetCarSalesPageContent</h6>
        <p class="card-text">
          <strong>Descrição:</strong> Obtém o conteúdo HTML bruto da página de anúncios de carros do sistema remoto.
        </p>
        <p><strong>Parâmetros:</strong> Nenhum</p>
        <p><strong>Retorno:</strong> String contendo todo o HTML da página</p>
        <p><strong>Uso:</strong> Primeira etapa do processo - extração de dados da fonte externa</p>
        <p class="mb-0"><strong>Observação:</strong> Retorna dados não formatados que o LLM interpreta automaticamente</p>
      </div>
    </div>

    <div class="card mb-3">
      <div class="card-body">
        <h6 class="card-title"><i class="bi bi-plus-circle px-2"></i> 2. CreateCarSale</h6>
        <p class="card-text">
          <strong>Descrição:</strong> Cria um novo registro de anúncio de carro no banco de dados com todas as informações extraídas e analisadas.
        </p>
        <p><strong>Parâmetros:</strong></p>
        <ul class="mb-2">
          <li><code>imageUrl</code> (string) - URL da imagem do veículo</li>
          <li><code>model</code> (string) - Modelo completo do veículo</li>
          <li><code>color</code> (string) - Cor do veículo</li>
          <li><code>licensePlate</code> (string) - Placa do veículo</li>
          <li><code>price</code> (decimal) - Preço anunciado</li>
          <li><code>fullDescription</code> (string) - Descrição completa do anúncio</li>
        </ul>
        <p><strong>Retorno:</strong> Objeto CarSale criado com ID e informações persistidas</p>
        <p class="mb-0"><strong>Uso:</strong> Chamada para cada veículo encontrado na página - persiste dados estruturados</p>
      </div>
    </div>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-diagram-3 px-2"></i> Fluxo Completo de Execução</h5>
    <div class="bg-light p-4 rounded">
      <ol class="mb-0 text-dark">
        <li><strong>Inicialização:</strong> Aplicação inicia → <code>CarSalesHostedService</code> verifica <code>ExecuteCarsSalesAgent</code></li>
        <li><strong>Extração:</strong> Se true → Agente chama <code>GetCarSalesPageContent()</code> → Obtém HTML da página remota</li>
        <li><strong>Interpretação:</strong> LLM analisa o HTML e identifica todos os cards de veículos</li>
        <li><strong>Processamento Individual:</strong> Para cada veículo:
          <ul>
            <li>Extrai dados estruturados (modelo, placa, cor, preço, etc.)</li>
            <li>Chama <code>CreateCarSale()</code> para persistir no banco</li>
            <li>Analisa descrição para identificar pontos fortes e fracos</li>
            <li>Compara com preço de referência (se disponível)</li>
            <li>Gera considerações sobre o valor do veículo</li>
            <li>Atualiza registro com análises completas</li>
          </ul>
        </li>
        <li><strong>Finalização:</strong> Todos os registros salvos → Dados disponíveis para visualização nesta página</li>
      </ol>
    </div>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-lightning-charge px-2"></i> Características do Agente Autônomo</h5>
    <ul>
      <li>✅ <strong>Execução Automática:</strong> Não requer intervenção manual quando ativado</li>
      <li>✅ <strong>Processamento Inteligente:</strong> LLM interpreta HTML não estruturado automaticamente</li>
      <li>✅ <strong>Análise Contextual:</strong> Avalia cada veículo considerando múltiplos fatores</li>
      <li>✅ <strong>Enriquecimento de Dados:</strong> Adiciona insights além das informações brutas (pontos fortes/fracos, considerações)</li>
      <li>✅ <strong>Persistência Automática:</strong> Salva resultados no banco de dados para consulta posterior</li>
      <li>✅ <strong>Escalável:</strong> Processa múltiplos anúncios em sequência automaticamente</li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-exclamation-triangle px-2"></i> Preço de Referência</h5>
    <p>
      O sistema mantém uma tabela de preços de referência (<code>CarReferencePrice</code>) para alguns modelos de veículos. Quando disponível, o agente compara o preço anunciado com o preço de referência e inclui essa análise nas considerações, indicando se o veículo está:
    </p>
    <ul>
      <li><strong>Justo:</strong> Preço próximo ao valor de referência</li>
      <li><strong>Caro:</strong> Preço significativamente acima da referência</li>
      <li><strong>Barato:</strong> Preço abaixo da referência (possível oportunidade)</li>
    </ul>
    <p>Para veículos sem preço de referência, o agente foca apenas nas considerações baseadas em estado de conservação, equipamentos e características mencionadas.</p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-bullseye px-2"></i> Objetivo</h5>
    <p>
      O objetivo deste sistema é <strong>automatizar a análise de anúncios de veículos</strong>, fornecendo insights inteligentes que auxiliam compradores e vendedores a tomarem decisões mais informadas. O agente funciona como um assistente especializado que processa grandes volumes de anúncios e destaca informações relevantes de forma estruturada e acessível.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-link-45deg px-2"></i> Links Úteis</h5>
    <ul>
      <li>
        <a href="https://learn.microsoft.com/pt-br/agent-framework/overview/agent-framework-overview" target="_blank" rel="noopener">
          Visão Geral do Microsoft Agent Framework
        </a>
      </li>
      <li>
        <a href="https://learn.microsoft.com/pt-br/agent-framework/tutorials/agents/function-tools?pivots=programming-language-csharp" target="_blank" rel="noopener">
          Using Function Tools with an Agent
        </a>
      </li>
      <li>
        <a href="https://learn.microsoft.com/en-au/agent-framework/tutorials/agents/structured-output?pivots=programming-language-csharp" target="_blank" rel="noopener">
          Structured Output with Agents
        </a>
      </li>
    </ul>
  </HelpButton>

  <div class="container-fluid py-4">
    <div class="row mb-4">
      <div class="col-12">
        <div class="d-flex justify-content-between align-items-center">
          <h2>
            <i class="bi bi-car-front-fill me-2"></i>
            Anúncios de carros
          </h2>
          <button
            class="btn btn-primary btn-sm"
            @click="loadCarSales"
            :disabled="loading"
          >
            <i class="bi bi-arrow-clockwise me-1"></i>
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
