namespace DynamicStructures.Part4_LinkedListAlgos;

/// <summary>
/// Задача 3: Определить количество различных элементов списка целых чисел.
/// </summary>
public static class Task3
{
    /// <summary>
    /// Возвращает количество различных значений в списке.
    /// Сложность: O(n) по времени (в среднем), O(k) по памяти, k — число различных значений.
    /// Для пустого списка возвращает 0.
    /// </summary>
    public static int CountDistinct(MyLinkedList list)
    {
        var seen = new HashSet<int>();
        ListNode? current = list.Head;

        while (current is not null)
        {
            seen.Add(current.Value);
            current = current.Next;
        }

        return seen.Count;
    }
}
