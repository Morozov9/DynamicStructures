namespace DynamicStructures.Common;

/// <summary>Консольный ввод с проверкой значений и корректным завершением при EOF.</summary>
public static class ConsoleInput
{
    public static string ReadLine() => Console.ReadLine() ?? throw new EndOfStreamException();

    public static int ReadInt(string prompt, int defaultValue, int min = int.MinValue, int max = int.MaxValue)
    {
        if (min > max || defaultValue < min || defaultValue > max)
            throw new ArgumentOutOfRangeException(nameof(defaultValue));

        while (true)
        {
            Console.Write(prompt);
            string text = ReadLine().Trim();
            if (text.Length == 0) return defaultValue;
            if (int.TryParse(text, out int value) && value >= min && value <= max) return value;
            Console.WriteLine($"  Введите целое число от {min} до {max}.");
        }
    }

    public static bool ReadYesNo(string question)
    {
        while (true)
        {
            Console.Write($"  {question} (да/нет): ");
            switch (ReadLine().Trim().ToLowerInvariant())
            {
                case "да": case "д": case "yes": case "y": case "1": return true;
                case "нет": case "н": case "no": case "n": case "0": return false;
                default: Console.WriteLine("  Ответьте «да» или «нет»."); break;
            }
        }
    }
}
