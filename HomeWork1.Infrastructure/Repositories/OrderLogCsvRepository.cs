using System.Globalization;
using HomeWork1.Application.Abstractions;
using HomeWork1.Domain.Models;

namespace HomeWork1.Infrastructure.Repositories;

/// <summary>
/// Была попытка в потокобезопасность с возможным заделом "на будущее" - 
/// - запись в файл и поддержка нескольких инстансов приложения
/// Но конце концов стало выглядеть как оверинжиниринг и изобретение велосипеда,
/// поэтому осталось как есть 
/// </summary>
public class OrderLogCsvRepository : IOrderLogRepository
{
    private readonly string _filePath;
    private readonly object _sync = new();

    /// <summary>
    /// Кэш заказов по клиентам, чтобы не читать файл сильно часто
    /// </summary>
    private Dictionary<long, List<OrderRecord>>? _ordersByClientCached;

    /// <summary>
    /// Хранит штамп файла, чтобы понимать, когда нужно пересобирать кэш.
    /// </summary>
    private (DateTimeOffset LastWriteUtc, long Length) _stamp;

    public OrderLogCsvRepository(string filePath)
    {
        _filePath = filePath;
    }

    public bool TryGetClientOrders(long clientId, out IReadOnlyCollection<OrderRecord> orders)
    {
        lock (_sync)
        {
            if (IsIndexStale())
            {
                RebuildIndex();
            }

            if (_ordersByClientCached!.TryGetValue(clientId, out var clientOrders))
            {
                orders = clientOrders;
                return true;
            }

            orders = [];
            return false;
        }
    }

    private bool IsIndexStale()
    {
        return _ordersByClientCached is null || GetFileStamp() != _stamp;
    }

    private void RebuildIndex()
    {
        var stamp = GetFileStamp();

        var ordersByClient = new Dictionary<long, List<OrderRecord>>();
        foreach (var line in File.ReadLines(_filePath))
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("client_id", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var columns = line.Split(',');
            if (columns.Length < 5)
            {
                continue;
            }

            var record = new OrderRecord
            {
                ClientId = long.Parse(columns[0], CultureInfo.InvariantCulture),
                OrderId = long.Parse(columns[1], CultureInfo.InvariantCulture),
                ItemName = columns[2],
                ItemQuantity = int.Parse(columns[3], CultureInfo.InvariantCulture),
                ItemPrice = decimal.Parse(columns[4], CultureInfo.InvariantCulture)
            };

            if (!ordersByClient.TryGetValue(record.ClientId, out var clientOrders))
            {
                ordersByClient[record.ClientId] = [];
            }

            ordersByClient[record.ClientId].Add(record);
        }

        _ordersByClientCached = ordersByClient;
        _stamp = stamp;
    }

    private (DateTimeOffset LastWriteUtc, long Length) GetFileStamp()
    {
        var info = new FileInfo(_filePath);
        return (info.LastWriteTimeUtc, info.Length);
    }

}