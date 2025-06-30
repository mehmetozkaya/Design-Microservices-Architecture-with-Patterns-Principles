
const int requestCount = 10;
var tasks = new List<Task>();
var httpClient = new HttpClient
{
    BaseAddress = new Uri("https://localhost:7029/")
};

for (int i = 0; i < requestCount; i++)
{
    tasks.Add(SendRequest(httpClient, i));
}

await Task.WhenAll(tasks);
Console.WriteLine("All requests completed.");

static async Task SendRequest(HttpClient httpClient, int requestId)
{
    try
    {
        var response = await httpClient.GetAsync("/products");
        Console.WriteLine($"Request {requestId}: {response.StatusCode}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Request {requestId} failed: {ex.Message}");
    }
}