using DynamicStructures.Common;
using DynamicStructures.Part1_Stack;
using DynamicStructures.Part2_Queue;
using DynamicStructures.Part3_Algorithms;
using DynamicStructures.Part3_Applications;
using DynamicStructures.Part4_LinkedListAlgos;

namespace DynamicStructures.Tests;

internal static class RegressionChecks
{
    public static void Run()
    {
        Check.Case("Append copies E and supports empty lists and self-append", Append);
        Check.Case("Split uses first occurrence and transfers existing nodes", Split);
        Check.Case("Doubling is finite and creates independent nodes", Double);
        Check.Case("Swap validates both positions/values before changing the list", Swap);
        Check.Case("Task 5 self-insertion at head, middle and tail", InsertSelf);
        Check.Case("Tasks 1-12 match array models on 500 random lists", RandomLists);
        Check.Case("Stack and queue match standard collections on 2000 operations", StackAndQueue);
        Check.Case("Path resolution handles Windows, Unix, UNC and relative parents", Paths);
        Check.Case("Path resolution matches Windows GetFullPath on 500 absolute paths", RandomPaths);
        Check.Case("Decision tree reaches all six leaves and prints all branches", Tree);
        Check.Case("List files preserve empty lists and reject malformed integers", Files);
        Check.Case("Josephus matches independent removal model, including Int32.MaxValue step", Josephus);
        Check.Case("Flood fill handles ragged grids, barriers and wave colors", Fill);
        Check.Case("Arithmetic respects unary minus, powers, functions and invalid syntax", Arithmetic);
        Check.Case("Numeric input never returns NaN or infinity", NonFinite);
    }

    private static void Append()
    {
        var l = new MyLinkedList([1, 2]);
        var e = new MyLinkedList([3, 4]);
        var oldHead = l.Head;
        Task9.AppendList(l, e);
        Check.Sequence([1, 2, 3, 4], Values(l));
        Check.Sequence([3, 4], Values(e));
        Check.True(ReferenceEquals(oldHead, l.Head), "Original L nodes must be retained.");
        e.Head!.Value = 99;
        Check.Sequence([1, 2, 3, 4], Values(l));
        Task9.AppendList(l, new MyLinkedList());
        Check.Sequence([1, 2, 3, 4], Values(l));
        var empty = new MyLinkedList();
        Task9.AppendList(empty, e);
        Check.Sequence([99, 4], Values(empty));
        Check.True(!ReferenceEquals(empty.Head, e.Head), "Appended nodes must be copies.");
        Task9.AppendList(empty, empty);
        Check.Sequence([99, 4, 99, 4], Values(empty));
        Task9.AppendList(new MyLinkedList(), new MyLinkedList());
    }

    private static void Split()
    {
        var list = new MyLinkedList([1, 2, 3, 2, 4]);
        var expectedHead = list.Head!.Next!.Next;
        var second = Task10.SplitAtFirst(list, 2);
        Check.Sequence([1, 2], Values(list));
        Check.Sequence([3, 2, 4], Values(second));
        Check.True(ReferenceEquals(expectedHead, second.Head), "Split must transfer nodes.");
        second.AddFirst(9);
        Check.Sequence([1, 2], Values(list));
        Check.True(Task10.SplitAtFirst(list, 8).IsEmpty, "Absent number must return an empty list.");
        Check.Sequence([1, 2], Values(list));
        Check.True(Task10.SplitAtFirst(new MyLinkedList(), 1).IsEmpty, "Empty split failed.");
        Check.True(Task10.SplitAtFirst(list, 2).IsEmpty, "Split at tail must return empty.");
        Check.Sequence([2], Values(Task10.SplitAtFirst(list, 1)));
        Check.Sequence([1], Values(list));
    }

