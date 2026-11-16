namespace DynamicStructures.Part3_Applications;

/// <summary>Узел бинарного дерева: либо вопрос с двумя ветвями, либо конечное решение.</summary>
public sealed class DecisionNode
{
    public string Text { get; }
    public DecisionNode? Yes { get; }
    public DecisionNode? No { get; }
    public bool IsLeaf => Yes is null;

    private DecisionNode(string text, DecisionNode? yes, DecisionNode? no)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        Text = text;
        Yes = yes;
        No = no;
    }

    public static DecisionNode Result(string text) => new(text, null, null);

    public static DecisionNode Question(string text, DecisionNode yes, DecisionNode no)
    {
        ArgumentNullException.ThrowIfNull(yes);
        ArgumentNullException.ThrowIfNull(no);
        return new DecisionNode(text, yes, no);
    }
}

/// <summary>Последовательный выбор ветвей «да/нет» до листа. Обход занимает O(h).</summary>
public sealed class DecisionTree
{
    public DecisionNode Root { get; }

    public DecisionTree(DecisionNode root)
    {
        ArgumentNullException.ThrowIfNull(root);
        Root = root;
    }

    public string Evaluate(Func<string, bool> answer)
    {
        ArgumentNullException.ThrowIfNull(answer);
        var current = Root;
        while (!current.IsLeaf)
            current = answer(current.Text) ? current.Yes! : current.No!;
        return current.Text;
    }

    /// <summary>Печатает все узлы и подписанные ветви без рекурсивного обхода.</summary>
    public void Print(TextWriter? output = null)
    {
        var writer = output ?? Console.Out;
        var pending = new Part1_Stack.LinkedStack<(DecisionNode Node, string Indent, string Branch)>();
        pending.Push((Root, "  ", ""));
        while (!pending.IsEmpty())
        {
            var (node, indent, branch) = pending.Pop();
            writer.WriteLine($"{indent}{branch}{(node.IsLeaf ? "Решение: " : "")}{node.Text}");
            if (node.IsLeaf) continue;
            pending.Push((node.No!, indent + "    ", "Нет → "));
            pending.Push((node.Yes!, indent + "    ", "Да  → "));
        }
    }

    public static DecisionTree CreateTransportExample() => new(
        DecisionNode.Question("Нужно ехать далеко?",
            DecisionNode.Question("Есть железнодорожное сообщение?",
                DecisionNode.Result("Поезд"),
                DecisionNode.Result("Междугородний автобус")),
            DecisionNode.Question("На улице хорошая погода?",
                DecisionNode.Question("Есть велосипед?",
                    DecisionNode.Result("Велосипед"),
                    DecisionNode.Result("Пешком")),
                DecisionNode.Question("Ходит общественный транспорт?",
                    DecisionNode.Result("Общественный транспорт"),
                    DecisionNode.Result("Такси")))));
}
