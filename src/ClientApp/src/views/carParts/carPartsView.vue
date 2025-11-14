<script setup lang="ts">
import HelpButton from "@/components/common/HelpButton.vue";
import MultiAgentChatWindow from "@/components/agent/MultiAgentChatWindow.vue";

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

    <h2 class="mb-5 mt-8">
      <i class="bi bi-diagram-3 px-2"></i>
      Descritivo da Página do Multi-Agente Orquestrador de Peças
    </h2>
    <p>
      Esta página apresenta um <strong>sistema de Multi-Agente Orquestrador</strong> que coordena automaticamente múltiplos agentes especializados para fornecer informações completas sobre peças automotivas. O orquestrador analisa a pergunta do usuário e delega a tarefa ao agente especializado mais adequado, utilizando o conceito de <strong>Agents as Tools</strong> e <strong>Model Context Protocol (MCP)</strong>.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-list-task px-2"></i> Funcionalidades
    </h5>
    <ul>
      <li><strong>Consulta de Produtos:</strong> Lista peças disponíveis com código, nome, marca e modelo através do Agente de Produtos.</li>
      <li><strong>Consulta de Preços:</strong> Busca preço de peças específicas por código através do Agente de Preços.</li>
      <li><strong>Consulta de Estoque:</strong> Verifica disponibilidade em estoque por código através do Agente de Estoque.</li>
      <li><strong>Roteamento Inteligente:</strong> Orquestrador analisa a intenção e delega automaticamente ao agente correto.</li>
      <li><strong>Interface Conversacional:</strong> Toda interação via chat em linguagem natural.</li>
      <li><strong>Respostas Integradas:</strong> Combina informações de múltiplos agentes quando necessário.</li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-robot px-2"></i> O que é Multi-Agente Orquestrador?
    </h5>
    <p>
      Um <strong>Multi-Agente Orquestrador</strong> é um agente coordenador que não executa tarefas diretamente, mas sim delega trabalho a agentes especializados. Cada agente especializado possui suas próprias ferramentas (tools via MCP) e expertise em um domínio específico.
    </p>

    <h6 class="mt-4 mb-2"><i class="bi bi-star px-2"></i> Benefícios da Arquitetura Multi-Agente:</h6>
    <ul>
      <li>✅ <strong>Separação de Responsabilidades:</strong> Cada agente foca em uma área específica (produtos, preços ou estoque)</li>
      <li>✅ <strong>Reutilização:</strong> Agentes especializados podem ser usados individualmente ou em orquestração</li>
      <li>✅ <strong>Escalabilidade:</strong> Fácil adicionar novos agentes especializados sem alterar os existentes</li>
      <li>✅ <strong>Manutenibilidade:</strong> Mudanças em um domínio não afetam outros agentes</li>
      <li>✅ <strong>Especialização:</strong> Cada agente é otimizado para sua tarefa específica</li>
      <li>✅ <strong>Transparência:</strong> Usuário não precisa saber qual agente está sendo usado</li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-diagram-3-fill px-2"></i> Arquitetura Multi-Agente
    </h5>
    <div class="bg-light p-4 rounded">
      <pre class="mb-0 text-dark" style="font-size: 0.85rem;"><code>┌─────────────────────────┐
│       USUÁRIO           │
└───────────┬─────────────┘
            │ Pergunta em linguagem natural
            ▼
