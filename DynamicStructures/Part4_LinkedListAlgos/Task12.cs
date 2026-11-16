namespace DynamicStructures.Part4_LinkedListAlgos;

/// <summary>
/// Задача 12: Поменять местами два элемента списка, заданных пользователем.
/// </summary>
public static class Task12
{
    /// <summary>
    /// Меняет местами значения на позициях pos1 и pos2 (нумерация с нуля).
    /// При недопустимой позиции бросает ArgumentOutOfRangeException, не меняя список.
    /// </summary>
    public static void SwapByPosition(MyLinkedList list, int pos1, int pos2)
    {
        ArgumentNullException.ThrowIfNull(list);
        ArgumentOutOfRangeException.ThrowIfNegative(pos1);
        ArgumentOutOfRangeException.ThrowIfNegative(pos2);

        ListNode? first = null, second = null;
        int index = 0;
        for (var node = list.Head; node is not null; node = node.Next, index++)
        {
            if (index == pos1) first = node;
            if (index == pos2) second = node;
            if (first is not null && second is not null) break;
        }

        if (first is null)
            throw new ArgumentOutOfRangeException(nameof(pos1), "Позиция за пределами списка.");
        if (second is null)
            throw new ArgumentOutOfRangeException(nameof(pos2), "Позиция за пределами списка.");
        (first.Value, second.Value) = (second.Value, first.Value);
    }

    /// <summary>
    /// Меняет местами первые вхождения значений val1 и val2.
    /// Если значение не найдено, бросает ArgumentException, не меняя список.
    /// </summary>
    public static void SwapByValue(MyLinkedList list, int val1, int val2)
    {
        ArgumentNullException.ThrowIfNull(list);
        ListNode? first = null, second = null;
        for (var node = list.Head; node is not null; node = node.Next)
        {
            if (first is null && node.Value == val1) first = node;
            if (second is null && node.Value == val2) second = node;
            if (first is not null && second is not null) break;
        }

        if (first is null)
            throw new ArgumentException($"Значение {val1} не найдено в списке.", nameof(val1));
        if (second is null)
            throw new ArgumentException($"Значение {val2} не найдено в списке.", nameof(val2));
        (first.Value, second.Value) = (second.Value, first.Value);
    }
}
