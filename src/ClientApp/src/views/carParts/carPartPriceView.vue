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
      <i class="bi bi-currency-dollar px-2"></i>
      Descritivo da Página do Agente de Consulta de Preço de Peças Automotivas
    </h2>

    <p>
      Esta página apresenta um
      <strong>assistente virtual especializado em consulta de preços de peças automotivas</strong>.
      Ele permite ao usuário <strong>informar o código da peça</strong> (ex: <code>WB005</code>)
      e obter instantaneamente o preço correspondente, utilizando integração com o servidor MCP
      (<em>Model Context Protocol</em>).
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-list-task px-2"></i> Funcionalidades
    </h5>
    <ul>
      <li>
        <strong>Consultar Preço por Código:</strong>
        Informa o preço de uma peça específica a partir do código do produto (ex: <code>CM002</code>, <code>BS003</code>).
      </li>

      <li>
        <strong>Interface Conversacional:</strong>
        Toda a interação ocorre via chat, tornando a consulta simples e natural.
      </li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-gear px-2"></i> Personalização
    </h5>
    <p>
      O agente permite configurar seu comportamento e as ferramentas MCP utilizadas por meio do botão
      <strong>"Instruções"</strong>. Assim, é possível adaptar o contexto de consulta, como limitar
      a pesquisa a determinadas marcas, categorias ou faixas de preço.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-bullseye px-2"></i> Objetivo
    </h5>
    <p>
      O objetivo deste agente é <strong>facilitar a consulta de preços de peças automotivas</strong>,
      fornecendo uma experiência de busca direta e eficiente sem a necessidade de navegar em catálogos
      ou sistemas manuais de estoque.
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
    :feature-id="4"
    title="Agente de Consulta de Preço de Peças Automotivas"
    welcome-message="👋 Olá! Sou o Agente de Consulta de Preço de Peças Automotivas. Informe o código da peça (ex: WB005) e eu lhe direi o preço correspondente. Deseja consultar alguma peça agora?"
    default-agent-name="Agente de Preço de Peças"
    default-instructions="Você é um assistente especializado em consulta de preços de peças automotivas. Use apenas as ferramentas MCP disponíveis para responder. Sempre solicite o código da peça se ele não for informado, e responda de forma clara e amigável."
  >
    <template #icon>
      <i class="bi bi-cash-coin px-3"></i>
    </template>
  </AgentChatWindow>
</template>
