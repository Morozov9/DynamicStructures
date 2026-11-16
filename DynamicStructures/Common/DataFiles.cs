namespace DynamicStructures.Common;

/// <summary>
/// Работа с входными файлами. Все файлы лежат в папке TestData рядом с exe
/// (bin\Debug\net10.0\TestData\...); для каждой задачи — свой подраздел.
/// Если файла нет, создаётся пример.
/// </summary>
public static class DataFiles
{
    public static string Root => Path.Combine(AppContext.BaseDirectory, "TestData");

    /// <summary>Папка TestData\{sub} (создаётся при необходимости).</summary>
    public static string Dir(string sub)
    {
        string dir = Path.Combine(Root, sub);
        Directory.CreateDirectory(dir);
        return dir;
    }

    /// <summary>Вернуть путь к файлу; если файла нет — создать с примером.</summary>
    public static string EnsureSample(string sub, string fileName, string content)
    {
        string path = Path.Combine(Dir(sub), fileName);
        if (!File.Exists(path))
        {
            File.WriteAllText(path, content);
            Console.WriteLine($"  → Создан пример файла: {path}");
        }
        return path;
    }

    /// <summary>Спросить путь к файлу; Enter — путь по умолчанию.</summary>
    public static string AskPath(string defaultPath)
    {
        Console.WriteLine($"  Файл по умолчанию: {defaultPath}");
        Console.Write("  Путь к другому файлу (Enter — по умолчанию): ");
        string s = ConsoleInput.ReadLine().Trim().Trim('"');
        return string.IsNullOrEmpty(s) ? defaultPath : s;
    }
}
