using DynamicStructures.Part1_Stack;

namespace DynamicStructures.Part3_Algorithms;

/// <summary>
/// Алгоритм вычисления арифметического выражения в постфиксной (обратной польской) записи.
///
/// Поддерживаемые операторы:
///   Бинарные:  +  -  *  :  ^
///   Унарные:   ln  cos  sin  sqrt  )
///
/// Ввод: токены через пробел, например:
///   7 8 + 3 2 + /   →   (7+8) / (3+2) = 3
///
/// Оценка сложности: O(n), где n — количество токенов.
/// Каждый токен обрабатывается ровно один раз.
/// </summary>
public static class PostfixEvaluator
{
    private static readonly HashSet<string> BinaryOps = new() { "+", "-", "*", ":", "/" , "^" };
    private static readonly HashSet<string> UnaryOps  = new() { "ln", "cos", "sin", "sqrt", ")" };

    /// <summary>Вычислить постфиксное выражение.</summary>
    /// <param name="expression">Строка с токенами, разделёнными пробелом.</param>
    /// <returns>Результат вычисления.</returns>
    public static double Evaluate(string expression)
    {
        var stack = new LinkedStack<double>();
        string[] tokens = expression.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        foreach (string token in tokens)
        {
            if (double.TryParse(token, System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out double number))
            {
                // Число — кладём на стек
                stack.Push(number);
            }
            else if (BinaryOps.Contains(token))
            {
                // Бинарная операция: извлекаем два операнда
                double b = stack.Pop();
                double a = stack.Pop();
                double result = token switch
                {
                    "+" => a + b,
                    "-" => a - b,
                    "*" => a * b,
                    ":" or "/" => b == 0
                        ? throw new DivideByZeroException("Деление на ноль.")
                        : a / b,
                    "^" => Math.Pow(a, b),
                    _ => throw new InvalidOperationException($"Неизвестный оператор: {token}")
                };
                stack.Push(result);
            }
            else if (UnaryOps.Contains(token))
            {
                // Унарная операция: извлекаем один операнд
                double a = stack.Pop();
                double result = token switch
                {
                    "ln"   => Math.Log(a),
                    "cos"  => Math.Cos(a),
                    "sin"  => Math.Sin(a),
                    "sqrt" => Math.Sqrt(a),
                    ")"    => -a, // унарный минус в постфиксной записи
                    _ => throw new InvalidOperationException($"Неизвестный оператор: {token}")
                };
                stack.Push(result);
            }
            else
            {
                throw new FormatException($"Неизвестный токен: '{token}'");
            }
        }

        if (stack.IsEmpty())
            throw new InvalidOperationException("Выражение пусто или некорректно.");

        return stack.Pop();
    }
}
