namespace Basket.ApiClients;

public class OrderingApiClient(HttpClient httpClient)
{
    // create order
    public async Task<bool> CreateOrder(OrderDto orderDto)
    {
        var response = await httpClient.PostAsJsonAsync<OrderDto>("/orders", orderDto);
        return response.IsSuccessStatusCode;
    }
}

public class OrderDto
{    
    public string UserName { get; set; } = default!;
    public decimal TotalPrice { get; set; }

    // Address    
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;
    public string AddressLine { get; set; } = default!;
}
