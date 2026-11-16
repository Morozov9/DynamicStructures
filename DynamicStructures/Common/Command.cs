namespace DynamicStructures.Common;

/// <summary>
/// Одна команда из файла input.txt.
/// Code: 1–5 (номер операции), -1 — токен не удалось разобрать.
/// Arg: значение после запятой ("1,56" → "56"), либо null.
/// Raw: исходный токен (для сообщений об ошибках).
/// </summary>
public readonly record struct Command(int Code, string? Arg, string Raw);

/// <summary>Разбор содержимого input.txt в массив команд.</summary>
public static class CommandParser
{
    /// <summary>Разобрать один токен вида "2" или "1,56" / "1,cat".</summary>
    public static Command ParseToken(string token)
    {
        int comma = token.IndexOf(',');
        string head = comma >= 0 ? token[..comma] : token;
        string? arg = comma >= 0 ? token[(comma + 1)..] : null;
        if (arg is { Length: 0 }) arg = null;

        return int.TryParse(head, out int code)
            ? new Command(code, arg, token)
            : new Command(-1, null, token);
    }

    /// <summary>Разобрать всё содержимое файла (токены разделены пробелами/переводами строк).</summary>
    public static Command[] ParseAll(string content)
    {
        string[] tokens = content.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
        var result = new Command[tokens.Length];
        for (int i = 0; i < tokens.Length; i++)
            result[i] = ParseToken(tokens[i]);
        return result;
    }
}
