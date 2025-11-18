<script setup lang="ts">
import HelpButton from "@/components/common/HelpButton.vue";
import AgentChatWindow from "@/components/agent/AgentChatWindow.vue";

const videoUrl = `${window.location.origin}/videos/price-agent.mp4`;
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
      <i class="bi bi-tags px-2"></i>
      Descritivo da Página do Agente de Consulta de Preços
    </h2>
    <p>
      Esta página apresenta um <strong>assistente virtual especializado em consulta de preços de peças automotivas</strong>.
      Ele permite ao usuário informar o código da peça e obter instantaneamente o preço correspondente, utilizando o <strong>Model Context Protocol (MCP)</strong>,
      um protocolo aberto que conecta agentes de IA a sistemas externos de forma padronizada e eficiente.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-list-task px-2"></i> Funcionalidades
    </h5>
    <ul>
      <li><strong>Consulta de Preço por Código:</strong> Busca o preço de uma peça específica através do código do produto (ex: WB005, CM002, BS003).</li>
      <li><strong>Interface Conversacional:</strong> Toda interação via chat em linguagem natural.</li>
      <li><strong>Integração MCP:</strong> Conecta-se a servidor MCP externo para obter preços em tempo real.</li>
      <li><strong>Gerenciamento de Threads:</strong> Mantém histórico de conversas organizadas.</li>
      <li><strong>Respostas Rápidas:</strong> Retorno instantâneo com informações precisas de preço.</li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-diagram-2 px-2"></i> O que é MCP (Model Context Protocol)?
    </h5>
    <p>
      O <strong>Model Context Protocol (MCP)</strong> é um protocolo aberto desenvolvido pela Anthropic que padroniza como agentes de IA se conectam a diferentes fontes de dados e ferramentas. Ele resolve o problema de integração fragmentada, permitindo que um agente converse com múltiplos sistemas externos através de uma interface unificada.
    </p>

    <h6 class="mt-4 mb-2"><i class="bi bi-star px-2"></i> Características Principais do MCP:</h6>
    <ul>
      <li>✅ <strong>Protocolo Aberto:</strong> Padrão livre e extensível para integrações de IA</li>
      <li>✅ <strong>Cliente-Servidor:</strong> Arquitetura que separa o agente (cliente) dos dados (servidor)</li>
      <li>✅ <strong>Agnóstico de Transporte:</strong> Pode usar HTTP, WebSockets, stdio, ou outros protocolos</li>
      <li>✅ <strong>Descoberta Dinâmica:</strong> Servidores expõem suas capacidades dinamicamente</li>
      <li>✅ <strong>Segurança:</strong> Controle granular de permissões e acesso a recursos</li>
      <li>✅ <strong>Reutilizável:</strong> Um servidor MCP pode ser usado por múltiplos agentes diferentes</li>
    </ul>

    <h6 class="mt-4 mb-2"><i class="bi bi-diagram-3 px-2"></i> Arquitetura MCP:</h6>
    <div class="bg-light p-4 rounded">
      <pre class="mb-0 text-dark" style="font-size: 0.9rem;"><code>┌─────────────────┐
│   AI Agent      │ (Cliente MCP - este agente de preços)
│  (Cliente MCP)  │
└────────┬────────┘
         │ Protocolo MCP
         │ (JSON-RPC)
         ▼
