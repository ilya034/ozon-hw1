using HomeWork1.Domain.Models;

namespace HomeWork1.Application.Abstractions;

public interface IOrderLogRepository
{
    bool TryGetClientOrders(long clientId, out IReadOnlyCollection<OrderRecord> orders);
}
