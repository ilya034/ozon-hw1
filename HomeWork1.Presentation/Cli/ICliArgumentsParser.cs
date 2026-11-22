using System.Diagnostics.CodeAnalysis;

namespace HomeWork1.Presentation.Cli;

public interface ICliArgumentsParser
{
    string Usage { get; }

    bool TryParse(
        IReadOnlyList<string> args,
        [NotNullWhen(true)] out CliArguments? arguments,
        [NotNullWhen(false)] out string? error);
}
