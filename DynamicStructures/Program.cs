using DynamicStructures.Part1_Stack;
using DynamicStructures.Part2_Queue;
using DynamicStructures.Part3_Algorithms;
using DynamicStructures.Part3_Applications;
using DynamicStructures.Part4_LinkedListAlgos;

// ═══════════════════════════════════════════════════════════════════════
//  Практическое занятие №3 — Динамические структуры данных
//  Авторы: Друзовский, Дулисов, Ротгань
//  Дата:  2026
// ═══════════════════════════════════════════════════════════════════════

Console.OutputEncoding = System.Text.Encoding.UTF8;

bool running = true;
while (running)
{
    PrintMenu();
    Console.Write("Выберите раздел: ");
    string? input = Console.ReadLine();

    if (!int.TryParse(input, out int choice))
    {
        Console.WriteLine("  Введите число.\n");
        continue;
    }

    switch (choice)
    {
        case 1:
            StackRunner.Run();
            break;

        case 2:
            QueueRunner.Run();
            break;

        case 3:
            Part3Runner.Run();
            break;

        case 4:
            Part4Runner.Run();
            break;

        case 5:
            ApplicationsRunner.Run();
            break;

        case 0:
            running = false;
            Console.WriteLine("\nДо свидания!");
            break;

        default:
            Console.WriteLine("  Неверный выбор. Введите 0–5.\n");
            break;
    }

    if (running)
    {
        Console.WriteLine("\nНажмите Enter для возврата в меню...");
        Console.ReadLine();
    }
}

static void PrintMenu()
{
    try { Console.Clear(); } catch { /* не все среды поддерживают Clear */ }
    const int W = 58;
    static string Line(string text) => "║" + text.PadRight(W) + "║";
    Console.WriteLine("╔" + new string('═', W) + "╗");
    Console.WriteLine(Line("   Практическое занятие №3: Динамические структуры"));
    Console.WriteLine("╠" + new string('═', W) + "╣");
    Console.WriteLine(Line("  1 — Часть 1: Стек (свой связный список)"));
    Console.WriteLine(Line("  2 — Часть 2: Очередь (своя + Queue<T>)"));
    Console.WriteLine(Line("  3 — Часть 1, п.4: Постфиксные вычисления"));
    Console.WriteLine(Line("  4 — Часть 4: Алгоритмы связных списков"));
    Console.WriteLine(Line("  5 — Часть 3: Применение структур (Иосиф, Flood Fill)"));
    Console.WriteLine(Line("  0 — Выход"));
    Console.WriteLine("╚" + new string('═', W) + "╝");
}
