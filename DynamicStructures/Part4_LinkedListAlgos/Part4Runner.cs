namespace DynamicStructures.Part4_LinkedListAlgos;

/// <summary>
/// Запуск демонстрации задач части 4 (алгоритмы над связными списками).
/// Задачи 9–12: заглушки (NotImplementedException).
/// Задачи 1–8: реализованы.
/// </summary>
public static class Part4Runner
{
    public static void Run()
    {
        Console.WriteLine("\n╔══════════════════════════════════════════════╗");
        Console.WriteLine("║   Часть 4: Алгоритмы связных списков        ║");
        Console.WriteLine("╚══════════════════════════════════════════════╝\n");

        Console.WriteLine("  [1–8]  Реализованы");
        Console.WriteLine("  [9–12] Не реализованы (заглушки для коллег)\n");
        Console.WriteLine("  Выберите задачу (1–8) или 0 для выхода:\n");

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

                // ── Реализованные задачи ─────────────────────────────────────
                case 1: RunTask1(); break;
                case 2: RunTask2(); break;
                case 3: RunTask3(); break;
                case 4: RunTask4(); break;
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

    // ── Задача 1 ─────────────────────────────────────────────────────────────

    private static void RunTask1()
    {
        Console.WriteLine("\n  ── Задача 1: Переворот списка ──────────────────");
        var list = new MyLinkedList(new[] { 1, 2, 3, 4, 5 });
        list.Print("До     ");
        Task1.Reverse(list);
        list.Print("После  ");
        Console.WriteLine("  Ожидается: 5 → 4 → 3 → 2 → 1\n");

        var single = new MyLinkedList(new[] { 7 });
        Task1.Reverse(single);
        single.Print("Один элемент");

        var empty = new MyLinkedList();
        Task1.Reverse(empty);
        empty.Print("Пустой список");
        Console.WriteLine();
    }

    // ── Задача 2 ─────────────────────────────────────────────────────────────

    private static void RunTask2()
    {
        Console.WriteLine("\n  ── Задача 2: Перенос последнего/первого элемента ──");
        var list = new MyLinkedList(new[] { 1, 2, 3, 4, 5 });
        list.Print("До (последний → в начало)");
        Task2.MoveLastToFirst(list);
        list.Print("После                    ");
        Console.WriteLine("  Ожидается: 5 → 1 → 2 → 3 → 4\n");

        var list2 = new MyLinkedList(new[] { 1, 2, 3, 4, 5 });
        list2.Print("До (первый → в конец)");
        Task2.MoveFirstToLast(list2);
        list2.Print("После                ");
        Console.WriteLine("  Ожидается: 2 → 3 → 4 → 5 → 1\n");
    }

    // ── Задача 3 ─────────────────────────────────────────────────────────────

    private static void RunTask3()
    {
        Console.WriteLine("\n  ── Задача 3: Количество различных элементов ────");
        var list = new MyLinkedList(new[] { 1, 2, 1, 3, 2, 4, 4 });
        list.Print("Список");
        Console.WriteLine($"  Различных элементов: {Task3.CountDistinct(list)}");
        Console.WriteLine("  Ожидается: 4\n");

        var empty = new MyLinkedList();
        Console.WriteLine($"  Пустой список: {Task3.CountDistinct(empty)} (ожидается 0)\n");
    }

    // ── Задача 4 ─────────────────────────────────────────────────────────────

    private static void RunTask4()
    {
        Console.WriteLine("\n  ── Задача 4: Удаление неуникальных элементов ───");
        var list = new MyLinkedList(new[] { 1, 2, 1, 3, 2, 4, 4, 1 });
        list.Print("До     ");
        Task4.RemoveDuplicates(list);
        list.Print("После  ");
        Console.WriteLine("  Ожидается: 1 → 2 → 3 → 4\n");
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
