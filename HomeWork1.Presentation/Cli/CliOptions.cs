namespace HomeWork1.Presentation.Cli;

public sealed record CliOptions(string OutputMode, IReadOnlyList<long> ClientIds);
