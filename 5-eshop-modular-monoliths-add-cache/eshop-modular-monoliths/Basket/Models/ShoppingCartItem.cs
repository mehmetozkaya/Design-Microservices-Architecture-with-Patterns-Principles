namespace Basket.Models;
public class ShoppingCartItem
{
    public int Id { get; set; }
    public int ShoppingCartId { get; set; }
    public int Quantity { get; set; } = default!;
    public string Color { get; set; } = default!;
    public int ProductId { get; set; } = default!;

    // will comes from Catalog module
    public decimal Price { get; set; } = default!;
    public string ProductName { get; set; } = default!;
}
