<script setup lang="ts">
import HelpButton from "@/components/common/HelpButton.vue";
import AgentChatWindow from "@/components/agent/AgentChatWindow.vue";

const customerPoliciesImage = `${window.location.origin}/images/Feature2.png`;
</script>

<template>
  <HelpButton>
    <div class="d-flex justify-content-center my-4">
      <img
        :src="customerPoliciesImage"
        alt="Assistente de Políticas de Relacionamento"
        class="img-fluid rounded"
        style="width: 100%; max-width: 800px;"
      />
    </div>

    <h2 class="mb-5 mt-8"><i class="bi bi-shield-check px-2"></i> Descritivo da Página de Políticas de Relacionamento com o Cliente</h2>
    <p>
      Esta página apresenta um <strong>assistente virtual especializado em políticas de relacionamento com o cliente</strong>. Ele auxilia usuários e equipes a consultar, esclarecer dúvidas e obter orientações sobre as políticas de atendimento, garantias, trocas, devoluções e demais diretrizes de relacionamento da empresa.
    </p>
    <p>
      <strong>🔐 Consulta de Documentos Privados:</strong> Este agente utiliza <strong>RAG (Retrieval Augmented Generation)</strong> para consultar e extrair informações de documentos privados da empresa, como manuais de políticas internas, regulamentos e diretrizes confidenciais, garantindo respostas precisas e baseadas em fontes oficiais.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-list-task px-2"></i> Funcionalidades</h5>
    <ul>
      <li><strong>Consulta de Políticas:</strong> O assistente fornece informações sobre políticas de atendimento, garantias e trocas.</li>
      <li><strong>Esclarecimento de Dúvidas:</strong> Responde questões sobre direitos do cliente e procedimentos.</li>
      <li><strong>Orientação de Processos:</strong> Guia sobre como proceder em casos de reclamações, devoluções ou solicitações especiais.</li>
      <li><strong>Interface Conversacional:</strong> Toda interação via chat em linguagem natural.</li>
      <li><strong>Gerenciamento de Threads:</strong> Mantém histórico de conversas organizadas em threads.</li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-gear px-2"></i> Personalização</h5>
    <p>
      O agente permite personalizar suas instruções e comportamento através do botão "Instruções", possibilitando ajustar o contexto e a forma como ele interage com o usuário.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-database px-2"></i> Abordagens de Recuperação de Dados</h5>
    <p>
      Este agente pode operar em <strong>dois modos diferentes</strong> de recuperação de informações, controlados pela configuração <code>UseBasicCustomerPoliciesAgentContext</code> no <code>appsettings.json</code>:
    </p>

    <h6 class="mt-4 mb-2"><i class="bi bi-1-circle px-2"></i> Modo Básico (UseBasicCustomerPoliciesAgentContext = true)</h6>
    <p>
      Utiliza o <strong>TextSearchProvider</strong> com dados estáticos predefinidos no código. Neste modo, o agente consulta um conjunto fixo de snippets de texto que simulam documentos de políticas.
    </p>
    <p><strong>Dados consultados (estáticos):</strong></p>
    <ul>
      <li><strong>Política de Devoluções e Reembolsos:</strong> Informações sobre prazos de devolução (30 dias para produtos, 7 dias para serviços), condições de reembolso e procedimentos de inspeção.</li>
      <li><strong>Política de Garantia:</strong> Detalhes sobre garantia de 12 meses para peças, 90 dias para serviços, e exclusões de garantia.</li>
      <li><strong>Política de Atendimento:</strong> Horários de funcionamento, canais de atendimento, tempos de resposta e procedimentos de reclamação.</li>
    </ul>
    <p>
      <strong>Vantagens:</strong> Simples, rápido, sem dependências externas. <br>
      <strong>Limitações:</strong> Dados limitados e não atualizáveis sem alteração de código.
    </p>

    <h6 class="mt-4 mb-2"><i class="bi bi-2-circle px-2"></i> Modo Avançado - Agentic Retrieval (UseBasicCustomerPoliciesAgentContext = false)</h6>
    <p>
      Utiliza o <strong>Azure AI Search Knowledge Agent</strong> com a tecnologia de <strong>Agentic Retrieval</strong>, uma pipeline multi-query avançada projetada para perguntas complexas em aplicações RAG.
    </p>

    <p><strong>Documentos indexados no Azure AI Search:</strong></p>
    <ul>
      <li><code>Politica_de_Relacionamento_ContosoAutoTech.pdf</code></li>
      <li><code>Politica_de_Troca_Devolucao_Reembolso_ContosoAutoTech.pdf</code></li>
      <li><code>Politica_de_Venda_ContosoAutoTech.pdf</code></li>
    </ul>

    <p><strong>Como funciona o Agentic Retrieval:</strong></p>
    <ol>
      <li><strong>Análise Inteligente:</strong> Um LLM analisa a pergunta complexa e o histórico do chat para identificar a real necessidade de informação.</li>
      <li><strong>Decomposição de Queries:</strong> Quebra perguntas compostas em subqueries focadas. Ex: "qual o prazo de devolução e como funciona o reembolso?" → 2 subqueries específicas.</li>
      <li><strong>Execução Paralela:</strong> Todas as subqueries são executadas simultaneamente (busca híbrida: keyword + vetorial).</li>
      <li><strong>Reranking Semântico:</strong> Cada subquery é semanticamente rerankeada para promover os matches mais relevantes.</li>
      <li><strong>Síntese Unificada:</strong> Combina os melhores resultados em uma resposta unificada com:
        <ul>
          <li>Conteúdo mesclado (grounding data)</li>
          <li>Referências das fontes (para citação)</li>
          <li>Detalhes de execução (plano de query)</li>
        </ul>
      </li>
    </ol>

    <p><strong>Recursos Avançados:</strong></p>
    <ul>
      <li>✅ Lê histórico do chat como entrada</li>
      <li>✅ Deconstrói queries complexas com múltiplas "perguntas"</li>
      <li>✅ Reescreve queries usando paráfrase gerada por LLM</li>
      <li>✅ Corrige erros ortográficos automaticamente</li>
      <li>✅ Suporta mapas de sinônimos (opcional)</li>
      <li>✅ Busca híbrida (texto + vetores) com ranking semântico</li>
    </ul>

    <p>
      <strong>Vantagens:</strong> Respostas muito mais precisas, suporte a perguntas complexas, documentos reais e atualizáveis, citação de fontes. <br>
      <strong>Trade-off:</strong> Maior latência devido à invocação do LLM no planejamento de queries (mitigado com modelos rápidos como gpt-4o-mini).
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-bullseye px-2"></i> Objetivo</h5>
    <p>
      O objetivo deste agente é <strong>facilitar o acesso e compreensão das políticas de relacionamento com o cliente</strong>, oferecendo respostas rápidas e precisas que melhoram a experiência do cliente e agilizam o trabalho das equipes de atendimento.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-link-45deg px-2"></i> Links Úteis</h5>
    <ul>
      <li>
        <a href="https://learn.microsoft.com/en-us/agent-framework/tutorials/agents/memory?pivots=programming-language-csharp" target="_blank" rel="noopener">
          Agent Memory - Gerenciamento de Memória em Agentes
        </a>
      </li>
      <li>
        <a href="https://learn.microsoft.com/en-au/azure/search/agentic-retrieval-overview" target="_blank" rel="noopener">
          Agentic Retrieval Overview - Azure AI Search
        </a>
      </li>
    </ul>
  </HelpButton>

  <AgentChatWindow
    :feature-id="2"
    title="Assistente de Políticas de Relacionamento"
    welcome-message="👋 Olá! Sou o Agente de Políticas de Relacionamento com o Cliente. Como posso ajudar você hoje? Posso esclarecer dúvidas sobre políticas de atendimento, garantias, trocas e devoluções."
    default-agent-name="Agente de Políticas de Relacionamento"
    default-instructions="
