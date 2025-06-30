namespace WebApp.ApiClients;

public class BasketApiClient(HttpClient httpClient)
{
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
