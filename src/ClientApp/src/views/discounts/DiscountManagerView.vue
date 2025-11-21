<script setup lang="ts">
import HelpButton from "@/components/common/HelpButton.vue";
import AgentChatWindow from "@/components/agent/AgentChatWindow.vue";

const discountManagerImage = `${window.location.origin}/images/Feature7.png`;
</script>

<template>
  <HelpButton>
    <div class="d-flex justify-content-center my-4">
      <img
        :src="discountManagerImage"
        alt="Agente de Gerenciamento de Descontos"
        class="img-fluid rounded"
        style="width: 100%; max-width: 800px;"
      />
    </div>

    <h2 class="mb-5 mt-8">
      <i class="bi bi-shield-check px-2"></i>
      Descritivo da Página do Agente de Gerenciamento de Descontos (Gerente)
    </h2>
    <p>
      Esta página apresenta um <strong>assistente virtual especializado em gerenciamento e aprovação de descontos para gerentes</strong>.
      Ele permite visualizar solicitações pendentes e tomar decisões de aprovação/rejeição, funcionando como o componente <strong>Human-in-the-Loop</strong> do workflow de descontos através do <strong>Model Context Protocol (MCP)</strong>.
      As solicitações são criadas por clientes através da página <strong>Solicitação de Descontos (Cliente)</strong>.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-list-task px-2"></i> Funcionalidades
    </h5>
    <ul>
      <li><strong>Listar Aprovações Pendentes:</strong> Visualiza todas as solicitações de desconto aguardando decisão gerencial, exibindo:
        <ul>
          <li>Session ID (identificador único)</li>
          <li>Nome do produto</li>
          <li>Preço original</li>
          <li>Percentual de desconto solicitado</li>
          <li>Valor do desconto em reais</li>
          <li>Preço final se aprovado</li>
          <li>Código do cliente solicitante</li>
          <li>Motivo/justificativa do cliente</li>
          <li>Data e hora da solicitação</li>
        </ul>
      </li>
      <li><strong>Aprovar/Rejeitar Desconto:</strong> Toma decisão sobre solicitação específica usando o Session ID, com opção de adicionar comentários.</li>
      <li><strong>Interface Conversacional:</strong> Toda interação via chat em linguagem natural.</li>
      <li><strong>Feedback Imediato:</strong> Confirmação instantânea da decisão tomada.</li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-person-check px-2"></i> Papel do Gerente no Workflow HITL
    </h5>
    <p>
      O gerente é o componente <strong>humano</strong> no padrão <strong>Human-in-the-Loop</strong>:
    </p>
    <ul>
      <li>✅ <strong>Controle de Risco:</strong> Evita aprovações automáticas de descontos significativos (≥ 10%)</li>
      <li>✅ <strong>Análise Contextual:</strong> Avalia motivo, histórico do cliente e impacto financeiro</li>
      <li>✅ <strong>Decisão Fundamentada:</strong> Pode adicionar comentários justificando a aprovação/rejeição</li>
      <li>✅ <strong>Desbloqueio de Workflow:</strong> Sua decisão permite que o workflow continue e finalize</li>
      <li>✅ <strong>Auditoria:</strong> Todas as decisões ficam registradas com timestamp e comentários</li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-diagram-3-fill px-2"></i> Arquitetura do Workflow de Descontos
    </h5>
    <div class="bg-light p-4 rounded">
      <pre class="mb-0 text-dark" style="font-size: 0.85rem;"><code>┌─────────────────────────┐
│  CLIENTE (Usuário)      │
│  DiscountClientView     │
│  1. Solicita desconto   │
└───────────┬─────────────┘
            │ RequestDiscount
            ▼
