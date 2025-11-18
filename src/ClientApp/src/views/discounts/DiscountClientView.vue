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
      <i class="bi bi-percent px-2"></i>
      Descritivo da Página do Agente de Solicitação de Descontos (Cliente)
    </h2>
    <p>
      Esta página apresenta um <strong>assistente virtual especializado em solicitação de descontos para clientes</strong>.
      Ele permite solicitar descontos em produtos e consultar o status de aprovação, utilizando um <strong>Workflow com Human-in-the-Loop</strong> através do <strong>Model Context Protocol (MCP)</strong>.
      As solicitações são enviadas para aprovação de gerentes através da página <strong>Gerenciamento de Descontos (Manager)</strong>.
    </p>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-list-task px-2"></i> Funcionalidades
    </h5>
    <ul>
      <li><strong>Solicitar Desconto:</strong> Permite ao cliente solicitar um desconto informando:
        <ul>
          <li>Nome do produto</li>
          <li>Preço original</li>
          <li>Percentual de desconto desejado (0% a 100% ou 0.0 a 1.0)</li>
          <li>Código do cliente</li>
          <li>Motivo/justificativa do pedido de desconto</li>
        </ul>
      </li>
      <li><strong>Consultar Status:</strong> Verifica o status de uma solicitação anterior usando o Session ID retornado.</li>
      <li><strong>Interface Conversacional:</strong> Toda interação via chat em linguagem natural.</li>
      <li><strong>Feedback Imediato:</strong> Resposta instantânea se aprovado automaticamente ou se requer aprovação manual.</li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-diagram-2 px-2"></i> O que é Human-in-the-Loop Workflow?
    </h5>
    <p>
      <strong>Human-in-the-Loop (HITL)</strong> é um padrão de workflow onde processos automatizados podem pausar e solicitar intervenção humana quando necessário. No contexto de aprovação de descontos:
    </p>
    <ul>
      <li>✅ <strong>Automação Inteligente:</strong> Descontos pequenos podem ser aprovados automaticamente</li>
      <li>✅ <strong>Aprovação Manual:</strong> Descontos maiores pausam o workflow e aguardam decisão de um gerente</li>
      <li>✅ <strong>Controle de Risco:</strong> Previne aprovações não autorizadas de descontos significativos</li>
      <li>✅ <strong>Rastreabilidade:</strong> Cada solicitação tem um Session ID para acompanhamento</li>
      <li>✅ <strong>Transparência:</strong> Cliente sabe imediatamente se precisa aguardar aprovação</li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-diagram-3-fill px-2"></i> Arquitetura do Workflow de Descontos
    </h5>
    <div class="bg-light p-4 rounded">
      <pre class="mb-0 text-dark" style="font-size: 0.85rem;"><code>┌─────────────────────────┐
│  CLIENTE (Usuário)      │
│  DiscountClientView     │
└───────────┬─────────────┘
            │ 1. Solicita desconto (RequestDiscount)
            ▼
┌─────────────────────────┐
│   Agente de Desconto    │ (Cliente MCP)
│   (Cliente)             │
└───────────┬─────────────┘
            │ MCP Protocol
            ▼
