namespace HomeWork1.Application.Models;

public sealed record ClientOrderReport(
    long ClientId,
    decimal OrdersSum,
    string FavoriteItemName);
