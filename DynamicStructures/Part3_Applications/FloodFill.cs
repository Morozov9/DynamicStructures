using DynamicStructures.Part2_Queue;

namespace DynamicStructures.Part3_Applications;

/// <summary>Результат заливки.</summary>
/// <param name="Painted">Сколько клеток закрашено.</param>
/// <param name="Waves">Сколько «волн» (слоёв поиска в ширину) потребовалось.</param>
/// <param name="MaxQueueSize">Максимальный размер очереди за время работы.</param>
public sealed record FloodFillResult(int Painted, int Waves, int MaxQueueSize);

/// <summary>
/// Заливка краской (Flood Fill) — применение структуры данных «ОЧЕРЕДЬ».
///
/// Условие: есть картинка (сетка клеток разного цвета). Нужно закрасить новым цветом
/// всю область одного цвета, связанную с выбранной клеткой по сторонам (вверх/вниз/влево/вправо),
/// как инструмент «ведро» в графическом редакторе.
///
/// Алгоритм — поиск в ширину (BFS):
///   1. Кладём стартовую клетку в очередь и отмечаем как посещённую.
///   2. Пока очередь не пуста: берём клетку из начала (Dequeue), закрашиваем её,
///      всех непосещённых соседей того же цвета отмечаем и кладём в конец (Enqueue).
/// Очередь (FIFO) даёт порядок «волнами»: сначала клетки на расстоянии 0, потом 1, 2, …
/// — заливка расходится от точки клика кругами. Используется наш LinkedQueue.
///
/// Сложность: каждая клетка попадает в очередь не более одного раза, у неё 4 соседа
/// =>  O(W·H) времени, память — O(W·H) в худшем случае (очередь хранит фронт волны).
/// </summary>
public static class FloodFill
{
    private static readonly (int dr, int dc)[] Directions = { (-1, 0), (1, 0), (0, -1), (0, 1) };

    /// <summary>
    /// Залить область, содержащую клетку (row, col), цветом newColor. Строки сетки могут быть разной длины.
    /// numberWaves = true: вместо newColor клетки красятся цифрой номера волны (0,1,2,… по модулю 10) —
    /// наглядно показывает, как работает очередь.
    /// </summary>
    public static FloodFillResult Fill(char[][] grid, int row, int col, char newColor, bool numberWaves = false)
    {
        if (row < 0 || row >= grid.Length || col < 0 || col >= grid[row].Length)
            throw new ArgumentOutOfRangeException(nameof(row), "Начальная клетка вне сетки.");

        char target = grid[row][col];
        if (!numberWaves && target == newColor)
            return new FloodFillResult(0, 0, 0); // уже нужного цвета — делать нечего

        var visited = new bool[grid.Length][];
        for (int r = 0; r < grid.Length; r++)
            visited[r] = new bool[grid[r].Length];

        var queue = new LinkedQueue<(int Row, int Col)>();
        queue.Enqueue((row, col));
        visited[row][col] = true;

        int painted = 0, waves = 0, maxQueue = 1;

        while (!queue.IsEmpty())
        {
            int layerSize = queue.Count;                    // клетки текущей волны
            char color = numberWaves ? (char)('0' + waves % 10) : newColor;

            for (int i = 0; i < layerSize; i++)
            {
                var (r, c) = queue.Dequeue();
                grid[r][c] = color;
                painted++;

                foreach (var (dr, dc) in Directions)
                {
                    int nr = r + dr, nc = c + dc;
                    if (nr < 0 || nr >= grid.Length || nc < 0 || nc >= grid[nr].Length) continue;
                    if (visited[nr][nc] || grid[nr][nc] != target) continue;

                    visited[nr][nc] = true;                 // помечаем при постановке в очередь —
                    queue.Enqueue((nr, nc));                // так клетка не попадёт туда дважды
                    if (queue.Count > maxQueue) maxQueue = queue.Count;
                }
            }
            waves++;
        }

        return new FloodFillResult(painted, waves, maxQueue);
    }
}
