using ApiService.Models;

namespace WebApp.ApiClients;

public class ApiServiceClient(HttpClient httpClient)
{
    // Product Endpoints
    public async Task<List<Product>> GetProducts()
    {
        var response = await httpClient.GetFromJsonAsync<List<Product>>($"/apiservice/products");
        return response!;
    }

    public async Task<Product> GetProductById(int id)
    {
        var response = await httpClient.GetFromJsonAsync<Product>($"/apiservice/products/{id}");
        return response!;
    }

    // Basket Endpoints
    public async Task<ShoppingCart> GetBasket(string userName)
    {
        var response = await httpClient.GetFromJsonAsync<ShoppingCart>($"/apiservice/basket/{userName}");
        return response!;
    }

    public async Task<ShoppingCart> UpdateBasket(ShoppingCart shoppingCart)
    {
        var response = await httpClient.PostAsJsonAsync($"/apiservice/basket", shoppingCart);
        response.EnsureSuccessStatusCode();
        var updatedBasket = await response.Content.ReadFromJsonAsync<ShoppingCart>();
        return updatedBasket!;
    }

    public async Task<Order> CheckoutBasket(Order checkoutOrder)
    {
        var response = await httpClient.PostAsJsonAsync($"/apiservice/basket/checkout", checkoutOrder);
        response.EnsureSuccessStatusCode();
        var order = await response.Content.ReadFromJsonAsync<Order>();
        return order!;
    }

    // Order Endpoints
    public async Task<List<Order>> GetAllOrders()
    {
        var response = await httpClient.GetFromJsonAsync<List<Order>>($"/apiservice/orders");
        return response!;
    }

    public async Task<List<Order>> GetOrdersByUserName(string userName)
    {
        var response = await httpClient.GetFromJsonAsync<List<Order>>($"/apiservice/orders/{userName}");
        return response!;
    }

    public async Task<ShoppingCart> LoadUserBasket()
    {
        // Get Basket If Not Exist Create New Basket with Default Logged In User Name: swn
        var userName = "swn";
        ShoppingCart basket;

        try
        {
            basket = await GetBasket(userName);            
        }
        catch
        {
            basket = new ShoppingCart
            {
                UserName = userName,
                Items = []
            };
        }

        return basket;
    }
}
