using ContosoAutoTech.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfigurations(builder.Configuration, builder.Environment);

builder.AddLogging();

var app = builder.Build();

await app.UseDatabaseSeeding();

app.ConfigureApplication();

app.Run();
