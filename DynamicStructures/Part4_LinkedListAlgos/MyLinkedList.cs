namespace DynamicStructures.Part4_LinkedListAlgos;

/// <summary>
/// Узел односвязного списка целых чисел.
/// </summary>
public class ListNode
{
    public int Value { get; set; }
    public ListNode? Next { get; set; }

    public ListNode(int value)
    {
        Value = value;
        Next = null;
    }
}

/// <summary>
/// Собственная реализация односвязного списка целых чисел.
/// Используется во всех задачах части 4.
/// Количество элементов не хранится отдельным полем, а считается обходом списка,
/// поэтому остаётся верным, даже если алгоритмы меняют ссылки (Head/Next) напрямую.
/// </summary>
public class MyLinkedList
{
    public ListNode? Head { get; set; }

    public MyLinkedList() { }

    /// <summary>Создать список из массива значений.</summary>
    public MyLinkedList(IEnumerable<int> values)
    {
        ListNode? tail = null;
        foreach (int v in values)
        {
            var node = new ListNode(v);
            if (tail is null) Head = node;
            else tail.Next = node;
            tail = node;
        }
    }

    /// <summary>Добавить элемент в конец списка.</summary>
    public void AddLast(int value)
    {
        var node = new ListNode(value);
        if (Head is null)
        {
            Head = node;
            return;
        }

        var current = Head;
        while (current.Next is not null)
            current = current.Next;
        current.Next = node;
    }

    /// <summary>Добавить элемент в начало списка.</summary>
    public void AddFirst(int value)
    {
        Head = new ListNode(value) { Next = Head };
    }

    /// <summary>Количество элементов (подсчёт обходом списка, O(n)).</summary>
    public int Count
    {
        get
        {
            int n = 0;
            for (var cur = Head; cur is not null; cur = cur.Next) n++;
            return n;
        }
    }

    /// <summary>Пуст ли список.</summary>
    public bool IsEmpty => Head is null;

    /// <summary>Вывести список в консоль.</summary>
    public void Print(string label = "")
    {
        if (!string.IsNullOrEmpty(label))
            Console.Write($"  {label}: ");
        else
            Console.Write("  Список: ");

        if (IsEmpty)
        {
            Console.WriteLine("[пуст]");
            return;
        }

        var current = Head;
        while (current is not null)
        {
            Console.Write(current.Value);
            if (current.Next is not null) Console.Write(" → ");
            current = current.Next;
        }
        Console.WriteLine();
    }

    /// <summary>Преобразовать в массив.</summary>
    public int[] ToArray()
    {
        var result = new List<int>();
        for (var cur = Head; cur is not null; cur = cur.Next)
            result.Add(cur.Value);
        return result.ToArray();
    }

    /// <summary>Создать независимую копию списка (deep copy).</summary>
    public MyLinkedList Clone() => new MyLinkedList(ToArray());
}
