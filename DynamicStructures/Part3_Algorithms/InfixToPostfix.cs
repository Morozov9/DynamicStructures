using DynamicStructures.Part1_Stack;

namespace DynamicStructures.Part3_Algorithms;

/// <summary>
/// Алгоритм перевода арифметического выражения из инфиксной записи (2+2)
/// в постфиксную запись (2 2 +) — алгоритм сортировочной станции Дейкстры.
///
/// Поддерживаемые операторы:
///   Бинарные:  +  -  *  /  :  ^
///   Унарные:   ln  cos  sin  sqrt
///   Скобки:    ( )
///
/// Пример: (2+3)*4   →   2 3 + 4 *
///
/// Оценка сложности: O(n), где n — количество токенов выражения.
/// </summary>
public static class InfixToPostfix
{
    // Приоритет операторов (чем выше число, тем выше приоритет)
    private static readonly Dictionary<string, int> Precedence = new()
    {
        { "+",    1 },
        { "-",    1 },
        { "*",    2 },
        { "/",    2 },
        { ":",    2 },
        { "^",    3 },   // правоассоциативный
        { "ln",   4 },   // унарные — наивысший приоритет
        { "cos",  4 },
        { "sin",  4 },
        { "sqrt", 4 },
    };

    private static readonly HashSet<string> RightAssociative = new() { "^", "ln", "cos", "sin", "sqrt" };
    private static readonly HashSet<string> UnaryFunctions   = new() { "ln", "cos", "sin", "sqrt" };

    /// <summary>Перевести инфиксное выражение в постфиксное.</summary>
    /// <param name="expression">Инфиксное выражение (токены через пробел или без).</param>
    /// <returns>Постфиксное выражение в виде строки.</returns>
    public static string Convert(string expression)
    {
        var tokens = Tokenize(expression);
        var output  = new System.Text.StringBuilder();
        var opStack = new LinkedStack<string>();

        foreach (string token in tokens)
        {
            if (IsNumber(token))
            {
                // Число → сразу в выходную очередь
                AppendToken(output, token);
            }
            else if (UnaryFunctions.Contains(token))
            {
                // Функция → на стек операторов
                opStack.Push(token);
            }
            else if (token == "(")
            {
                opStack.Push(token);
            }
            else if (token == ")")
            {
                // Выталкиваем операторы до открывающей скобки
                while (!opStack.IsEmpty() && opStack.Top() != "(")
                    AppendToken(output, opStack.Pop());

                if (opStack.IsEmpty())
                    throw new FormatException("Несбалансированные скобки: лишняя ')'.");

                opStack.Pop(); // убрать "("

                // Если на вершине функция — выталкиваем её
                if (!opStack.IsEmpty() && UnaryFunctions.Contains(opStack.Top()))
                    AppendToken(output, opStack.Pop());
            }
            else if (Precedence.ContainsKey(token))
            {
                // Оператор: выталкиваем операторы с большим или равным приоритетом
                while (!opStack.IsEmpty()
                       && opStack.Top() != "("
                       && Precedence.TryGetValue(opStack.Top(), out int topPrecedence)
                       && (topPrecedence > Precedence[token]
                           || (topPrecedence == Precedence[token] && !RightAssociative.Contains(token))))
                {
                    AppendToken(output, opStack.Pop());
                }
                opStack.Push(token);
            }
            else
            {
                throw new FormatException($"Неизвестный токен: '{token}'");
            }
        }

        // Выталкиваем оставшиеся операторы
        while (!opStack.IsEmpty())
        {
            string op = opStack.Pop();
            if (op == "(" || op == ")")
                throw new FormatException("Несбалансированные скобки: лишняя '('.");
            AppendToken(output, op);
        }

        return output.ToString().Trim();
    }

    // ── Вспомогательные методы ────────────────────────────────────────────────

    private static void AppendToken(System.Text.StringBuilder sb, string token)
    {
        if (sb.Length > 0) sb.Append(' ');
        sb.Append(token);
    }

    private static bool IsNumber(string token)
        => double.TryParse(token, System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out _);

    /// <summary>
    /// Разбивает выражение на токены, поддерживая как запись с пробелами,
    /// так и «слитную» запись вида (2+3)*4.
    /// </summary>
    private static List<string> Tokenize(string expression)
    {
        var tokens = new List<string>();
        int i = 0;
        expression = expression.Trim();

        while (i < expression.Length)
        {
            char ch = expression[i];

            if (char.IsWhiteSpace(ch)) { i++; continue; }

            // Число (включая отрицательное и дробное)
            if (char.IsDigit(ch) || (ch == '-' && i + 1 < expression.Length && char.IsDigit(expression[i + 1])
                                     && (tokens.Count == 0 || tokens[^1] == "(")))
            {
                int start = i++;
                while (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.' || expression[i] == ','))
                    i++;
                tokens.Add(expression[start..i].Replace(',', '.'));
                continue;
            }

            // Идентификатор (функция: sin, cos, ln, sqrt)
            if (char.IsLetter(ch))
            {
                int start = i++;
                while (i < expression.Length && char.IsLetter(expression[i])) i++;
                tokens.Add(expression[start..i]);
                continue;
            }

            // Одиночный символ-оператор или скобка
            tokens.Add(ch.ToString());
            i++;
        }

        return tokens;
    }
}