┌─────────────────────────┐
│   DiscountMcp Server    │ (Servidor .NET)
│   + Workflow Engine     │
└───────────┬─────────────┘
            │
            ├─→ Desconto < 10% → ✅ APROVADO AUTOMATICAMENTE
            │
            └─→ Desconto ≥ 10% → ⏸️ PAUSA (Human-in-the-Loop)
                                   │
                                   ▼
                        ┌─────────────────────────┐
                        │  GERENTE (Admin)        │
                        │  DiscountManagerView    │
                        │  2. Aprova/Rejeita      │
                        └─────────────┬───────────┘
                                      │ (DecideApproval)
                                      ▼
                        ┌─────────────────────────┐
                        │   Workflow Continua     │
                        │   ✅ APPROVED ou        │
                        │   ❌ REJECTED           │
                        └─────────────────────────┘</code></pre>
    </div>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-tools px-2"></i> Ferramentas (Tools) Disponíveis via MCP
    </h5>
    <p>
      O servidor DiscountMcp expõe <strong>2 tools principais</strong> para clientes:
    </p>

    <div class="card mb-3">
      <div class="card-body">
        <h6 class="card-title"><i class="bi bi-plus-circle px-2"></i> 1. RequestDiscount</h6>
        <p class="card-text">
          <strong>Descrição:</strong> Inicia uma solicitação de desconto e retorna se foi aprovado automaticamente ou se requer aprovação manual.
        </p>
        <p><strong>Parâmetros (todos obrigatórios):</strong></p>
        <ul class="mb-2">
          <li><code>productName</code> (string) - Nome do produto</li>
          <li><code>originalPrice</code> (decimal) - Preço original do produto</li>
          <li><code>requestedDiscount</code> (decimal) - Percentual de desconto (0.0 a 1.0, ex: 0.15 = 15%)</li>
          <li><code>customerCode</code> (string) - Código/ID do cliente</li>
          <li><code>reason</code> (string) - Motivo/justificativa para o desconto</li>
        </ul>
        <p><strong>Retorno:</strong> DiscountResponseDto contendo:</p>
        <ul class="mb-2">
          <li><code>sessionId</code> (string) - ID único da sessão para acompanhamento</li>
          <li><code>message</code> (string) - Mensagem descritiva do resultado</li>
          <li><code>requiresApproval</code> (bool) - Se true, aguarda aprovação do gerente</li>
          <li><code>approved</code> (bool) - Se false e não requer aprovação, foi rejeitado automaticamente</li>
          <li><code>finalPrice</code> (decimal) - Preço final se aprovado</li>
          <li><code>status</code> (string) - PENDING_APPROVAL, APPROVED ou REJECTED</li>
        </ul>
        <p class="mb-0"><strong>Regra de Negócio:</strong> Descontos < 10% são aprovados automaticamente. Descontos ≥ 10% requerem aprovação de gerente.</p>
      </div>
    </div>

    <div class="card mb-3">
      <div class="card-body">
        <h6 class="card-title"><i class="bi bi-search px-2"></i> 2. GetDiscountStatus</h6>
        <p class="card-text">
          <strong>Descrição:</strong> Consulta o status atual de uma solicitação de desconto usando o Session ID.
        </p>
        <p><strong>Parâmetros:</strong></p>
        <ul class="mb-2">
          <li><code>sessionId</code> (string, obrigatório) - ID da sessão retornado pelo RequestDiscount</li>
        </ul>
        <p><strong>Retorno:</strong> DiscountStatusDto contendo:</p>
        <ul class="mb-2">
          <li><code>sessionId</code> (string) - ID da sessão</li>
          <li><code>status</code> (string) - Status atual (PENDING_APPROVAL, APPROVED, REJECTED)</li>
          <li><code>requiresApproval</code> (bool) - Se está aguardando aprovação</li>
          <li><code>isCompleted</code> (bool) - Se o workflow foi finalizado</li>
          <li><code>request</code> (object) - Dados originais da solicitação</li>
        </ul>
        <p class="mb-0"><strong>Exemplo de Uso:</strong> "Qual o status da solicitação [SESSION_ID]?"</p>
      </div>
    </div>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-cpu px-2"></i> Como o Workflow Funciona (Perspectiva do Cliente)
    </h5>
    <ol>
      <li class="mb-2">
        <strong>Solicitação:</strong> Cliente envia pedido de desconto através do agente conversacional.
      </li>
      <li class="mb-2">
        <strong>Validação:</strong> Agente confirma que todos os dados necessários foram fornecidos.
      </li>
      <li class="mb-2">
        <strong>Chamada MCP:</strong> Tool RequestDiscount é acionada com todos os parâmetros.
      </li>
      <li class="mb-2">
        <strong>Workflow Inicia:</strong> Servidor MCP cria um workflow em background.
      </li>
      <li class="mb-2">
        <strong>Decisão Automática:</strong>
        <ul>
          <li>Se desconto < 10%: ✅ Aprovado automaticamente (resposta imediata)</li>
          <li>Se desconto ≥ 10%: ⏸️ Workflow pausa e aguarda Human-in-the-Loop</li>
        </ul>
      </li>
      <li class="mb-2">
        <strong>Resposta Inicial:</strong> Cliente recebe Session ID e status (APPROVED ou PENDING_APPROVAL).
      </li>
      <li class="mb-2">
        <strong>Acompanhamento:</strong> Cliente pode usar GetDiscountStatus para verificar se a aprovação manual foi concedida.
      </li>
    </ol>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-arrow-left-right px-2"></i> Correlação com Página de Gerenciamento (Manager)
    </h5>
    <p>
      Esta página está <strong>diretamente conectada</strong> com a <strong>DiscountManagerView</strong> (página de admin):
    </p>
    <ul>
      <li><strong>Cliente solicita</strong> (aqui) → <strong>Gerente aprova/rejeita</strong> (DiscountManagerView)</li>
      <li><strong>RequestDiscount</strong> (cliente) cria sessão → <strong>GetPendingApprovals</strong> (gerente) lista sessões pendentes</li>
      <li><strong>GetDiscountStatus</strong> (cliente) verifica status → <strong>DecideApproval</strong> (gerente) altera status</li>
      <li>Ambas as páginas usam o <strong>mesmo servidor MCP</strong> (DiscountMcp) com tools diferentes</li>
      <li>Workflow compartilhado: sessões criadas aqui são processadas pelo gerente</li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bg-lightning-charge px-2"></i> Exemplo de Fluxo Completo
    </h5>
    <div class="bg-light p-4 rounded">
      <p class="mb-2"><strong>Cenário:</strong> Cliente quer 15% de desconto em um produto de R$ 1.000,00</p>
      <ol class="mb-0 text-dark">
        <li><strong>Cliente:</strong> "Quero solicitar 15% de desconto no produto X de R$ 1000"</li>
        <li><strong>Agente Cliente:</strong> Coleta informações faltantes (código do cliente, motivo)</li>
        <li><strong>Agente Cliente:</strong> Chama RequestDiscount(productName="X", originalPrice=1000, requestedDiscount=0.15, ...)</li>
        <li><strong>Workflow:</strong> Identifica que 15% ≥ 10% → Requer aprovação manual</li>
        <li><strong>Resposta:</strong> "⏳ Sua solicitação [SESSION_ABC123] está aguardando aprovação do gerente"</li>
        <li><strong>Gerente (outro sistema):</strong> Vê a solicitação na lista de aprovações pendentes</li>
        <li><strong>Gerente:</strong> Aprova com comentário "Cliente fidelizado, aprovado"</li>
        <li><strong>Workflow:</strong> Continua e finaliza com status APPROVED</li>
        <li><strong>Cliente:</strong> Consulta status → "✅ Sua solicitação foi aprovada! Preço final: R$ 850,00"</li>
      </ol>
    </div>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-lightning-charge px-2"></i> Vantagens do Workflow HITL
    </h5>
    <ul>
      <li>✅ <strong>Agilidade:</strong> Aprovações automáticas para valores baixos</li>
      <li>✅ <strong>Controle:</strong> Aprovação manual obrigatória para descontos significativos</li>
      <li>✅ <strong>Transparência:</strong> Cliente sabe imediatamente se precisa aguardar</li>
      <li>✅ <strong>Rastreabilidade:</strong> Session ID permite acompanhamento completo</li>
      <li>✅ <strong>Auditoria:</strong> Todas as decisões ficam registradas no workflow</li>
      <li>✅ <strong>Flexibilidade:</strong> Regras de aprovação podem ser ajustadas facilmente</li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-gear px-2"></i> Personalização
    </h5>
    <p>
      O agente permite ajustar suas instruções através do botão <strong>"Instruções"</strong>, possibilitando:
    </p>
    <ul>
      <li>Alterar o tom e forma de coleta de informações</li>
      <li>Personalizar mensagens de feedback ao cliente</li>
      <li>Ajustar validações de dados antes de enviar ao MCP</li>
      <li>Definir comportamentos para diferentes cenários de resposta</li>
    </ul>

    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h">
      <i class="bi bi-bullseye px-2"></i> Objetivo
    </h5>
    <p>
      O objetivo desta página é <strong>simplificar e democratizar o processo de solicitação de descontos</strong>, oferecendo uma experiência conversacional intuitiva que elimina formulários complexos, enquanto mantém controle empresarial através de aprovações obrigatórias para valores significativos.
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
    :feature-id="7"
    title="Agente de Solicitação de Descontos"
    welcome-message="👋 Olá! Sou o Agente de Solicitação de Descontos. Posso ajudá-lo a solicitar um desconto ou consultar o status de uma solicitação anterior. Como posso ajudá-lo?"
    default-agent-name="Agente de Solicitação de Descontos"
    default-instructions="Você é um assistente virtual especializado em solicitações de desconto para clientes da Contoso AutoTech.

