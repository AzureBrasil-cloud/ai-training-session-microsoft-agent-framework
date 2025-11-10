using ContosoAutoTech.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AiInferenceService = ContosoAutoTech.Infrastructure.AiInference.AiInferenceService;

namespace ContosoAutoTech.Application.Reviews;

public partial class ReviewService(
    AppDbContext context,
    IConfiguration configuration,
    ILogger<ReviewService> logger,
    AiInferenceService aiInferenceService);