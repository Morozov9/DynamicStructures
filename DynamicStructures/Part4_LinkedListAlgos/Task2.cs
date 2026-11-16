namespace DynamicStructures.Part4_LinkedListAlgos;

/// <summary>
/// Задача 2: Перенести в начало (в конец) непустого списка L его последний (первый) элемент.
/// </summary>
public static class Task2
{
    /// <summary>
    /// Переносит последний элемент в начало списка (узлы не копируются, меняются ссылки).
    /// Для пустого списка и списка из одного элемента ничего не делает.
    /// </summary>
    public static void MoveLastToFirst(MyLinkedList list)
    {
        if (list.Head is null || list.Head.Next is null) return;

        // Ищем предпоследний узел
        ListNode prev = list.Head;
        while (prev.Next!.Next is not null)
            prev = prev.Next;

        ListNode last = prev.Next;   // последний узел
        prev.Next = null;            // отцепляем его от хвоста
        last.Next = list.Head;       // ставим перед бывшим первым
        list.Head = last;            // он становится головой
    }

    /// <summary>
    /// Переносит первый элемент в конец списка (узлы не копируются, меняются ссылки).
    /// Для пустого списка и списка из одного элемента ничего не делает.
    /// </summary>
    public static void MoveFirstToLast(MyLinkedList list)
    {
        if (list.Head is null || list.Head.Next is null) return;

        ListNode first = list.Head;
        list.Head = first.Next;      // голова сдвигается на второй элемент
        first.Next = null;

        // Ищем новый хвост и цепляем к нему бывший первый узел
        ListNode tail = list.Head;
        while (tail.Next is not null)
            tail = tail.Next;
        tail.Next = first;
    }
}
