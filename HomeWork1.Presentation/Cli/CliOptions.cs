using HomeWork1.Presentation.ReportWriters;

namespace HomeWork1.Presentation.Cli;

public sealed record CliOptions(ReportOutputMode OutputMode, IReadOnlyList<long> ClientIds);