    private static void Double()
    {
        var empty = new MyLinkedList();
        Task11.DoubleList(empty);
        Check.True(empty.IsEmpty, "Doubling empty list failed.");
        var list = new MyLinkedList([7]);
        Task11.DoubleList(list);
        Check.Sequence([7, 7], Values(list));
        Check.True(!ReferenceEquals(list.Head, list.Head!.Next), "Copies must be independent.");
        list.Head.Value = 8;
        Check.Sequence([8, 7], Values(list));
        Task11.DoubleList(list);
        Check.Sequence([8, 7, 8, 7], Values(list));
    }

    private static void Swap()
    {
        var list = new MyLinkedList([1, 2, 1, 3]);
        Task12.SwapByPosition(list, 3, 0);
        Check.Sequence([3, 2, 1, 1], Values(list));
        Task12.SwapByPosition(list, 1, 1);
        Check.Sequence([3, 2, 1, 1], Values(list));
        Check.Throws<ArgumentOutOfRangeException>(() => Task12.SwapByPosition(list, 0, 10));
        Check.Throws<ArgumentOutOfRangeException>(() => Task12.SwapByPosition(list, -1, 0));
        Check.Throws<ArgumentOutOfRangeException>(() => Task12.SwapByPosition(list, 10, 10));
        Check.Sequence([3, 2, 1, 1], Values(list));
        Task12.SwapByValue(list, 1, 3);
        Check.Sequence([1, 2, 3, 1], Values(list));
        Task12.SwapByValue(list, 1, 1);
        Check.Throws<ArgumentException>(() => Task12.SwapByValue(list, 1, 99));
        Check.Sequence([1, 2, 3, 1], Values(list));
        Check.Throws<ArgumentOutOfRangeException>(() => Task12.SwapByPosition(new MyLinkedList(), 0, 0));
        Check.Throws<ArgumentException>(() => Task12.SwapByValue(new MyLinkedList(), 1, 1));
    }

    private static void InsertSelf()
    {
        foreach (int x in new[] { 1, 2, 3, 8 })
        {
            var list = new MyLinkedList([1, 2, 3]);
            Task5.InsertSelfAfterFirstX(list, x);
            int[] expected = x == 8 ? [1, 2, 3] : Enumerable.Range(1, x).Concat([1, 2, 3]).Concat(Enumerable.Range(x + 1, 3 - x)).ToArray();
            Check.Sequence(expected, Values(list));
        }
        var single = new MyLinkedList([1]);
        Task5.InsertSelfAfterFirstX(single, 1);
        Check.Sequence([1, 1], Values(single));
        Task5.InsertSelfAfterFirstX(new MyLinkedList(), 1);
    }

