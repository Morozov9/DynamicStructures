namespace DynamicStructures.Part1_Stack;

/// <summary>
/// Узел односвязного списка.
/// </summary>
/// <typeparam name="T">Тип хранимого значения.</typeparam>
public class Node<T>
{
    public T Value { get; set; }
    public Node<T>? Next { get; set; }

    public Node(T value)
    {
        Value = value;
        Next = null;
    }
}
