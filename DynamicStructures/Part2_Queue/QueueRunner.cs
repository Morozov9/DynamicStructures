using System.Diagnostics;
using DynamicStructures.Common;

namespace DynamicStructures.Part2_Queue;

/// <summary>
/// Часть 2: выполнение команд над очередью из файла input.txt для двух реализаций
/// (LinkedQueue — свой список, Queue&lt;T&gt; — стандартный класс) и замеры времени.
///
/// Формат команд в файле (токены через пробел):
///   1,значение  → вставка (Enqueue)   (значение — число либо слово)
///   2           → удаление (Dequeue)
///   3           → просмотр начала очереди (Peek)
///   4           → проверка на пустоту (IsEmpty)
///   5           → печать (Print)
///
/// Пример: 3 4 1,56 1,7 1,cat 2 5 4
/// </summary>
public static class QueueRunner
{
    private const string SampleContent = "3 4 1,56 1,7 1,cat 2 5 4";
    private const int Repeats = 5; // сколько раз повторяется замер (берётся медиана)

    public static void Run()
    {
        bool back = false;
        while (!back)
        {
            Console.WriteLine("\n╔══════════════════════════════════════════════╗");
            Console.WriteLine("║      Часть 2: Очередь (две реализации)       ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");
            Console.WriteLine("  1 — Выполнить команды из файла (обе реализации, с выводом результата)");
            Console.WriteLine("  2 — Замеры времени на наборах разной длины и разного состава");
            Console.WriteLine("  0 — Назад");
            Console.Write("  > ");

            switch (ConsoleInput.ReadLine().Trim())
            {
                case "1": RunFromFile(); break;
                case "2": RunBenchmark(); break;
                case "0": back = true; break;
                default: Console.WriteLine("  Введите 0, 1 или 2."); break;
            }
        }
    }

    // ── Пункт 1 ТЗ: выполнение команд из файла ───────────────────────────────

    private static void RunFromFile()
    {
        string defaultPath = DataFiles.EnsureSample("Queue", "input.txt", SampleContent);
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
        Console.WriteLine("    1,значение -> вставка (Enqueue)              - поместить элемент (число или слово) в конец очереди");
        Console.WriteLine("    2          -> удаление (Dequeue)             - извлечь и вернуть первый элемент из начала очереди");
        Console.WriteLine("    3          -> просмотр начала очереди (Peek) - просмотреть первый элемент без удаления");
        Console.WriteLine("    4          -> проверка на пустоту (isEmpty)  - проверить, пуста ли очередь");
        Console.WriteLine("    5          -> печать (Print)                 - напечатать все элементы очереди (начало -> конец)");

        // Реализация 1: свой список
        Console.WriteLine("\n── Реализация 1: LinkedQueue (свой связный список) ──");
        var swL = Stopwatch.StartNew();
        ExecuteLinked(new LinkedQueue<string>(), commands, Console.Out);
        swL.Stop();

        // Реализация 2: стандартный Queue<T>
        Console.WriteLine("\n── Реализация 2: Queue<T> (стандартный класс) ──────");
        var swS = Stopwatch.StartNew();
        ExecuteStandard(new Queue<string>(), commands, Console.Out);
        swS.Stop();

        // Чистое время без вывода на экран
        var swL2 = Stopwatch.StartNew();
        ExecuteLinked(new LinkedQueue<string>(), commands, null);
        swL2.Stop();
        var swS2 = Stopwatch.StartNew();
        ExecuteStandard(new Queue<string>(), commands, null);
        swS2.Stop();

        Console.WriteLine("\n── Замер времени ───────────────────────────────────");
        Console.WriteLine($"  Считывание (чтение файла + разбор): {readMs * 1000:F2} мкс");
        Console.WriteLine($"  LinkedQueue с выводом / без вывода: {swL.Elapsed.TotalMicroseconds:F2} / {swL2.Elapsed.TotalMicroseconds:F2} мкс");
        Console.WriteLine($"  Queue<T>    с выводом / без вывода: {swS.Elapsed.TotalMicroseconds:F2} / {swS2.Elapsed.TotalMicroseconds:F2} мкс");
        Console.WriteLine("  (для маленьких файлов время сопоставимо с погрешностью таймера;");
        Console.WriteLine("   большие наборы смотрите в пункте 2 меню)");
    }

    // ── Выполнение для LinkedQueue (output == null — режим замера, без вывода) ──