RESPONSABILIDADES:
- Auxiliar clientes na solicitação de descontos em produtos
- Coletar todas as informações necessárias de forma conversacional e amigável
- Utilizar exclusivamente as ferramentas MCP disponíveis
- Fornecer feedback claro sobre o status da solicitação
- Manter um tom profissional, cordial e prestativo

INFORMAÇÕES NECESSÁRIAS PARA SOLICITAR DESCONTO:
Antes de chamar a ferramenta RequestDiscount, você DEVE coletar e validar:
1. Nome do produto
2. Preço original (valor numérico em R$)
3. Percentual de desconto desejado (pode ser em % ou valor decimal 0.0-1.0)
   - Se o cliente informar preço final desejado, calcule o percentual
   - Ex: Preço R$ 1000, quer pagar R$ 850 → desconto de 15% (0.15)
4. Código do cliente
5. Motivo/justificativa para o desconto

DIRETRIZES DE COMUNICAÇÃO:
1. Sempre confirme os dados antes de enviar a solicitação
2. Se alguma informação estiver faltando, pergunte de forma clara e natural
3. Converta percentuais informados como % para decimal (ex: 15% → 0.15)
4. Após solicitar, informe claramente:
   - O Session ID (para acompanhamento futuro)
   - Se foi aprovado automaticamente ou se aguarda aprovação
   - O preço final se aprovado
5. Para consulta de status, use GetDiscountStatus com o Session ID

FERRAMENTAS DISPONÍVEIS:
- RequestDiscount: Para criar nova solicitação de desconto (obrigatório: productName, originalPrice, requestedDiscount, customerCode, reason)
- GetDiscountStatus: Para verificar status de solicitação existente (obrigatório: sessionId)

REGRAS DE APROVAÇÃO:
- Descontos < 10%: Aprovados automaticamente
- Descontos ≥ 10%: Requerem aprovação do gerente (Human-in-the-Loop)

FORMATO DE RESPOSTA:
- Para solicitação bem-sucedida: Informar Session ID, status e próximos passos
- Para aprovação automática: Informar preço final e Session ID
- Para aprovação pendente: Informar que está aguardando gerente e como consultar status
- Para consulta de status: Informar status atual de forma clara

Responda sempre de maneira profissional, clara e organizada, priorizando a experiência do cliente."
  >
    <template #icon>
      <i class="bi bi-percent px-3"></i>
    </template>
  </AgentChatWindow>
</template>
