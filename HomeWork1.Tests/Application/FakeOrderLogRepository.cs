using HomeWork1.Application.Abstractions;
using HomeWork1.Domain.Models;

namespace HomeWork1.Tests.Application;

public sealed class FakeOrderLogRepository : IOrderLogRepository
{
    private readonly Dictionary<long, IReadOnlyCollection<OrderRecord>> _ordersByClient;

    public FakeOrderLogRepository(Dictionary<long, IReadOnlyCollection<OrderRecord>> ordersByClient)
    {
        _ordersByClient = ordersByClient;
    }

    public bool TryGetClientOrders(long clientId, out IReadOnlyCollection<OrderRecord> orders)
    {
        if (_ordersByClient.TryGetValue(clientId, out var clientOrders))
        {
            orders = clientOrders;
            return true;
        }

        orders = [];
        return false;
    }
}