Você é um assistente especializado em políticas internas de relacionamento com o cliente da Contoso AutoTech.
Seu objetivo é responder perguntas exclusivamente com base nos documentos privados fornecidos no contexto.

---

### Regras Principais

#### 1. Uso estrito das fontes

* Utilize apenas informações que estejam explicitamente presentes nos documentos oficiais.
* Nunca invente, presuma ou deduza informações que não estejam escritas nos textos fornecidos.

#### 2. Quando a informação não existir

Se a pergunta não puder ser respondida com base nos documentos disponíveis, a resposta deve ser exatamente:

“Não encontrei essa informação nas políticas disponíveis. Posso ajudá-lo com informações sobre: políticas de atendimento, garantias, trocas e devoluções.”

#### 3. Estilo e formato das respostas

* Seja claro, objetivo e direto.
* Use somente trechos e informações dos documentos.
* Não oculte, altere o sentido ou resuma de forma imprecisa.
* Não utilize expressões como “Leia mais” ou similares.
* Organize a resposta de forma simples e fácil de entender, com títulos e listas quando necessário.
* Ao final de cada resposta, inclua sempre a seção:

  * Fontes consultadas: listar exatamente os documentos utilizados.

---

### Propósito do agente

Este agente foi projetado para atuar como um consultor seguro de informações privadas, garantindo respostas fiéis às políticas internas, uso rigoroso das fontes e completa eliminação de conteúdo fora das diretrizes corporativas.
"
  >
    <template #icon>
      <i class="bi bi-shield-check px-3"></i>
    </template>
  </AgentChatWindow>
</template>

