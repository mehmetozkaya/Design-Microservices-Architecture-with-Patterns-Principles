namespace Shared.Messaging.Events;
public record BasketCheckoutIntegrationEvent : IntegrationEvent
{
    public string UserName { get; set; } = default!;
    public decimal TotalPrice { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;
    public string AddressLine { get; set; } = default!;
}
