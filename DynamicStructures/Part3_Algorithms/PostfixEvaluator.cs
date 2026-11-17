using System.Globalization;
using DynamicStructures.Part1_Stack;

namespace DynamicStructures.Part3_Algorithms;

/// <summary>
/// Вычисление арифметического выражения в постфиксной (обратной польской) записи
/// с помощью стека на собственном связном списке (LinkedStack).
///
/// Поддерживаемые операции:
///   Бинарные:  +  -  *  :  (или /)  ^
///   Унарные:   ln  cos  sin  sqrt  и унарный минус (токен ")" — как в ТЗ; также принимается "~")
///
/// Токены разделяются пробелами. Пример: 7 8 + 3 2 + /   →   3
///
/// ОЦЕНКА СЛОЖНОСТИ:
///   Каждый токен обрабатывается ровно один раз, на токен приходится не более 3 операций со
///   стеком (число: 1 Push; унарная операция: Pop + Push; бинарная: 2 Pop + Push), каждая за O(1),
///   плюс O(1) на определение вида токена (HashSet, разбор числа).
///   Итого T(n) ≤ c·n  =>  T(n) = O(n), где n — число токенов.
///   Память: максимум стека — число операндов, ожидающих операции, в худшем случае
///   (n+1)/2 элементов  =>  O(n); для выражений вида «1 1 + 1 + …» — O(1).
/// </summary>
public static class PostfixEvaluator
{
    /// <summary>Токен унарного минуса в постфиксной записи (по ТЗ — «)»).</summary>
    public const string UnaryMinus = ")";

    private static readonly HashSet<string> BinaryOps = new() { "+", "-", "*", ":", "/", "^" };
    private static readonly HashSet<string> UnaryOps = new() { "ln", "cos", "sin", "sqrt", UnaryMinus, "~" };

    /// <summary>Вычислить постфиксное выражение.</summary>
    public static double Evaluate(string expression) => Evaluate(expression, out _);

    /// <summary>
    /// Вычислить постфиксное выражение и вернуть число выполненных операций Push/Pop
    /// (нужно для эмпирической проверки оценки сложности).
    /// </summary>
    /// <exception cref="FormatException">Некорректное выражение (неизвестный токен, не хватает/лишние операнды).</exception>
    /// <exception cref="ArithmeticException">Недопустимая операция (деление на 0, ln от x ≤ 0, sqrt от x &lt; 0 и т.п.).</exception>
    public static double Evaluate(string expression, out long stackOps)
    {
        ArgumentNullException.ThrowIfNull(expression);
        var stack = new LinkedStack<double>();
        long ops = 0;
        string[] tokens = expression.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);

        if (tokens.Length == 0)
            throw new FormatException("Выражение пусто.");

        foreach (string token in tokens)
        {
            if (TryParseNumber(token, out double number))
            {
                CheckResult(number, token);
                stack.Push(number);
                ops++;
            }
            else if (BinaryOps.Contains(token))
            {
                double b = PopOperand(stack, token);
                double a = PopOperand(stack, token);

                double result;
                switch (token)
                {
                    case "+": result = a + b; break;
                    case "-": result = a - b; break;
                    case "*": result = a * b; break;
                    case ":":
                    case "/":
                        if (b == 0) throw new DivideByZeroException("Деление на ноль.");
                        result = a / b;
                        break;
                    case "^": result = Math.Pow(a, b); break;
                    default: throw new FormatException($"Неизвестный оператор: '{token}'.");
                }

                CheckResult(result, token);
                stack.Push(result);
                ops += 3;
            }
            else if (UnaryOps.Contains(token))
            {
                double a = PopOperand(stack, token);

                double result;
                switch (token)
                {
                    case "ln":
                        if (a <= 0) throw new ArithmeticException("ln определён только для x > 0.");
                        result = Math.Log(a);
                        break;
                    case "cos": result = Math.Cos(a); break;
                    case "sin": result = Math.Sin(a); break;
                    case "sqrt":
                        if (a < 0) throw new ArithmeticException("sqrt определён только для x ≥ 0.");
                        result = Math.Sqrt(a);
                        break;
                    case UnaryMinus:
                    case "~":
                        result = -a;
                        break;
                    default: throw new FormatException($"Неизвестный оператор: '{token}'.");
                }

                CheckResult(result, token);
                stack.Push(result);
                ops += 2;
            }
            else
            {
                throw new FormatException($"Неизвестный токен: '{token}'.");
            }
        }

        double answer = stack.Pop(); // стек не пуст: в выражении был хотя бы один токен, и каждая операция кладёт результат
        ops++;

        if (!stack.IsEmpty())
            throw new FormatException("Лишние операнды: в выражении не хватает операций.");

