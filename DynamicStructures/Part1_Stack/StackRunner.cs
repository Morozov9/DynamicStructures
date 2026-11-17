using System.Diagnostics;
using DynamicStructures.Common;

namespace DynamicStructures.Part1_Stack;

/// <summary>
/// Часть 1 (пп. 2–3): выполнение команд над стеком из файла input.txt и замеры времени.
///
/// Формат команд в файле (токены через пробел):
///   1,значение  → Push(значение)    (значение — число либо слово)
///   2           → Pop()
///   3           → Top()
///   4           → IsEmpty()
///   5           → Print()
///
/// Пример: 3 4 1,56 1,7 1,cat 2 5 4
/// </summary>
public static class StackRunner
{
    private const string SampleContent = "3 4 1,56 1,7 1,cat 2 5 4";
    private const int Repeats = 5; // сколько раз повторяется замер (берётся медиана)

    public static void Run()
    {
        bool back = false;
        while (!back)
        {
            Console.WriteLine("\n╔══════════════════════════════════════╗");
            Console.WriteLine("║      Часть 1: Стек (LinkedStack)     ║");
            Console.WriteLine("╚══════════════════════════════════════╝");
            Console.WriteLine("  1 — Выполнить команды из файла (с выводом результата каждой операции)");
            Console.WriteLine("  2 — Замеры времени на наборах разного размера и состава");
            Console.WriteLine("  3 — Постфиксные вычисления (вычисление, инфикс→постфикс, сложность)");
            Console.WriteLine("  0 — Назад");
            Console.Write("  > ");

            switch (ConsoleInput.ReadLine().Trim())
            {
                case "1": RunFromFile(); break;
                case "2": RunBenchmark(); break;
                case "3": DynamicStructures.Part3_Algorithms.Part3Runner.Run(); break;
                case "0": back = true; break;
                default: Console.WriteLine("  Введите 0, 1, 2 или 3."); break;
            }
        }
    }

    // ── Пункт 2 ТЗ: выполнение команд из файла ───────────────────────────────

    private static void RunFromFile()
    {
        string defaultPath = DataFiles.EnsureSample("Stack", "input.txt", SampleContent);
        string path = DataFiles.AskPath(defaultPath);

        string content;
        Command[] commands;
        double readMs;
        try
        {
            var sw = Stopwatch.StartNew();
            content = File.ReadAllText(path);
            commands = CommandParser.ParseAll(content);
            sw.Stop();
            readMs = sw.Elapsed.TotalMilliseconds;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            Console.WriteLine($"\n  Ошибка чтения файла: {ex.Message}");
            return;
        }

        string shown = content.Trim();
        if (shown.Length > 200) shown = shown[..200] + " ...";
        Console.WriteLine($"\n  Файл: {Path.GetFullPath(path)}");
        Console.WriteLine($"  Содержимое: {shown}");
        Console.WriteLine($"  Команд: {commands.Length}\n");
        Console.WriteLine("  Обозначения команд:");
        Console.WriteLine("    1,значение -> Push(elem)  - поместить элемент (число или слово) на вершину стека");
        Console.WriteLine("    2          -> Pop()       - извлечь и вернуть верхний элемент со стека");
        Console.WriteLine("    3          -> Top()       - просмотреть верхний элемент без извлечения");
        Console.WriteLine("    4          -> isEmpty()   - проверить, пуст ли стек");
        Console.WriteLine("    5          -> Print()     - напечатать все элементы стека (вершина -> основание)\n");

        // Выполнение с выводом результата каждой операции
        var sw1 = Stopwatch.StartNew();
        Execute(new LinkedStack<string>(), commands, Console.Out);
        sw1.Stop();

        // Повторное выполнение без вывода — чистое время работы стека
        var sw2 = Stopwatch.StartNew();
        Execute(new LinkedStack<string>(), commands, null);
        sw2.Stop();

        Console.WriteLine();
        Console.WriteLine("── Замер времени ──────────────────────────────────");
        Console.WriteLine($"  Считывание (чтение файла + разбор): {readMs * 1000:F2} мкс");
        Console.WriteLine($"  Выполнение с выводом на экран:      {sw1.Elapsed.TotalMicroseconds:F2} мкс");
        Console.WriteLine($"  Выполнение без вывода (стек):       {sw2.Elapsed.TotalMicroseconds:F2} мкс");
        Console.WriteLine("  (для файлов в несколько команд время сопоставимо с погрешностью таймера;");
        Console.WriteLine("   большие наборы смотрите в пункте 2 меню)");
    }

    /// <summary>
    /// Выполнить команды. Если output == null, ничего не выводится (режим замера времени).
    /// </summary>
    internal static void Execute(LinkedStack<string> stack, Command[] commands, TextWriter? output)
    {
        foreach (Command c in commands)
        {
            switch (c.Code)
            {
                case 1: // Push
                    if (c.Arg is null)
                    {
                        output?.WriteLine("  [Push] Ошибка: не указано значение элемента.");
                        break;
                    }
                    stack.Push(c.Arg);
                    if (output is not null) output.WriteLine($"  [Push] Добавлен элемент: {c.Arg}");
                    break;

                case 2: // Pop
                    if (stack.IsEmpty())
                    {
                        output?.WriteLine("  [Pop]  Стек пуст — операция Pop невозможна.");
                        break;
                    }
                    string popped = stack.Pop();
                    if (output is not null) output.WriteLine($"  [Pop]  Извлечён элемент: {popped}");
                    break;

                case 3: // Top
                    if (stack.IsEmpty())
                    {
                        output?.WriteLine("  [Top]  Стек пуст — операция Top невозможна.");
                        break;
                    }
                    string top = stack.Top();
                    if (output is not null) output.WriteLine($"  [Top]  Вершина стека: {top}");
                    break;

                case 4: // IsEmpty
                    bool empty = stack.IsEmpty();
                    if (output is not null) output.WriteLine($"  [IsEmpty] Стек пуст: {empty}");
                    break;

                case 5: // Print
                    output?.Write("  [Print] ");
                    stack.Print(output ?? TextWriter.Null);
                    break;

                case -1:
                    output?.WriteLine($"  [???] Не удалось разобрать команду: '{c.Raw}'");
                    break;

                default:
                    output?.WriteLine($"  [???] Неизвестная команда: {c.Code}");
                    break;
            }
        }
    }

