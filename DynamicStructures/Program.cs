using DynamicStructures.Part1_Stack;
using DynamicStructures.Part2_Queue;
using DynamicStructures.Part3_Algorithms;
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

        case 0:
            running = false;
            Console.WriteLine("\nДо свидания!");
            break;

        default:
            Console.WriteLine("  Неверный выбор. Введите 0–4.\n");
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
    Console.WriteLine("╔══════════════════════════════════════════════════════╗");
    Console.WriteLine("║    Практическое занятие №3: Динамические структуры   ║");
    Console.WriteLine("╠══════════════════════════════════════════════════════╣");
    Console.WriteLine("║  1 — Часть 1: Стек (свой связный список)            ║");
    Console.WriteLine("║  2 — Часть 2: Очередь (своя + Queue<T>)             ║");
    Console.WriteLine("║  3 — Часть 3: Алгоритмы постфиксных вычислений      ║");
    Console.WriteLine("║  4 — Часть 4: Алгоритмы связных списков (5–8)       ║");
    Console.WriteLine("║  0 — Выход                                          ║");
    Console.WriteLine("╚══════════════════════════════════════════════════════╝");
}