        stackOps = ops;
        return answer;
    }

    /// <summary>
    /// Вычисляет постфиксное выражение с подробным пошаговым протоколированием
    /// каждого действия над стеком (как на иллюстрации в методичке).
    /// </summary>
    public static double EvaluateWithExplanation(string expression, Action<string> log)
    {
        ArgumentNullException.ThrowIfNull(expression);
        ArgumentNullException.ThrowIfNull(log);

        var stack = new LinkedStack<double>();
        string[] tokens = expression.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);

        if (tokens.Length == 0)
            throw new FormatException("Выражение пусто.");

        int step = 0;
        foreach (string token in tokens)
        {
            step++;
            if (TryParseNumber(token, out double number))
            {
                CheckResult(number, token);
                stack.Push(number);
                log($"  Шаг {step,2}: Токен «{token}» (число) -> Push({FormatNum(number)}). Стек: {FormatStack(stack)}");
            }
            else if (BinaryOps.Contains(token))
            {
                double b = PopOperand(stack, token);
                double a = PopOperand(stack, token);

                double result;
                string opName;
                switch (token)
                {
                    case "+": result = a + b; opName = "сложение"; break;
                    case "-": result = a - b; opName = "вычитание"; break;
                    case "*": result = a * b; opName = "умножение"; break;
                    case ":":
                    case "/":
                        if (b == 0) throw new DivideByZeroException("Деление на ноль.");
                        result = a / b;
                        opName = "деление";
                        break;
                    case "^": result = Math.Pow(a, b); opName = "возведение в степень"; break;
                    default: throw new FormatException($"Неизвестный оператор: '{token}'.");
                }

                CheckResult(result, token);
                stack.Push(result);
                log($"  Шаг {step,2}: Токен «{token}» ({opName}) -> сняли {FormatNum(b)} и {FormatNum(a)}, {FormatNum(a)} {token} {FormatNum(b)} = {FormatNum(result)}, Push({FormatNum(result)}). Стек: {FormatStack(stack)}");
            }
            else if (UnaryOps.Contains(token))
            {
                double a = PopOperand(stack, token);

                double result;
                string opName;
                switch (token)
                {
                    case "ln":
                        if (a <= 0) throw new ArithmeticException("ln определён только для x > 0.");
                        result = Math.Log(a);
                        opName = "ln";
                        break;
                    case "cos": result = Math.Cos(a); opName = "cos"; break;
                    case "sin": result = Math.Sin(a); opName = "sin"; break;
                    case "sqrt":
                        if (a < 0) throw new ArithmeticException("sqrt определён только для x ≥ 0.");
                        result = Math.Sqrt(a);
                        opName = "sqrt";
                        break;
                    case UnaryMinus:
                    case "~":
                        result = -a;
                        opName = "унарный минус";
                        break;
                    default: throw new FormatException($"Неизвестный оператор: '{token}'.");
                }

                CheckResult(result, token);
                stack.Push(result);
                log($"  Шаг {step,2}: Токен «{token}» ({opName}) -> сняли {FormatNum(a)}, вычислили {FormatNum(result)}, Push({FormatNum(result)}). Стек: {FormatStack(stack)}");
            }
            else
            {
                throw new FormatException($"Неизвестный токен: '{token}'.");
            }
        }

        double answer = stack.Pop();
        if (!stack.IsEmpty())
            throw new FormatException("Лишние операнды: в выражении не хватает операций.");

        log($"  Итог: Pop() -> результат выражения равен {FormatNum(answer)}.");
        return answer;
    }

    private static string FormatNum(double n) =>
        n.ToString("G", CultureInfo.InvariantCulture);

    private static string FormatStack(LinkedStack<double> stack)
    {
        if (stack.IsEmpty()) return "[пуст]";
        // Снимем элементы во временный список и вернем обратно в стек
        var list = new List<double>();
        while (!stack.IsEmpty())
            list.Add(stack.Pop());

        // Восстанавливаем стек
        for (int i = list.Count - 1; i >= 0; i--)
            stack.Push(list[i]);

        // list[0] - вершина (Top), list[^1] - основание (Bottom)
        // Для визуализации дно -> вершина:
        var bottomToTop = list.AsEnumerable().Reverse().Select(FormatNum);
        return "[" + string.Join(", ", bottomToTop) + "] (вершина: " + FormatNum(list[0]) + ")";
    }

    private static double PopOperand(LinkedStack<double> stack, string op)
    {
        if (stack.IsEmpty())
            throw new FormatException($"Недостаточно операндов для операции '{op}'.");
        return stack.Pop();
    }

    private static void CheckResult(double value, string op)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
            throw new ArithmeticException($"Результат операции '{op}' не определён (NaN/∞).");
    }

    private static bool TryParseNumber(string token, out double value)
    {
        value = 0;
        // Число начинается с цифры, знака или дробного разделителя; NaN/Infinity — не числа ввода.
        if (!char.IsDigit(token[0]) && token[0] is not ('+' or '-' or '.' or ',')) return false;

        string t = token.Contains(',') ? token.Replace(',', '.') : token;
        return double.TryParse(t, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
    }
}
