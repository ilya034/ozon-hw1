using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using HomeWork1.Presentation.ReportWriters;

namespace HomeWork1.Presentation.Cli;

public static class CliArgumentsParser
{
    public static string Usage { get; } =
        $"Использование: HomeWork1 [{string.Join("|", ReportOutputModes.All)}] <client-id>...{Environment.NewLine}" +
        $"  Режим вывода необязателен, по умолчанию {ReportOutputModes.Console}.";

    public static bool TryParse(
        IReadOnlyList<string> args,
        [NotNullWhen(true)] out CliOptions? options,
        [NotNullWhen(false)] out string? error)
    {
        options = null;

        var mode = ReportOutputModes.Console;
        var idsStart = 0;

        if (args.Count > 0 && !TryParseClientId(args[0], out _))
        {
            mode = args[0];
            idsStart = 1;

            if (!ReportOutputModes.All.Contains(mode))
            {
                error = $"Неизвестный режим вывода '{mode}'. Допустимые: {string.Join(", ", ReportOutputModes.All)}.";
                return false;
            }
        }

        if (args.Count == idsStart)
        {
            error = "Не указан ни один ID клиента.";
            return false;
        }

        var clientIds = new List<long>(args.Count - idsStart);
        for (var i = idsStart; i < args.Count; i++)
        {
            if (!TryParseClientId(args[i], out var clientId))
            {
                error = $"Некорректный ID клиента: '{args[i]}'. Ожидается целое число.";
                return false;
            }

            clientIds.Add(clientId);
        }

        options = new CliOptions(mode, clientIds);
        error = null;
        return true;
    }

    private static bool TryParseClientId(string value, out long clientId)
    {
        return long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out clientId);
    }
}
