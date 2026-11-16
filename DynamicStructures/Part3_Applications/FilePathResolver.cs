using System.Text;
using DynamicStructures.Part1_Stack;

namespace DynamicStructures.Part3_Applications;

/// <summary>
/// Лексическое разрешение компонентов пути через собственный LinkedStack.
/// Обычная папка — Push, «..» — Pop, «.» пропускается. Время и память O(n).
/// Файлы не открываются, символические ссылки не разрешаются.
/// </summary>
public static class FilePathResolver
{
    /// <summary>
    /// Нормализует абсолютный или относительный путь Windows/Unix.
    /// В относительном пути сохраняет начальные «..», в абсолютном не выходит за корень.
    /// Пути Windows вида C:folder и служебные префиксы устройств не поддерживаются.
    /// </summary>
    public static string Resolve(string path, Action<string>? onStep = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        if (path.Contains('\0'))
            throw new ArgumentException("Путь содержит нулевой символ.", nameof(path));

        bool drive = path.Length >= 2 && char.IsAsciiLetter(path[0]) && path[1] == ':';
        bool windows = drive || path.Contains('\\');
        char separator = windows ? '\\' : '/';
        string normalized = path.Replace('\\', '/');
        string root = "";
        int start = 0;

        if (drive)
        {
            if (normalized.Length < 3 || normalized[2] != '/')
                throw new ArgumentException("После буквы диска укажите корень: например, C:\\folder.", nameof(path));
            root = $"{char.ToUpperInvariant(path[0])}:{separator}";
            start = 3;
        }
        else if (path.StartsWith("\\\\", StringComparison.Ordinal))
        {
            string[] parts = normalized[2..].Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2 || parts[0] is "." or ".." or "?" || parts[1] is "." or "..")
                throw new ArgumentException("UNC-путь должен начинаться с \\\\server\\share.", nameof(path));
            root = $"\\\\{parts[0]}\\{parts[1]}\\";
            normalized = string.Join('/', parts.Skip(2));
        }
        else if (normalized.StartsWith('/'))
        {
            root = separator.ToString();
            start = 1;
        }

        var directories = new LinkedStack<string>();
        foreach (string part in normalized[start..].Split('/', StringSplitOptions.RemoveEmptyEntries))
        {
            if (part == ".")
            {
                onStep?.Invoke("«.» — текущая папка, пропускаем.");
            }
            else if (part == ".." && !directories.IsEmpty() && directories.Top() != "..")
            {
                string removed = directories.Pop();
                onStep?.Invoke($"«..» — Pop: удалена папка «{removed}».");
            }
            else if (part == ".." && root.Length > 0)
            {
                onStep?.Invoke("«..» — достигнут корень, выше не поднимаемся.");
            }
            else
            {
                directories.Push(part);
                onStep?.Invoke($"Push: «{part}» (в стеке: {directories.Count}).");
            }
        }

        // Стек хранит папки в обратном порядке; второй стек восстанавливает порядок пути.
        var ordered = new LinkedStack<string>();
        while (!directories.IsEmpty()) ordered.Push(directories.Pop());
        var result = new StringBuilder(root);
        bool first = true;
        while (!ordered.IsEmpty())
        {
            if (!first) result.Append(separator);
            result.Append(ordered.Pop());
            first = false;
        }
        return result.Length == 0 ? "." : result.ToString();
    }
}
