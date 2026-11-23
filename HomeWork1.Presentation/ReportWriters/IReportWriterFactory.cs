namespace HomeWork1.Presentation.ReportWriters;

public interface IReportWriterFactory
{
    IReportWriter Create(ReportOutputMode mode);
}
