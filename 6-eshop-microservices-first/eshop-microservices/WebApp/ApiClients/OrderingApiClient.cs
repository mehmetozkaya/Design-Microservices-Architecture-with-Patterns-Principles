namespace WebApp.ApiClients;

public class OrderingApiClient(HttpClient httpClient)
{
    public async Task<List<Order>> GetAllOrders()
    {
        var response = await httpClient.GetFromJsonAsync<List<Order>>($"/orders");
        return response!;
    }

    public async Task<List<Order>> GetOrdersByUserName(string userName)
    {
        var response = await httpClient.GetFromJsonAsync<List<Order>>($"/orders/{userName}");
        return response!;
    }
}
