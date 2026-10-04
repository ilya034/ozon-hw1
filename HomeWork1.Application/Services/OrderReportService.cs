using HomeWork1.Application.Abstractions;
using HomeWork1.Application.Exceptions;
using HomeWork1.Domain.Models;

namespace HomeWork1.Application.Services;

public class OrderReportService : IOrderReportService
{
    private readonly IOrderLogRepository _orderLogRepository;

    public OrderReportService(IOrderLogRepository orderLogRepository)
    {
        _orderLogRepository = orderLogRepository;
    }

    public IReadOnlyCollection<ClientOrderReport> GetReportForCustomers(IEnumerable<long> customerIds)
    {
        var reports = new List<ClientOrderReport>();

        foreach (var clientId in customerIds.Distinct())
        {
            if (!_orderLogRepository.TryGetClientOrders(clientId, out var orders))
                throw new ClientNotFoundException(clientId);

            reports.Add(BuildClientReport(clientId, orders));
        }

        return reports;
    }

    private static ClientOrderReport BuildClientReport(long clientId, IReadOnlyCollection<OrderRecord> orders)
    {
        return new ClientOrderReport
        {
            ClientId = clientId,
            OrdersSum = orders.Sum(order => order.ItemQuantity * order.ItemPrice),
            FavoriteItemName = orders
                .GroupBy(order => order.ItemName)
                .MaxBy(group => group.Sum(order => order.ItemQuantity))!
                .Key
        };
    }
}