┌─────────────────────────┐
│  AGENTE ORQUESTRADOR    │ (Analisa intenção e roteia)
│  "Multi-Agente Peças"   │
└───────────┬─────────────┘
            │
            ├─────────────────┬─────────────────┐
            ▼                 ▼                 ▼
    ┌──────────────┐  ┌──────────────┐  ┌──────────────┐
    │   Agente de  │  │  Agente de   │  │  Agente de   │
    │   Produtos   │  │    Preços    │  │   Estoque    │
    └──────┬───────┘  └──────┬───────┘  └──────┬───────┘
           │                 │                  │
           │ MCP             │ MCP              │ MCP
           ▼                 ▼                  ▼
    ┌──────────────┐  ┌──────────────┐  ┌──────────────┐
    │CarProductMcp │  │ CarPriceMcp  │  │ CarStockMcp  │
    │  (Servidor)  │  │  (Servidor)  │  │  (Servidor)  │
    └──────────────┘  └──────────────┘  └──────────────┘</code></pre>
    </div>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-people px-2"></i> Agentes Especializados Disponíveis
    </h5>

    <div class="card mb-3">
      <div class="card-body">
        <h6 class="card-title"><i class="bi bi-box-seam px-2"></i> 1. Agente de Produtos</h6>
        <p><strong>Responsabilidade:</strong> Consultar catálogo de peças automotivas</p>
        <p><strong>Tool MCP:</strong> GetAllProductsByParam (servidor CarProductMcp)</p>
        <p><strong>Retorna:</strong> Código, nome, marca e modelo de produtos</p>
        <p class="mb-0"><strong>Exemplo:</strong> "Liste todos os produtos disponíveis"</p>
      </div>
    </div>

    <div class="card mb-3">
      <div class="card-body">
        <h6 class="card-title"><i class="bi bi-tags px-2"></i> 2. Agente de Preços</h6>
        <p><strong>Responsabilidade:</strong> Consultar preços de peças por código</p>
        <p><strong>Tool MCP:</strong> GetPriceByProductCode (servidor CarPriceMcp)</p>
        <p><strong>Retorna:</strong> Código do produto e valor/preço</p>
        <p class="mb-0"><strong>Exemplo:</strong> "Qual o preço do produto WB005?"</p>
      </div>
    </div>

    <div class="card mb-3">
      <div class="card-body">
        <h6 class="card-title"><i class="bi bi-boxes px-2"></i> 3. Agente de Estoque</h6>
        <p><strong>Responsabilidade:</strong> Verificar disponibilidade em estoque</p>
        <p><strong>Tool MCP:</strong> GetStockByProductCode (servidor CarStockMcp)</p>
        <p><strong>Retorna:</strong> Código do produto e quantidade disponível</p>
        <p class="mb-0"><strong>Exemplo:</strong> "Qual o estoque do código CM002?"</p>
      </div>
    </div>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-cpu px-2"></i> Como o Orquestrador Funciona
    </h5>
    <ol>
      <li class="mb-2">
        <strong>Recepção da Mensagem:</strong> Usuário envia uma pergunta em linguagem natural (ex: "qual o preço e estoque do produto WB005?").
      </li>
      <li class="mb-2">
        <strong>Análise de Intenção:</strong> O Agente Orquestrador analisa a mensagem e identifica quais domínios estão envolvidos (produtos, preços, estoque).
      </li>
      <li class="mb-2">
        <strong>Seleção do(s) Agente(s):</strong> Com base na análise, o orquestrador decide qual(is) agente(s) especializado(s) deve(m) ser acionado(s).
      </li>
      <li class="mb-2">
        <strong>Delegação (Agents as Tools):</strong> O orquestrador "chama" o agente especializado como se fosse uma ferramenta, passando o contexto necessário.
      </li>
      <li class="mb-2">
        <strong>Execução Especializada:</strong> O agente especializado se conecta ao seu servidor MCP, executa a tool apropriada e retorna os dados estruturados.
      </li>
      <li class="mb-2">
        <strong>Agregação de Respostas:</strong> Se múltiplos agentes foram acionados, o orquestrador combina as respostas de forma coerente.
      </li>
      <li class="mb-2">
        <strong>Resposta ao Usuário:</strong> Mensagem formatada e integrada é exibida no chat com todas as informações solicitadas.
      </li>
    </ol>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-wrench px-2"></i> Conceito: Agents as Tools
    </h5>
    <p>
      <strong>Agents as Tools</strong> é um padrão do Microsoft Agent Framework onde um agente pode ser tratado como uma ferramenta (tool) por outro agente. Isso permite:
    </p>
    <ul>
      <li><strong>Composição de Agentes:</strong> Agentes podem ser combinados hierarquicamente</li>
      <li><strong>Abstração:</strong> Orquestrador não precisa conhecer detalhes internos dos agentes especializados</li>
      <li><strong>Reutilização:</strong> Mesmos agentes especializados podem ser usados em diferentes orquestrações</li>
      <li><strong>Manutenção Independente:</strong> Cada agente pode evoluir separadamente</li>
    </ul>

    <p class="mt-3">
      <strong>Diferença entre Handoff e Agents as Tools:</strong>
    </p>
    <ul>
      <li><strong>Handoff:</strong> Transfere completamente a conversa para outro agente (troca de controle)</li>
      <li><strong>Agents as Tools:</strong> Agente orquestrador mantém controle e usa outros agentes como ferramentas para obter informações</li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-gear px-2"></i> Exemplo de Fluxo Completo
    </h5>
    <div class="bg-light p-4 rounded">
      <p class="mb-2"><strong>Pergunta do Usuário:</strong> "Quero saber o preço e estoque do produto WB005"</p>
      <ol class="mb-0 text-dark">
        <li><strong>Orquestrador:</strong> Identifica que precisa de informações de Preços E Estoque</li>
        <li><strong>Delegação 1:</strong> Chama Agente de Preços como tool</li>
        <li><strong>Agente de Preços:</strong> Conecta ao CarPriceMcp → GetPriceByProductCode("WB005") → Retorna R$ 150,00</li>
        <li><strong>Delegação 2:</strong> Chama Agente de Estoque como tool</li>
        <li><strong>Agente de Estoque:</strong> Conecta ao CarStockMcp → GetStockByProductCode("WB005") → Retorna 23 unidades</li>
        <li><strong>Orquestrador:</strong> Combina respostas e formata para o usuário</li>
        <li><strong>Resposta Final:</strong> "Produto WB005: Preço R$ 150,00 | Estoque: 23 unidades disponíveis"</li>
      </ol>
    </div>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-lightning-charge px-2"></i> Vantagens desta Arquitetura
    </h5>
    <ul>
      <li>✅ <strong>Experiência Unificada:</strong> Usuário não precisa saber qual agente acionar</li>
      <li>✅ <strong>Inteligência de Roteamento:</strong> Orquestrador escolhe automaticamente o melhor agente</li>
      <li>✅ <strong>Respostas Completas:</strong> Pode combinar informações de múltiplos domínios</li>
      <li>✅ <strong>Modularidade:</strong> Fácil adicionar ou remover agentes especializados</li>
      <li>✅ <strong>Eficiência:</strong> Cada agente se conecta apenas ao MCP necessário</li>
      <li>✅ <strong>Manutenção Simplificada:</strong> Mudanças em um domínio são isoladas</li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-gear px-2"></i> Personalização
    </h5>
    <p>
      O sistema permite personalizar tanto o <strong>Agente Orquestrador</strong> quanto cada <strong>Agente Especializado</strong> através do botão "Instruções":
    </p>
    <ul>
      <li><strong>Orquestrador:</strong> Ajustar lógica de roteamento e priorização de agentes</li>
      <li><strong>Agentes Especializados:</strong> Personalizar tom, formato e comportamento de cada agente</li>
      <li><strong>Adição de Agentes:</strong> Incluir novos agentes especializados na orquestração</li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-bullseye px-2"></i> Objetivo
    </h5>
    <p>
      O objetivo deste Multi-Agente Orquestrador é <strong>fornecer uma experiência unificada e inteligente para consulta de peças automotivas</strong>, combinando automaticamente informações de produtos, preços e estoque através de uma única interface conversacional, eliminando a necessidade do usuário navegar entre diferentes sistemas ou agentes.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-link-45deg px-2"></i> Links Úteis
    </h5>
    <ul>
      <li>
        <a href="https://learn.microsoft.com/pt-br/agent-framework/overview/agent-framework-overview" target="_blank" rel="noopener">
          Visão Geral do Microsoft Agent Framework
        </a>
      </li>
      <li>
        <a href="https://learn.microsoft.com/en-us/agent-framework/tutorials/agents/agent-as-function-tool?pivots=programming-language-csharp" target="_blank" rel="noopener">
          AI Agents as Tools - Microsoft Agent Framework
        </a>
      </li>
      <li>
        <a href="https://learn.microsoft.com/en-us/agent-framework/user-guide/workflows/orchestrations/handoff?pivots=programming-language-csharp#differences-between-handoff-and-agent-as-tools" target="_blank" rel="noopener">
          Handoff vs Agents as Tools - Diferenças e Casos de Uso
        </a>
      </li>
      <li>
        <a href="https://modelcontextprotocol.io/introduction" target="_blank" rel="noopener">
          Model Context Protocol - Documentação Oficial
        </a>
      </li>
    </ul>
  </HelpButton>

  <MultiAgentChatWindow
  :feature-id="5"
  title="Multi-Agente Orquestrador de Peças"
  welcome-message="👋 Olá! Sou o Multi-Agente Orquestrador de Peças. Coordeno agentes especializados para fornecer informações completas sobre produtos, preços e estoque. Como posso ajudá-lo?"
  default-orchestrator-name="Orquestrador de Peças Automotivas"
  default-orchestrator-instructions="Você é o Agente Orquestrador de Peças Automotivas da Contoso AutoTech, responsável por coordenar agentes especializados em produtos, preços e estoque.

