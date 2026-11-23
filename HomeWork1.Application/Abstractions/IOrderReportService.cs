using HomeWork1.Application.Models;

namespace HomeWork1.Application.Abstractions;

public interface IOrderReportService
{
    IReadOnlyCollection<ClientOrderReport> GetReportForCustomers(IEnumerable<long> customerIds);
}