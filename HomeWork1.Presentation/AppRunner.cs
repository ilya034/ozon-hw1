using HomeWork1.Application.Abstractions;
using HomeWork1.Application.Exceptions;
using HomeWork1.Presentation.Cli;
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

    public Task<int> RunAsync(IReadOnlyList<string> args)
    {
        if (!CliArgumentsParser.TryParse(args, out var options, out var error))
        {
            Console.Error.WriteLine(error);
            Console.Error.WriteLine(CliArgumentsParser.Usage);
            return Task.FromResult(1);
        }

        var writer = _reportWriterFactory.Create(options.OutputMode);

        try
        {
            var reports = _orderReportService.GetReportForCustomers(options.ClientIds);
            writer.Write(reports);
            return Task.FromResult(0);
        }
        catch (ClientNotFoundException ex)
        {
            Console.Error.WriteLine(ex.Message);
            return Task.FromResult(1);
        }
    }
}
