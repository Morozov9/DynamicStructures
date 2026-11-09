namespace DynamicStructures.Part3_Algorithms;

/// <summary>
/// Запуск демонстрации алгоритмов части 3:
///   - Вычисление постфиксного выражения
///   - Перевод инфиксной записи в постфиксную
/// </summary>
public static class Part3Runner
{
    private const string PostfixFile = "input_postfix.txt";
    private const string InfixFile   = "input_infix.txt";

    public static void Run()
    {
        Console.WriteLine("\n╔══════════════════════════════════════════════╗");
        Console.WriteLine("║   Часть 3: Алгоритмы постфиксных вычислений ║");
        Console.WriteLine("╚══════════════════════════════════════════════╝\n");

        RunPostfixEvaluator();
        Console.WriteLine();
        RunInfixToPostfix();
    }

    // ── Задание: вычисление постфиксного выражения ────────────────────────────

    private static void RunPostfixEvaluator()
    {
        Console.WriteLine("── Алгоритм 1: Вычисление постфиксного выражения ──");
        Console.WriteLine("   Оценка сложности: O(n), где n — число токенов.");
        Console.WriteLine("   Каждый токен обрабатывается ровно 1 раз.\n");

        EnsureFile(PostfixFile, "7 8 + 3 2 + /");

        Console.WriteLine($"   Файл: {Path.GetFullPath(PostfixFile)}");

        string[] lines = File.ReadAllLines(PostfixFile);
        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            Console.Write($"   Выражение: {line}");
            try
            {
                double result = PostfixEvaluator.Evaluate(line);
                Console.WriteLine($"  →  Результат: {result:G10}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n   Ошибка: {ex.Message}");
            }
        }
    }

    // ── Задание: перевод инфикс → постфикс ───────────────────────────────────

    private static void RunInfixToPostfix()
    {
        Console.WriteLine("── Алгоритм 2: Перевод инфикс → постфикс ──────────");
        Console.WriteLine("   Алгоритм сортировочной станции Дейкстры.");
        Console.WriteLine("   Оценка сложности: O(n), где n — число токенов.\n");

        EnsureFile(InfixFile, "(2+3)*4\n7+8-3*2\nsin(0)+cos(0)");

        Console.WriteLine($"   Файл: {Path.GetFullPath(InfixFile)}");

        string[] lines = File.ReadAllLines(InfixFile);
        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            Console.Write($"   Инфикс:   {line}");
            try
            {
                string postfix = InfixToPostfix.Convert(line);
                Console.WriteLine($"\n   Постфикс: {postfix}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n   Ошибка: {ex.Message}\n");
            }
        }
    }

    private static void EnsureFile(string path, string content)
    {
        if (!File.Exists(path))
        {
            File.WriteAllText(path, content);
            Console.WriteLine($"   → Создан пример файла: {path}");
        }
    }
}
