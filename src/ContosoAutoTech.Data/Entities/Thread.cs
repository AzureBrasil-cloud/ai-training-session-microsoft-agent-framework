using ContosoAutoTech.Data.Entities.Interfaces;

namespace ContosoAutoTech.Data.Entities;

public class Thread : IEntity
{
    public Guid Id { get; set; }
    public string State { get; set; } = null!;

    // EF
    private Thread() { }

    public Thread(string state)
    {
        Id = Guid.NewGuid();
        State = state;
    }
}