using HomeWork1.Application.Abstractions;
using HomeWork1.Infrastructure.ReportWriter;
using Microsoft.VisualBasic.CompilerServices;

namespace HomeWork1.Presentation;

public class AppRunner
{
    private readonly IOrderReportService _orderReportService;
    private readonly IEnumerable<IReportWriter> _reportWriters;

    public AppRunner(IOrderReportService orderReportService, IEnumerable<IReportWriter> reportWriters)
    {
        _orderReportService = orderReportService;
        _reportWriters = reportWriters;
    }

    public Task RunAsync()
    {
        var args = Environment.GetCommandLineArgs().Skip(1).ToArray();
        
        var mode = "console"; // Default
        var toSkip = 0;
        if (args.Length >= 2 && !long.TryParse(args[0], out _))
        {
            mode = args[0];
            toSkip = 1;
        }

        var customerIds = args.Skip(toSkip)
            .Select(ParseClientId)
            .ToArray();

        var reports = _orderReportService.GetReportForCustomers(customerIds);
        IReportWriter writer = mode switch
        {
            "console" => _reportWriters.OfType<ConsoleReportWriter>().Single(),
            "file" => _reportWriters.OfType<FileReportWriter>().Single(),
            _ => throw new ArgumentException("Mode must be either 'console' or 'file'.")
        };

        writer.Write(reports);
        return Task.CompletedTask;
    }

    private static long ParseClientId(string value)
    {
        if (!long.TryParse(value, out var clientId)) throw new ArgumentException($"Invalid client id: {value}");

        return clientId;
    }
}