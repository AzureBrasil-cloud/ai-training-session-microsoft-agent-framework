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

    <h2 class="mb-5 mt-8"><i class="bi bi-car-front px-2"></i> Descritivo da Página do Assistente de Cadastro de Carros</h2>
    <p>
      Esta página apresenta um <strong>assistente virtual especializado em cadastro e gerenciamento de veículos</strong>. Ele auxilia o usuário a cadastrar, consultar e atualizar informações de carros como marca, modelo, ano, placa e outros dados relevantes.
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

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-tools px-2"></i> Ferramentas (Tools) Disponíveis</h5>
    <p>
      Este agente utiliza <strong>Function Tools</strong> para executar operações no sistema. Durante a conversa, ele pode invocar automaticamente as seguintes ferramentas:
    </p>
    <ul>
      <li><strong>CreateCar:</strong> Cria um novo registro de veículo no sistema com informações como nome, proprietário, ano, modelo, preço e placa.</li>
      <li><strong>UpdateCar:</strong> Atualiza as informações de um veículo existente, permitindo modificar qualquer campo cadastrado.</li>
      <li><strong>DeleteCar:</strong> Remove um veículo do sistema com base em critérios específicos.</li>
      <li><strong>ListCars:</strong> Consulta e lista veículos cadastrados, permitindo filtros por diversos parâmetros (nome, ano, modelo, placa, etc.).</li>
    </ul>
    <p>
      Essas ferramentas são chamadas automaticamente pelo agente quando necessário, tornando a interação transparente e natural para o usuário. Por exemplo, quando você solicita "cadastre um Toyota Corolla 2022", o agente identifica a intenção e chama a tool <code>CreateCar</code> com os parâmetros apropriados.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-bullseye px-2"></i> Objetivo</h5>
    <p>
      O objetivo deste agente é <strong>simplificar o processo de cadastro e gerenciamento de veículos</strong>, oferecendo uma interface intuitiva e conversacional que elimina a necessidade de formulários complexos.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-link-45deg px-2"></i> Links Úteis</h5>
    <ul>
      <li>
        <a href="https://learn.microsoft.com/en-au/agent-framework/overview/agent-framework-overview" target="_blank" rel="noopener">
          Visão Geral do Microsoft Agent Framework
        </a>
      </li>
      <li>
        <a href="https://learn.microsoft.com/en-au/agent-framework/tutorials/agents/function-tools?pivots=programming-language-csharp" target="_blank" rel="noopener">
          Usando ferramentas de função com um agente
        </a>
      </li>
    </ul>
  </HelpButton>

  <AgentChatWindow
    :feature-id="1"
    title="Assistente de Cadastro de Carros"
    welcome-message="👋 Olá! Sou o Agente de Cadastro de Carros. Como posso ajudar você hoje? Posso auxiliar no cadastro, consulta ou atualização de informações de veículos."
    default-agent-name="Agente de Cadastro de Carros"
    default-instructions="Você é um assistente que ajuda a cadastrar e consultar carros, solicitando informações como modelo, marca, ano e placa."
  >
    <template #icon>
      <i class="bi bi-car-front px-3"></i>
    </template>
  </AgentChatWindow>
</template>