    private static void RandomLists()
    {
        var random = new Random(20261116);
        for (int trial = 0; trial < 500; trial++)
        {
            int[] a = Enumerable.Range(0, random.Next(25)).Select(_ => random.Next(-4, 5)).ToArray();
            int[] b = Enumerable.Range(0, random.Next(15)).Select(_ => random.Next(-4, 5)).ToArray();
            int x = random.Next(-5, 6);
            int index = Array.IndexOf(a, x);
            var list = new MyLinkedList(a);
            Task1.Reverse(list);
            Check.Sequence(a.Reverse(), Values(list));
            list = new(a); Task2.MoveLastToFirst(list);
            Check.Sequence(a.TakeLast(1).Concat(a.Take(Math.Max(0, a.Length - 1))), Values(list));
            list = new(a); Task2.MoveFirstToLast(list);
            Check.Sequence(a.Skip(1).Concat(a.Take(1)), Values(list));
            Check.Equal(a.Distinct().Count(), Task3.CountDistinct(new(a)));
            list = new(a); Task4.RemoveDuplicates(list);
            Check.Sequence(a.Distinct(), Values(list));
            list = new(a); Task5.InsertSelfAfterFirstX(list, x);
            Check.Sequence(index < 0 ? a : a.Take(index + 1).Concat(a).Concat(a.Skip(index + 1)), Values(list));
            list = new(a.Order()); Task6.InsertSorted(list, x);
            Check.Sequence(a.Append(x).Order(), Values(list));
            list = new(a); Task7.RemoveAllOccurrences(list, x);
            Check.Sequence(a.Where(v => v != x), Values(list));
            list = new(a); Task8.InsertBeforeFirstOccurrence(list, x, 99);
            Check.Sequence(index < 0 ? a : a.Take(index).Append(99).Concat(a.Skip(index)), Values(list));
            list = new(a); Task9.AppendList(list, new(b));
            Check.Sequence(a.Concat(b), Values(list));
            list = new(a); var second = Task10.SplitAtFirst(list, x);
            Check.Sequence(index < 0 ? a : a.Take(index + 1), Values(list));
            Check.Sequence(index < 0 ? [] : a.Skip(index + 1), Values(second));
            list = new(a); Task11.DoubleList(list);
            Check.Sequence(a.Concat(a), Values(list));
            if (a.Length == 0) continue;
            int first = random.Next(a.Length), last = random.Next(a.Length);
            list = new(a); Task12.SwapByPosition(list, first, last);
            int[] expected = (int[])a.Clone();
            (expected[first], expected[last]) = (expected[last], expected[first]);
            Check.Sequence(expected, Values(list));
            int val1 = a[first], val2 = a[last];
            first = Array.IndexOf(a, val1); last = Array.IndexOf(a, val2);
            expected = (int[])a.Clone();
            (expected[first], expected[last]) = (expected[last], expected[first]);
            list = new(a); Task12.SwapByValue(list, val1, val2);
            Check.Sequence(expected, Values(list));
        }
    }

    private static void StackAndQueue()
    {
        var stack = new LinkedStack<int>(); var stackModel = new Stack<int>();
        var queue = new LinkedQueue<int>(); var queueModel = new Queue<int>();
        Check.Throws<InvalidOperationException>(() => stack.Pop());
        Check.Throws<InvalidOperationException>(() => stack.Top());
        Check.Throws<InvalidOperationException>(() => queue.Dequeue());
        Check.Throws<InvalidOperationException>(() => queue.Peek());
        var random = new Random(42);
        for (int i = 0; i < 2000; i++)
        {
            if (stackModel.Count == 0 || random.Next(2) == 0)
            {
                stack.Push(i); stackModel.Push(i); queue.Enqueue(i); queueModel.Enqueue(i);
            }
            else
            {
                Check.Equal(stackModel.Pop(), stack.Pop());
                Check.Equal(queueModel.Dequeue(), queue.Dequeue());
            }
            Check.Equal(stackModel.Count, stack.Count);
            Check.Equal(queueModel.Count, queue.Count);
            if (stackModel.Count > 0) Check.Equal(stackModel.Peek(), stack.Top());
            if (queueModel.Count > 0) Check.Equal(queueModel.Peek(), queue.Peek());
        }
        while (stackModel.Count > 0)
        {
            Check.Equal(stackModel.Pop(), stack.Pop());
            Check.Equal(queueModel.Dequeue(), queue.Dequeue());
        }
        queue.Enqueue(7); Check.Equal(7, queue.Dequeue());
        Check.True(queue.IsEmpty() && stack.IsEmpty(), "Structures must be empty.");
    }

