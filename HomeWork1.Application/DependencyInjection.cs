using HomeWork1.Application.Abstractions;
using HomeWork1.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HomeWork1.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IOrderReportService, OrderReportService>();
        return services;
    }
}