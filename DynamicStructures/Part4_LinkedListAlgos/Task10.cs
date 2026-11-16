namespace DynamicStructures.Part4_LinkedListAlgos;

/// <summary>
/// Задача 10: Разбить список целых чисел на два по первому вхождению числа x.
/// Если x не найден — второй список пуст, первый не изменяется.
/// </summary>
public static class Task10
{
    /// <summary>
    /// Разбивает список l на два: l — до x (включительно), second — остаток.
    /// </summary>
    /// <returns>Второй список (после x).</returns>
    public static MyLinkedList SplitAtFirst(MyLinkedList l, int x)
    {
        ArgumentNullException.ThrowIfNull(l);
        var second = new MyLinkedList();
        for (var current = l.Head; current is not null; current = current.Next)
        {
            if (current.Value != x) continue;
            second.Head = current.Next;
            current.Next = null;
            break;
        }
        return second;
    }
}
