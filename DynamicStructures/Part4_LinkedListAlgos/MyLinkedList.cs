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
/// </summary>
public class MyLinkedList
{
    public ListNode? Head { get; set; }
    private int _count;

    public MyLinkedList() { }

    /// <summary>Создать список из массива значений.</summary>
    public MyLinkedList(IEnumerable<int> values)
    {
        foreach (int v in values)
            AddLast(v);
    }

    /// <summary>Добавить элемент в конец списка.</summary>
    public void AddLast(int value)
    {
        var node = new ListNode(value);
        if (Head is null)
        {
            Head = node;
        }
        else
        {
            var current = Head;
            while (current.Next is not null)
                current = current.Next;
            current.Next = node;
        }
        _count++;
    }

    /// <summary>Добавить элемент в начало списка.</summary>
    public void AddFirst(int value)
    {
        var node = new ListNode(value) { Next = Head };
        Head = node;
        _count++;
    }

    /// <summary>Количество элементов.</summary>
    public int Count => _count;

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
        var result = new int[_count];
        var current = Head;
        for (int i = 0; i < _count; i++)
        {
            result[i] = current!.Value;
            current = current.Next;
        }
        return result;
    }

    /// <summary>Создать независимую копию списка (deep copy).</summary>
    public MyLinkedList Clone()
    {
        var clone = new MyLinkedList();
        var current = Head;
        while (current is not null)
        {
            clone.AddLast(current.Value);
            current = current.Next;
        }
        return clone;
    }
}
