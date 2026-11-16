namespace DynamicStructures.Tests;

internal static class Check
{
    public static int Passed { get; private set; }
    public static int Failed { get; private set; }
    public static int Assertions { get; private set; }

    public static void Case(string name, Action test)
    {
        try
        {
            test();
            Passed++;
            Console.WriteLine($"PASS {name}");
        }
        catch (Exception ex)
        {
            Failed++;
            Console.WriteLine($"FAIL {name}: {ex.Message}");
        }
    }

    public static void True(bool value, string message)
    {
        Assertions++;
        if (!value) throw new InvalidOperationException(message);
    }

    public static void Equal<T>(T expected, T actual)
        => True(EqualityComparer<T>.Default.Equals(expected, actual), $"Expected {expected}; got {actual}.");

    public static void Sequence<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        => True(expected.SequenceEqual(actual), $"Expected [{string.Join(", ", expected)}]; got [{string.Join(", ", actual)}].");

    public static void Throws<T>(Action action) where T : Exception
    {
        Assertions++;
        try { action(); }
        catch (T) { return; }
        throw new InvalidOperationException($"Expected {typeof(T).Name}.");
    }
}
