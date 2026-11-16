namespace DynamicStructures.Part4_LinkedListAlgos;

/// <summary>
/// Задача 9: Дописать к списку L список E.
/// Оба списка содержат целые числа, считанные из файла.
/// </summary>
public static class Task9
{
    /// <summary>
    /// Дописывает независимую копию e в конец l; e не изменяется.
    /// Допускается передать один и тот же список. Время O(|l| + |e|), память O(|e|).
    /// </summary>
    public static void AppendList(MyLinkedList l, MyLinkedList e)
    {
        ArgumentNullException.ThrowIfNull(l);
        ArgumentNullException.ThrowIfNull(e);

        // Сначала завершаем копирование: l и e могут быть одним списком.
        var copy = e.Clone();
        if (copy.IsEmpty) return;
        if (l.Head is null)
        {
            l.Head = copy.Head;
            return;
        }

        var tail = l.Head;
        while (tail.Next is not null)
            tail = tail.Next;
        tail.Next = copy.Head;
    }
}
