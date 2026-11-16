namespace DynamicStructures.Part4_LinkedListAlgos;

using DynamicStructures.Common;

/// <summary>
/// Запуск демонстрации задач части 4 (алгоритмы над связными списками).
/// Задачи 1–8 демонстрируются на примерах, 9–12 читают списки из файлов.
/// </summary>
public static class Part4Runner
{
    public static void Run()
    {
        Console.WriteLine("\n╔══════════════════════════════════════════════╗");
        Console.WriteLine("║   Часть 4: Алгоритмы связных списков        ║");
        Console.WriteLine("╚══════════════════════════════════════════════╝\n");

        Console.WriteLine("  1 — Перевернуть список       2 — Перенести крайний элемент");
        Console.WriteLine("  3 — Различные значения      4 — Удалить повторы");
        Console.WriteLine("  5 — Вставить список в себя  6 — Вставить в упорядоченный список");
        Console.WriteLine("  7 — Удалить все вхождения   8 — Вставить перед первым вхождением");
        Console.WriteLine("  9 — Дописать список E к L (оба из файла)");
        Console.WriteLine(" 10 — Разбить список после первого вхождения числа");
        Console.WriteLine(" 11 — Удвоить список");
        Console.WriteLine(" 12 — Поменять местами два элемента");
        Console.WriteLine("  0 — Назад\n");

        bool running = true;
        while (running)
        {
            Console.Write("  Задача > ");
            string input = ConsoleInput.ReadLine();
            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("  Введите число от 0 до 12.");
                continue;
            }

            try
            {
                switch (choice)
                {
                    case 0:
                        running = false;
                        break;

                    case 1: RunTask1(); break;
                    case 2: RunTask2(); break;
                    case 3: RunTask3(); break;
                    case 4: RunTask4(); break;
                    case 5: RunTask5(); break;
                    case 6: RunTask6(); break;
                    case 7: RunTask7(); break;
                    case 8: RunTask8(); break;

                    case 9: RunTask9(); break;
                    case 10: RunTask10(); break;
                    case 11: RunTask11(); break;
                    case 12: RunTask12(); break;

                    default:
                        Console.WriteLine("  Введите число от 0 до 12.");
                        break;
                }
            }
            catch (Exception ex) when (ex is not EndOfStreamException &&
                ex is IOException or UnauthorizedAccessException or FormatException or ArgumentException)
            {
                Console.WriteLine($"  Ошибка: {ex.Message}\n");
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

    private static void RunTask9()
    {
        Console.WriteLine("\n  ── Задача 9: Дописать список E к L ──");
        Console.WriteLine("  В файле две строки: первая — L, вторая — E. Числа через пробел.");
        var lists = ReadLists(9, "1 2 3\n4 5 6", 2);
        lists[0].Print("L до");
        lists[1].Print("E");
        Task9.AppendList(lists[0], lists[1]);
        lists[0].Print("L после");
        lists[1].Print("E (сохранён)");
    }

    private static void RunTask10()
    {
        Console.WriteLine("\n  ── Задача 10: Разбить список ──");
        var list = ReadLists(10, "1 2 3 2 4", 1)[0];
        list.Print("До");
        int x = ConsoleInput.ReadInt("  Число x (Enter = 2): ", 2);
        Console.WriteLine("  Первое вхождение x остаётся в первом списке; остаток переходит во второй.");
        var second = Task10.SplitAtFirst(list, x);
        list.Print("Первый");
        second.Print("Второй");
    }

    private static void RunTask11()
    {
        Console.WriteLine("\n  ── Задача 11: Удвоить список ──");
        var list = ReadLists(11, "1 2 3", 1)[0];
        list.Print("До");
        Task11.DoubleList(list);
        list.Print("После");
    }

    private static void RunTask12()
    {
        Console.WriteLine("\n  ── Задача 12: Перестановка двух элементов ──");
        var list = ReadLists(12, "10 20 30 40 50", 1)[0];
        list.Print("До");
        if (list.IsEmpty)
        {
            Console.WriteLine("  В пустом списке нет элементов для перестановки.");
            return;
        }

        int mode = ConsoleInput.ReadInt("  Режим: 1 — по позициям, 2 — по значениям (Enter = 1): ", 1, 1, 2);
        if (mode == 1)
        {
            int count = list.Count;
            int first = ConsoleInput.ReadInt($"  Первая позиция (1..{count}, Enter = 1): ", 1, 1, count);
            int second = ConsoleInput.ReadInt($"  Вторая позиция (1..{count}, Enter = {count}): ", count, 1, count);
            Task12.SwapByPosition(list, first - 1, second - 1);
        }
        else
        {
            int defaultValue = list.Head!.Value;
            int first = ConsoleInput.ReadInt($"  Первое значение (Enter = {defaultValue}): ", defaultValue);
            int second = ConsoleInput.ReadInt($"  Второе значение (Enter = {defaultValue}): ", defaultValue);
            Console.WriteLine("  Меняются местами только первые вхождения выбранных значений.");
            Task12.SwapByValue(list, first, second);
        }
        list.Print("После");
    }

    private static MyLinkedList[] ReadLists(int task, string sample, int count)
    {
        string defaultPath = DataFiles.EnsureSample("LinkedLists", $"task{task}.txt", sample);
        return ListFileReader.Read(DataFiles.AskPath(defaultPath), count);
    }
}
