using System.Text;

namespace DynamicStructures.Part3_Applications;

internal static class BinaryTreeDrawing
{
    private sealed record Node(char Label, Node? Left, Node? Right)
    {
        public int Column { get; set; }
    }

    public static void Print(string input, TextWriter writer)
    {
        // При чтении справа налево поддеревья уже собраны к моменту чтения узла.
        var subtrees = new Stack<Node?>();
        for (int i = input.Length - 1; i >= 0; i--)
        {
            char token = input[i];
            if (char.IsWhiteSpace(token)) continue;
            if (token == '*')
            {
                subtrees.Push(null);
                continue;
            }

            if (subtrees.Count < 2)
            {
                writer.WriteLine("  Некорректная запись дерева.");
                return;
            }
            var left = subtrees.Pop();
            var right = subtrees.Pop();
            subtrees.Push(new Node(token, left, right));
        }

        if (subtrees.Count != 1)
        {
            writer.WriteLine("  Некорректная запись дерева.");
            return;
        }

        var root = subtrees.Pop();
        if (root is null)
        {
            writer.WriteLine("  Дерево пусто.");
            return;
        }

        var levels = new List<List<Node>>();
        var pending = new Stack<(Node Node, int Depth)>();
        Node? current = root;
        int depth = 0;
        const int boxCenter = 5;
        const int nodeSpacing = 12;
        int column = boxCenter;

        // Симметричный обход размещает левое поддерево слева от родителя.
        while (current is not null || pending.Count > 0)
        {
            while (current is not null)
            {
                pending.Push((current, depth++));
                current = current.Left;
            }

            (current, depth) = pending.Pop();
            current.Column = column;
            column += nodeSpacing;
            while (levels.Count <= depth) levels.Add(new List<Node>());
            levels[depth].Add(current);
            current = current.Right;
            depth++;
        }

        foreach (var level in levels)
        {
            var top = new StringBuilder();
            var labels = new StringBuilder();
            var divider = new StringBuilder();
            var pointers = new StringBuilder();
            var bottom = new StringBuilder();
            var branches = new StringBuilder();
            foreach (var node in level)
            {
                int boxStart = node.Column - boxCenter;
                AppendAt(top, boxStart, "┌─────────┐");
                AppendAt(labels, boxStart, $"│    {node.Label}    │");
                AppendAt(divider, boxStart, "├────┬────┤");
                string leftCell = node.Left is null ? "NULL" : "  ● ";
                string rightCell = node.Right is null ? "NULL" : "  ● ";
                AppendAt(pointers, boxStart, $"│{leftCell}│{rightCell}│");
                char leftPort = node.Left is null ? '─' : '┬';
                char rightPort = node.Right is null ? '─' : '┬';
                AppendAt(bottom, boxStart, $"└──{leftPort}─┴──{rightPort}─┘");

                if (node.Left is { } left)
                    AppendAt(branches, left.Column, "┌")
                        .Append('─', node.Column - 2 - left.Column - 1).Append('┘');
                if (node.Right is { } right)
                    AppendAt(branches, node.Column + 3, "└")
                        .Append('─', right.Column - (node.Column + 3) - 1).Append('┐');
            }
            writer.WriteLine(top);
            writer.WriteLine(labels);
            writer.WriteLine(divider);
            writer.WriteLine(pointers);
            writer.WriteLine(bottom);
            if (branches.Length > 0) writer.WriteLine(branches);
        }
    }

    private static StringBuilder AppendAt(StringBuilder row, int column, string text)
        => row.Append(' ', column - row.Length).Append(text);
}
