using System.Diagnostics;
using System.Text;
using DynamicStructures.Part4_LinkedListAlgos;

namespace DynamicStructures.Tests;

internal static class ConsoleChecks
{
    public static void Run()
    {
        Check.Case("Console: tasks 1-8 finish, including self-insertion", () =>
        {
            string output = RunApp("4\n1\n2\n3\n4\n5\n6\n7\n8\n0\n\n0\n");
            Contains(output, "1 → 2 → 1 → 2 → 3 → 3");
            Contains(output, "До свидания!");
        });
        Check.Case("Console: tasks 9-12, swap by position and value", () =>
        {
            string output = RunApp("4\n9\n\n10\n\n2\n11\n\n12\n\n1\n1\n5\n12\n\n2\n10\n50\n0\n\n0\n");
            Contains(output, "L после: 1 → 2 → 3 → 4 → 5 → 6");
            Contains(output, "Первый: 1 → 2");
            Contains(output, "Второй: 3 → 2 → 4");
            Contains(output, "После: 1 → 2 → 3 → 1 → 2 → 3");
            Check.Equal(2, output.Split("После: 50 → 20 → 30 → 40 → 10").Length - 1);
        });
        Check.Case("Console: existing stack and queue file workflows", () =>
        {
            Contains(RunApp("1\n1\n\n0\n\n0\n"), "[Push] Добавлен элемент: cat");
            Contains(RunApp("2\n1\n\n0\n\n0\n"), "[Enqueue] Добавлен: cat");
        });
        Check.Case("Console: all three expression file workflows", () =>
        {
            string output = RunApp("3\n1\n\n2\n\n3\n\n0\n\n0\n");
            Contains(output, "Постфикс: 2 3 + 4 *");
            Contains(output, "Результат: 20");
            Contains(output, "Деление на ноль");
        });
        Check.Case("Console: Josephus demonstration and 9000-case self-check", () =>
        {
            string output = RunApp("5\n1\n\n\n2\n0\n\n0\n");
            Contains(output, "Остался: 4");
            Contains(output, "9000, расхождений: 0");
        });
        Check.Case("Console: flood fill sample and file path stack trace", () =>
        {
            string output = RunApp("5\n3\n\n\n\n\n4\n\n0\n\n0\n");
            Contains(output, "Закрашено клеток:");
            Contains(output, "Pop: удалена папка «Documents»");
            Contains(output, @"Результат: C:\Users\Student\Projects\report.txt");
        });
        (string Answers, string Result)[] routes =
        [
            ("да\nда", "Поезд"), ("да\nнет", "Междугородний автобус"),
            ("нет\nда\nда", "Велосипед"), ("нет\nда\nнет", "Пешком"),
            ("нет\nнет\nда", "Общественный транспорт"), ("нет\nнет\nнет", "Такси")
        ];
        foreach (var (answers, result) in routes)
            Check.Case($"Console: decision tree -> {result}", () =>
                Contains(RunApp($"5\n5\n{answers}\n0\n\n0\n"), $"\n  Решение: {result}"));

        Check.Case("Console: invalid choices, numeric positions and yes/no answers recover", () =>
        {
            string output = RunApp("oops\n4\n99\nabc\n12\n\n0\n1\n0\n1\n99\n5\n0\n\n5\n5\nmaybe\nyes\nno\n0\n\n0\n");
            Contains(output, "Введите число.");
            Contains(output, "Введите число от 0 до 12.");
            Contains(output, "Введите целое число от 1 до 5.");
            Contains(output, "Ответьте «да» или «нет».");
            Contains(output, "\n  Решение: Междугородний автобус");
        });
        Check.Case("Console: unsupported drive-relative path reports an error", () =>
            Contains(RunApp("5\n4\nC:folder\n0\n\n0\n"), "Ошибка пути:"));

        string directory = Directory.CreateTempSubdirectory("DynamicStructures-console-").FullName;
        try
        {
            string grid = Path.Combine(directory, "one cell.txt");
            File.WriteAllText(grid, ".");
            Check.Case("Console: one-cell flood fill accepts Enter defaults", () =>
                Contains(RunApp($"5\n3\n\"{grid}\"\n\n\n\n0\n\n0\n"), "Закрашено клеток: 1"));
            string ragged = Path.Combine(directory, "blank row.txt");
            File.WriteAllText(ragged, ".\n\n.");
            Check.Case("Console: blank grid row remains a barrier", () =>
                Contains(RunApp($"5\n3\n{ragged}\n1\n1\n*\n0\n\n0\n"), "Закрашено клеток: 1"));
            string empty = Path.Combine(directory, "empty.txt");
            File.WriteAllText(empty, "");
            Check.Case("Console: empty list can be doubled and cannot be swapped", () =>
            {
                string output = RunApp($"4\n11\n{empty}\n12\n{empty}\n0\n\n0\n");
                Contains(output, "После: [пуст]");
                Contains(output, "нет элементов для перестановки");
            });
            string bad = Path.Combine(directory, "bad.txt");
            File.WriteAllText(bad, "1 bad\n2");
            Check.Case("Console: bad/missing files and absent swap values preserve navigation", () =>
            {
                string output = RunApp($"4\n9\n{bad}\n9\n{bad}.missing\n12\n\n2\n10\n999\n11\n\n0\n\n0\n");
                Contains(output, "«bad» — не целое число");
                Contains(output, "Значение 999 не найдено");
                Contains(output, "После: 1 → 2 → 3 → 1 → 2 → 3");
            });
        }
        finally { Directory.Delete(directory, true); }

        string[] eofInputs =
        [
            "", "1\n", "1\n1\n", "2\n", "2\n1\n", "3\n", "3\n1\n", "4\n", "4\n9\n",
            "4\n10\n\n", "4\n12\n\n", "5\n", "5\n1\n", "5\n3\n", "5\n3\n\n", "5\n4\n", "5\n5\n", "5\n5\nнет\n"
        ];
        for (int i = 0; i < eofInputs.Length; i++)
        {
            string input = eofInputs[i];
            Check.Case($"Console: EOF exits cleanly at prompt {i + 1}", () => RunApp(input));
        }
    }

    private static string RunApp(string input)
    {
        var start = new ProcessStartInfo("dotnet")
        {
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            StandardInputEncoding = new UTF8Encoding(false),
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8
        };
        start.ArgumentList.Add("exec");
        start.ArgumentList.Add("--runtimeconfig");
        start.ArgumentList.Add(Path.ChangeExtension(typeof(ConsoleChecks).Assembly.Location, ".runtimeconfig.json"));
        start.ArgumentList.Add(typeof(MyLinkedList).Assembly.Location);
        using var process = Process.Start(start) ?? throw new InvalidOperationException("Cannot start application.");
        var output = process.StandardOutput.ReadToEndAsync();
        var error = process.StandardError.ReadToEndAsync();
        process.StandardInput.Write(input);
        process.StandardInput.Close();
        if (!process.WaitForExit(15_000))
        {
            process.Kill(entireProcessTree: true);
            throw new TimeoutException("Application did not finish within 15 seconds.");
        }
        string stdout = output.GetAwaiter().GetResult();
        string stderr = error.GetAwaiter().GetResult();
        Check.True(process.ExitCode == 0, $"Exit code {process.ExitCode}: {stderr}");
        Check.True(stderr.Length == 0, $"Unexpected stderr: {stderr}");
        return stdout;
    }

    private static void Contains(string output, string expected)
        => Check.True(output.Contains(expected, StringComparison.Ordinal), $"Missing output: {expected}");
}
