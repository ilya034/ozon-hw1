namespace HomeWork1.Domain.Entities;

public class ClientOrderReport
{
    public long ClientId { get; set; }
    public decimal OrdersSum { get; set; }
    public string FavoriteItemName { get; set; } = string.Empty;
}
