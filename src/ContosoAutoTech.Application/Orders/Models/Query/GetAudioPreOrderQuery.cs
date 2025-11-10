namespace ContosoAutoTech.Application.Orders.Models.Query;

public record GetAudioPreOrderQuery(Guid Id, bool ApplyAiTransformation = false);