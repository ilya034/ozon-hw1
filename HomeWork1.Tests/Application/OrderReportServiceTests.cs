using HomeWork1.Application.Exceptions;
using HomeWork1.Application.Services;
using HomeWork1.Domain.Models;

namespace HomeWork1.Tests.Application;

public class OrderReportServiceTests
{
    [Fact]
    public void GetReportForCustomers_SingleClient_CalculatesSumAndFavoriteItem()
    {
        var repository = new FakeOrderLogRepository(new Dictionary<long, IReadOnlyCollection<OrderRecord>>
        {
            [1] =
            [
                new OrderRecord(1, 1, "Огурчики маринованные", 1, 100m),
                new OrderRecord(1, 1, "Чипсеки", 4, 99.99m)
            ]
        });
        var orderReportService = new OrderReportService(repository);

        var reports = orderReportService.GetReportForCustomers([1]);

        var report = Assert.Single(reports);
        Assert.Equal(1, report.ClientId);
        Assert.Equal(499.96m, report.OrdersSum);
        Assert.Equal("Чипсеки", report.FavoriteItemName);
    }

    [Fact]
    public void GetReportForCustomers_MultipleOrders_FavoriteItemChosenByTotalQuantity()
    {
        var repository = new FakeOrderLogRepository(new Dictionary<long, IReadOnlyCollection<OrderRecord>>
        {
            [1] =
            [
                new OrderRecord(1, 1, "A", 10, 5m),
                new OrderRecord(1, 2, "B", 2, 10m),
                new OrderRecord(1, 3, "B", 2, 10m),
            ]
        });
        var orderReportService = new OrderReportService(repository);

        var reports = orderReportService.GetReportForCustomers([1]);

        var report = Assert.Single(reports);
        Assert.Equal("A", report.FavoriteItemName);
        Assert.Equal(90m, report.OrdersSum);
    }

    [Fact]
    public void GetReportForCustomers_MultipleOrders_SumsQuantitiesOfSameItem()
    {
        var repository = new FakeOrderLogRepository(new Dictionary<long, IReadOnlyCollection<OrderRecord>>
        {
            [1] =
            [
                new OrderRecord(1, 1, "A", 2, 10m),
                new OrderRecord(1, 2, "B", 1, 10m),
                new OrderRecord(1, 3, "A", 2, 10m)
            ]
        });
        var orderReportService = new OrderReportService(repository);

        var reports = orderReportService.GetReportForCustomers([1]);

        var report = Assert.Single(reports);
        Assert.Equal("A", report.FavoriteItemName);
        Assert.Equal(50m, report.OrdersSum);
    }

    [Fact]
    public void GetReportForCustomers_MultipleItems_ReturnsOneOfTopFavoriteItems()
    {
        var repository = new FakeOrderLogRepository(new Dictionary<long, IReadOnlyCollection<OrderRecord>>
        {
            [1] =
            [
                new OrderRecord(1, 1, "A", 5, 10m),
                new OrderRecord(1, 2, "B", 5, 20m),
                new OrderRecord(1, 3, "C", 1, 100m)
            ]
        });
        var orderReportService = new OrderReportService(repository);

        var reports = orderReportService.GetReportForCustomers([1]);

        var report = Assert.Single(reports);
        Assert.Contains(report.FavoriteItemName, new[] { "A", "B" });
        Assert.Equal(250m, report.OrdersSum);
    }

    [Fact]
    public void GetReportForCustomers_MultipleClients_ReturnsReportsInRequestedOrder()
    {
        var repository = new FakeOrderLogRepository(new Dictionary<long, IReadOnlyCollection<OrderRecord>>
        {
            [1] = [new OrderRecord(1, 1, "A", 4, 99.99m)],
            [2] = [new OrderRecord(2, 2, "B", 20, 33m)]
        });
        var orderReportService = new OrderReportService(repository);

        var reports = orderReportService.GetReportForCustomers([2, 1]).ToArray();

        Assert.Equal(2, reports.Length);
        Assert.Equal(2, reports[0].ClientId);
        Assert.Equal(660m, reports[0].OrdersSum);
        Assert.Equal("B", reports[0].FavoriteItemName);

        Assert.Equal(1, reports[1].ClientId);
        Assert.Equal(399.96m, reports[1].OrdersSum);
        Assert.Equal("A", reports[1].FavoriteItemName);
    }

    [Fact]
    public void GetReportForCustomers_DuplicateClientIds_Deduplicates()
    {
        var repository = new FakeOrderLogRepository(new Dictionary<long, IReadOnlyCollection<OrderRecord>>
        {
            [1] = [new OrderRecord(1, 1, "A", 1, 10m)],
            [2] = [new OrderRecord(2, 2, "B", 2, 20m)]
        });
        var orderReportService = new OrderReportService(repository);

        var reports = orderReportService.GetReportForCustomers([1, 2, 1, 2]).ToArray();

        Assert.Equal([1, 2], reports.Select(r => r.ClientId));
    }

    [Fact]
    public void GetReportForCustomers_UnknownClient_ThrowsClientNotFoundException()
    {
        var repository = new FakeOrderLogRepository(new Dictionary<long, IReadOnlyCollection<OrderRecord>>());
        var orderReportService = new OrderReportService(repository);

        var exception = Assert.Throws<ClientNotFoundException>(() => orderReportService.GetReportForCustomers([0]));

        Assert.Equal(0, exception.ClientId);
    }

    [Fact]
    public void GetReportForCustomers_MissingClients_ThrowsClientNotFoundException()
    {
        var repository = new FakeOrderLogRepository(new Dictionary<long, IReadOnlyCollection<OrderRecord>>
        {
            [1] = [new OrderRecord(1, 1, "Чипсеки", 1, 10m)]
        });
        var orderReportService = new OrderReportService(repository);

        var exception = Assert.Throws<ClientNotFoundException>(() => orderReportService.GetReportForCustomers([1, 0]));

        Assert.Equal(0, exception.ClientId);
    }

    [Fact]
    public void GetReportForCustomers_EmptyClientIds_ReturnsEmpty()
    {
        var repository = new FakeOrderLogRepository(new Dictionary<long, IReadOnlyCollection<OrderRecord>>
        {
            [1] = [new OrderRecord(1, 1, "Чипсеки", 1, 10m)]
        });
        var orderReportService = new OrderReportService(repository);

        var reports = orderReportService.GetReportForCustomers([]);

        Assert.Empty(reports);
    }
}
