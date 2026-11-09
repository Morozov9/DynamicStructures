namespace DynamicStructures.Part4_LinkedListAlgos;

/// <summary>
/// Задача 6: Вставить в непустой упорядоченный (по неубыванию) список L
/// новый элемент E так, чтобы сохранилась упорядоченность.
/// </summary>
public static class Task6
{
    /// <summary>
    /// Вставляет элемент e в отсортированный список, сохраняя порядок неубывания.
    /// </summary>
    /// <param name="list">Упорядоченный список (по неубыванию).</param>
    /// <param name="e">Новый элемент для вставки.</param>
    public static void InsertSorted(MyLinkedList list, int e)
    {
        var newNode = new ListNode(e);

        // Вставка в начало, если список пуст или e меньше первого элемента
        if (list.IsEmpty || list.Head!.Value >= e)
        {
            newNode.Next = list.Head;
            list.Head = newNode;
            return;
        }

        // Ищем место вставки: current.Next.Value >= e
        var current = list.Head;
        while (current.Next is not null && current.Next.Value < e)
            current = current.Next;

        // Вставляем newNode между current и current.Next
        newNode.Next = current.Next;
        current.Next = newNode;
    }
}
