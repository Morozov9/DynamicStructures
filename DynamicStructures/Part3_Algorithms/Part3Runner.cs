using System.Globalization;
using System.Text;
using DynamicStructures.Common;

namespace DynamicStructures.Part3_Algorithms;

/// <summary>
/// Часть 1, п. 4 ТЗ: вычисление выражения в постфиксной записи (из файла),
/// оценка сложности алгоритма и перевод инфиксной записи в постфиксную.
/// </summary>
public static class Part3Runner
{
    private const string PostfixSample =
        "7 8 + 3 2 + /\n2 3 ^ 1 -\n9 sqrt 2 *\n0 sin 1 cos +\n5 ) 3 +\n1 ln\n4 0 /\n1 +";

    private const string InfixSample =
        "(2+3)*4\n7+8-3*2\nsin(0)+cos(0)\n2^3^2\n-(2+3)*4\n(7+8):(3+2)\nsqrt(16)+ln(1)\n2*(3+";

    private const int Repeats = 5;

    public static void Run()
    {
        bool back = false;
        while (!back)
        {
            Console.WriteLine("\n╔══════════════════════════════════════════════╗");
            Console.WriteLine("║   Часть 1, п.4: Постфиксные вычисления       ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");
            Console.WriteLine("  1 — Вычислить постфиксные выражения из файла");
            Console.WriteLine("  2 — Перевести инфиксные выражения из файла в постфиксные");
            Console.WriteLine("  3 — Инфикс → постфикс → результат (файл с инфиксными выражениями)");
            Console.WriteLine("  4 — Оценка сложности (теория + замеры)");
            Console.WriteLine("  0 — Назад");
            Console.Write("  > ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": RunPostfixEvaluator(); break;
                case "2": RunInfixToPostfix(convertOnly: true); break;
                case "3": RunInfixToPostfix(convertOnly: false); break;
                case "4": RunComplexity(); break;
                case "0": back = true; break;
                default: Console.WriteLine("  Введите число от 0 до 4."); break;
            }
        }
    }

    // ── Вычисление постфиксного выражения ────────────────────────────────────

