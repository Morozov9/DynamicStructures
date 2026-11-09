namespace DynamicStructures.Part2_Queue;

/// <summary>
/// Узел двусвязного списка для реализации очереди.
/// </summary>
/// <typeparam name="T">Тип хранимого значения.</typeparam>
public class QueueNode<T>
{
    public T Value { get; set; }
    public QueueNode<T>? Next { get; set; }

    public QueueNode(T value)
    {
        Value = value;
        Next = null;
    }
}