    internal static void ExecuteLinked(LinkedQueue<string> queue, Command[] commands, TextWriter? output)
    {
        foreach (Command c in commands)
        {
            switch (c.Code)
            {
                case 1: // вставка
                    if (c.Arg is null)
                    {
                        output?.WriteLine("  [Enqueue] Ошибка: не указано значение элемента.");
                        break;
                    }
                    queue.Enqueue(c.Arg);
                    if (output is not null) output.WriteLine($"  [Enqueue] Добавлен: {c.Arg}");
                    break;

                case 2: // удаление
                    if (queue.IsEmpty())
                    {
                        output?.WriteLine("  [Dequeue] Очередь пуста — операция Dequeue невозможна.");
                        break;
                    }
                    string removed = queue.Dequeue();
                    if (output is not null) output.WriteLine($"  [Dequeue] Извлечён: {removed}");
                    break;

                case 3: // начало очереди
                    if (queue.IsEmpty())
                    {
                        output?.WriteLine("  [Peek]    Очередь пуста — операция Peek невозможна.");
                        break;
                    }
                    string first = queue.Peek();
                    if (output is not null) output.WriteLine($"  [Peek]    Начало очереди: {first}");
                    break;

                case 4: // пустота
                    bool empty = queue.IsEmpty();
                    if (output is not null) output.WriteLine($"  [IsEmpty] Очередь пуста: {empty}");
                    break;

                case 5: // печать
                    output?.Write("  [Print]   ");
                    queue.Print(output ?? TextWriter.Null);
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

    // ── Выполнение для стандартного Queue<T> ─────────────────────────────────

    internal static void ExecuteStandard(Queue<string> queue, Command[] commands, TextWriter? output)
    {
        foreach (Command c in commands)
        {
            switch (c.Code)
            {
                case 1: // вставка
                    if (c.Arg is null)
                    {
                        output?.WriteLine("  [Enqueue] Ошибка: не указано значение элемента.");
                        break;
                    }
                    queue.Enqueue(c.Arg);
                    if (output is not null) output.WriteLine($"  [Enqueue] Добавлен: {c.Arg}");
                    break;

                case 2: // удаление
                    if (queue.Count == 0)
                    {
                        output?.WriteLine("  [Dequeue] Очередь пуста — операция Dequeue невозможна.");
                        break;
                    }
                    string removed = queue.Dequeue();
                    if (output is not null) output.WriteLine($"  [Dequeue] Извлечён: {removed}");
                    break;

                case 3: // начало очереди
                    if (queue.Count == 0)
                    {
                        output?.WriteLine("  [Peek]    Очередь пуста — операция Peek невозможна.");
                        break;
                    }
                    string first = queue.Peek();
                    if (output is not null) output.WriteLine($"  [Peek]    Начало очереди: {first}");
                    break;

                case 4: // пустота
                    bool empty = queue.Count == 0;
                    if (output is not null) output.WriteLine($"  [IsEmpty] Очередь пуста: {empty}");
                    break;

                case 5: // печать
                    output?.Write("  [Print]   ");
                    if (queue.Count == 0)
                    {
                        (output ?? TextWriter.Null).WriteLine("  [очередь пуста]");
                        break;
                    }
                    // строка формируется всегда — как и в LinkedQueue.Print, чтобы замер был честным
                    string line = "  Очередь (начало → конец): " + string.Join(" → ", queue);
                    TextWriter w = output ?? TextWriter.Null;
                    w.WriteLine(line);
                    w.WriteLine($"  Всего элементов: {queue.Count}");
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

    // ── Пункт 2 ТЗ: замеры времени ───────────────────────────────────────────

    private sealed record BenchSet(string Title, int Count, OpProfile Profile);

    private static void RunBenchmark()
    {
        string dir = DataFiles.Dir("Queue");

        // 1) Наборы разной длины (одинаковый состав: смесь операций)
        var mixed = new OpProfile(Push: 30, Pop: 30, Peek: 17, IsEmpty: 18, Print: 5);
        var setsA = new List<BenchSet>();
        foreach (int n in new[] { 10, 100, 1_000, 10_000, 100_000, 1_000_000 })
            setsA.Add(new BenchSet($"Смесь, {Bench.Num(n)} оп.", n, mixed));

        // 2) Наборы одинаковой длины, но разного состава (ищем максимальное расхождение)
        const int N = 50_000;
        var setsB = new List<BenchSet>
        {
            new("Только вставка",                    N, new OpProfile(100, 0, 0, 0, 0)),
            new("Вставка / удаление 50 на 50",       N, new OpProfile(50, 50, 0, 0, 0)),
            new("Сначала вставка, затем удаление",   N, new OpProfile(0, 0, 0, 0, 0, PushThenPop: true)),
            new("Peek/IsEmpty 90% + вставка 10%",    N, new OpProfile(10, 0, 45, 45, 0)),
            new("Вставка 99% + печать 1%",           N, new OpProfile(99, 0, 0, 0, 1)),
            new("Вст./удал. по 47.5% + печать 5%",   N, new OpProfile(475, 475, 0, 0, 50)),
        };

        Console.WriteLine($"\n  {Bench.BuildNote()}");
        Console.WriteLine($"  Каждый замер повторяется {Repeats} раз, берётся медиана. Вывод на экран отключён.");
        Console.WriteLine("  Генерация файлов и замеры могут занять до минуты...\n");

        // Прогрев JIT
        string warm = Path.Combine(dir, "bench_warmup.txt");
        File.WriteAllText(warm, OpGenerator.Generate(2_000, mixed));
        Measure(warm);

        string tableA = RunSets(dir, "a", setsA, out _);
        string tableB = RunSets(dir, "b", setsB, out string maxNote);

        string report =
            "ЧАСТЬ 2. ОЧЕРЕДЬ. РЕЗУЛЬТАТЫ ЗАМЕРОВ ВРЕМЕНИ\n" +
            $"{DateTime.Now:yyyy-MM-dd HH:mm}. {Bench.BuildNote()}\n" +
            $"Повторов на набор: {Repeats}, значение — медиана. «Чтение» = чтение файла + разбор команд (одинаково для обеих реализаций),\n" +
            "«LinkedQueue» и «Queue<T>» — время выполнения команд без вывода на экран. «Отношение» = LinkedQueue / Queue<T>.\n\n" +
            "1) Наборы разной длины:\n" + tableA + "\n" +
            "2) Наборы одинаковой длины, разный состав операций:\n" + tableB + maxNote;

        Console.WriteLine("1) Наборы разной длины:\n" + tableA);
        Console.WriteLine("2) Наборы одинаковой длины, разный состав операций:\n" + tableB + maxNote);

        string resultPath = Path.Combine(dir, "results.txt");
        File.WriteAllText(resultPath, report);
        Console.WriteLine($"  Таблицы сохранены в файл: {resultPath}");
        Console.WriteLine($"  Сгенерированные наборы (input-файлы): {dir}");
    }

    private static string RunSets(string dir, string prefix, List<BenchSet> sets, out string maxNote)
    {
        var table = new TableWriter("Набор", "Операций", "Файл, КБ", "Чтение, мс",
            "LinkedQueue, мс", "Queue<T>, мс", "Отношение");

        double bestDivergence = 0;
        string bestTitle = "";
        double bestRatio = 1;

        for (int i = 0; i < sets.Count; i++)
        {
            BenchSet s = sets[i];
            string path = Path.Combine(dir, $"bench_{prefix}{i + 1}.txt");
            File.WriteAllText(path, OpGenerator.Generate(s.Count, s.Profile));
            long kb = new FileInfo(path).Length / 1024;

            Console.WriteLine($"  … {s.Title}");
            var (read, linked, standard) = Measure(path);
            double ratio = standard > 0 ? linked / standard : 0;
            table.AddRow(s.Title, Bench.Num(s.Count), Bench.Num(kb), Bench.Ms(read),
                Bench.Ms(linked), Bench.Ms(standard), ratio.ToString("F2", System.Globalization.CultureInfo.InvariantCulture));

            double divergence = ratio >= 1 ? ratio : (ratio > 0 ? 1 / ratio : 0);
            if (divergence > bestDivergence)
            {
                bestDivergence = divergence;
                bestTitle = s.Title;
                bestRatio = ratio;
            }
        }

        string faster = bestRatio >= 1 ? "Queue<T> быстрее" : "LinkedQueue быстрее";
        maxNote = $"Максимальное расхождение: «{bestTitle}» — {faster} в {bestDivergence:F2} раза.\n";
        return table.ToString();
    }

    /// <summary>
    /// Замерить чтение файла и выполнение команд обеими реализациями
    /// (медиана по Repeats повторам).
    /// </summary>
    private static (double read, double linked, double standard) Measure(string path)
    {
        var reads = new List<double>();
        var linkedTimes = new List<double>();
        var standardTimes = new List<double>();
        Command[] commands = Array.Empty<Command>();

        for (int r = 0; r < Repeats; r++)
        {
            reads.Add(Bench.TimeMs(() => commands = CommandParser.ParseAll(File.ReadAllText(path))));
            linkedTimes.Add(Bench.TimeMs(() => ExecuteLinked(new LinkedQueue<string>(), commands, null)));
            standardTimes.Add(Bench.TimeMs(() => ExecuteStandard(new Queue<string>(), commands, null)));
        }
        return (Bench.Median(reads), Bench.Median(linkedTimes), Bench.Median(standardTimes));
    }
}
