using System.Diagnostics;

namespace DynamicStructures.Part1_Stack;

/// <summary>
/// Выполняет команды над стеком из файла input.txt и замеряет время.
/// 
/// Формат команд в файле (числа через пробел):
///   1,значение  → Push(значение)
///   2           → Pop()
///   3           → Top()
///   4           → IsEmpty()
///   5           → Print()
/// 
/// Пример: 3 4 1,56 1,7 1,cat 2 5 4
/// </summary>
public static class StackRunner
{
    private const string InputFile = "input_stack.txt";

    public static void Run()
    {
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║      Часть 1: Стек (LinkedStack)     ║");
        Console.WriteLine("╚══════════════════════════════════════╝\n");

        // Создать пример файла, если его нет
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

        Console.WriteLine();

        // --- Замер времени чтения + выполнения ---
        var totalWatch = Stopwatch.StartNew();

        // Парсинг команд
        string[] tokens = content.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        // Замер только выполнения
        var execWatch = Stopwatch.StartNew();

        var stack = new LinkedStack<string>();
        ExecuteCommands(stack, tokens);

        execWatch.Stop();
        totalWatch.Stop();

        Console.WriteLine();
        Console.WriteLine("── Замер времени ──────────────────────");
        Console.WriteLine($"  Считывание + выполнение: {totalWatch.Elapsed.TotalMicroseconds:F2} мкс");
        Console.WriteLine($"  Только выполнение:        {execWatch.Elapsed.TotalMicroseconds:F2} мкс");
    }

    private static void ExecuteCommands(LinkedStack<string> stack, string[] tokens)
    {
        foreach (string token in tokens)
        {
            // Разбиваем по запятой: "1,56" → command=1, arg="56"
            string[] parts = token.Split(',', 2);
            int command = int.Parse(parts[0]);
            string? arg = parts.Length > 1 ? parts[1] : null;

            switch (command)
            {
                case 1: // Push
                    if (arg is null)
                    {
                        Console.WriteLine("  [Push] Ошибка: не указано значение элемента.");
                        break;
                    }
                    stack.Push(arg);
                    Console.WriteLine($"  [Push] Добавлен элемент: {arg}");
                    break;

                case 2: // Pop
                    try
                    {
                        string popped = stack.Pop();
                        Console.WriteLine($"  [Pop]  Извлечён элемент: {popped}");
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine($"  [Pop]  {e.Message}");
                    }
                    break;

                case 3: // Top
                    try
                    {
                        string top = stack.Top();
                        Console.WriteLine($"  [Top]  Вершина стека: {top}");
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine($"  [Top]  {e.Message}");
                    }
                    break;

                case 4: // IsEmpty
                    Console.WriteLine($"  [IsEmpty] Стек пуст: {stack.IsEmpty()}");
                    break;

                case 5: // Print
                    Console.Write("  [Print] ");
                    stack.Print();
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
