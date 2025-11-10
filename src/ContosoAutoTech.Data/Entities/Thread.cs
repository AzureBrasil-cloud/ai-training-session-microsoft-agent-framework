using ContosoAutoTech.Data.Entities.Interfaces;

namespace ContosoAutoTech.Data.Entities;

public class Thread : IEntity
{
    public Guid Id { get; set; }
    public string State { get; set; } = null!;
    public Feature Feature { get; set; }
    public string? FirstTruncatedMessage { get; set; }

    // EF
    private Thread() { }

    public Thread(string state, Feature feature)
    {
        Id = Guid.NewGuid();
        State = state;
        Feature = feature;
        FirstTruncatedMessage = null;
    }
}

public enum Feature
{
    CarRegistration = 1
}