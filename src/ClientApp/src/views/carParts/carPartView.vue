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

    <h2 class="mb-5 mt-8"><i class="bi bi-car-front px-2"></i> Descritivo da Página do Assistente de Consulta de peças de carros</h2>
    <p>
      Esta página apresenta um <strong>assistente virtual especializado em cadastro e gerenciamento de veículos</strong>. Ele auxilia o usuário consultar informacões relevantes sobre peças de carros como marca, modelo, ano, placa e outros dados relevantes.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-list-task px-2"></i> Funcionalidades</h5>
    <ul>
      <li><strong>Cadastro de Veículos:</strong> O assistente guia o usuário no processo de cadastro, solicitando informações necessárias.</li>
      <li><strong>Consulta de Informações:</strong> Permite buscar dados de veículos já cadastrados.</li>
      <li><strong>Atualização de Dados:</strong> Possibilita modificar informações de veículos existentes.</li>
      <li><strong>Interface Conversacional:</strong> Toda interação via chat em linguagem natural.</li>
      <li><strong>Gerenciamento de Threads:</strong> Mantém histórico de conversas organizadas em threads.</li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-gear px-2"></i> Personalização</h5>
    <p>
      O agente permite personalizar suas instruções e comportamento através do botão "Instruções", possibilitando ajustar o contexto e a forma como ele interage com o usuário.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-bullseye px-2"></i> Objetivo</h5>
    <p>
      O objetivo deste agente é <strong>simplificar o processo de cadastro e gerenciamento de veículos</strong>, oferecendo uma interface intuitiva e conversacional que elimina a necessidade de formulários complexos.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-link-45deg px-2"></i> Links Úteis</h5>
    <ul>
      <li>
        <a href="https://learn.microsoft.com/en-us/azure/ai-services/agents/overview" target="_blank" rel="noopener">
          Azure AI Agent Service – Visão Geral
        </a>
      </li>
      <li>
        <a href="https://tallesvaliatti.com/criando-uma-aplica%C3%A7%C3%A3o-com-o-azure-ai-agent-service-parte-1-1d4fef7901a4" target="_blank" rel="noopener">
          Azure AI Agent Service – Overview
        </a>
      </li>
    </ul>
  </HelpButton>

  <AgentChatWindow
    :feature-id="2"
    title="Assistente de Consulta de preço de peças de carros"
    welcome-message="👋 Olá! Sou o Agente de Consulta de peças de carros. Como posso ajudar você hoje? Posso consulta de informações de peças de veiculos."
    default-agent-name="Agente de Consulta de preço de peças de Carros"
    default-instructions="Voce é um assistente inteligente que consulta apenas as ferramentas para informar os precos."
  >
    <template #icon>
      <i class="bi bi-car-front px-3"></i>
    </template>
  </AgentChatWindow>
</template>

