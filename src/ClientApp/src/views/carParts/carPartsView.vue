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
      <i class="bi bi-car-front px-2"></i>
      Descritivo da Página do Multi Agente de Peças de Carros
    </h2>
    <p>
      Esta página apresenta um
      <strong>assistente virtual especializado em consulta de peças automotivas, preço, estoque.</strong>.
      Ele permite ao usuário buscar informações de peças automotivas de forma conversacional,
      utilizando ferramentas integradas ao servidor MCP (<em>Model Context Protocol</em>).
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-list-task px-2"></i> Funcionalidades
    </h5>
    <ul>
      <li>
        <strong>Listar Catálogo Completo:</strong>
        Retorna todas as peças disponíveis com nome, marca, modelo, categoria e preço.
      </li>
      <li>
        <strong>Buscar preço</strong>
        Busca o preço de uma peça específica a partir do código do produto (ex: <code>CM002</code>, <code>BS003</code>).
      </li>
      <li>
        <strong>Buscar estoque:</strong>
        Busca a disponibilidade em estoque de uma peça específica a partir do código do produto (ex: <code>CM002</code>, <code>BS003</code>).
      </li>
      <li>
        <strong>Interface Conversacional:</strong>
        Toda a interação ocorre via chat, de forma natural e contextualizada.
      </li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-gear px-2"></i> Personalização
    </h5>
    <p>
      O agente permite ajustar suas instruções de comportamento e ferramentas por meio do botão
      <strong>"Instruções"</strong>, possibilitando adaptar o contexto de consulta conforme o cenário.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-bullseye px-2"></i> Objetivo
    </h5>
    <p>
      O objetivo deste agente é <strong>simplificar a busca e o gerenciamento de informações de peças automotivas</strong>,
      oferecendo uma experiência conversacional eficiente e intuitiva que elimina a necessidade de navegação manual em catálogos extensos.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-link-45deg px-2"></i> Links Úteis
    </h5>
    <ul>
      <li>
        <a
          href="https://learn.microsoft.com/en-us/azure/ai-services/agents/overview"
          target="_blank"
          rel="noopener"
        >
          Azure AI Agent Service – Visão Geral
        </a>
      </li>
      <li>
        <a
          href="https://tallesvaliatti.com/criando-uma-aplica%C3%A7%C3%A3o-com-o-azure-ai-agent-service-parte-1-1d4fef7901a4"
          target="_blank"
          rel="noopener"
        >
          Azure AI Agent Service – Tutorial em Português
        </a>
      </li>
    </ul>
  </HelpButton>

  <MultiAgentChatWindow
  :feature-id="5"
  title="Multi Agente de Peças de Carros"
  welcome-message="👋 Olá! Sou o Multi Agente de Peças de Carros. Coordeno diversos agentes especializados para ajudá-lo com estoque e preços. O que você precisa?"
  default-orchestrator-name="Multi Agente de Peças de Carros"
  default-orchestrator-instructions="Você é o Orquestrador de Peças, responsável por coordenar agentes especializados em preços e estoque de peças automotivas.
    Quando o usuário fizer uma pergunta sobre **preço**, chame o **Agente de Preços**.
    Quando o usuário fizer uma pergunta sobre **estoque**, chame o **Agente de Estoque**.
    Para perguntas gerais sobre peças, chame o **Agente de Produtos**.
    Nunca tente responder diretamente — sempre delegue ao agente correto.
    Responda SEMPRE em português brasileiro."
  :default-specialized-agents="[
    {
      name: 'Agente de Estoque',
      instructions: `
Você é um assistente especializado em controle de estoque automotivo. Use apenas as ferramentas disponíveis para responder sobre disponibilidade de peças
`,
      featureId: 4
    },
    {
      name: 'Agente de Precos',
      instructions: `
Você é um assistente especializado em catálogo automotivo. Use apenas as ferramentas disponíveis para responder sobre preços
`,
      featureId: 3
    },
    {
      name: 'Agente de Produtos',
      instructions: `
        Você é um assistente especializado em catálogo de peças automotivas. Use as ferramentas disponiveis para listar peças e fornecer detalhes completos. Mantenha as respostas claras e amigáveis. Sempre responda com o codigo do produto. o nome, marca, modelo.
`,
      featureId: 9
    }
  ]"
>
  <template #icon>
    <i class="bi bi-diagram-3 px-3"></i>
  </template>
</MultiAgentChatWindow>
</template>
