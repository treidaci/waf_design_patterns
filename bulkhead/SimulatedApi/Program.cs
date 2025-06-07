// File: Program.cs
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/delay/2", async () =>
{
    await Task.Delay(2000);
    return Results.Ok("Simulated delay response");
});

app.MapGet("/status/200", () => Results.Ok("Simulated 200 OK"));

app.Run();