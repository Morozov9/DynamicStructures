namespace DynamicStructures.Part1_Stack;

/// <summary>
/// Стек, реализованный через собственный односвязный список.
/// Поддерживает операции: Push, Pop, Top, IsEmpty, Print.
/// </summary>
/// <typeparam name="T">Тип элементов стека.</typeparam>
public class LinkedStack<T>
{
    private Node<T>? _top;
    private int _count;

    public LinkedStack()
    {
        _top = null;
        _count = 0;
    }

    /// <summary>Добавить элемент на вершину стека.</summary>
    public void Push(T value)
    {
        var node = new Node<T>(value)
        {
            Next = _top
        };
        _top = node;
        _count++;
    }

    /// <summary>Извлечь и вернуть верхний элемент стека.</summary>
    /// <exception cref="InvalidOperationException">Если стек пуст.</exception>
    public T Pop()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Стек пуст — операция Pop невозможна.");

        T value = _top!.Value;
        _top = _top.Next;
        _count--;
        return value;
    }

    /// <summary>Просмотреть верхний элемент без извлечения.</summary>
    /// <exception cref="InvalidOperationException">Если стек пуст.</exception>
    public T Top()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Стек пуст — операция Top невозможна.");

        return _top!.Value;
    }

    /// <summary>Проверить, пуст ли стек.</summary>
    public bool IsEmpty() => _top is null;

    /// <summary>Вывести содержимое стека от вершины к основанию.</summary>
    public void Print()
    {
        if (IsEmpty())
        {
            Console.WriteLine("  [стек пуст]");
            return;
        }

        var current = _top;
        var parts = new System.Text.StringBuilder();
        parts.Append("  Стек (вершина → основание): ");
        while (current is not null)
        {
            parts.Append(current.Value);
            if (current.Next is not null) parts.Append(" → ");
            current = current.Next;
        }
        Console.WriteLine(parts.ToString());
        Console.WriteLine($"  Всего элементов: {_count}");
    }
}
