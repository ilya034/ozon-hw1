using HomeWork1.Application.Abstractions;
using HomeWork1.Domain.Models;

namespace HomeWork1.Infrastructure.ReportWriter;

public class ConsoleReportWriter : IReportWriter
{
    public void Write(IEnumerable<ClientOrderReport> reports)
    {
        foreach (var report in reports)
        {
            Console.WriteLine(report);
        }
    }
}
