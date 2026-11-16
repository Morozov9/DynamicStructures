using DynamicStructures.Common;

namespace DynamicStructures.Part3_Applications;

/// <summary>
/// Часть 3: примеры применения динамических структур данных.
///   Список  — задача Иосифа Флавия (кольцевой список).
///   Очередь — заливка краской Flood Fill (поиск в ширину на LinkedQueue).
/// </summary>
public static class ApplicationsRunner
{
    private const string SampleGrid =
        "##########\n" +
        "#....#...#\n" +
        "#....#...#\n" +
        "###..#####\n" +
        "#....#...#\n" +
        "#....#...#\n" +
        "##########";

    public static void Run()
    {
        bool back = false;
        while (!back)
        {
            Console.WriteLine("\n╔══════════════════════════════════════════════╗");
            Console.WriteLine("║   Часть 3: Применение динамических структур  ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");
            Console.WriteLine("  1 — Список:  задача Иосифа Флавия (решение)");
            Console.WriteLine("  2 — Список:  Иосиф Флавий — автопроверка списка по формуле");
            Console.WriteLine("  3 — Очередь: заливка краской (Flood Fill)");
            Console.WriteLine("  0 — Назад");
            Console.Write("  > ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": RunJosephus(); break;
                case "2": RunJosephusSelfCheck(); break;
                case "3": RunFloodFill(); break;
                case "0": back = true; break;
                default: Console.WriteLine("  Введите число от 0 до 3."); break;
            }
        }
    }

    // ── Иосиф Флавий ─────────────────────────────────────────────────────────

    private static void RunJosephus()
    {
        Console.WriteLine("\n  ── Задача Иосифа Флавия ──");
        Console.WriteLine("  n человек стоят по кругу, каждого k-го выводят из круга, пока не останется один.");
        int n = ReadInt("  Количество человек n (Enter = 7): ", 7, 1, 1_000_000);
        int k = ReadInt("  Шаг k (Enter = 3): ", 3, 1, 1_000_000);

        Console.WriteLine();
        bool verbose = n <= 15;
        if (verbose)
            Console.WriteLine($"  Начало: {string.Join(" → ", Enumerable.Range(1, n))} ↺");

        int step = 0;
        Action<int, string>? onStep = null;
        if (verbose)
            onStep = (victim, circle) => Console.WriteLine($"  Шаг {++step,2}: выбывает {victim,-3} круг: {circle}");
        var (order, survivor) = JosephusProblem.Solve(n, k, onStep);

        string orderText = order.Count <= 60
            ? string.Join(" ", order)
            : string.Join(" ", order.Take(60)) + " ...";
        Console.WriteLine($"\n  Порядок выбывания: {(order.Count == 0 ? "(никто)" : orderText)}");
        Console.WriteLine($"  Остался: {survivor}");

        int byFormula = JosephusProblem.SolveByFormula(n, k);
        Console.WriteLine($"  Проверка по формуле J(n)=(J(n-1)+k) mod n: {byFormula} " +
                          (byFormula == survivor ? "— совпало" : "— НЕ СОВПАЛО!"));
    }

    private static void RunJosephusSelfCheck()
    {
        int total = 0, wrong = 0;
        for (int n = 1; n <= 300; n++)
        {
            for (int k = 1; k <= 30; k++)
            {
                total++;
                if (JosephusProblem.Solve(n, k).Survivor != JosephusProblem.SolveByFormula(n, k))
                {
                    wrong++;
                    Console.WriteLine($"  Расхождение: n={n}, k={k}");
                }
            }
        }
        Console.WriteLine($"\n  Проверено сочетаний (n=1..300, k=1..30): {total}, расхождений: {wrong}");
        Console.WriteLine(wrong == 0
            ? "  Решение на списке совпадает с формулой во всех случаях."
            : "  Есть расхождения — смотрите список выше.");
    }

    // ── Flood Fill ───────────────────────────────────────────────────────────

    private static void RunFloodFill()
    {
        Console.WriteLine("\n  ── Заливка краской (Flood Fill) на очереди ──");
        string defaultPath = DataFiles.EnsureSample("FloodFill", "input.txt", SampleGrid);
        string path = DataFiles.AskPath(defaultPath);

        string[] lines;
        try
        {
            lines = File.ReadAllLines(path).Where(l => l.Length > 0).ToArray();
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            Console.WriteLine($"\n  Ошибка чтения файла: {ex.Message}");
            return;
        }

        if (lines.Length == 0)
        {
            Console.WriteLine("  Файл пуст.");
            return;
        }

        Console.WriteLine("\n  Картинка (каждый символ — клетка цвета этого символа; '#' — стена):\n");
        PrintGrid(ToGrid(lines), withIndex: true);

        int rows = lines.Length;
        int cols = lines.Max(l => l.Length);
        int row = ReadInt($"\n  Строка стартовой клетки (1..{rows}, Enter = 2): ", 2, 1, rows) - 1;
        int col = ReadInt($"  Столбец стартовой клетки (1..{cols}, Enter = 2): ", 2, 1, cols) - 1;
        if (col >= lines[row].Length)
        {
            Console.WriteLine("  В этой строке нет такого столбца.");
            return;
        }

        Console.Write("  Новый цвет — один символ (Enter = '*'): ");
        string? colorInput = Console.ReadLine();
        char color = string.IsNullOrEmpty(colorInput) ? '*' : colorInput[0];

        // 1) обычная заливка
        char[][] grid = ToGrid(lines);
        var result = FloodFill.Fill(grid, row, col, color);
        Console.WriteLine("\n  Результат заливки:\n");
        PrintGrid(grid, withIndex: true);
        Console.WriteLine($"\n  Закрашено клеток: {result.Painted}, волн (слоёв очереди): {result.Waves}, " +
                          $"максимум в очереди: {result.MaxQueueSize}");

        // 2) та же заливка, но клетки помечены номером волны — видно работу очереди
        char[][] waveGrid = ToGrid(lines);
        FloodFill.Fill(waveGrid, row, col, color, numberWaves: true);
        Console.WriteLine("\n  Та же заливка по волнам (цифра = номер волны mod 10, т.е. расстояние от старта):\n");
        PrintGrid(waveGrid, withIndex: true);
    }

    // ── Вспомогательные методы ───────────────────────────────────────────────

    private static char[][] ToGrid(string[] lines)
        => lines.Select(l => l.ToCharArray()).ToArray();

    private static void PrintGrid(char[][] grid, bool withIndex)
    {
        int cols = grid.Max(r => r.Length);
        if (withIndex)
        {
            Console.Write("      ");
            for (int c = 1; c <= cols; c++) Console.Write(c % 10);
            Console.WriteLine();
        }
        for (int r = 0; r < grid.Length; r++)
        {
            Console.Write(withIndex ? $"  {r + 1,3} " : "  ");
            Console.WriteLine(new string(grid[r]));
        }
    }

    private static int ReadInt(string prompt, int defaultValue, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            string? s = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(s)) return defaultValue;
            if (int.TryParse(s, out int v) && v >= min && v <= max) return v;
            Console.WriteLine($"  Введите целое число от {min} до {max}.");
        }
    }
}
