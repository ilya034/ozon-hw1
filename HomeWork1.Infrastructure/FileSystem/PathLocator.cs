namespace HomeWork1.Infrastructure.FileSystem;

public static class PathLocator
{
    /// <summary>
    ///     Ищет файл в текущей и родительских директориях.
    /// </summary>
    public static string? FindFileUpwards(string fileName)
    {
        var directory = new DirectoryInfo(Environment.CurrentDirectory);

        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, fileName);
            if (File.Exists(candidate)) return candidate;

            directory = directory.Parent;
        }

        return null;
    }
}