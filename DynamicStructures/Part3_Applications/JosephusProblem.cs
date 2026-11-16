using System.Text;

namespace DynamicStructures.Part3_Applications;

/// <summary>Узел кругового односвязного списка (последний узел ссылается на первый).</summary>
public sealed class CircleNode
{
    public int Number { get; }
    public CircleNode? Next { get; set; }

    public CircleNode(int number) => Number = number;
}

/// <summary>
/// Задача Иосифа Флавия (алгоритм выбывания) — применение структуры ДАННЫХ «СПИСОК».
///
/// Условие: n человек стоят по кругу и пронумерованы 1..n. Начиная с первого, считаем
/// по кругу и выводим из круга каждого k-го. Счёт продолжается со следующего за выбывшим.
/// Так продолжается, пока не останется один человек. Нужно найти порядок выбывания и
/// номер уцелевшего.
///
/// Почему список: круг — это кольцо, где «следующий» всегда определён, а выбывание —
/// удаление узла из середины. В кольцевом односвязном списке удаление узла по известному
/// предыдущему — O(1) (одно переприсваивание ссылки), в массиве пришлось бы сдвигать элементы O(n).
///
/// Сложность: n удалений, перед каждым — не более min(k-1, остаток-1) шагов по ссылкам,
/// т.е. O(n·min(k, n)) времени, O(n) памяти.
/// </summary>
public static class JosephusProblem
{
    /// <summary>
    /// Решить задачу списком.
    /// onStep (необязательно) вызывается после каждого выбывания: (номер выбывшего, состояние круга).
    /// </summary>
    public static (List<int> Order, int Survivor) Solve(int n, int k, Action<int, string>? onStep = null)
    {
        if (n < 1) throw new ArgumentOutOfRangeException(nameof(n), "n должно быть ≥ 1.");
        if (k < 1) throw new ArgumentOutOfRangeException(nameof(k), "k должно быть ≥ 1.");

        // 1. Строим список 1 → 2 → … → n и замыкаем его в кольцо
        var head = new CircleNode(1);
        CircleNode tail = head;
        for (int i = 2; i <= n; i++)
        {
            var node = new CircleNode(i);
            tail.Next = node;
            tail = node;
        }
        tail.Next = head;

        // 2. prev — узел ПЕРЕД тем, с которого начинается счёт (сначала это последний узел)
        CircleNode prev = tail;
        var order = new List<int>(n);
        int remaining = n;

        while (remaining > 1)
        {
            // Делаем k-1 шагов по кругу (полные обороты пропускаем: % remaining)
            int steps = (k - 1) % remaining;
            for (int s = 0; s < steps; s++)
                prev = prev.Next!;

            CircleNode victim = prev.Next!;   // k-й по счёту
            order.Add(victim.Number);
            prev.Next = victim.Next;          // выбрасываем узел из кольца: O(1)
            remaining--;

            if (onStep is not null)
                onStep(victim.Number, Snapshot(prev.Next!, remaining));
        }

        return (order, prev.Next!.Number);
    }

    /// <summary>Контроль: то же решение по рекуррентной формуле J(1)=0, J(i)=(J(i-1)+k) mod i. O(n), без списка.</summary>
    public static int SolveByFormula(int n, int k)
    {
        int j = 0;
        for (int i = 2; i <= n; i++)
            j = (j + k) % i;
        return j + 1;
    }

    // Состояние круга, начиная с узла start (откуда продолжится счёт)
    private static string Snapshot(CircleNode start, int count)
    {
        var sb = new StringBuilder();
        CircleNode cur = start;
        for (int i = 0; i < count; i++)
        {
            if (i > 0) sb.Append(" → ");
            sb.Append(cur.Number);
            cur = cur.Next!;
        }
        sb.Append(" ↺");
        return sb.ToString();
    }
}
