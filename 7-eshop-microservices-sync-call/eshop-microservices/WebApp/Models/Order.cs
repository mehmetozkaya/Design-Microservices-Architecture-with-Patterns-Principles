namespace WebApp.Models;
public class Order
{
    public int Id { get; set; }
    public string UserName { get; set; } = default!;
    public decimal TotalPrice { get; set; }

    // Address    
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;
    public string AddressLine { get; set; } = default!;
}
