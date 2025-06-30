namespace Basket.Models;

public class OutboxMessage
{
    public Guid Id { get; set; } = default!;
    public string Type { get; set; } = default!;
    public string Content { get; set; } = default!;
    public DateTime OccuredOn { get; set; } = default!;
    public DateTime? ProcessedOn { get; set; } = default!;
}
