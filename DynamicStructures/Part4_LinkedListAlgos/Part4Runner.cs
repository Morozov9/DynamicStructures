namespace DynamicStructures.Part4_LinkedListAlgos;

/// <summary>
/// Запуск демонстрации задач части 4 (алгоритмы над связными списками).
/// Задачи 1–4, 9–12: заглушки (NotImplementedException).
/// Задачи 5–8: полностью реализованы.
/// </summary>
public static class Part4Runner
{
    public static void Run()
    {
        Console.WriteLine("\n╔══════════════════════════════════════════════╗");
        Console.WriteLine("║   Часть 4: Алгоритмы связных списков        ║");
        Console.WriteLine("╚══════════════════════════════════════════════╝\n");

        Console.WriteLine("  [1–4] Не реализованы (заглушки для коллег)");
        Console.WriteLine("  [5–8] Реализованы полностью\n");
        Console.WriteLine("  Выберите задачу (5–8) или 0 для выхода:\n");

        bool running = true;
        while (running)
        {
            Console.Write("  Задача > ");
            string? input = Console.ReadLine();
            if (!int.TryParse(input, out int choice)) continue;

            switch (choice)
            {
                case 0:
                    running = false;
                    break;

                // ── Заглушки ────────────────────────────────────────────────
                case 1: TryRunStub("Задача 1: Переворот списка", () => Task1.Reverse(new MyLinkedList())); break;
                case 2: TryRunStub("Задача 2: Перенос элемента", () => Task2.MoveLastToFirst(new MyLinkedList())); break;
                case 3: TryRunStub("Задача 3: Количество различных", () => Task3.CountDistinct(new MyLinkedList())); break;
                case 4: TryRunStub("Задача 4: Удаление неуникальных", () => Task4.RemoveDuplicates(new MyLinkedList())); break;

                // ── Реализованные задачи ─────────────────────────────────────
                case 5: RunTask5(); break;
                case 6: RunTask6(); break;
                case 7: RunTask7(); break;
                case 8: RunTask8(); break;

                // ── Заглушки ─────────────────────────────────────────────────
                case 9:  TryRunStub("Задача 9: Дописать список", () => Task9.AppendList(new MyLinkedList(), new MyLinkedList())); break;
                case 10: TryRunStub("Задача 10: Разбивка списка", () => Task10.SplitAtFirst(new MyLinkedList(), 0)); break;
                case 11: TryRunStub("Задача 11: Удвоение списка", () => Task11.DoubleList(new MyLinkedList())); break;
                case 12: TryRunStub("Задача 12: Перестановка", () => Task12.SwapByPosition(new MyLinkedList(), 0, 1)); break;

                default:
                    Console.WriteLine("  Введите число от 0 до 12.");
                    break;
            }
        }
    }

    // ── Задача 5 ─────────────────────────────────────────────────────────────

    private static void RunTask5()
    {
        Console.WriteLine("\n  ── Задача 5: Вставка списка в себя вслед за x ──");
        var list = new MyLinkedList(new[] { 1, 2, 3 });
        list.Print("До (L=[1,2,3], x=2)");
        Task5.InsertSelfAfterFirstX(list, 2);
        list.Print("После              ");
        Console.WriteLine("  Ожидается: 1 → 2 → 1 → 2 → 3 → 3\n");
    }

    // ── Задача 6 ─────────────────────────────────────────────────────────────

    private static void RunTask6()
    {
        Console.WriteLine("\n  ── Задача 6: Вставка E в упорядоченный список ──");
        var list = new MyLinkedList(new[] { 1, 3, 5, 7, 9 });
        list.Print("До (sorted, E=4) ");
        Task6.InsertSorted(list, 4);
        list.Print("После            ");
        Console.WriteLine("  Ожидается: 1 → 3 → 4 → 5 → 7 → 9\n");

        // Вставка в начало
        var list2 = new MyLinkedList(new[] { 5, 10, 15 });
        list2.Print("До (sorted, E=2)");
        Task6.InsertSorted(list2, 2);
        list2.Print("После           ");
        Console.WriteLine("  Ожидается: 2 → 5 → 10 → 15\n");
    }

    // ── Задача 7 ─────────────────────────────────────────────────────────────

    private static void RunTask7()
    {
        Console.WriteLine("\n  ── Задача 7: Удаление всех вхождений E ──────────");
        var list = new MyLinkedList(new[] { 1, 2, 3, 2, 4, 2, 5 });
        list.Print("До (E=2)  ");
        Task7.RemoveAllOccurrences(list, 2);
        list.Print("После     ");
        Console.WriteLine("  Ожидается: 1 → 3 → 4 → 5\n");

        // Удаление с начала
        var list2 = new MyLinkedList(new[] { 2, 2, 1, 2, 3 });
        list2.Print("До (E=2)  ");
        Task7.RemoveAllOccurrences(list2, 2);
        list2.Print("После     ");
        Console.WriteLine("  Ожидается: 1 → 3\n");
    }

    // ── Задача 8 ─────────────────────────────────────────────────────────────

    private static void RunTask8()
    {
        Console.WriteLine("\n  ── Задача 8: Вставка F перед первым вхождением E ──");
        var list = new MyLinkedList(new[] { 1, 2, 3, 4, 5 });
        list.Print("До (E=3, F=99)");
        Task8.InsertBeforeFirstOccurrence(list, 3, 99);
        list.Print("После         ");
        Console.WriteLine("  Ожидается: 1 → 2 → 99 → 3 → 4 → 5\n");

        // Вставка перед первым элементом
        var list2 = new MyLinkedList(new[] { 3, 4, 5 });
        list2.Print("До (E=3, F=0)");
        Task8.InsertBeforeFirstOccurrence(list2, 3, 0);
        list2.Print("После        ");
        Console.WriteLine("  Ожидается: 0 → 3 → 4 → 5\n");
    }

    // ── Вспомогательное: запуск заглушки с перехватом ────────────────────────

    private static void TryRunStub(string name, Action action)
    {
        try
        {
            action();
        }
        catch (NotImplementedException)
        {
            Console.WriteLine($"\n  [{name}] — ещё не реализована (заглушка для коллег).\n");
        }
    }
}
