using HomeWork1.Domain.Models;
using HomeWork1.Presentation.Formatting;
using Microsoft.Extensions.Options;

namespace HomeWork1.Presentation.ReportWriters;

public class FileReportWriter : IReportWriter
{
    private readonly string _outputDirectory;

    public FileReportWriter(IOptions<FileReportOptions> options)
    {
        _outputDirectory = options.Value.OutputDirectory;
    }

    public void Write(IEnumerable<ClientOrderReport> reports)
    {
        var fileName = $"report_{DateTime.UtcNow:yyyyMMddHHmmssfff}.txt";
        var outputPath = Path.Combine(_outputDirectory, fileName);

        var lines = reports
            .Select(ClientOrderReportFormatter.Format)
            .ToArray();

        File.WriteAllLines(outputPath, lines);
        Console.WriteLine($"Report saved to: {outputPath}");
    }
}
