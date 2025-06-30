using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class Order
{
    public int Id { get; set; }
    public string UserName { get; set; } = default!;
    public decimal TotalPrice { get; set; }

    // Address
    [Required(ErrorMessage = "First Name is required.")]
    public string FirstName { get; set; } = default!;
    
    [Required(ErrorMessage = "Last Name is required.")]
    public string LastName { get; set; } = default!;
    
    [Required(ErrorMessage = "EmailAddress is required."), EmailAddress]    
    public string EmailAddress { get; set; } = default!;
    
    [Required(ErrorMessage = "AddressLine is required.")]
    public string AddressLine { get; set; } = default!;
}
