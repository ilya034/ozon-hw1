using HomeWork1.Application.Abstractions;
using HomeWork1.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HomeWork1.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string orderLogPath)
    {
        services.Configure<OrderLogCsvOptions>(options => options.FilePath = orderLogPath);
        services.AddSingleton<IOrderLogRepository, OrderLogCsvRepository>();
        return services;
    }
}
