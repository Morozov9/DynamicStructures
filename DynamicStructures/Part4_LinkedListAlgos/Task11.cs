namespace DynamicStructures.Part4_LinkedListAlgos;

/// <summary>
/// Задача 11: Удвоить список — приписать в конец списка копию самого себя.
/// </summary>
public static class Task11
{
    /// <summary>Приписывает в конец списка его копию.</summary>
    public static void DoubleList(MyLinkedList list)
    {
        Task9.AppendList(list, list);
    }
}