┌─────────────────────────┐
│   DiscountMcp Server    │ (Servidor .NET)
│   + Workflow Engine     │
└───────────┬─────────────┘
            │
            ├─→ Desconto < 10% → ✅ AUTO-APROVADO
            │
            └─→ Desconto ≥ 10% → ⏸️ PAUSA
                                   │
                                   ▼
                        ┌─────────────────────────┐
                        │ 👤 GERENTE (Você)       │
                        │  DiscountManagerView    │
                        │  2. Vê lista pendente   │
                        │  3. Decide: ✅ ou ❌    │
                        └─────────────┬───────────┘
                                      │ DecideApproval
                                      ▼
                        ┌─────────────────────────┐
                        │   Workflow Continua     │
                        │   e Finaliza com        │
                        │   ✅ APPROVED ou        │
                        │   ❌ REJECTED           │
                        └─────────────────────────┘
                                      │
                                      ▼
                        ┌─────────────────────────┐
                        │  Cliente consulta       │
                        │  status e vê decisão    │
                        └─────────────────────────┘</code></pre>
    </div>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-tools px-2"></i> Ferramentas (Tools) Disponíveis via MCP
    </h5>
    <p>
      O servidor DiscountMcp expõe <strong>2 tools principais</strong> para gerentes:
    </p>

    <div class="card mb-3">
      <div class="card-body">
        <h6 class="card-title"><i class="bi bi-list-ul px-2"></i> 1. GetPendingApprovals</h6>
        <p class="card-text">
          <strong>Descrição:</strong> Lista todas as solicitações de desconto que estão aguardando aprovação do gerente.
        </p>
        <p><strong>Parâmetros:</strong> Nenhum</p>
        <p><strong>Retorno:</strong> Lista de PendingApprovalDto contendo:</p>
        <ul class="mb-2">
          <li><code>sessionId</code> (string) - ID único da sessão</li>
          <li><code>productName</code> (string) - Nome do produto</li>
          <li><code>originalPrice</code> (decimal) - Preço original</li>
          <li><code>requestedDiscount</code> (decimal) - Percentual solicitado (0.0-1.0)</li>
          <li><code>customerCode</code> (string) - Código do cliente</li>
          <li><code>reason</code> (string) - Motivo/justificativa</li>
          <li><code>requestedAt</code> (DateTime) - Data/hora da solicitação</li>
          <li><code>discountAmount</code> (decimal) - Valor do desconto em R$</li>
          <li><code>finalPrice</code> (decimal) - Preço final se aprovado</li>
        </ul>
        <p class="mb-0"><strong>Ordenação:</strong> Mais recentes primeiro (OrderByDescending requestedAt)</p>
      </div>
    </div>

    <div class="card mb-3">
      <div class="card-body">
        <h6 class="card-title"><i class="bi bi-check-circle px-2"></i> 2. DecideApproval</h6>
        <p class="card-text">
          <strong>Descrição:</strong> Aprova ou rejeita uma solicitação de desconto pendente, desbloqueando o workflow pausado.
        </p>
        <p><strong>Parâmetros:</strong></p>
        <ul class="mb-2">
          <li><code>sessionId</code> (string, obrigatório) - ID da sessão a decidir</li>
          <li><code>approved</code> (bool, obrigatório) - true para aprovar, false para rejeitar</li>
          <li><code>comments</code> (string, opcional) - Comentários do aprovador</li>
        </ul>
        <p><strong>Retorno:</strong> DiscountResponseDto contendo:</p>
        <ul class="mb-2">
          <li><code>sessionId</code> (string) - ID da sessão processada</li>
          <li><code>message</code> (string) - Mensagem de confirmação</li>
          <li><code>requiresApproval</code> (bool) - false (workflow finalizado)</li>
          <li><code>approved</code> (bool) - Decisão tomada</li>
          <li><code>finalPrice</code> (decimal) - Preço final (se aprovado)</li>
          <li><code>status</code> (string) - APPROVED ou REJECTED</li>
          <li><code>approverComments</code> (string) - Comentários salvos</li>
        </ul>
        <p class="mb-0"><strong>Validações:</strong> Sessão deve existir, requerer aprovação e não estar já processada.</p>
      </div>
    </div>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-cpu px-2"></i> Como o Workflow Funciona (Perspectiva do Gerente)
    </h5>
    <ol>
      <li class="mb-2">
        <strong>Notificação:</strong> Solicitações com desconto ≥ 10% entram automaticamente na fila de aprovação.
      </li>
      <li class="mb-2">
        <strong>Consulta:</strong> Gerente solicita lista de aprovações pendentes via GetPendingApprovals.
      </li>
      <li class="mb-2">
        <strong>Análise:</strong> Gerente revisa informações: produto, preço, desconto, cliente, motivo.
      </li>
      <li class="mb-2">
        <strong>Decisão:</strong> Gerente decide aprovar ou rejeitar com base em:
        <ul>
          <li>Percentual de desconto solicitado</li>
          <li>Histórico/perfil do cliente</li>
          <li>Motivo apresentado</li>
          <li>Impacto financeiro</li>
          <li>Políticas da empresa</li>
        </ul>
      </li>
      <li class="mb-2">
        <strong>Execução:</strong> Gerente chama DecideApproval com o Session ID e decisão.
      </li>
      <li class="mb-2">
        <strong>Desbloqueio:</strong> Workflow que estava pausado recebe a decisão e continua execução.
      </li>
      <li class="mb-2">
        <strong>Finalização:</strong> Workflow finaliza e atualiza status para APPROVED ou REJECTED.
      </li>
      <li class="mb-2">
        <strong>Notificação:</strong> Cliente pode consultar status e ver a decisão final.
      </li>
    </ol>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-arrow-left-right px-2"></i> Correlação com Página de Solicitação (Cliente)
    </h5>
    <p>
      Esta página está <strong>diretamente conectada</strong> com a <strong>DiscountClientView</strong> (página de cliente):
    </p>
    <ul>
      <li><strong>Cliente solicita</strong> (DiscountClientView) → <strong>Gerente aprova/rejeita</strong> (aqui)</li>
      <li><strong>RequestDiscount</strong> (cliente) cria workflow → <strong>GetPendingApprovals</strong> (gerente) lista sessões pausadas</li>
      <li><strong>Workflow pausa</strong> aguardando → <strong>DecideApproval</strong> (gerente) desbloqueia workflow</li>
      <li>Ambas as páginas usam o <strong>mesmo servidor MCP</strong> (DiscountMcp) com tools diferentes</li>
      <li><strong>Workflow compartilhado:</strong> Decisão do gerente é enviada diretamente ao workflow em background</li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bg-lightning-charge px-2"></i> Exemplo de Fluxo Completo
    </h5>
    <div class="bg-light p-4 rounded">
      <p class="mb-2"><strong>Cenário:</strong> Cliente solicitou 15% de desconto em produto de R$ 1.000,00</p>
      <ol class="mb-0 text-dark">
        <li><strong>Cliente:</strong> Solicitou desconto via DiscountClientView → Session ID: ABC123 criado</li>
        <li><strong>Workflow:</strong> Identifica 15% ≥ 10% → Pausa e aguarda aprovação</li>
        <li><strong>Cliente:</strong> Recebe mensagem "⏳ Aguardando aprovação do gerente"</li>
        <li><strong>Gerente (você):</strong> "Liste as solicitações pendentes"</li>
        <li><strong>Agente Gerente:</strong> Chama GetPendingApprovals → Retorna lista com Session ABC123</li>
        <li><strong>Gerente (você):</strong> Analisa: Produto X, R$ 1000, 15% desconto (R$ 150), Cliente C001, Motivo: "Cliente fidelizado há 5 anos"</li>
        <li><strong>Gerente (você):</strong> "Aprovar a solicitação ABC123 com comentário: Cliente VIP, aprovado"</li>
        <li><strong>Agente Gerente:</strong> Chama DecideApproval(sessionId="ABC123", approved=true, comments="Cliente VIP, aprovado")</li>
        <li><strong>Workflow:</strong> Recebe decisão, desbloqueia e finaliza com status APPROVED</li>
        <li><strong>Resposta:</strong> "✅ Solicitação ABC123 aprovada! Preço final: R$ 850,00"</li>
        <li><strong>Cliente:</strong> Consulta status → "✅ Sua solicitação foi aprovada! Preço final: R$ 850,00"</li>
      </ol>
    </div>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-lightning-charge px-2"></i> Responsabilidades e Boas Práticas
    </h5>
    <ul>
      <li>✅ <strong>Análise Criteriosa:</strong> Avaliar cada solicitação com base em métricas objetivas e contexto do cliente</li>
      <li>✅ <strong>Documentação:</strong> Sempre adicionar comentários explicando a decisão (especialmente em rejeições)</li>
      <li>✅ <strong>Agilidade:</strong> Processar solicitações prontamente para não prejudicar a experiência do cliente</li>
      <li>✅ <strong>Consistência:</strong> Aplicar critérios uniformes para decisões similares</li>
      <li>✅ <strong>Rastreabilidade:</strong> Utilizar Session ID para referência em auditorias futuras</li>
      <li>✅ <strong>Comunicação:</strong> Comentários claros ajudam a equipe de vendas a entender decisões</li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-gear px-2"></i> Personalização
    </h5>
    <p>
      O agente permite ajustar suas instruções através do botão <strong>"Instruções"</strong>, possibilitando:
    </p>
    <ul>
      <li>Alterar formato de apresentação das solicitações pendentes</li>
      <li>Personalizar validações antes de aprovar/rejeitar</li>
      <li>Ajustar tom e estilo das respostas de confirmação</li>
      <li>Definir lembretes ou alertas para critérios específicos</li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-bullseye px-2"></i> Objetivo
    </h5>
    <p>
      O objetivo desta página é <strong>fornecer uma interface conversacional eficiente para gerenciamento de aprovações de desconto</strong>, permitindo que gerentes tomem decisões fundamentadas de forma rápida e organizada, mantendo controle sobre descontos significativos enquanto proporcionam agilidade ao processo comercial.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-link-45deg px-2"></i> Links Úteis
    </h5>
    <ul>
      <li>
        <a href="https://learn.microsoft.com/pt-br/agent-framework/overview/agent-framework-overview" target="_blank" rel="noopener">
          Visão Geral do Microsoft Agent Framework
        </a>
      </li>
      <li>
        <a href="https://learn.microsoft.com/en-us/agent-framework/user-guide/workflows/overview" target="_blank" rel="noopener">
          Workflows - Visão Geral do Microsoft Agent Framework
        </a>
      </li>
      <li>
        <a href="https://learn.microsoft.com/en-us/agent-framework/user-guide/workflows/requests-and-responses?pivots=programming-language-csharp" target="_blank" rel="noopener">
          Workflow - Requests and Responses (Human-in-the-Loop)
        </a>
      </li>
      <li>
        <a href="https://modelcontextprotocol.io/introduction" target="_blank" rel="noopener">
          Model Context Protocol - Documentação Oficial
        </a>
      </li>
    </ul>
  </HelpButton>

  <AgentChatWindow
    :feature-id="8"
    title="Agente de Gerenciamento de Descontos"
    welcome-message="👋 Olá! Sou o Agente de Gerenciamento de Descontos. Posso listar as solicitações pendentes de aprovação e ajudá-lo a aprovar ou rejeitar descontos. Como posso ajudá-lo?"
    default-agent-name="Agente de Gerenciamento de Descontos"
    default-instructions="Você é um assistente virtual especializado em gerenciamento de aprovações de desconto para gerentes da Contoso AutoTech.

