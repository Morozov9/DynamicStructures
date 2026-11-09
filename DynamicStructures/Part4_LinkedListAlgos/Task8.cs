namespace DynamicStructures.Part4_LinkedListAlgos;

/// <summary>
/// Задача 8: Вставить в список L новый элемент F перед первым вхождением E,
/// если E входит в L.
/// </summary>
public static class Task8
{
    /// <summary>
    /// Вставляет элемент f перед первым вхождением e.
    /// Если e не найден — список не изменяется.
    /// </summary>
    /// <param name="list">Список для обработки.</param>
    /// <param name="e">Элемент, перед которым вставляем.</param>
    /// <param name="f">Новый элемент для вставки.</param>
    public static void InsertBeforeFirstOccurrence(MyLinkedList list, int e, int f)
    {
        if (list.IsEmpty) return;

        // Случай: e — первый элемент
        if (list.Head!.Value == e)
        {
            var newNode = new ListNode(f) { Next = list.Head };
            list.Head = newNode;
            return;
        }

        // Ищем узел, следующий за которым стоит e
        var current = list.Head;
        while (current.Next is not null)
        {
            if (current.Next.Value == e)
            {
                // Вставляем f между current и current.Next
                var newNode = new ListNode(f) { Next = current.Next };
                current.Next = newNode;
                return;
            }
            current = current.Next;
        }

        // e не найден — список не меняется
    }
}
