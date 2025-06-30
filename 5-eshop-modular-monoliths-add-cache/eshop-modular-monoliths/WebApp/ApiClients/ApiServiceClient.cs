using Basket.Models;
using Catalog.Models;
using Ordering.Models;

namespace WebApp.ApiClients;

public class ApiServiceClient(HttpClient httpClient)
{
    // Product Endpoints
    public async Task<List<Product>> GetProducts()
    {
        var response = await httpClient.GetFromJsonAsync<List<Product>>($"/products");
        return response!;
    }

    public async Task<Product> GetProductById(int id)
    {
        var response = await httpClient.GetFromJsonAsync<Product>($"/products/{id}");
        return response!;
    }

    // Basket Endpoints
    public async Task<ShoppingCart> GetBasket(string userName)
    {
        var response = await httpClient.GetFromJsonAsync<ShoppingCart>($"/basket/{userName}");
        return response!;
    }

    public async Task<ShoppingCart> UpdateBasket(ShoppingCart shoppingCart)
    {
        var response = await httpClient.PostAsJsonAsync($"/basket", shoppingCart);
        response.EnsureSuccessStatusCode();
        var updatedBasket = await response.Content.ReadFromJsonAsync<ShoppingCart>();
        return updatedBasket!;
    }

    public async Task CheckoutBasket(BasketCheckout basketCheckout)
    {
        var response = await httpClient.PostAsJsonAsync($"/basket/checkout", basketCheckout);
        response.EnsureSuccessStatusCode();        
    }

    // Order Endpoints
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
