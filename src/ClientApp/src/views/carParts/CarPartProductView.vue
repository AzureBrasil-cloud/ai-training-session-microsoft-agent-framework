<script setup lang="ts">
import HelpButton from "@/components/common/HelpButton.vue";
import AgentChatWindow from "@/components/agent/AgentChatWindow.vue";

const carPartProductImage = `${window.location.origin}/images/Feature3.png`;
</script>

<template>
  <HelpButton>
    <div class="d-flex justify-content-center my-4">
      <img
        :src="carPartProductImage"
        alt="Agente de Catálogo de Produtos"
        class="img-fluid rounded"
        style="width: 100%; max-width: 800px;"
      />
    </div>

    <h2 class="mb-5 mt-8">
      <i class="bi bi-box-seam px-2"></i>
      Descritivo da Página do Agente de Catálogo de Produtos
    </h2>
    <p>
      Esta página apresenta um <strong>assistente virtual especializado em consulta de produtos automotivos</strong>.
      Ele permite ao usuário buscar informações de produtos utilizando o <strong>Model Context Protocol (MCP)</strong>,
      um protocolo aberto que conecta agentes de IA a sistemas externos de forma padronizada e eficiente.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-list-task px-2"></i> Funcionalidades
    </h5>
    <ul>
      <li><strong>Consulta de Produtos:</strong> Lista produtos automotivos com nome, marca, modelo, categoria e preço.</li>
      <li><strong>Interface Conversacional:</strong> Toda interação via chat em linguagem natural.</li>
      <li><strong>Integração MCP:</strong> Conecta-se a servidor MCP externo para obter dados em tempo real.</li>
      <li><strong>Gerenciamento de Threads:</strong> Mantém histórico de conversas organizadas.</li>
      <li><strong>Respostas Estruturadas:</strong> Informações formatadas de forma clara e organizada.</li>
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
      <li>✅ <strong>Descoberta Dinâmica:</strong> Servidores expõem suas capacidades (tools, resources, prompts) dinamicamente</li>
      <li>✅ <strong>Segurança:</strong> Controle granular de permissões e acesso a recursos</li>
      <li>✅ <strong>Reutilizável:</strong> Um servidor MCP pode ser usado por múltiplos agentes diferentes</li>
    </ul>

    <h6 class="mt-4 mb-2"><i class="bi bi-diagram-3 px-2"></i> Arquitetura MCP:</h6>
    <div class="bg-light p-4 rounded">
      <pre class="mb-0 text-dark" style="font-size: 0.9rem;"><code>┌─────────────────┐
│   AI Agent      │ (Cliente MCP - este agente de produtos)
│  (Cliente MCP)  │
└────────┬────────┘
         │ Protocolo MCP
         │ (JSON-RPC)
         ▼
