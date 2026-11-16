namespace DynamicStructures.Part4_LinkedListAlgos;

/// <summary>
/// Задача 5: Вставить список L в себя вслед за первым вхождением числа x.
///
/// Если x = 2, L = [1, 2, 3], результат: [1, 2, 1, 2, 3, 3]
/// </summary>
public static class Task5
{
    /// <summary>
    /// Вставляет копию списка L вслед за первым вхождением x.
    /// Если x не найден — список не изменяется.
    /// </summary>
    /// <param name="list">Исходный список.</param>
    /// <param name="x">Значение, после которого вставляется копия.</param>
    public static void InsertSelfAfterFirstX(MyLinkedList list, int x)
    {
        if (list.IsEmpty) return;

        // Ищем первое вхождение x
        ListNode? insertAfter = null;
        var current = list.Head;
        while (current is not null)
        {
            if (current.Value == x)
            {
                insertAfter = current;
                break;
            }
            current = current.Next;
        }

        if (insertAfter is null) return; // x не найден

        // Копируем до изменения исходных ссылок, иначе обход попадёт в новые узлы.
        var copy = list.Clone();
        var tail = copy.Head!;
        while (tail.Next is not null)
            tail = tail.Next;
        tail.Next = insertAfter.Next;
        insertAfter.Next = copy.Head;
    }
}
