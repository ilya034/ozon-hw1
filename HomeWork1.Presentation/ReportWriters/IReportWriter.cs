using HomeWork1.Application.Models;

namespace HomeWork1.Presentation.ReportWriters;

public interface IReportWriter
{
    void Write(IEnumerable<ClientOrderReport> reports);
}
