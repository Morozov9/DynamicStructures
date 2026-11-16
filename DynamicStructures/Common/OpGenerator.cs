using System.Text;

namespace DynamicStructures.Common;

/// <summary>
/// Профиль набора операций: веса операций 1–5 (Push/Enqueue, Pop/Dequeue, Top/Peek, IsEmpty, Print).
/// PushThenPop: первую половину набора — только вставки, вторую — только удаления.
/// </summary>
public sealed record OpProfile(
    int Push, int Pop, int Peek, int IsEmpty, int Print, bool PushThenPop = false);

/// <summary>Генератор тестовых файлов input.txt в формате "3 4 1,56 1,7 1,cat 2 5 4".</summary>
public static class OpGenerator
{
    private static readonly string[] Words =
        { "cat", "dog", "apple", "tree", "node", "list", "stack", "queue" };

    /// <summary>
    /// Сгенерировать count команд по профилю. Pop/Peek на пустой структуре не генерируются
    /// (вместо них ставится вставка), чтобы замер не искажали сообщения об ошибках.
    /// Seed фиксирован — наборы воспроизводимы.
    /// </summary>
    public static string Generate(int count, OpProfile p, int seed = 12345)
    {
        var rnd = new Random(seed);
        var sb = new StringBuilder(count * 6);
        int size = 0;
        int total = p.Push + p.Pop + p.Peek + p.IsEmpty + p.Print;

        for (int i = 0; i < count; i++)
        {
            int code;
            if (p.PushThenPop)
            {
                code = i < count / 2 ? 1 : (size > 0 ? 2 : 4);
            }
            else
            {
                int r = rnd.Next(total);
                if ((r -= p.Push) < 0) code = 1;
                else if ((r -= p.Pop) < 0) code = 2;
                else if ((r -= p.Peek) < 0) code = 3;
                else if ((r -= p.IsEmpty) < 0) code = 4;
                else code = 5;

                if (size == 0 && (code == 2 || code == 3)) code = 1;
            }

            if (i > 0) sb.Append(' ');
            switch (code)
            {
                case 1:
                    sb.Append("1,");
                    sb.Append(rnd.Next(2) == 0
                        ? rnd.Next(1000).ToString()
                        : Words[rnd.Next(Words.Length)]);
                    size++;
                    break;
                case 2:
                    sb.Append('2');
                    size--;
                    break;
                default:
                    sb.Append((char)('0' + code));
                    break;
            }
        }
        return sb.ToString();
    }
}
