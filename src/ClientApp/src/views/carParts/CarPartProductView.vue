<script setup lang="ts">
import HelpButton from "@/components/common/HelpButton.vue";
import AgentChatWindow from "@/components/agent/AgentChatWindow.vue";

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
          href="https://learn.microsoft.com/en-us/agent-framework/overview/agent-framework-overview#ai-agents"
          target="_blank"
          rel="noopener"
        >
         Microsoft Agent Framework - AI Agents – Visão Geral
        </a>
      </li>
      <li>
        <a
          href="https://medium.com/@carlosmachel_46688/entendendo-model-context-protocol-mcp-4026f3bcc3af"
          target="_blank"
          rel="noopener"
        >
          Entendendo Model Context Protocol (MCP)
        </a>
      </li>
    </ul>
  </HelpButton>

  <AgentChatWindow
    :feature-id="9"
    title="Agente de Catálogo de Peças de Carros"
    welcome-message="👋 Olá! Sou o Agente de Catálogo de Peças Automotivas. Posso listar todas as peças disponíveis. O que você deseja consultar?"
    default-agent-name="Agente de Catálogo de Peças"
    default-instructions="Você é um assistente especializado em catálogo automotivo. Use apenas as ferramentas MCP disponíveis para responder. Responda sempre de forma clara, organizada e amigável."
  >
    <template #icon>
      <i class="bi bi-tools px-3"></i>
    </template>
  </AgentChatWindow>
</template>