    // ── Пункт 3 ТЗ: замеры времени ───────────────────────────────────────────

    private sealed record BenchSet(string Title, int Count, OpProfile Profile);

    private static void RunBenchmark()
    {
        string dir = DataFiles.Dir("Stack");

        // Набор А: разный размер, одинаковый состав (смесь операций)
        var mixed = new OpProfile(Push: 30, Pop: 30, Peek: 17, IsEmpty: 18, Print: 5);
        var setsA = new List<BenchSet>();
        foreach (int n in new[] { 10, 100, 1_000, 10_000, 100_000, 1_000_000 })
            setsA.Add(new BenchSet($"Смесь, {Bench.Num(n)} оп.", n, mixed));

        // Набор Б: одинаковый размер, разный состав операций
        const int N = 50_000;
        var setsB = new List<BenchSet>
        {
            new("Только Push",                     N, new OpProfile(100, 0, 0, 0, 0)),
            new("Push / Pop 50 на 50",             N, new OpProfile(50, 50, 0, 0, 0)),
            new("Сначала Push, затем Pop",         N, new OpProfile(0, 0, 0, 0, 0, PushThenPop: true)),
            new("Top/IsEmpty 90% + Push 10%",      N, new OpProfile(10, 0, 45, 45, 0)),
            new("Push 99% + Print 1%",             N, new OpProfile(99, 0, 0, 0, 1)),
            new("Push/Pop по 47.5% + Print 5%",    N, new OpProfile(475, 475, 0, 0, 50)),
        };

        Console.WriteLine($"\n  {Bench.BuildNote()}");
        Console.WriteLine($"  Каждый замер повторяется {Repeats} раз, берётся медиана. Вывод на экран отключён.");
        Console.WriteLine("  Генерация файлов и замеры могут занять до минуты...\n");

        // Прогрев JIT
        string warm = Path.Combine(dir, "bench_warmup.txt");
        File.WriteAllText(warm, OpGenerator.Generate(2_000, mixed));
        Measure(warm);

        string tableA = RunSets(dir, "a", setsA);
        string tableB = RunSets(dir, "b", setsB);

        string report =
            "ЧАСТЬ 1. СТЕК. РЕЗУЛЬТАТЫ ЗАМЕРОВ ВРЕМЕНИ\n" +
            $"{DateTime.Now:yyyy-MM-dd HH:mm}. {Bench.BuildNote()}\n" +
            $"Повторов на набор: {Repeats}, значение — медиана. «Чтение» = чтение файла + разбор команд, «Выполнение» = работа стека без вывода на экран.\n\n" +
            "А) Наборы разного размера (одинаковый состав операций):\n" + tableA + "\n" +
            "Б) Наборы одинакового размера, разный состав операций:\n" + tableB;

        Console.WriteLine("А) Наборы разного размера (одинаковый состав операций):\n" + tableA);
        Console.WriteLine("Б) Наборы одинакового размера, разный состав операций:\n" + tableB);

        string resultPath = Path.Combine(dir, "results.txt");
        File.WriteAllText(resultPath, report);
        Console.WriteLine($"  Таблицы сохранены в файл: {resultPath}");
        Console.WriteLine($"  Сгенерированные наборы (input-файлы): {dir}");
    }

    private static string RunSets(string dir, string prefix, List<BenchSet> sets)
    {
        var table = new TableWriter("Набор", "Операций", "Файл, КБ", "Чтение, мс", "Выполнение, мс", "Всего, мс");
        for (int i = 0; i < sets.Count; i++)
        {
            BenchSet s = sets[i];
            string path = Path.Combine(dir, $"bench_{prefix}{i + 1}.txt");
            File.WriteAllText(path, OpGenerator.Generate(s.Count, s.Profile));
            long kb = new FileInfo(path).Length / 1024;

            Console.WriteLine($"  … {s.Title}");
            var (read, exec) = Measure(path);
            table.AddRow(s.Title, Bench.Num(s.Count), Bench.Num(kb), Bench.Ms(read), Bench.Ms(exec), Bench.Ms(read + exec));
        }
        return table.ToString();
    }

    /// <summary>Замерить чтение файла и выполнение команд стеком (медиана по Repeats повторам).</summary>
    private static (double read, double exec) Measure(string path)
    {
        var reads = new List<double>();
        var execs = new List<double>();
        Command[] commands = Array.Empty<Command>();

        for (int r = 0; r < Repeats; r++)
        {
            reads.Add(Bench.TimeMs(() => commands = CommandParser.ParseAll(File.ReadAllText(path))));
            execs.Add(Bench.TimeMs(() => Execute(new LinkedStack<string>(), commands, null)));
        }
        return (Bench.Median(reads), Bench.Median(execs));
    }
}

