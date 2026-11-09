namespace DynamicStructures.Part2_Queue;

/// <summary>
/// Очередь (FIFO), реализованная через собственный односвязный список.
/// Поддерживает операции: Enqueue, Dequeue, Peek, IsEmpty, Print.
/// </summary>
/// <typeparam name="T">Тип элементов очереди.</typeparam>
public class LinkedQueue<T>
{
    private QueueNode<T>? _head; // начало очереди (откуда извлекаем)
    private QueueNode<T>? _tail; // конец очереди (куда добавляем)
    private int _count;

    public LinkedQueue()
    {
        _head = null;
        _tail = null;
        _count = 0;
    }

    /// <summary>Добавить элемент в конец очереди.</summary>
    public void Enqueue(T value)
    {
        var node = new QueueNode<T>(value);
        if (_tail is not null)
            _tail.Next = node;
        _tail = node;
        _head ??= node;
        _count++;
    }

    /// <summary>Извлечь и вернуть первый элемент очереди.</summary>
    /// <exception cref="InvalidOperationException">Если очередь пуста.</exception>
    public T Dequeue()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Очередь пуста — операция Dequeue невозможна.");

        T value = _head!.Value;
        _head = _head.Next;
        if (_head is null) _tail = null;
        _count--;
        return value;
    }

    /// <summary>Просмотреть первый элемент без извлечения.</summary>
    /// <exception cref="InvalidOperationException">Если очередь пуста.</exception>
    public T Peek()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Очередь пуста — операция Peek невозможна.");

        return _head!.Value;
    }

    /// <summary>Проверить, пуста ли очередь.</summary>
    public bool IsEmpty() => _head is null;

    /// <summary>Вывести содержимое очереди от начала к концу.</summary>
    public void Print()
    {
        if (IsEmpty())
        {
            Console.WriteLine("  [очередь пуста]");
            return;
        }

        var current = _head;
        var parts = new System.Text.StringBuilder();
        parts.Append("  Очередь (начало → конец): ");
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