┌─────────────────┐
│  Servidor MCP   │ (CarPartProductMcp - servidor .NET)
│   (Produtos)    │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  Base de Dados  │ (SQL Server / SQLite)
│    Produtos     │
└─────────────────┘</code></pre>
    </div>

    <p class="mt-4">
      <strong>Servidor MCP:</strong> Este agente conecta-se ao <strong>CarPartProductMcp</strong>, um servidor MCP implementado em .NET que expõe ferramentas para consulta de produtos automotivos.
    </p>

    <p>
      <strong>Configuração:</strong> A URL do servidor é definida no <code>appsettings.json</code>:
    </p>
    <pre class="bg-light p-3 rounded" style="font-size: 0.85rem;"><code class="text-dark">{
  "Application": {
    "CarProductMcpRemoteUrl": "https://app-cars-product-mcp-...azurewebsites.net/mcp"
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
        <h6 class="card-title"><i class="bi bi-search px-2"></i> GetAllProductsByParam</h6>
        <p class="card-text">
          <strong>Descrição:</strong> Retorna uma lista de produtos automotivos filtrados por nome ou código. Se nenhum filtro for fornecido, retorna todos os produtos disponíveis no catálogo.
        </p>
        <p><strong>Parâmetros (todos opcionais):</strong></p>
        <ul class="mb-2">
          <li><code>name</code> (string, opcional) - Filtro por nome do produto (busca parcial, case-insensitive)</li>
          <li><code>code</code> (string, opcional) - Filtro por código do produto (busca parcial, case-insensitive)</li>
        </ul>
        <p><strong>Retorno:</strong> Lista de objetos Product contendo:</p>
        <ul class="mb-2">
          <li><code>id</code> (Guid) - Identificador único do produto</li>
          <li><code>productCode</code> (string) - Código do produto</li>
          <li><code>name</code> (string) - Nome do produto</li>
          <li><code>brand</code> (string) - Marca do produto</li>
          <li><code>model</code> (string) - Modelo compatível</li>
        </ul>
        <p class="mb-0"><strong>Exemplos de Uso:</strong> "Liste todos os produtos disponíveis", "Buscar produtos com nome 'freio'", "Produtos com código 'BRK'"</p>
      </div>
    </div>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-cpu px-2"></i> Como o Agente Funciona
    </h5>
    <ol>
      <li class="mb-2">
        <strong>Conexão com MCP:</strong> Ao iniciar, o agente estabelece conexão SSE com o servidor CarPartProductMcp e descobre as tools disponíveis (GetAllProductsByParam).
      </li>
      <li class="mb-2">
        <strong>Recepção da Mensagem:</strong> Usuário envia uma pergunta em linguagem natural (ex: "quero ver os produtos", "buscar produtos com nome freio").
      </li>
      <li class="mb-2">
        <strong>Análise e Decisão:</strong> O LLM analisa a intenção e decide chamar a tool GetAllProductsByParam via protocolo MCP, identificando os parâmetros necessários (name e/ou code).
      </li>
      <li class="mb-2">
        <strong>Chamada MCP:</strong> Agente envia requisição JSON-RPC ao servidor MCP solicitando execução da tool com os parâmetros apropriados.
      </li>
      <li class="mb-2">
        <strong>Execução no Servidor:</strong> Servidor MCP executa GetAllProductsByParam, aplica os filtros (se fornecidos), consulta o banco de dados e retorna os dados.
      </li>
      <li class="mb-2">
        <strong>Processamento:</strong> Agente recebe os dados estruturados do MCP e o LLM formata a resposta em linguagem natural.
      </li>
      <li class="mb-2">
        <strong>Resposta ao Usuário:</strong> Mensagem formatada é exibida no chat com a lista de produtos encontrados.
      </li>
    </ol>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-lightning-charge px-2"></i> Vantagens da Arquitetura MCP
    </h5>
    <ul>
      <li>✅ <strong>Separação de Responsabilidades:</strong> Agente foca em conversação, servidor MCP em acesso a dados</li>
      <li>✅ <strong>Escalabilidade:</strong> Servidor MCP pode atender múltiplos agentes simultaneamente</li>
      <li>✅ <strong>Manutenibilidade:</strong> Mudanças na fonte de dados não afetam a lógica do agente</li>
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
      <li>Personalizar formatação das respostas</li>
      <li>Definir comportamentos específicos para diferentes cenários</li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-bullseye px-2"></i> Objetivo
    </h5>
    <p>
      O objetivo deste agente é <strong>democratizar o acesso ao catálogo de produtos automotivos</strong>, permitindo consultas em linguagem natural sem necessidade de conhecer SQL, APIs ou estruturas de dados complexas. O uso de MCP garante que o sistema seja extensível e fácil de manter.
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
    :feature-id="9"
    title="Agente de Catálogo de Peças de Carros"
    welcome-message="👋 Olá! Sou o Agente de Catálogo de Peças Automotivas. Posso ajudá-lo a consultar e localizar produtos em nosso catálogo. Como posso auxiliá-lo hoje?"
    default-agent-name="Agente de Catálogo de Produtos"
    default-instructions="Você é um assistente virtual especializado em catálogo de produtos automotivos da Contoso AutoTech.

RESPONSABILIDADES:
- Auxiliar clientes na consulta de produtos do catálogo
- Utilizar exclusivamente as ferramentas MCP disponíveis para obter informações precisas
- Fornecer respostas claras, organizadas e objetivas
- Manter um tom profissional, cordial e prestativo

DIRETRIZES DE COMUNICAÇÃO:
1. Sempre confirme o entendimento da solicitação do cliente antes de executar buscas
2. Apresente os resultados de forma estruturada e fácil de ler
3. Inclua informações relevantes como código, nome, marca e modelo dos produtos
4. Se nenhum produto for encontrado, informe claramente e sugira alternativas de busca
5. Mantenha o foco apenas em produtos disponíveis no catálogo

Responda sempre de maneira profissional, clara e organizada, priorizando a experiência do cliente."
  >
    <template #icon>
      <i class="bi bi-tools px-3"></i>
    </template>
  </AgentChatWindow>
</template>
