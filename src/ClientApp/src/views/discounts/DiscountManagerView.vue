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
      Descritivo da Página Agente de Gerenciamento de Descontos
    </h2>
    <p>
      Esta página apresenta um
      <strong>assistente virtual especializado em lidar com pedidos de desconto por clientes.</strong>.
      Ele permite ao usuário gerenciar e aprovar descontos em produtos automotivos de forma conversacional,
      utilizando ferramentas integradas ao servidor MCP (<em>Model Context Protocol</em>).
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-list-task px-2"></i> Funcionalidades
    </h5>
    <ul>
      <li>
        <strong>Lista as aprovações pendentes.</strong>
        Retorna todas as informacoes das aprovações pendentes.
      </li>
      <li>
        <strong>Aprovar/Rejeitar descontos</strong>
        A partir de um ID de solicitação, aprova ou rejeita o desconto solicitado.
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
      O objetivo deste agente é <strong>simplificar o gerenciamento de pedidos de desconto.</strong>,
      oferecendo uma experiência conversacional eficiente e intuitiva que elimina a necessidade de processos manuais demorados.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-link-45deg px-2"></i> Links Úteis
    </h5>
    <ul>
     <li>
        <a
          href="https://learn.microsoft.com/en-us/agent-framework/user-guide/workflows/overview"
          target="_blank"
          rel="noopener"
        >
          Workflow - Visão Geral do Microsoft Agent Framework
        </a>
      </li>
      <li>
        <a
          href="https://learn.microsoft.com/en-us/agent-framework/user-guide/workflows/requests-and-responses?pivots=programming-language-csharp"
          target="_blank"
          rel="noopener"
        >
          Workflow - Requests and Responses no Microsoft Agent Framework
        </a>
      </li>
    </ul>
  </HelpButton>

  <AgentChatWindow
    :feature-id="8"
    title="Agente de Catálogo de Peças de Carros"
    welcome-message="👋 Olá! Sou o Agente de Gerenciamento de Descontos. Posso listar as solicitações pendentes e aprovar/rejeitar a solicitacao."
    default-agent-name="Agente de Catálogo de Peças de Carros"
    default-instructions="Voce é um assistente virtual especializado em ajudar o usuario a verificar e aprovar descontos em produtos automotivos. Utilize as ferramentas disponiveis para verificar e aprovar o desconto."
  >
    <template #icon>
      <i class="bi bi-tools px-3"></i>
    </template>
  </AgentChatWindow>
</template>
