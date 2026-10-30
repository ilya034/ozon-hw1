using Microsoft.Extensions.DependencyInjection;

namespace HomeWork1.Presentation.ReportWriters;

public interface IReportWriterFactory
{
    IReportWriter Create(ReportOutputMode mode);
}

public class ReportWriterFactory : IReportWriterFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ReportWriterFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IReportWriter Create(ReportOutputMode mode)
    {
        return _serviceProvider.GetKeyedService<IReportWriter>(mode)
               ?? throw new ArgumentOutOfRangeException(nameof(mode), mode,
                   $"No report writer registered for mode '{mode}'.");
    }
}
