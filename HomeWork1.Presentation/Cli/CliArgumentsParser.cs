using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using HomeWork1.Presentation.ReportWriters;

namespace HomeWork1.Presentation.Cli;

public static class CliArgumentsParser
{
    private static readonly string[] SupportedModes =
        Enum.GetNames<ReportOutputMode>()
            .Select(name => name.ToLowerInvariant())
            .ToArray();

    public static string Usage { get; } =
        $"Использование: HomeWork1 [{string.Join("|", SupportedModes)}] <client-id>...{Environment.NewLine}" +
        $"  Режим вывода необязателен, по умолчанию {nameof(ReportOutputMode.Console).ToLowerInvariant()}.";

    public static bool TryParse(
        IReadOnlyList<string> args,
        [NotNullWhen(true)] out CliOptions? options,
        [NotNullWhen(false)] out string? error)
    {
        options = null;

        var mode = ReportOutputMode.Console;
        var idsStart = 0;

        if (args.Count > 0 && !TryParseClientId(args[0], out _))
        {
            idsStart = 1;

            if (!TryParseOutputMode(args[0], out mode))
            {
                error = $"Неизвестный режим вывода '{args[0]}'. Допустимые: {string.Join(", ", SupportedModes)}.";
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

    private static bool TryParseOutputMode(string value, out ReportOutputMode mode)
    {
        return Enum.TryParse(value, ignoreCase: true, out mode)
               && string.Equals(value, mode.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    private static bool TryParseClientId(string value, out long clientId)
    {
        return long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out clientId);
    }
}
