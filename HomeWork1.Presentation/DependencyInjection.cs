using HomeWork1.Presentation.ReportWriters;
using Microsoft.Extensions.DependencyInjection;

namespace HomeWork1.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, string reportOutputDirectory)
    {
        services.AddKeyedTransient<IReportWriter, ConsoleReportWriter>(ReportOutputModes.Console);
        services.AddKeyedTransient<IReportWriter>(ReportOutputModes.File,
            (_, _) => new FileReportWriter(reportOutputDirectory));
        services.AddTransient<IReportWriterFactory, ReportWriterFactory>();

        services.AddTransient<AppRunner>();
        return services;
    }
}
