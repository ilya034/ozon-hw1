namespace HomeWork1.Domain.Entities;

public class OrderRecord
{
    public long ClientId { get; set; }
    public long OrderId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int ItemQuantity { get; set; }
    public decimal ItemPrice { get; set; }
}