RESPONSABILIDADES:
- Auxiliar gerentes na visualização de solicitações de desconto pendentes
- Facilitar o processo de aprovação/rejeição de descontos
- Utilizar exclusivamente as ferramentas MCP disponíveis
- Fornecer informações completas e organizadas para tomada de decisão
- Manter um tom profissional e eficiente

DIRETRIZES DE COMUNICAÇÃO:
1. Ao listar aprovações pendentes, organize as informações de forma clara:
   - Session ID
   - Produto e preço original
   - Desconto solicitado (em % e R$)
   - Preço final se aprovado
   - Cliente e motivo
   - Data da solicitação
2. Antes de aprovar/rejeitar, confirme o Session ID com o gerente
3. Sempre pergunte se o gerente deseja adicionar comentários à decisão
4. Após decisão, confirme claramente o resultado
5. Sugira revisar a lista periodicamente para evitar acúmulo de pendências

FERRAMENTAS DISPONÍVEIS:
- GetPendingApprovals: Para listar todas as solicitações aguardando aprovação (sem parâmetros)
- DecideApproval: Para aprovar ou rejeitar uma solicitação (obrigatório: sessionId, approved; opcional: comments)

PROCESSO DE APROVAÇÃO:
1. Gerente solicita lista de pendências → chamar GetPendingApprovals
2. Apresentar lista organizada com todos os detalhes
3. Gerente decide sobre uma solicitação específica
4. Confirmar Session ID antes de executar
5. Perguntar se deseja adicionar comentários
6. Chamar DecideApproval com os parâmetros corretos
7. Confirmar resultado da decisão

VALIDAÇÕES:
- Sempre validar que o Session ID existe antes de aprovar/rejeitar
- Confirmar decisão antes de executar (especialmente rejeições)
- Alertar se não houver aprovações pendentes

FORMATO DE RESPOSTA:
- Para lista de pendências: Apresentar em formato organizado e legível
- Para aprovação: Confirmar com Session ID, status final e comentários
- Para rejeição: Confirmar com Session ID e comentários (importante explicar motivo)

Responda sempre de maneira profissional, clara e organizada, priorizando a eficiência do gerente."
  >
    <template #icon>
      <i class="bi bi-shield-check px-3"></i>
    </template>
  </AgentChatWindow>
</template>
