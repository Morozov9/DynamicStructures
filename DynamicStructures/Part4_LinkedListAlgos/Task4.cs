namespace DynamicStructures.Part4_LinkedListAlgos;

/// <summary>
/// Задача 4: Удалить из списка L неуникальные элементы —
/// оставить только первое вхождение каждого значения.
/// Пример: 1 2 1 3 2 4  →  1 2 3 4
/// </summary>
public static class Task4
{
    /// <summary>
    /// Удаляет повторяющиеся элементы, сохраняя первое вхождение и порядок.
    /// Сложность: O(n) по времени (в среднем), O(k) по памяти.
    /// </summary>
    public static void RemoveDuplicates(MyLinkedList list)
    {
        if (list.Head is null) return;

        var seen = new HashSet<int> { list.Head.Value };
        ListNode current = list.Head;

        while (current.Next is not null)
        {
            if (seen.Add(current.Next.Value))
            {
                current = current.Next;           // значение новое — оставляем
            }
            else
            {
                current.Next = current.Next.Next; // значение уже было — выбрасываем узел
            }
        }
    }
}
