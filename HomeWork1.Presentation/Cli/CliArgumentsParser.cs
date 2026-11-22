using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using HomeWork1.Presentation.ReportWriters;

namespace HomeWork1.Presentation.Cli;

public class CliArgumentsParser : ICliArgumentsParser
{
    private static readonly Dictionary<string, ReportOutputMode> Modes = new(StringComparer.OrdinalIgnoreCase)
    {
        ["console"] = ReportOutputMode.Console,
        ["file"] = ReportOutputMode.File
    };

    public string Usage { get; } =
        $"Использование: HomeWork1 [{string.Join("|", Modes.Keys)}] <client-id>...{Environment.NewLine}" +
        "  Режим вывода необязателен, по умолчанию console.";

    public bool TryParse(
        IReadOnlyList<string> args,
        [NotNullWhen(true)] out CliArguments? arguments,
        [NotNullWhen(false)] out string? error)
    {
        arguments = null;

        var mode = ReportOutputMode.Console;
        IEnumerable<string> rawIds = args;

        if (args.Count > 0 && Modes.TryGetValue(args[0], out var parsedMode))
        {
            mode = parsedMode;
            rawIds = args.Skip(1);
        }

        var clientIds = new List<long>();

        foreach (var arg in rawIds)
        {
            if (!long.TryParse(arg, NumberStyles.Integer, CultureInfo.InvariantCulture, out var clientId))
            {
                error = $"Некорректный ID клиента: '{arg}'. Ожидается целое число.";
                return false;
            }

            clientIds.Add(clientId);
        }

        if (clientIds.Count == 0)
        {
            error = "Не указан ни один ID клиента.";
            return false;
        }

        arguments = new CliArguments(mode, clientIds);
        error = null;
        return true;
    }
}
