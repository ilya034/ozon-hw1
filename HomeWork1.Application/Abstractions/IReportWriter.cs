using HomeWork1.Domain.Models;

namespace HomeWork1.Application.Abstractions;

public interface IReportWriter
{
    void Write(IEnumerable<ClientOrderReport> reports);
}