    private static void RunPostfixEvaluator()
    {
        string path = DataFiles.AskPath(DataFiles.EnsureSample("Postfix", "input.txt", PostfixSample));
        string[] lines = ReadLines(path);
        if (lines.Length == 0) return;

        Console.WriteLine($"\n  Файл: {Path.GetFullPath(path)}");
        Console.WriteLine("  (унарный минус в постфиксе записывается токеном «)», например: 5 ) 3 +)\n");

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            Console.Write($"  {line.Trim()}  →  ");
            Console.WriteLine(Calculate(line));
        }
    }

    // ── Перевод инфикс → постфикс (и, по желанию, вычисление) ───────────────

    private static void RunInfixToPostfix(bool convertOnly)
    {
        string path = DataFiles.AskPath(DataFiles.EnsureSample("Infix", "input.txt", InfixSample));
        string[] lines = ReadLines(path);
        if (lines.Length == 0) return;

        Console.WriteLine($"\n  Файл: {Path.GetFullPath(path)}\n");

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            Console.WriteLine($"  Инфикс:   {line.Trim()}");
            try
            {
                string postfix = InfixToPostfix.Convert(line);
                Console.WriteLine($"  Постфикс: {postfix}");
                if (!convertOnly)
                    Console.WriteLine($"  Результат: {Calculate(postfix)}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"  Ошибка:   {ex.Message}");
            }
            Console.WriteLine();
        }
    }

    // ── Оценка сложности ─────────────────────────────────────────────────────

    private static void RunComplexity()
    {
        const string theory =
            "ОЦЕНКА СЛОЖНОСТИ АЛГОРИТМА ВЫЧИСЛЕНИЯ ПОСТФИКСНОЙ ЗАПИСИ\n" +
            "Пусть n — число токенов выражения.\n" +
            "• Цикл выполняется ровно n раз, каждый токен обрабатывается один раз и не возвращается к нему.\n" +
            "• На один токен: число — 1 Push; унарная операция — 1 Pop + 1 Push; бинарная — 2 Pop + 1 Push.\n" +
            "  Каждая операция стека на связном списке выполняется за O(1) (меняется только вершина).\n" +
            "• Определение вида токена — O(1) (проверка в HashSet, разбор числа).\n" +
            "• Итог: T(n) ≤ 3n·c  =>  T(n) = O(n) — время линейно и в лучшем, и в худшем случае (Θ(n)).\n" +
            "• Память: максимум стека — число операндов, ждущих операции: до (n+1)/2 элементов, т.е. O(n)\n" +
            "  (худший случай «1 1 1 … + + +»); для «1 1 + 1 + …» стек не глубже 2 элементов.\n" +
            "• Перевод инфикс → постфикс (алгоритм Дейкстры): каждый токен один раз попадает в стек операторов\n" +
            "  и один раз снимается  =>  T(n) = O(n), память O(n) (глубина вложенности скобок/операторов).\n";

        Console.WriteLine("\n" + theory);
        Console.WriteLine($"  {Bench.BuildNote()}");
        Console.WriteLine($"  Эмпирическая проверка: каждый замер повторяется {Repeats} раз, медиана.");
        Console.WriteLine("  Если время/n примерно постоянно при росте n в 10 раз — зависимость линейная.\n");

        var table = new TableWriter("Что измеряем", "Токенов n", "Push+Pop", "(Push+Pop)/n", "Время, мс", "Время/n, нс");

        // Прогрев JIT
        PostfixEvaluator.Evaluate(BuildFlat(101), out _);
        InfixToPostfix.Convert(BuildInfixFlat(101));

        foreach (int n in new[] { 1_001, 10_001, 100_001, 1_000_001 })
        {
            AddEvalRow(table, "Вычисление: 1 1 + 1 + … (глубина стека 2)", BuildFlat(n), n);
            AddEvalRow(table, "Вычисление: 1 1 … 1 + + … + (глубина n/2)", BuildDeep(n), n);
        }
        foreach (int n in new[] { 1_001, 10_001, 100_001, 1_000_001 })
        {
            AddConvertRow(table, "Инфикс→постфикс: 1+1+1+…", BuildInfixFlat(n), n);
            int nested = (n - 1) / 4 * 4 + 1;
            AddConvertRow(table, "Инфикс→постфикс: 1+(1+(1+…))", BuildInfixNested(n), nested);
        }

        string tableText = table.ToString();
        Console.WriteLine(tableText);

        string dir = DataFiles.Dir("Postfix");
        string resultPath = Path.Combine(dir, "complexity.txt");
        File.WriteAllText(resultPath,
            theory + $"\n{DateTime.Now:yyyy-MM-dd HH:mm}. {Bench.BuildNote()}\n" +
            $"Повторов: {Repeats}, значение — медиана.\n\n" + tableText);
        Console.WriteLine($"  Теория и таблица сохранены в файл: {resultPath}");
    }

    private static void AddEvalRow(TableWriter table, string title, string expr, int n)
    {
        long ops = 0;
        var times = new List<double>();
        for (int r = 0; r < Repeats; r++)
            times.Add(Bench.TimeMs(() => PostfixEvaluator.Evaluate(expr, out ops)));

        double ms = Bench.Median(times);
        table.AddRow(title, Bench.Num(n), Bench.Num(ops),
            ((double)ops / n).ToString("F2", CultureInfo.InvariantCulture),
            Bench.Ms(ms), (ms * 1_000_000 / n).ToString("F1", CultureInfo.InvariantCulture));
    }

    private static void AddConvertRow(TableWriter table, string title, string expr, int n)
    {
        var times = new List<double>();
        for (int r = 0; r < Repeats; r++)
            times.Add(Bench.TimeMs(() => InfixToPostfix.Convert(expr)));

        double ms = Bench.Median(times);
        table.AddRow(title, Bench.Num(n), "—", "—",
            Bench.Ms(ms), (ms * 1_000_000 / n).ToString("F1", CultureInfo.InvariantCulture));
    }

    // Выражение «1 1 + 1 + 1 + …» из n токенов (n нечётное)
    private static string BuildFlat(int n)
    {
        var sb = new StringBuilder(n * 2);
        sb.Append('1');
        for (int i = 0; i < (n - 1) / 2; i++) sb.Append(" 1 +");
        return sb.ToString();
    }

    // Выражение «1 1 1 … 1 + + … +» из n токенов (n нечётное): (n+1)/2 единиц и (n-1)/2 плюсов
    private static string BuildDeep(int n)
    {
        int ones = (n + 1) / 2;
        var sb = new StringBuilder(n * 2);
        for (int i = 0; i < ones; i++) sb.Append(i == 0 ? "1" : " 1");
        for (int i = 0; i < ones - 1; i++) sb.Append(" +");
        return sb.ToString();
    }

    // Инфикс «1+1+1+…» из n токенов (n нечётное)
    private static string BuildInfixFlat(int n)
    {
        var sb = new StringBuilder(n * 2);
        sb.Append('1');
        for (int i = 0; i < (n - 1) / 2; i++) sb.Append("+1");
        return sb.ToString();
    }

    // Инфикс «1+(1+(1+( … 1 )))»: k уровней вложенности, 4k+1 токенов
    private static string BuildInfixNested(int n)
    {
        int k = (n - 1) / 4;
        var sb = new StringBuilder(n * 2);
        for (int i = 0; i < k; i++) sb.Append("1+(");
        sb.Append('1');
        sb.Append(')', k);
        return sb.ToString();
    }

    // ── Общие вспомогательные методы ─────────────────────────────────────────

    private static string Calculate(string postfix)
    {
        try
        {
            double result = PostfixEvaluator.Evaluate(postfix, out long ops);
            return $"{result.ToString("G10", CultureInfo.InvariantCulture)}   (операций со стеком: {ops})";
        }
        catch (Exception ex) when (ex is FormatException or ArithmeticException)
        {
            return $"ошибка: {ex.Message}";
        }
    }

    private static string[] ReadLines(string path)
    {
        try
        {
            return File.ReadAllLines(path);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            Console.WriteLine($"\n  Ошибка чтения файла: {ex.Message}");
            return Array.Empty<string>();
        }
    }
}
