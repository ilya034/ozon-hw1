namespace HomeWork1.Infrastructure.FileSystem;

public static class PathLocator
{
    /// <summary>
    ///     Ищет файл в стартовой директории и её родителях.
    /// </summary>
    /// <param name="fileName">Имя искомого файла.</param>
    /// <param name="startDirectory">С какой директории начинать. По умолчанию — текущая директория процесса.</param>
    /// <returns>Полный путь к найденному файлу или <c>null</c>.</returns>
    public static string? FindFileUpwards(string fileName, string? startDirectory = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

        var directory = new DirectoryInfo(startDirectory ?? Environment.CurrentDirectory);

        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, fileName);
            if (File.Exists(candidate)) return candidate;

            directory = directory.Parent;
        }

        return null;
    }
}
