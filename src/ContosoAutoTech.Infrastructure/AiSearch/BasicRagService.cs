using ContosoAutoTech.Infrastructure.Shared;
using Microsoft.Extensions.Logging;

namespace ContosoAutoTech.Infrastructure.AiSearch;

public class BasicRagService(ILogger<BasicRagService> logger)
{
    public Task<IEnumerable<SearchResult>> SearchCustomerPoliciesAsync(
        string query, 
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Searching customer policies with query: {Query}", query);

        var results = new List<SearchResult>();
        var queryLower = query.ToLowerInvariant();

        foreach (var policy in InitializePolicyDocuments()
                     .Where(policy => policy.Keywords.Any(keyword => queryLower.Contains(keyword, StringComparison.InvariantCultureIgnoreCase))))
        {
            results.Add(new SearchResult
            {
                SourceName = policy.SourceName,
                SourceLink = policy.SourceName,
                Text = policy.Text
            });

            logger.LogDebug("Matched policy: {PolicyName}", policy.SourceName);
        }

        logger.LogInformation("Found {Count} matching policies", results.Count);
        return Task.FromResult<IEnumerable<SearchResult>>(results);
    }
    
    private List<PolicyDocument> InitializePolicyDocuments()
    {
        return new List<PolicyDocument>
        {
            new PolicyDocument
            {
                SourceName = "politica-devolucoes-reembolsos.pdf",
                Text = "Os clientes podem devolver qualquer item dentro de 30 dias após a entrega. " +
                       "Os itens devem estar sem uso e incluir a embalagem original. " +
                       "Os reembolsos são emitidos no método de pagamento original dentro de 5 dias úteis após a inspeção. " +
                       "Para serviços automotivos, o prazo de devolução é de 7 dias, desde que não tenha havido instalação completa.",
                Keywords = new[] { "return", "refund", "devolução", "reembolso", "devolver" }
            },
            new PolicyDocument
            {
                SourceName = "politica-garantia.pdf",
                Text = "Todos os veículos vendidos pela Contoso AutoTech incluem garantia de fábrica. " +
                       "Veículos novos: 3 anos ou 100.000 km. Veículos seminovos: 1 ano ou 20.000 km. " +
                       "A garantia cobre defeitos de fabricação e problemas mecânicos não causados por uso inadequado. " +
                       "Manutenções regulares em concessionárias autorizadas são obrigatórias para manter a garantia válida.",
                Keywords = new[] { "warranty", "guarantee", "garantia" }
            },
            new PolicyDocument
            {
                SourceName = "politica-atendimento-cliente.pdf",
                Text = "A Contoso AutoTech oferece atendimento ao cliente 24/7 através de telefone, email e chat online. " +
                       "Todas as reclamações são analisadas em até 48 horas úteis. " +
                       "Clientes têm direito a acompanhar o status de suas solicitações através do portal online. " +
                       "Em caso de insatisfação, é garantido o direito de escalonamento para um supervisor.",
                Keywords = new[] { "service", "support", "complaint", "atendimento", "reclamação", "suporte" }
            },
            new PolicyDocument
            {
                SourceName = "politica-trocas.pdf",
                Text = "Peças e acessórios podem ser trocados dentro de 15 dias, mediante apresentação da nota fiscal. " +
                       "A troca está sujeita à disponibilidade em estoque. " +
                       "Produtos com defeito de fabricação podem ser trocados independente do prazo, mediante análise técnica. " +
                       "Para veículos, a troca só é permitida em caso de vícios ocultos identificados nos primeiros 90 dias.",
                Keywords = new[] { "exchange", "replacement", "troca", "trocar" }
            },
            new PolicyDocument
            {
                SourceName = "politica-privacidade-lgpd.pdf",
                Text = "A Contoso AutoTech está em conformidade com a LGPD (Lei Geral de Proteção de Dados). " +
                       "Dados pessoais são coletados apenas para fins de vendas e atendimento. " +
                       "Clientes podem solicitar acesso, correção ou exclusão de seus dados a qualquer momento. " +
                       "Dados não são compartilhados com terceiros sem consentimento explícito.",
                Keywords = new[] { "privacy", "data", "privacidade", "dados", "lgpd", "proteção" }
            },
            new PolicyDocument
            {
                SourceName = "politica-entrega.pdf",
                Text = "Peças e acessórios: entrega em até 7 dias úteis para capitais e 15 dias para demais regiões. " +
                       "Veículos: entrega agendada em até 30 dias após aprovação do financiamento. " +
                       "Frete grátis para compras acima de R$ 500,00. " +
                       "Cliente é notificado por SMS e email em todas as etapas da entrega.",
                Keywords = new[] { "delivery", "shipping", "entrega", "frete", "envio" }
            },
            new PolicyDocument
            {
                SourceName = "politica-financiamento.pdf",
                Text = "A Contoso AutoTech oferece opções de financiamento com taxas competitivas. " +
                       "Financiamento disponível em até 60 meses para veículos novos e 48 meses para seminovos. " +
                       "Entrada mínima de 20% do valor do veículo. " +
                       "Aprovação de crédito sujeita à análise. Simulações podem ser feitas online ou presencialmente.",
                Keywords = new[] { "financing", "financiamento", "crédito", "parcela", "prestação", "entrada" }
            },
            new PolicyDocument
            {
                SourceName = "politica-manutencao-preventiva.pdf",
                Text = "Recomendamos manutenção preventiva a cada 10.000 km ou 6 meses, o que ocorrer primeiro. " +
                       "Manutenções incluem troca de óleo, filtros, verificação de freios e suspensão. " +
                       "Clientes com plano de manutenção têm desconto de 15% em peças e serviços. " +
                       "Agendamento pode ser feito através do app ou site.",
                Keywords = ["maintenance", "manutenção", "revisão", "preventiva", "óleo", "filtro"]
            }
        };
    }

    private class PolicyDocument
    {
        public string SourceName { get; set; } = string.Empty;
        public string SourceLink { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string[] Keywords { get; set; } = [];
    }
}

