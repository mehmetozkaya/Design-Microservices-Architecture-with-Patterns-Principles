namespace WebApp.ApiClients;

public class YarpApiClient(HttpClient httpClient)
{
    //// Catalog Endpoints
    public async Task<List<Product>> GetProducts()
    {
        var response = await httpClient.GetFromJsonAsync<List<Product>>($"/catalog-service/products");
        return response!;
    }
    public async Task<Product> GetProductById(int id)
    {
        var response = await httpClient.GetFromJsonAsync<Product>($"/catalog-service/products/{id}");
        return response!;
    }
    public async Task<List<Product>?> SearchProducts(string query)
    {
        return await httpClient.GetFromJsonAsync<List<Product>>($"/catalog-service/products/search/{query}");
    }

    //// Basket Endpoints
    public async Task<ShoppingCart> GetBasket(string userName)
    {
        var response = await httpClient.GetFromJsonAsync<ShoppingCart>($"/basket-service/basket/{userName}");
        return response!;
    }

    public async Task<ShoppingCart> UpdateBasket(ShoppingCart shoppingCart)
    {
        var response = await httpClient.PostAsJsonAsync($"/basket-service/basket", shoppingCart);
        response.EnsureSuccessStatusCode();
        var updatedBasket = await response.Content.ReadFromJsonAsync<ShoppingCart>();
        return updatedBasket!;
    }

    public async Task CheckoutBasket(BasketCheckout basketCheckout)
    {
        var response = await httpClient.PostAsJsonAsync($"/basket-service/basket/checkout", basketCheckout);
        response.EnsureSuccessStatusCode();
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

    //// Ordering Endpoints
    public async Task<List<Order>> GetAllOrders()
    {
        var response = await httpClient.GetFromJsonAsync<List<Order>>($"/ordering-service/orders");
        return response!;
    }

    public async Task<List<Order>> GetOrdersByUserName(string userName)
    {
        var response = await httpClient.GetFromJsonAsync<List<Order>>($"/ordering-service/orders/{userName}");
        return response!;
    }
}
