using var client = new HttpClient();
client.BaseAddress = new Uri("http://localhost:5026");
while (true)
{
    var response = await client.GetAsync("/external-data/");
    Console.WriteLine($"Status: {response.StatusCode}");
    Thread.Sleep(1000);
}