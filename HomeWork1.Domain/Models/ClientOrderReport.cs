namespace HomeWork1.Domain.Models;

public class ClientOrderReport
{
    public long ClientId { get; set; }
    public decimal OrdersSum { get; set; }
    public string FavoriteItemName { get; set; } = string.Empty;
}
