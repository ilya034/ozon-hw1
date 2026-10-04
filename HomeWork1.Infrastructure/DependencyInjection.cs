using HomeWork1.Application.Abstractions;
using HomeWork1.Infrastructure.FileSystem;
using HomeWork1.Infrastructure.ReportWriter;
using HomeWork1.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HomeWork1.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        var csvPath = PathLocator.FindFileUpwards("orders_log.csv")
                      ?? throw new FileNotFoundException("orders_log.csv not found");

        services.AddSingleton<IOrderLogRepository>(new OrderLogCsvRepository(csvPath));
        services.AddTransient<IReportWriter, ConsoleReportWriter>();
        services.AddTransient<IReportWriter>(_ => new FileReportWriter(Path.GetDirectoryName(csvPath)!));
        return services;
    }
}