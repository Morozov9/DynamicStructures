namespace DynamicStructures.Part4_LinkedListAlgos;

/// <summary>
/// Задача 1: Перевернуть список L — изменить ссылки так, чтобы элементы
/// оказались в обратном порядке.
/// </summary>
public static class Task1
{
    /// <summary>
    /// Переворачивает список на месте (меняет ссылки, новые узлы не создаются).
    /// Сложность: O(n) по времени, O(1) по памяти.
    /// </summary>
    /// <param name="list">Список для переворота.</param>
    public static void Reverse(MyLinkedList list)
    {
        ListNode? prev = null;
        ListNode? current = list.Head;

        while (current is not null)
        {
            ListNode? next = current.Next; // запоминаем следующий узел
            current.Next = prev;           // разворачиваем ссылку назад
            prev = current;                // сдвигаем prev
            current = next;                // сдвигаем current
        }

        list.Head = prev; // prev указывает на бывший последний узел
    }
}
