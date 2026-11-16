using DynamicStructures.Part1_Stack;

using System.Globalization;

namespace DynamicStructures.Part3_Algorithms;

/// <summary>
/// Перевод арифметического выражения из инфиксной записи (2+2) в постфиксную (2 2 +) —
/// алгоритм сортировочной станции Дейкстры на стеке LinkedStack.
///
/// Поддерживаемые элементы:
///   Бинарные:  +  -  *  /  :  ^   (^ — правоассоциативная)
///   Унарные:   ln  cos  sin  sqrt, унарный минус (в постфиксе записывается токеном ")" — как в ТЗ)
///   Скобки:    ( )
///   Числа:     целые и дробные (разделитель '.' или ',')
///
/// Приоритеты: + - (1) &lt; * / : (2) &lt; унарный минус (3) &lt; ^ (4) &lt; функции (5).
/// Поэтому -2^2 = -(2^2), а 2^-3 = 2^(-3).
///
/// Примеры:  (2+3)*4 → 2 3 + 4 *      -(2+3)*4 → 2 3 + ) 4 *      sin(0)+cos(0) → 0 sin 0 cos +
///
/// ОЦЕНКА СЛОЖНОСТИ: каждый токен один раз обрабатывается и не более одного раза кладётся
/// в стек и снимается с него (каждая операция O(1))  =>  T(n) = O(n) по числу токенов
/// (разбиение строки на токены — O(L) по длине строки). Память — O(n) (стек операторов
/// максимум глубины вложенности скобок).
/// </summary>
public static class InfixToPostfix
{
    // Внутреннее обозначение унарного минуса во время разбора (чтобы не путать с бинарным '-')
    private const string UMinus = "u-";

    private static readonly Dictionary<string, int> Precedence = new()
    {
        { "+",    1 },
        { "-",    1 },
        { "*",    2 },
        { "/",    2 },
        { ":",    2 },
        { UMinus, 3 },
        { "^",    4 },
        { "ln",   5 },
        { "cos",  5 },
        { "sin",  5 },
        { "sqrt", 5 },
    };

    private static readonly HashSet<string> RightAssociative = new() { "^", UMinus };
    private static readonly HashSet<string> Functions = new() { "ln", "cos", "sin", "sqrt" };

    /// <summary>Перевести инфиксное выражение в постфиксное.</summary>
    /// <param name="expression">Инфиксное выражение (с пробелами или без).</param>
    /// <exception cref="FormatException">Некорректное выражение (скобки, пропущенный оператор/операнд, неизвестный символ).</exception>
    public static string Convert(string expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        List<string> tokens = Tokenize(expression);
        if (tokens.Count == 0)
            throw new FormatException("Выражение пусто.");

        var output = new System.Text.StringBuilder();
        var opStack = new LinkedStack<string>();
        bool prevOperand = false; // предыдущий токен — число или ')' (т.е. «закончился операнд»)

        foreach (string token in tokens)
        {
            if (IsNumber(token))
            {
                if (prevOperand) throw new FormatException($"Пропущен оператор перед '{token}'.");
                Append(output, token);
                prevOperand = true;
            }
            else if (Functions.Contains(token))
            {
                if (prevOperand) throw new FormatException($"Пропущен оператор перед '{token}'.");
                opStack.Push(token);
                prevOperand = false;
            }
            else if (token == "(")
            {
                if (prevOperand) throw new FormatException("Пропущен оператор перед '('.");
                opStack.Push(token);
                prevOperand = false;
            }
            else if (token == ")")
            {
                if (!prevOperand) throw new FormatException("Пустые скобки или оператор без правого операнда перед ')'.");

                while (!opStack.IsEmpty() && opStack.Top() != "(")
                    Append(output, opStack.Pop());

                if (opStack.IsEmpty())
                    throw new FormatException("Несбалансированные скобки: лишняя ')'.");

                opStack.Pop(); // убрать "("

                // Если перед скобкой была функция — выталкиваем её
                if (!opStack.IsEmpty() && Functions.Contains(opStack.Top()))
                    Append(output, opStack.Pop());

                prevOperand = true;
            }
            else if (token == UMinus)
            {
                // Префиксный оператор: ничего не выталкивает, сразу ждёт операнд
                opStack.Push(token);
                prevOperand = false;
            }
            else if (Precedence.ContainsKey(token))
            {
                if (!prevOperand) throw new FormatException($"Оператор '{token}' не имеет левого операнда.");

                int prec = Precedence[token];
                while (!opStack.IsEmpty()
                       && opStack.Top() != "("
                       && Precedence.TryGetValue(opStack.Top(), out int topPrec)
                       && (topPrec > prec || (topPrec == prec && !RightAssociative.Contains(token))))
                {
                    Append(output, opStack.Pop());
                }
                opStack.Push(token);
                prevOperand = false;
            }
            else
            {
                throw new FormatException($"Неизвестный токен: '{token}'.");
            }
        }

        if (!prevOperand)
            throw new FormatException("Выражение заканчивается оператором или функцией без операнда.");

        // Выталкиваем оставшиеся операторы
        while (!opStack.IsEmpty())
        {
            string op = opStack.Pop();
            if (op == "(")
                throw new FormatException("Несбалансированные скобки: лишняя '('.");
            Append(output, op);
        }

        return output.ToString();
    }

    // ── Вспомогательные методы ────────────────────────────────────────────────

    private static void Append(System.Text.StringBuilder sb, string token)
    {
        if (sb.Length > 0) sb.Append(' ');
        sb.Append(token == UMinus ? PostfixEvaluator.UnaryMinus : token);
    }

    private static bool IsNumber(string token)
        => double.TryParse(token, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double value)
           && double.IsFinite(value);

    /// <summary>
    /// Разбивает выражение на токены, поддерживая запись с пробелами и без («(2+3)*4»).
    /// Минус в начале выражения, после '(' или после другого оператора — унарный.
    /// </summary>
    private static List<string> Tokenize(string expression)
    {
        var tokens = new List<string>();
        int i = 0;

        while (i < expression.Length)
        {
            char ch = expression[i];

            if (char.IsWhiteSpace(ch)) { i++; continue; }

            // Число (целое или дробное; разделитель '.' или ',')
            if (char.IsDigit(ch) || ch is '.' or ',')
            {
                int start = i++;
                while (i < expression.Length
                       && (char.IsDigit(expression[i]) || expression[i] == '.' || expression[i] == ','))
                    i++;
                string number = expression[start..i].Replace(',', '.');
                if (!IsNumber(number))
                    throw new FormatException($"Некорректное число: '{expression[start..i]}'.");
                tokens.Add(number);
                continue;
            }

            // Функция: sin, cos, ln, sqrt
            if (char.IsLetter(ch))
            {
                int start = i++;
                while (i < expression.Length && char.IsLetter(expression[i])) i++;
                tokens.Add(expression[start..i].ToLowerInvariant());
                continue;
            }

            bool operandBefore = tokens.Count > 0 && (IsNumber(tokens[^1]) || tokens[^1] == ")");

            if (ch == '-' && !operandBefore)
            {
                tokens.Add(UMinus);
                i++;
                continue;
            }
            if (ch == '+' && !operandBefore)
            {
                i++; // унарный плюс ничего не меняет
                continue;
            }

            // Одиночный символ-оператор или скобка (неизвестные символы отсеются в Convert)
            tokens.Add(ch.ToString());
            i++;
        }

        return tokens;
    }
}
