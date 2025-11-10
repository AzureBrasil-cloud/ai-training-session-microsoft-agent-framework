using ContosoAutoTech.Data;
using ContosoAutoTech.Infrastructure.Azure.DocumentIntelligence;
using ContosoAutoTech.Infrastructure.Azure.Speech;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AiInferenceService = ContosoAutoTech.Infrastructure.AiInference.AiInferenceService;
using SpeechService = ContosoAutoTech.Infrastructure.Speech.SpeechService;

namespace ContosoAutoTech.Application.Orders;

public partial class OrderService(
    AppDbContext context,
    IConfiguration configuration,
    DocumentIntelligenceService documentIntelligenceService,
    ILogger<OrderService> logger,
    AiInferenceService aiInferenceService,
    SpeechService speechService);
