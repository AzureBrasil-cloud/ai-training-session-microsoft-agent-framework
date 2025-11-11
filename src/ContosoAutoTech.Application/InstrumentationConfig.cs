using System.Diagnostics;

namespace ContosoAutoTech.Application;

public class InstrumentationConfig
{
    private const string ServiceName = "ContosoAutoTech.Api";

    public static ActivitySource ActivitySource { get; set; } = new(ServiceName);

    public static string Service => ServiceName;
}