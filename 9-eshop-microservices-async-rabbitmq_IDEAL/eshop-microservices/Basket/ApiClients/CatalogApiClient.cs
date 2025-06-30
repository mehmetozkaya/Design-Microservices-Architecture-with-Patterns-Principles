namespace Basket.ApiClients;

public class CatalogApiClient(HttpClient httpClient)
{
    public async Task<ProductDto> GetProductById(int id)
    {
        var response = await httpClient.GetFromJsonAsync<ProductDto>($"/products/{id}");
        return response!;
    }
}

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = default!;
}
