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
      Descritivo da Página do Agente de Descontos
    </h2>
    <p>
      Esta página apresenta um
      <strong>assistente virtual especializado em solicitação de descontos.</strong>.
      Ele permite ao usuario solicitar descontos em produtos e consultar o status de suas solicitações,
      utilizando ferramentas integradas ao servidor MCP (<em>Model Context Protocol</em>).
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
  <i class="bi bi-list-task px-2"></i> Funcionalidades
</h5>
<ul>
  <li>
    <strong>Solicitar Desconto</strong>
    Permite ao usuário solicitar um desconto em um produto, informando:
    <ul>
      <li>Nome do produto</li>
      <li>Preço original</li>
      <li>Percentual de desconto desejado (0.0 a 1.0)</li>
      <li>Código do cliente</li>
      <li>Motivo do pedido de desconto</li>
    </ul>
  </li>
  <li>
    <strong>Consultar Status de Desconto</strong>
    Informa o status atual de uma solicitação de desconto previamente feita utilizando o código.
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

  <AgentChatWindow
    :feature-id="7"
    title="Agente de Descontos"
    welcome-message="👋 Olá! Sou o Agente de Descontos. Posso solicitar um desconto ou consultar uma solicitação de desconto. O que você deseja fazer?"
    default-agent-name="Agente de Descontos"
    default-instructions="Você é um assistente virtual especializado em peças automotivas e solicitações de desconto.
Antes de chamar qualquer ferramenta, sempre confirme se possui todas as informações necessárias.

Para solicitar um desconto (RequestDiscount), pergunte e valide as seguintes informações:
1. Nome do produto
2. Preço original
3. Percentual de desconto solicitado (0.0 a 1.0) ou o preco final desejado e calcule o percentual
4. Código do cliente
5. Motivo da solicitação

Se alguma dessas informações estiver faltando, pergunte ao usuário de forma clara e natural.
Quando tiver tudo, chame a ferramenta RequestDiscount e retorne para o usuário a confirmação do pedido e o ID do pedido.

Use GetDiscountStatus apenas quando o usuário pedir para verificar o andamento de uma solicitação anterior."

  >
    <template #icon>
      <i class="bi bi-tools px-3"></i>
    </template>
  </AgentChatWindow>
</template>
