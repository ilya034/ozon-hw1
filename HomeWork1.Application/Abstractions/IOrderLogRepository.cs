using HomeWork1.Domain.Entities;

namespace HomeWork1.Application.Abstractions;

public interface IOrderLogRepository
{
    bool TryGetClientOrders(long clientId, out IReadOnlyCollection<OrderRecord> orders);
}
