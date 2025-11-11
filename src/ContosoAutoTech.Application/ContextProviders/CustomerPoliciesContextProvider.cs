using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Data;

namespace ContosoAutoTech.Application.ContextProviders;

public class CustomerPoliciesContextProvider
{
    /// <summary>
    /// Creates a factory function for TextSearchProvider for Customer Relations Policies.
    /// This factory receives the context (SerializedState and JsonSerializerOptions) from the infrastructure layer.
    /// </summary>
    public static Func<ChatClientAgentOptions.AIContextProviderFactoryContext, AIContextProvider> CreateProviderFactory()
    {
        return ctx =>
        {
            TextSearchProviderOptions textSearchOptions = new()
            {
                SearchTime = TextSearchProviderOptions.TextSearchBehavior.BeforeAIInvoke
            };

            return new TextSearchProvider(
                SearchAdapter,
                ctx.SerializedState,
                ctx.JsonSerializerOptions,
                textSearchOptions);
        };
    }
    
    private static Task<IEnumerable<TextSearchProvider.TextSearchResult>> SearchAdapter(
        string query, 
        CancellationToken cancellationToken)
    {
        var results = new List<TextSearchProvider.TextSearchResult>();

        // Return and Refund Policy
        if (query.Contains("return", StringComparison.OrdinalIgnoreCase) || 
            query.Contains("refund", StringComparison.OrdinalIgnoreCase) ||
            query.Contains("devolução", StringComparison.OrdinalIgnoreCase) ||
            query.Contains("reembolso", StringComparison.OrdinalIgnoreCase))
        {
            results.Add(new TextSearchProvider.TextSearchResult
            {
                SourceName = "Política de Devoluções e Reembolsos",
                SourceLink = "https://contosoautotech.com/policies/returns",
                Text = "Os clientes podem devolver qualquer item dentro de 30 dias após a entrega. " +
                       "Os itens devem estar sem uso e incluir a embalagem original. " +
                       "Os reembolsos são emitidos no método de pagamento original dentro de 5 dias úteis após a inspeção. " +
                       "Para serviços automotivos, o prazo de devolução é de 7 dias, desde que não tenha havido instalação completa."
            });
        }

        // Warranty Policy
        if (query.Contains("warranty", StringComparison.OrdinalIgnoreCase) || 
            query.Contains("guarantee", StringComparison.OrdinalIgnoreCase) ||
            query.Contains("garantia", StringComparison.OrdinalIgnoreCase))
        {
            results.Add(new TextSearchProvider.TextSearchResult
            {
                SourceName = "Política de Garantia",
                SourceLink = "https://contosoautotech.com/policies/warranty",
                Text = "Todos os veículos vendidos pela Contoso AutoTech incluem garantia de fábrica. " +
                       "Veículos novos: 3 anos ou 100.000 km. Veículos seminovos: 1 ano ou 20.000 km. " +
                       "A garantia cobre defeitos de fabricação e problemas mecânicos não causados por uso inadequado. " +
                       "Manutenções regulares em concessionárias autorizadas são obrigatórias para manter a garantia válida."
            });
        }

        // Customer Service Policy
        if (query.Contains("service", StringComparison.OrdinalIgnoreCase) || 
            query.Contains("support", StringComparison.OrdinalIgnoreCase) ||
            query.Contains("complaint", StringComparison.OrdinalIgnoreCase) ||
            query.Contains("atendimento", StringComparison.OrdinalIgnoreCase) ||
            query.Contains("reclamação", StringComparison.OrdinalIgnoreCase))
        {
            results.Add(new TextSearchProvider.TextSearchResult
            {
                SourceName = "Política de Atendimento ao Cliente",
                SourceLink = "https://contosoautotech.com/policies/customer-service",
                Text = "A Contoso AutoTech oferece atendimento ao cliente 24/7 através de telefone, email e chat online. " +
                       "Todas as reclamações são analisadas em até 48 horas úteis. " +
                       "Clientes têm direito a acompanhar o status de suas solicitações através do portal online. " +
                       "Em caso de insatisfação, é garantido o direito de escalonamento para um supervisor."
            });
        }

        // Exchange Policy
        if (query.Contains("exchange", StringComparison.OrdinalIgnoreCase) || 
            query.Contains("replacement", StringComparison.OrdinalIgnoreCase) ||
            query.Contains("troca", StringComparison.OrdinalIgnoreCase))
        {
            results.Add(new TextSearchProvider.TextSearchResult
            {
                SourceName = "Política de Trocas",
                SourceLink = "https://contosoautotech.com/policies/exchange",
                Text = "Peças e acessórios podem ser trocados dentro de 15 dias, mediante apresentação da nota fiscal. " +
                       "A troca está sujeita à disponibilidade em estoque. " +
                       "Produtos com defeito de fabricação podem ser trocados independente do prazo, mediante análise técnica. " +
                       "Para veículos, a troca só é permitida em caso de vícios ocultos identificados nos primeiros 90 dias."
            });
        }

        // Privacy and Data Protection Policy
        if (query.Contains("privacy", StringComparison.OrdinalIgnoreCase) || 
            query.Contains("data", StringComparison.OrdinalIgnoreCase) ||
            query.Contains("privacidade", StringComparison.OrdinalIgnoreCase) ||
            query.Contains("dados", StringComparison.OrdinalIgnoreCase) ||
            query.Contains("lgpd", StringComparison.OrdinalIgnoreCase))
        {
            results.Add(new TextSearchProvider.TextSearchResult
            {
                SourceName = "Política de Privacidade e Proteção de Dados",
                SourceLink = "https://contosoautotech.com/policies/privacy",
                Text = "A Contoso AutoTech está em conformidade com a LGPD (Lei Geral de Proteção de Dados). " +
                       "Dados pessoais são coletados apenas para fins de vendas e atendimento. " +
                       "Clientes podem solicitar acesso, correção ou exclusão de seus dados a qualquer momento. " +
                       "Dados não são compartilhados com terceiros sem consentimento explícito."
            });
        }

        // Delivery and Shipping Policy
        if (query.Contains("delivery", StringComparison.OrdinalIgnoreCase) || 
            query.Contains("shipping", StringComparison.OrdinalIgnoreCase) ||
            query.Contains("entrega", StringComparison.OrdinalIgnoreCase) ||
            query.Contains("frete", StringComparison.OrdinalIgnoreCase))
        {
            results.Add(new TextSearchProvider.TextSearchResult
            {
                SourceName = "Política de Entrega",
                SourceLink = "https://contosoautotech.com/policies/delivery",
                Text = "Peças e acessórios: entrega em até 7 dias úteis para capitais e 15 dias para demais regiões. " +
                       "Veículos: entrega agendada em até 30 dias após aprovação do financiamento. " +
                       "Frete grátis para compras acima de R$ 500,00. " +
                       "Cliente é notificado por SMS e email em todas as etapas da entrega."
            });
        }

        return Task.FromResult<IEnumerable<TextSearchProvider.TextSearchResult>>(results);
    }
}

