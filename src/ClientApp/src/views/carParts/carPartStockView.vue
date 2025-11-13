<script setup lang="ts">
import HelpButton from "@/components/common/HelpButton.vue";
import AgentChatWindow from "@/components/agent/AgentChatWindow.vue";

const videoUrl = `${window.location.origin}/videos/car-stock.mp4`;
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
      <i class="bi bi-box-seam px-2"></i>
      Descritivo da Página do Agente de Estoque de Peças Automotivas
    </h2>
    <p>
      Esta página apresenta um
      <strong>assistente virtual especializado no controle de estoque de peças automotivas</strong>.
      Ele permite consultar utilizando código do produto o estoque dele, por meio de
      integração direta com o servidor MCP (<em>Model Context Protocol</em>).
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-list-task px-2"></i> Funcionalidades
    </h5>
    <ul>
      <li>
        <strong>Filtrar por Código do produto:</strong>
        Retorna apenas as peças correspondentes ao código informado (ex: CM002, BS003).
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
      O agente permite ajustar suas instruções de comportamento e o uso das ferramentas MCP por meio
      do botão <strong>"Instruções"</strong>, possibilitando criar contextos personalizados — como
      monitoramento de estoque baixo, atualizações em lote ou consultas por categoria.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-bullseye px-2"></i> Objetivo
    </h5>
    <p>
      O objetivo deste agente é <strong>automatizar e simplificar o controle de estoque de peças automotivas</strong>,
      permitindo consultas, alertas e atualizações de forma conversacional e integrada ao sistema MCP.
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

  <AgentChatWindow
    :feature-id="4"
    title="Agente de Estoque de Peças Automotivas"
    welcome-message="👋 Olá! Sou o Agente de Estoque de Peças. Posso te informar o estoque com base no código do produto. O que você gostaria de fazer?"
    default-agent-name="Agente de Estoque"
    default-instructions="Você é um assistente especializado em controle de estoque automotivo. Use apenas as ferramentas MCP disponíveis para responder. Sempre apresente respostas claras, com títulos e formatação legível."
  >
    <template #icon>
      <i class="bi bi-box2-heart px-3"></i>
    </template>
  </AgentChatWindow>
</template>
