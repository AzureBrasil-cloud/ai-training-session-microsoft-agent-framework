<script setup lang="ts">
import HelpButton from "@/components/common/HelpButton.vue";
import AgentChatWindow from "@/components/agent/AgentChatWindow.vue";

const videoUrl = `${window.location.origin}/videos/customer-policies-agent.mp4`;
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

    <h2 class="mb-5 mt-8"><i class="bi bi-shield-check px-2"></i> Descritivo da Página de Políticas de Relacionamento com o Cliente</h2>
    <p>
      Esta página apresenta um <strong>assistente virtual especializado em políticas de relacionamento com o cliente</strong>. Ele auxilia usuários e equipes a consultar, esclarecer dúvidas e obter orientações sobre as políticas de atendimento, garantias, trocas, devoluções e demais diretrizes de relacionamento da empresa.
    </p>
    <p>
      <strong>🔐 Consulta de Documentos Privados:</strong> Este agente utiliza <strong>RAG (Retrieval Augmented Generation)</strong> para consultar e extrair informações de documentos privados da empresa, como manuais de políticas internas, regulamentos e diretrizes confidenciais, garantindo respostas precisas e baseadas em fontes oficiais.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-list-task px-2"></i> Funcionalidades</h5>
    <ul>
      <li><strong>Consulta de Políticas:</strong> O assistente fornece informações sobre políticas de atendimento, garantias e trocas.</li>
      <li><strong>Esclarecimento de Dúvidas:</strong> Responde questões sobre direitos do cliente e procedimentos.</li>
      <li><strong>Orientação de Processos:</strong> Guia sobre como proceder em casos de reclamações, devoluções ou solicitações especiais.</li>
      <li><strong>Interface Conversacional:</strong> Toda interação via chat em linguagem natural.</li>
      <li><strong>Gerenciamento de Threads:</strong> Mantém histórico de conversas organizadas em threads.</li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-gear px-2"></i> Personalização</h5>
    <p>
      O agente permite personalizar suas instruções e comportamento através do botão "Instruções", possibilitando ajustar o contexto e a forma como ele interage com o usuário.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-bullseye px-2"></i> Objetivo</h5>
    <p>
      O objetivo deste agente é <strong>facilitar o acesso e compreensão das políticas de relacionamento com o cliente</strong>, oferecendo respostas rápidas e precisas que melhoram a experiência do cliente e agilizam o trabalho das equipes de atendimento.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-link-45deg px-2"></i> Links Úteis</h5>
    <ul>
      <li>
        <a href="https://learn.microsoft.com/en-au/agent-framework/overview/agent-framework-overview" target="_blank" rel="noopener">
          Visão Geral do Microsoft Agent Framework
        </a>
      </li>
      <li>
        <a href="https://learn.microsoft.com/en-us/agent-framework/user-guide/agents/agent-rag?pivots=programming-language-csharp" target="_blank" rel="noopener">
          Agent Retrieval Augmented Generation (RAG)
        </a>
      </li>
    </ul>
  </HelpButton>

  <AgentChatWindow
    :feature-id="2"
    title="Assistente de Políticas de Relacionamento"
    welcome-message="👋 Olá! Sou o Agente de Políticas de Relacionamento com o Cliente. Como posso ajudar você hoje? Posso esclarecer dúvidas sobre políticas de atendimento, garantias, trocas e devoluções."
    default-agent-name="Agente de Políticas de Relacionamento"
    default-instructions="Você é um assistente especializado em políticas de relacionamento com o cliente. Ajuda a esclarecer dúvidas sobre políticas de atendimento, garantias, trocas, devoluções e procedimentos de reclamação. Sempre forneça informações claras e precisas sobre os direitos do cliente e os processos da empresa."
  >
    <template #icon>
      <i class="bi bi-shield-check px-3"></i>
    </template>
  </AgentChatWindow>
</template>

