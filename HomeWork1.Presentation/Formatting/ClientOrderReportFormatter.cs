using System.Globalization;
using HomeWork1.Application.Models;

namespace HomeWork1.Presentation.Formatting;

public static class ClientOrderReportFormatter
{
    public static string Format(ClientOrderReport report)
    {
        return $"{report.ClientId} {report.OrdersSum.ToString("F2", CultureInfo.InvariantCulture)} {report.FavoriteItemName}";
    }
}