    private static void Paths()
    {
        (string Input, string Expected)[] cases =
        [
            ("/a/./b/../../c/", "/c"), ("/../../", "/"), ("a/../../b", "../b"),
            ("../../a/..", "../.."), ("a/..", "."), (".", "."), ("..", ".."),
            ("a//b/../c", "a/c"), ("/home/Иван/../Мария", "/home/Мария"),
            (@"c:\a\.\b\..\file.txt", @"C:\a\file.txt"), (@"C:\..\..", @"C:\"),
            (@"a\..\..\b", @"..\b"), (@"\a\..", @"\"),
            (@"\\server\share\a\..\..\b", @"\\server\share\b"),
            (@"\\server\share", @"\\server\share\"), ("C:/a//b/../c", @"C:\a\c"),
            ("my folder/../report.txt", "report.txt"), ("///a////b", "/a/b")
        ];
        foreach (var (input, expected) in cases)
        {
            string actual = FilePathResolver.Resolve(input);
            Check.Equal(expected, actual);
            Check.Equal(actual, FilePathResolver.Resolve(actual));
        }
        foreach (string bad in new[] { "", "   ", "C:folder", "C:", @"\\server", @"\\?\C:\a", @"\\.\pipe\test", "a\0b" })
            Check.Throws<ArgumentException>(() => FilePathResolver.Resolve(bad));
        var steps = new List<string>();
        FilePathResolver.Resolve("a/../b", steps.Add);
        Check.True(steps.Any(s => s.Contains("Push")) && steps.Any(s => s.Contains("Pop")), "Stack trace missing.");
    }

    private static void RandomPaths()
    {
        if (!OperatingSystem.IsWindows()) return;
        var random = new Random(15);
        string[] parts = ["a", "b", "c", ".", "..", ""];
        for (int trial = 0; trial < 500; trial++)
        {
            string path = @"C:\" + string.Join('\\', Enumerable.Range(0, random.Next(1, 30)).Select(_ => parts[random.Next(parts.Length)]));
            string expected = Path.GetFullPath(path);
            if (expected.Length > 3) expected = expected.TrimEnd('\\');
            Check.Equal(expected, FilePathResolver.Resolve(path));
        }
    }

    private static void Tree()
    {
        var tree = DecisionTree.CreateTransportExample();
        (bool[] Answers, string Result)[] cases =
        [
            ([true, true], "Поезд"), ([true, false], "Междугородний автобус"),
            ([false, true, true], "Велосипед"), ([false, true, false], "Пешком"),
            ([false, false, true], "Общественный транспорт"), ([false, false, false], "Такси")
        ];
        foreach (var (answers, result) in cases)
        {
            int next = 0;
            Check.Equal(result, tree.Evaluate(_ => answers[next++]));
            Check.Equal(answers.Length, next);
        }
        var output = new StringWriter(); tree.Print(output);
        Check.Equal(11, output.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Length);
        Check.Equal("Done", new DecisionTree(DecisionNode.Result("Done")).Evaluate(_ => throw new Exception("Leaf asked a question.")));
        Check.Throws<ArgumentException>(() => DecisionNode.Result(" "));
        Check.Throws<ArgumentNullException>(() => DecisionNode.Question("?", null!, DecisionNode.Result("No")));
    }

    private static void Files()
    {
        string directory = Directory.CreateTempSubdirectory("DynamicStructures-files-").FullName;
        string path = Path.Combine(directory, "lists.txt");
        try
        {
            File.WriteAllText(path, "1 -2\t3\r\n4 5\r\n");
            var lists = ListFileReader.Read(path, 2);
            Check.Sequence([1, -2, 3], Values(lists[0]));
            Check.Sequence([4, 5], Values(lists[1]));
            File.WriteAllText(path, "\n\n");
            Check.True(ListFileReader.Read(path, 2).All(l => l.IsEmpty), "Empty lines must be empty lists.");
            File.WriteAllText(path, "");
            Check.True(ListFileReader.Read(path, 1)[0].IsEmpty, "Empty file must allow a single empty list.");
            Check.Throws<FormatException>(() => ListFileReader.Read(path, 2));
            File.WriteAllText(path, "1 nope\n2");
            Check.Throws<FormatException>(() => ListFileReader.Read(path, 2));
            File.WriteAllText(path, "2147483648");
            Check.Throws<FormatException>(() => ListFileReader.Read(path, 1));
            Check.Throws<FileNotFoundException>(() => ListFileReader.Read(path + ".missing", 1));
        }
        finally { Directory.Delete(directory, true); }
    }

    private static void Josephus()
    {
        foreach (int n in Enumerable.Range(1, 50))
        foreach (int k in new[] { 1, 2, 3, 1_000_000, int.MaxValue })
        {
            var circle = Enumerable.Range(1, n).ToList();
            var order = new List<int>();
            int position = 0;
            while (circle.Count > 1)
            {
                position = (int)(((long)position + k - 1) % circle.Count);
                order.Add(circle[position]); circle.RemoveAt(position);
            }
            var actual = JosephusProblem.Solve(n, k);
            Check.Sequence(order, actual.Order);
            Check.Equal(circle[0], actual.Survivor);
            Check.Equal(circle[0], JosephusProblem.SolveByFormula(n, k));
        }
        Check.Throws<ArgumentOutOfRangeException>(() => JosephusProblem.SolveByFormula(0, 1));
        Check.Throws<ArgumentOutOfRangeException>(() => JosephusProblem.SolveByFormula(1, 0));
    }

    private static void Fill()
    {
        char[][] grid = ["..#".ToCharArray(), ".".ToCharArray(), "...".ToCharArray(), [], "..".ToCharArray()];
        var result = FloodFill.Fill(grid, 0, 0, '*');
        Check.Equal(6, result.Painted);
        Check.Equal("**#", new string(grid[0]));
        Check.Equal("..", new string(grid[4]));
        Check.Equal(0, FloodFill.Fill(grid, 0, 0, '*').Painted);
        Check.Throws<ArgumentOutOfRangeException>(() => FloodFill.Fill(grid, 3, 0, '*'));
        char[][] waves = ["000".ToCharArray(), "000".ToCharArray()];
        Check.Equal(6, FloodFill.Fill(waves, 0, 0, '0', numberWaves: true).Painted);
        Check.Equal("012", new string(waves[0]));
        Check.Equal("123", new string(waves[1]));
    }

    private static void Arithmetic()
    {
        (string Input, double Expected)[] cases =
        [
            ("(2+3)*4", 20), ("2^3^2", 512), ("-2^2", -4), ("2^-3", 0.125),
            ("--2", 2), ("-(2+3)*4", -20), ("sin(0)+cos(0)", 1), ("sqrt(16)+ln(1)", 4),
            ("1,5+2.5", 4), (".5+1.", 1.5)
        ];
        foreach (var (input, expected) in cases)
            Check.True(Math.Abs(expected - PostfixEvaluator.Evaluate(InfixToPostfix.Convert(input))) < 1e-12, $"Wrong result: {input}");
        foreach (string bad in new[] { "", "1 2", "2(3)", "(2+)", "((2)", "1..2", "1,2.3", "sqrt()" })
            Check.Throws<FormatException>(() => InfixToPostfix.Convert(bad));
        Check.Throws<FormatException>(() => PostfixEvaluator.Evaluate("1 +"));
        Check.Throws<DivideByZeroException>(() => PostfixEvaluator.Evaluate("1 0 /"));
        Check.Throws<ArithmeticException>(() => PostfixEvaluator.Evaluate("-1 sqrt"));
        Check.Equal(5.0, PostfixEvaluator.Evaluate("2 3 +", out long operations));
        Check.Equal(6L, operations);
    }

    private static void NonFinite()
    {
        Check.Throws<ArithmeticException>(() => PostfixEvaluator.Evaluate("1e309"));
        Check.Throws<ArithmeticException>(() => PostfixEvaluator.Evaluate("10 400 ^"));
    }

    private static int[] Values(MyLinkedList list)
    {
        var nodes = new HashSet<ListNode>();
        var values = new List<int>();
        for (var node = list.Head; node is not null; node = node.Next)
        {
            if (!nodes.Add(node)) throw new InvalidOperationException("Cycle in linked list.");
            values.Add(node.Value);
        }
        Check.Equal(values.Count, list.Count);
        return values.ToArray();
    }
}
