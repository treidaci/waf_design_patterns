using Shared.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/clients", () =>
{
    var clients = new List<ClientOnboarding>
    {
        new() { Id = 1, FirstName = "Alice", LastName = "Smith", Email = "alice@example.com", CreatedAt = DateTime.UtcNow.AddDays(-10), IsVerified = true },
        new() { Id = 2, FirstName = "Bob", LastName = "Jones", Email = "bob@example.com", CreatedAt = DateTime.UtcNow.AddDays(-5), IsVerified = false },
    };

    return Results.Ok(clients);
});

app.Run();