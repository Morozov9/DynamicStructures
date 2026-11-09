using System.Diagnostics;

namespace DynamicStructures.Part2_Queue;

/// <summary>
/// Выполняет команды над очередью из файла и замеряет время
/// для обеих реализаций (LinkedQueue и стандартный Queue&lt;T&gt;).
///
/// Формат команд в файле (числа через пробел):
///   1,значение  → Enqueue(значение)
///   2           → Dequeue()
///   3           → Peek() — просмотр начала
///   4           → IsEmpty()
///   5           → Print()
///
/// Пример: 3 4 1,56 1,7 1,cat 2 5 4
/// </summary>
public static class QueueRunner
{
    private const string InputFile = "input_queue.txt";

    public static void Run()
    {
        Console.WriteLine("\n╔══════════════════════════════════════════════╗");
        Console.WriteLine("║      Часть 2: Очередь (две реализации)      ║");
        Console.WriteLine("╚══════════════════════════════════════════════╝\n");

        EnsureSampleFile();

        Console.WriteLine($"Файл данных: {Path.GetFullPath(InputFile)}");
        Console.Write("Содержимое: ");

        string content;
        try
        {
            content = File.ReadAllText(InputFile).Trim();
            Console.WriteLine(content);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nОшибка чтения файла: {ex.Message}");
            return;
        }

        string[] tokens = content.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        // === Реализация 1: LinkedQueue ===
        Console.WriteLine("\n── Реализация 1: LinkedQueue (свой связный список) ──");
        var linkedQueue = new LinkedQueue<string>();
        var sw1 = Stopwatch.StartNew();
        ExecuteLinked(linkedQueue, tokens);
        sw1.Stop();
        Console.WriteLine($"\n  Время выполнения (LinkedQueue): {sw1.Elapsed.TotalMicroseconds:F2} мкс");

        // === Реализация 2: Queue<T> (стандарт) ===
        Console.WriteLine("\n── Реализация 2: Queue<T> (стандартный класс) ────");
        var standardQueue = new Queue<string>();
        var sw2 = Stopwatch.StartNew();
        ExecuteStandard(standardQueue, tokens);
        sw2.Stop();
        Console.WriteLine($"\n  Время выполнения (Queue<T>):   {sw2.Elapsed.TotalMicroseconds:F2} мкс");

        // === Итог ===
        Console.WriteLine("\n── Сравнение времени ──────────────────────────────");
        Console.WriteLine($"  LinkedQueue : {sw1.Elapsed.TotalMicroseconds:F2} мкс");
        Console.WriteLine($"  Queue<T>    : {sw2.Elapsed.TotalMicroseconds:F2} мкс");
        string faster = sw1.Elapsed < sw2.Elapsed ? "LinkedQueue" : "Queue<T>";
        Console.WriteLine($"  Быстрее: {faster}");
    }

    // ── Выполнение для LinkedQueue ────────────────────────────────────────────

    private static void ExecuteLinked(LinkedQueue<string> queue, string[] tokens)
    {
        foreach (string token in tokens)
        {
            string[] parts = token.Split(',', 2);
            int command = int.Parse(parts[0]);
            string? arg = parts.Length > 1 ? parts[1] : null;

            switch (command)
            {
                case 1: // Enqueue
                    if (arg is null) { Console.WriteLine("  [Enqueue] Ошибка: нет значения."); break; }
                    queue.Enqueue(arg);
                    Console.WriteLine($"  [Enqueue] Добавлен: {arg}");
                    break;

                case 2: // Dequeue
                    try
                    {
                        string val = queue.Dequeue();
                        Console.WriteLine($"  [Dequeue] Извлечён: {val}");
                    }
                    catch (InvalidOperationException e) { Console.WriteLine($"  [Dequeue] {e.Message}"); }
                    break;

                case 3: // Peek
                    try
                    {
                        Console.WriteLine($"  [Peek]    Начало очереди: {queue.Peek()}");
                    }
                    catch (InvalidOperationException e) { Console.WriteLine($"  [Peek]    {e.Message}"); }
                    break;

                case 4: // IsEmpty
                    Console.WriteLine($"  [IsEmpty] Очередь пуста: {queue.IsEmpty()}");
                    break;

                case 5: // Print
                    Console.Write("  [Print]   ");
                    queue.Print();
                    break;

                default:
                    Console.WriteLine($"  [???] Неизвестная команда: {command}");
                    break;
            }
        }
    }

    // ── Выполнение для стандартного Queue<T> ─────────────────────────────────

    private static void ExecuteStandard(Queue<string> queue, string[] tokens)
    {
        foreach (string token in tokens)
        {
            string[] parts = token.Split(',', 2);
            int command = int.Parse(parts[0]);
            string? arg = parts.Length > 1 ? parts[1] : null;

            switch (command)
            {
                case 1: // Enqueue
                    if (arg is null) { Console.WriteLine("  [Enqueue] Ошибка: нет значения."); break; }
                    queue.Enqueue(arg);
                    Console.WriteLine($"  [Enqueue] Добавлен: {arg}");
                    break;

                case 2: // Dequeue
                    if (queue.Count == 0) { Console.WriteLine("  [Dequeue] Очередь пуста."); break; }
                    Console.WriteLine($"  [Dequeue] Извлечён: {queue.Dequeue()}");
                    break;

                case 3: // Peek
                    if (queue.Count == 0) { Console.WriteLine("  [Peek]    Очередь пуста."); break; }
                    Console.WriteLine($"  [Peek]    Начало очереди: {queue.Peek()}");
                    break;

                case 4: // IsEmpty
                    Console.WriteLine($"  [IsEmpty] Очередь пуста: {queue.Count == 0}");
                    break;

                case 5: // Print
                    if (queue.Count == 0)
                    {
                        Console.WriteLine("  [Print]   [очередь пуста]");
                        break;
                    }
                    Console.Write("  [Print]   Очередь (начало → конец): ");
                    Console.WriteLine(string.Join(" → ", queue));
                    Console.WriteLine($"             Всего элементов: {queue.Count}");
                    break;

                default:
                    Console.WriteLine($"  [???] Неизвестная команда: {command}");
                    break;
            }
        }
    }

    private static void EnsureSampleFile()
    {
        if (!File.Exists(InputFile))
        {
            File.WriteAllText(InputFile, "3 4 1,56 1,7 1,cat 2 5 4");
            Console.WriteLine($"  → Создан пример файла: {InputFile}");
        }
    }
}
