namespace HomeWork1.Domain.Models;

public sealed record OrderRecord(
    long ClientId,
    long OrderId,
    string ItemName,
    int ItemQuantity,
    decimal ItemPrice);