RESPONSABILIDADES:
- Analisar a pergunta do usuário e identificar qual(is) domínio(s) está(ão) envolvido(s)
- Delegar tarefas aos agentes especializados apropriados usando Agents as Tools
- Combinar e apresentar respostas de múltiplos agentes de forma coerente e organizada
- NUNCA tentar responder diretamente - SEMPRE delegar aos agentes especializados

REGRAS DE ROTEAMENTO:
1. Perguntas sobre **catálogo, lista de produtos, peças disponíveis** → Agente de Produtos
2. Perguntas sobre **preço, valor, custo** → Agente de Precos
3. Perguntas sobre **estoque, disponibilidade, quantidade** → Agente de Estoque
4. Perguntas que envolvem múltiplos domínios → Acionar múltiplos agentes e combinar respostas

FORMATO DE RESPOSTA:
- Sempre mencionar qual(is) agente(s) foi(ram) acionado(s)
- Apresentar informações de forma clara e estruturada
- Se código do produto não for fornecido, solicitar ao usuário
- Responder SEMPRE em português brasileiro

Mantenha tom profissional, cordial e eficiente. Priorize a experiência do usuário combinando informações relevantes."
  :default-specialized-agents="[
    {
      name: 'Agente de Produtos',
      instructions: 'Você é um assistente virtual especializado em catálogo de produtos automotivos da Contoso AutoTech.\n\nRESPONSABILIDADES:\n- Auxiliar na consulta de produtos do catálogo\n- Utilizar exclusivamente as ferramentas MCP disponíveis\n- Fornecer respostas claras com código, nome, marca e modelo\n- Manter tom profissional e cordial\n\nFERRAMENTAS DISPONÍVEIS:\n- GetAllProductsByParam: Para consultar produtos por nome ou código\n\nResponda sempre de maneira profissional, clara e organizada.',
      featureId: 9
    },
    {
      name: 'Agente de Precos',
      instructions: 'Você é um assistente virtual especializado em consulta de preços de peças automotivas da Contoso AutoTech.\n\nRESPONSABILIDADES:\n- Auxiliar na consulta de preços através do código do produto\n- Utilizar exclusivamente a ferramenta MCP disponível\n- Fornecer respostas claras com código e preço formatado (R$)\n- Solicitar código se não fornecido\n\nFERRAMENTAS DISPONÍVEIS:\n- GetPriceByProductCode: Para consultar preço por código (parâmetro obrigatório: code)\n\nResponda sempre de maneira profissional, clara e organizada.',
      featureId: 3
    },
    {
      name: 'Agente de Estoque',
      instructions: 'Você é um assistente virtual especializado em consulta de estoque de peças automotivas da Contoso AutoTech.\n\nRESPONSABILIDADES:\n- Auxiliar na consulta de estoque através do código do produto\n- Utilizar exclusivamente a ferramenta MCP disponível\n- Fornecer respostas claras com código e quantidade disponível\n- Alertar se estoque baixo (menos de 5 unidades)\n- Solicitar código se não fornecido\n\nFERRAMENTAS DISPONÍVEIS:\n- GetStockByProductCode: Para consultar quantidade em estoque (parâmetro obrigatório: code)\n\nResponda sempre de maneira profissional, clara e organizada.',
      featureId: 4
    }
  ]"
>
  <template #icon>
    <i class="bi bi-diagram-3 px-3"></i>
  </template>
</MultiAgentChatWindow>
</template>
