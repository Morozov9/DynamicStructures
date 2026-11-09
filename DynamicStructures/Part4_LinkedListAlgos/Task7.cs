namespace DynamicStructures.Part4_LinkedListAlgos;

/// <summary>
/// Задача 7: Удалить из списка L все вхождения элемента E (если они есть).
/// </summary>
public static class Task7
{
    /// <summary>
    /// Удаляет из списка все узлы со значением e.
    /// Работает с заголовком Head через публичный интерфейс MyLinkedList.
    /// </summary>
    /// <param name="list">Список для обработки.</param>
    /// <param name="e">Значение, все вхождения которого нужно удалить.</param>
    public static void RemoveAllOccurrences(MyLinkedList list, int e)
    {
        if (list.IsEmpty) return;

        // Пропускаем совпадающие узлы в начале
        while (list.Head is not null && list.Head.Value == e)
        {
            list.Head = list.Head.Next;
        }

        if (list.Head is null) return;

        // Проходим по оставшимся узлам
        var current = list.Head;
        while (current.Next is not null)
        {
            if (current.Next.Value == e)
            {
                // Удаляем current.Next из цепочки
                current.Next = current.Next.Next;
            }
            else
            {
                current = current.Next;
            }
        }
    }
}
