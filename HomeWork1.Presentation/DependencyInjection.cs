using HomeWork1.Presentation.ReportWriters;
using Microsoft.Extensions.DependencyInjection;

namespace HomeWork1.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, string reportOutputDirectory)
    {
        services.Configure<FileReportOptions>(options => options.OutputDirectory = reportOutputDirectory);
        services.AddKeyedTransient<IReportWriter, ConsoleReportWriter>(ReportOutputMode.Console);
        services.AddKeyedTransient<IReportWriter, FileReportWriter>(ReportOutputMode.File);
        services.AddTransient<IReportWriterFactory, ReportWriterFactory>();

        services.AddTransient<AppRunner>();
        return services;
    }
}
