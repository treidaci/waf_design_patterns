using Shared.Models;

var builder = WebApplication.CreateBuilder(args);
var onboardingServiceBaseUrl = builder.Configuration["OnboardingService__BaseUrl"] ?? "http://onboarding-service";
builder.Services.AddHttpClient("OnboardingService", client =>
{
    client.BaseAddress = new Uri(onboardingServiceBaseUrl);
});



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/api/clients", async (IHttpClientFactory factory) =>
{
    var httpClient = factory.CreateClient("OnboardingService");
    var clients = await httpClient.GetFromJsonAsync<List<ClientOnboarding>>("clients");
    return Results.Ok(clients);
});

app.Run();