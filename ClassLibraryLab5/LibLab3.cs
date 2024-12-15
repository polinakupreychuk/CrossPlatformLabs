namespace ClassLibraryLab5
{
    public class LibLab3
    {
        private static char[,] _maze;
        private static int _rows, _cols;
        private static int[] _keyCosts = new int[4]; // Вартість ключів: R, G, B, Y
        private static (int, int) _start, _end;

        private static readonly int[] DRow = { -1, 1, 0, 0 }; // Напрями руху: вгору, вниз, ліво, право
        private static readonly int[] DCol = { 0, 0, -1, 1 };

        public static string ExecuteLab3(string input)
        {
            try
            {
                ParseInput(input);
                return FindShortestPath();
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        private static void ParseInput(string input)
        {
            var parts = input.Split(' ');
            
            if (parts.Length < 7)
                throw new ArgumentException("Invalid input format");

            _rows = int.Parse(parts[0]);
            _cols = int.Parse(parts[1]);

            for (int i = 0; i < 4; i++)
            {
                _keyCosts[i] = int.Parse(parts[2 + i]);
            }

            string mazeString = parts[6];
            if (mazeString.Length != _rows * _cols)
                throw new ArgumentException("Maze string does not match the specified dimensions");

            _maze = new char[_rows, _cols];
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    char cell = mazeString[i * _cols + j];
                    _maze[i, j] = cell;

                    if (cell == 'S') _start = (i, j);
                    if (cell == 'E') _end = (i, j);
                }
            }
        }

        private static string FindShortestPath()
        {
            var queue = new Queue<(int Row, int Col, int KeyMask, int Cost)>();
            var visited = new bool[_rows, _cols, 16]; // Відвідані клітини з різними наборами ключів

            queue.Enqueue((_start.Item1, _start.Item2, 0, 0));
            visited[_start.Item1, _start.Item2, 0] = true;

            while (queue.Count > 0)
            {
                var (row, col, keyMask, cost) = queue.Dequeue();

                if ((row, col) == _end)
                {
                    return cost.ToString();
                }

                for (int i = 0; i < 4; i++)
                {
                    int newRow = row + DRow[i];
                    int newCol = col + DCol[i];

                    if (!IsValidCell(newRow, newCol))
                        continue;

                    char currentCell = _maze[newRow, newCol];

                    if (IsDoor(currentCell) && !HasKey(keyMask, currentCell))
                        continue;

                    int newKeyMask = AddKey(keyMask, currentCell);
                    int newCost = CalculateCost(cost, keyMask, currentCell);

                    if (!visited[newRow, newCol, newKeyMask])
                    {
                        visited[newRow, newCol, newKeyMask] = true;
                        queue.Enqueue((newRow, newCol, newKeyMask, newCost));
                    }
                }
            }

            return "Sleep";
        }

        private static bool IsValidCell(int row, int col)
        {
            return row >= 0 && row < _rows && col >= 0 && col < _cols && _maze[row, col] != 'X';
        }

        private static bool IsDoor(char cell)
        {
            return cell == 'R' || cell == 'G' || cell == 'B' || cell == 'Y';
        }

        private static bool HasKey(int keyMask, char door)
        {
            int keyIndex = GetKeyIndex(door);
            return (keyMask & (1 << keyIndex)) != 0;
        }

        private static int AddKey(int keyMask, char cell)
        {
            if (!IsDoor(cell)) return keyMask;

            int keyIndex = GetKeyIndex(cell);
            return keyMask | (1 << keyIndex);
        }

        private static int CalculateCost(int currentCost, int keyMask, char cell)
        {
            if (!IsDoor(cell)) return currentCost;

            int keyIndex = GetKeyIndex(cell);
            if ((keyMask & (1 << keyIndex)) == 0) // Якщо ключа ще не було
            {
                return currentCost + _keyCosts[keyIndex];
            }

            return currentCost;
        }

        private static int GetKeyIndex(char cell)
        {
            return cell switch
            {
                'R' => 0,
                'G' => 1,
                'B' => 2,
                'Y' => 3,
                _ => throw new ArgumentException("Invalid door or key character")
            };
        }
    }
}
