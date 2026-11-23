using HomeWork1.Application.Models;
using HomeWork1.Presentation.Formatting;

namespace HomeWork1.Presentation.ReportWriters;

public class ConsoleReportWriter : IReportWriter
{
    public void Write(IEnumerable<ClientOrderReport> reports)
    {
        foreach (var report in reports)
        {
            Console.WriteLine(ClientOrderReportFormatter.Format(report));
        }
    }
}
