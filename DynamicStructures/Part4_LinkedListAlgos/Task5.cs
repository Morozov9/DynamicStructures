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

        // Запоминаем, что стоит после x (туда воткнём конец копии)
        ListNode? afterX = insertAfter.Next;

        // Копируем весь список и вставляем после insertAfter
        // Копируем поузлово
        var src = list.Head;
        ListNode? insertPoint = insertAfter;
        while (src is not null)
        {
            var newNode = new ListNode(src.Value) { Next = afterX };
            insertPoint!.Next = newNode;
            insertPoint = newNode;
            src = src.Next;
        }
        // insertPoint.Next уже указывает на afterX — хвост сшит
    }
}
