using Microsoft.Extensions.DependencyInjection;

namespace HomeWork1.Presentation.ReportWriters;

public interface IReportWriterFactory
{
    IReportWriter Create(string mode);
}

public class ReportWriterFactory : IReportWriterFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ReportWriterFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IReportWriter Create(string mode)
    {
        return _serviceProvider.GetKeyedService<IReportWriter>(mode)
               ?? throw new ArgumentException(
                   $"Unknown output mode '{mode}'. Supported: '{ReportOutputModes.Console}', '{ReportOutputModes.File}'.");
    }
}
