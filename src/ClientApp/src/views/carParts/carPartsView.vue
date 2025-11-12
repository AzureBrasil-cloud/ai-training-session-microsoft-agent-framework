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
      Descritivo da Página do Agente de Catálogo de Peças de Carros
    </h2>
    <p>
      Esta página apresenta um
      <strong>assistente virtual especializado em consulta de peças automotivas</strong>.
      Ele permite ao usuário buscar informações de peças com base em marca, modelo e categoria,
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
        <strong>Buscar por Marca:</strong>
        Lista todas as peças de uma marca específica (ex: Honda, Toyota, Chevrolet).
      </li>
      <li>
        <strong>Buscar por Modelo:</strong>
        Lista peças associadas a um modelo específico (ex: Civic, Onix, HB20).
      </li>
      <li>
        <strong>Listar Marcas Disponíveis:</strong>
        Mostra todas as marcas atualmente registradas no catálogo.
      </li>
      <li>
        <strong>Listar Modelos Disponíveis:</strong>
        Exibe todos os modelos de veículos disponíveis.
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
      <strong>"Instruções"</strong>, possibilitando adaptar o contexto de consulta conforme o cenário
      (por exemplo, filtrar apenas peças de determinadas categorias ou faixas de preço).
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
  title="Workflow de Gestão de Peças"
  welcome-message="👋 Olá! Sou o Orquestrador de peças. Coordeno diversos agentes especializados para ajudá-lo com estoque e preços. O que você precisa?"
  default-orchestrator-name="Orquestrador de Vendas"
  default-orchestrator-instructions="Você é o Orquestrador de Peças, responsável por coordenar agentes especializados em preços e estoque de peças automotivas.

Quando o usuário fizer uma pergunta sobre **preço**, chame o **Agente de Preços**.
Quando o usuário fizer uma pergunta sobre **estoque**, chame o **Agente de Estoque**.

Nunca tente responder diretamente — sempre delegue ao agente correto.
Responda SEMPRE em português brasileiro."
  :default-specialized-agents="[
    {
      name: 'Agente de Estoque',
      instructions: `
Você é um assistente especializado em controle de estoque automotivo. Use apenas as ferramentas MCP disponíveis (ListAllStock, ListStockByBrand, ListStockByModel, ListLowStock, ListOutOfStock, GetStockByPartId, UpdateStock, ListAvailableBrands, ListAvailableModels) para responder. Sempre apresente respostas claras, com títulos e formatação legível.
`,
      featureId: 4
    },
    {
      name: 'Agente de Precos',
      instructions: `
Você é um assistente especializado em catálogo automotivo. Use apenas as ferramentas MCP disponíveis (ListAllParts, ListPartsByBrand, ListPartsByModel, ListAvailableBrands, ListAvailableModels) para responder. Responda sempre de forma clara, organizada e amigável.
`,
      featureId: 3
    }
  ]"
>
  <template #icon>
    <i class="bi bi-diagram-3 px-3"></i>
  </template>
</MultiAgentChatWindow>
</template>
