using HomeWork1.Application.Abstractions;
using HomeWork1.Presentation.ReportWriters;

namespace HomeWork1.Presentation;

public class AppRunner
{
    private readonly IOrderReportService _orderReportService;
    private readonly IReportWriterFactory _reportWriterFactory;

    public AppRunner(IOrderReportService orderReportService, IReportWriterFactory reportWriterFactory)
    {
        _orderReportService = orderReportService;
        _reportWriterFactory = reportWriterFactory;
    }

    public Task RunAsync()
    {
        var args = Environment.GetCommandLineArgs().Skip(1).ToArray();
        
        var mode = ReportOutputModes.Console; // Default
        var toSkip = 0;
        if (args.Length >= 2 && !long.TryParse(args[0], out _))
        {
            mode = args[0];
            toSkip = 1;
        }

        var customerIds = args.Skip(toSkip)
            .Select(ParseClientId)
            .ToArray();

        var writer = _reportWriterFactory.Create(mode);
        var reports = _orderReportService.GetReportForCustomers(customerIds);

        writer.Write(reports);
        return Task.CompletedTask;
    }

    private static long ParseClientId(string value)
    {
        if (!long.TryParse(value, out var clientId)) throw new ArgumentException($"Invalid client id: {value}");

        return clientId;
    }
}
