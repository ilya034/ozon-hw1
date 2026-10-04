using System.Globalization;

namespace HomeWork1.Domain.Models;

public class ClientOrderReport
{
    public long ClientId { get; set; }
    public decimal OrdersSum { get; set; }
    public string FavoriteItemName { get; set; } = string.Empty;

    public override string ToString() =>
        $"{ClientId} {OrdersSum.ToString("F2", CultureInfo.InvariantCulture)} {FavoriteItemName}";
}
