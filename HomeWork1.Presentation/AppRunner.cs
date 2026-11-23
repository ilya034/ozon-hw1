using HomeWork1.Application.Abstractions;
using HomeWork1.Presentation.Cli;
using HomeWork1.Presentation.ReportWriters;

namespace HomeWork1.Presentation;

public class AppRunner
{
    private readonly ICliArgumentsParser _cliArgumentsParser;
    private readonly IOrderReportService _orderReportService;
    private readonly IReportWriterFactory _reportWriterFactory;

    public AppRunner(
        ICliArgumentsParser cliArgumentsParser,
        IOrderReportService orderReportService,
        IReportWriterFactory reportWriterFactory)
    {
        _cliArgumentsParser = cliArgumentsParser;
        _orderReportService = orderReportService;
        _reportWriterFactory = reportWriterFactory;
    }

    public Task<int> RunAsync(IReadOnlyList<string> args)
    {
        if (!_cliArgumentsParser.TryParse(args, out var arguments, out var error))
        {
            Console.Error.WriteLine(error);
            Console.Error.WriteLine(_cliArgumentsParser.Usage);
            return Task.FromResult(1);
        }

        var writer = _reportWriterFactory.Create(arguments.OutputMode);
        var reports = _orderReportService.GetReportForCustomers(arguments.ClientIds);

        writer.Write(reports);
        return Task.FromResult(0);
    }
}
