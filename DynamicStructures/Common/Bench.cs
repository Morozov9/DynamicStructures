using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace DynamicStructures.Common;

/// <summary>Вспомогательные средства для замеров времени и вывода таблиц.</summary>
public static class Bench
{
    /// <summary>Выполнить действие и вернуть время в миллисекундах (перед замером — сборка мусора).</summary>
    public static double TimeMs(Action action)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        var sw = Stopwatch.StartNew();
        action();
        sw.Stop();
        return sw.Elapsed.TotalMilliseconds;
    }

    /// <summary>Медиана набора измерений (устойчива к случайным выбросам).</summary>
    public static double Median(List<double> values)
    {
        var s = values.OrderBy(v => v).ToList();
        int m = s.Count / 2;
        return s.Count % 2 == 1 ? s[m] : (s[m - 1] + s[m]) / 2;
    }

    public static string Ms(double v) => v.ToString("F3", CultureInfo.InvariantCulture);

    public static string Num(long v) => v.ToString("N0", CultureInfo.InvariantCulture).Replace(',', ' ');

    /// <summary>Предупреждение, если программа собрана в Debug.</summary>
    public static string BuildNote()
    {
#if DEBUG
        return "ВНИМАНИЕ: Debug-сборка. Для честных замеров переключите конфигурацию на Release и запустите без отладки (Ctrl+F5).";
#else
        return "Сборка: Release.";
#endif
    }
}

/// <summary>Простая текстовая таблица с выравниванием столбцов.</summary>
public sealed class TableWriter
{
    private readonly string[] _headers;
    private readonly List<string[]> _rows = new();

    public TableWriter(params string[] headers) => _headers = headers;

    public void AddRow(params string[] cells) => _rows.Add(cells);

    public override string ToString()
    {
        int cols = _headers.Length;
        var w = new int[cols];
        for (int c = 0; c < cols; c++)
        {
            w[c] = _headers[c].Length;
            foreach (var row in _rows)
                w[c] = Math.Max(w[c], row[c].Length);
        }

        var sb = new StringBuilder();

        void Line(string[] cells)
        {
            for (int c = 0; c < cols; c++)
            {
                sb.Append(c == 0 ? cells[c].PadRight(w[c]) : cells[c].PadLeft(w[c]));
                if (c < cols - 1) sb.Append(" | ");
            }
            sb.AppendLine();
        }

        Line(_headers);
        sb.AppendLine(new string('-', w.Sum() + 3 * (cols - 1)));
        foreach (var row in _rows) Line(row);
        return sb.ToString();
    }
}
