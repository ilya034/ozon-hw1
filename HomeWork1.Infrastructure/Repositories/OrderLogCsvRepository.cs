using System.Globalization;
using HomeWork1.Application.Abstractions;
using HomeWork1.Domain.Models;
using Microsoft.Extensions.Options;

namespace HomeWork1.Infrastructure.Repositories;

public class OrderLogCsvRepository : IOrderLogRepository
{
    private readonly string _filePath;
    private readonly object _sync = new();

    private Dictionary<long, List<OrderRecord>>? _ordersByClientCached;
    private (DateTimeOffset LastWriteUtc, long Length) _stamp;

    public OrderLogCsvRepository(IOptions<OrderLogCsvOptions> options)
    {
        _filePath = options.Value.FilePath;
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
            if (string.IsNullOrWhiteSpace(line) ||
                line.StartsWith("client_id", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var columns = line.Split(',');
            if (columns.Length < 5)
            {
                continue;
            }

            var record = new OrderRecord(
                ClientId: long.Parse(columns[0], CultureInfo.InvariantCulture),
                OrderId: long.Parse(columns[1], CultureInfo.InvariantCulture),
                ItemName: columns[2],
                ItemQuantity: int.Parse(columns[3], CultureInfo.InvariantCulture),
                ItemPrice: decimal.Parse(columns[4], CultureInfo.InvariantCulture));

            if (!ordersByClient.TryGetValue(record.ClientId, out var clientOrders))
            {
                clientOrders = [];
                ordersByClient[record.ClientId] = clientOrders;
            }

            clientOrders.Add(record);
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