┌─────────────────┐
│  Servidor MCP   │ (CarPriceMcp - servidor .NET)
│    (Preços)     │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  Base de Dados  │ (SQL Server / SQLite)
│     Preços      │
└─────────────────┘</code></pre>
    </div>

    <p class="mt-4">
      <strong>Servidor MCP:</strong> Este agente conecta-se ao <strong>CarPriceMcp</strong>, um servidor MCP implementado em .NET que expõe ferramentas para consulta de preços de peças automotivas.
    </p>

    <p>
      <strong>Configuração:</strong> A URL do servidor é definida no <code>appsettings.json</code>:
    </p>
    <pre class="bg-light p-3 rounded" style="font-size: 0.85rem;"><code class="text-dark">{
  "Application": {
    "CarPriceMcpRemoteUrl": "https://app-cars-price-mcp-...azurewebsites.net/mcp"
  }
}</code></pre>

    <p>
      A conexão é estabelecida via <strong>SSE (Server-Sent Events)</strong>, permitindo comunicação persistente e bidirecional eficiente entre o agente e o servidor MCP.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-tools px-2"></i> Ferramentas (Tools) Disponíveis via MCP
    </h5>
    <p>
      O servidor MCP expõe <strong>1 tool principal</strong> que o agente pode invocar durante a conversa:
    </p>

    <div class="card mb-3">
      <div class="card-body">
        <h6 class="card-title"><i class="bi bi-search px-2"></i> GetPriceByProductCode</h6>
        <p class="card-text">
          <strong>Descrição:</strong> Retorna o preço de uma peça específica com base no código do produto. Realiza busca exata (case-insensitive).
        </p>
        <p><strong>Parâmetros:</strong></p>
        <ul class="mb-2">
          <li><code>code</code> (string, obrigatório) - Código do produto (busca exata, case-insensitive)</li>
        </ul>
        <p><strong>Retorno:</strong> Objeto Price contendo:</p>
        <ul class="mb-2">
          <li><code>id</code> (Guid) - Identificador único do registro de preço</li>
          <li><code>productCode</code> (string) - Código do produto</li>
          <li><code>value</code> (decimal) - Valor/preço do produto</li>
        </ul>
        <p class="mb-0"><strong>Exemplos de Uso:</strong> "Qual o preço do produto WB005?", "Buscar preço do código CM002", "Preço da peça BS003"</p>
      </div>
    </div>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-cpu px-2"></i> Como o Agente Funciona
    </h5>
    <ol>
      <li class="mb-2">
        <strong>Conexão com MCP:</strong> Ao iniciar, o agente estabelece conexão SSE com o servidor CarPriceMcp e descobre as tools disponíveis (GetPriceByProductCode).
      </li>
      <li class="mb-2">
        <strong>Recepção da Mensagem:</strong> Usuário envia uma pergunta em linguagem natural (ex: "qual o preço do produto WB005?").
      </li>
      <li class="mb-2">
        <strong>Análise e Decisão:</strong> O LLM analisa a intenção, extrai o código do produto da mensagem e decide chamar a tool GetPriceByProductCode via protocolo MCP.
      </li>
      <li class="mb-2">
        <strong>Chamada MCP:</strong> Agente envia requisição JSON-RPC ao servidor MCP solicitando execução da tool com o código do produto.
      </li>
      <li class="mb-2">
        <strong>Execução no Servidor:</strong> Servidor MCP executa GetPriceByProductCode, busca o preço no banco de dados (match exato do código) e retorna os dados.
      </li>
      <li class="mb-2">
        <strong>Processamento:</strong> Agente recebe o objeto Price estruturado do MCP e o LLM formata a resposta em linguagem natural.
      </li>
      <li class="mb-2">
        <strong>Resposta ao Usuário:</strong> Mensagem formatada é exibida no chat com o preço encontrado ou informação de que o produto não foi localizado.
      </li>
    </ol>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-lightning-charge px-2"></i> Vantagens da Arquitetura MCP
    </h5>
    <ul>
      <li>✅ <strong>Separação de Responsabilidades:</strong> Agente foca em conversação, servidor MCP em acesso a dados de preços</li>
      <li>✅ <strong>Escalabilidade:</strong> Servidor MCP pode atender múltiplos agentes simultaneamente</li>
      <li>✅ <strong>Manutenibilidade:</strong> Mudanças nos preços não afetam a lógica do agente</li>
      <li>✅ <strong>Segurança:</strong> Servidor MCP controla acesso aos dados sem expor credenciais ao agente</li>
      <li>✅ <strong>Reutilização:</strong> Mesmo servidor pode ser usado por diferentes aplicações</li>
      <li>✅ <strong>Padronização:</strong> Protocolo comum facilita integrações futuras</li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-gear px-2"></i> Personalização
    </h5>
    <p>
      O agente permite ajustar suas instruções através do botão <strong>"Instruções"</strong>, possibilitando:
    </p>
    <ul>
      <li>Alterar o tom e estilo de resposta</li>
      <li>Adicionar contexto específico de negócio</li>
      <li>Personalizar formatação das respostas de preço</li>
      <li>Definir comportamentos para produtos não encontrados</li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-bullseye px-2"></i> Objetivo
    </h5>
    <p>
      O objetivo deste agente é <strong>facilitar a consulta rápida de preços de peças automotivas</strong>, fornecendo uma experiência de busca direta e eficiente sem a necessidade de navegar em catálogos ou sistemas manuais de precificação.
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
        <a href="https://modelcontextprotocol.io/introduction" target="_blank" rel="noopener">
          Model Context Protocol - Documentação Oficial
        </a>
      </li>
      <li>
        <a href="https://github.com/modelcontextprotocol" target="_blank" rel="noopener">
          MCP GitHub Repository
        </a>
      </li>
      <li>
        <a href="https://www.azurebrasil.cloud/blog/desvendando-o-model-context-protocol-mcp/" target="_blank" rel="noopener">
          Desvendando o Model Context Protocol (MCP) - Azure Brasil
        </a>
      </li>
    </ul>
  </HelpButton>

  <AgentChatWindow
    :feature-id="3"
    title="Agente de Consulta de Preços"
    welcome-message="👋 Olá! Sou o Agente de Consulta de Preços de Peças Automotivas. Informe o código da peça (ex: WB005) e eu lhe direi o preço correspondente. Como posso ajudá-lo?"
    default-agent-name="Agente de Consulta de Preços"
    default-instructions="Você é um assistente virtual especializado em consulta de preços de peças automotivas da Contoso AutoTech.

RESPONSABILIDADES:
- Auxiliar clientes na consulta de preços através do código do produto
- Utilizar exclusivamente a ferramenta MCP disponível para obter preços precisos
- Fornecer respostas claras, objetivas e formatadas adequadamente
- Manter um tom profissional, cordial e prestativo

DIRETRIZES DE COMUNICAÇÃO:
1. Sempre solicite o código do produto se não for informado na mensagem
2. Confirme o código recebido antes de executar a busca
3. Apresente o preço de forma clara e formatada (ex: R$ 150,00)
4. Se o produto não for encontrado, informe claramente e sugira verificação do código
5. Inclua informações completas: código do produto e valor

FORMATO DE RESPOSTA:
- Para produto encontrado: Informar código e preço formatado
- Para produto não encontrado: Sugerir verificação do código e oferecer ajuda adicional

Responda sempre de maneira profissional, clara e organizada, priorizando a experiência do cliente."
  >
    <template #icon>
      <i class="bi bi-cash-coin px-3"></i>
    </template>
  </AgentChatWindow>
</template>
