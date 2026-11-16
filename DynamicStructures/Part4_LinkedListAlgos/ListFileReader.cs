namespace DynamicStructures.Part4_LinkedListAlgos;

/// <summary>Каждая строка файла — отдельный список целых чисел, разделённых пробелами.</summary>
public static class ListFileReader
{
    public static MyLinkedList[] Read(string path, int listCount)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(listCount, 1);
        string[] lines = File.ReadAllLines(path);
        // Пустой файл допустим как один пустой список.
        if (lines.Length == 0 && listCount == 1) lines = [""];
        if (lines.Length != listCount)
            throw new FormatException($"Ожидается строк: {listCount}, найдено: {lines.Length}. Каждая строка — один список; пустая строка — пустой список.");

        var lists = new MyLinkedList[listCount];
        for (int i = 0; i < lines.Length; i++)
        {
            string[] tokens = lines[i].Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
            var values = new int[tokens.Length];
            for (int j = 0; j < tokens.Length; j++)
            {
                if (!int.TryParse(tokens[j], out values[j]))
                    throw new FormatException($"Строка {i + 1}: «{tokens[j]}» — не целое число в диапазоне Int32.");
            }
            lists[i] = new MyLinkedList(values);
        }
        return lists;
    }
}
