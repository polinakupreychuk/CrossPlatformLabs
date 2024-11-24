using System;
using System.Collections.Generic;
using System.IO;

public static class Program
{
    public static char[,]? grid;
    public static int n;
    public static (int x, int y) start;
    public static (int x, int y) end;

    public static void Main(string[] args)
    {
        // Отримуємо шлях до поточної директорії
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        
        // Піднімаємось на 4 рівні вгору до папки ResultExecution
        string resultExecutionPath = Path.GetFullPath(Path.Combine(baseDirectory, "..", "..", "..", "ResultExecution"));
        
        // Формуємо шляхи до файлів
        string inputPath = Path.Combine(resultExecutionPath, "INPUT.TXT");
        string outputPath = Path.Combine(resultExecutionPath, "OUTPUT.TXT");

        string[] input;
        try
        {
            input = File.ReadAllLines(inputPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading input file: {ex.Message}");
            return;
        }

        if (Execute(input, out var outputGrid))
        {
            try
            {
                File.WriteAllText(outputPath, "Yes\n" + string.Join("\n", outputGrid));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing output file: {ex.Message}");
            }
        }
        else
        {
            try
            {
                File.WriteAllText(outputPath, "No");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing output file: {ex.Message}");
            }
        }
    }

    public static bool Execute(string[] input, out string[] outputGrid)
    {
        n = int.Parse(input[0]);
        grid = new char[n, n];

        start = (-1, -1);
        end = (-1, -1);

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                grid[i, j] = input[i + 1][j];
                if (grid[i, j] == '@')
                {
                    start = (i, j);
                }
                else if (grid[i, j] == 'X')
                {
                    end = (i, j);
                }
            }
        }

        if (start == (-1, -1) || end == (-1, -1))
        {
            // Invalid input format
            outputGrid = new string[0];
            return false;
        }

        bool pathExists = Dijkstra();

        // Генеруємо вихідну сітку
        outputGrid = new string[n];
        for (int i = 0; i < n; i++)
        {
            var row = new char[n];
            for (int j = 0; j < n; j++)
            {
                row[j] = grid[i, j];
            }
            outputGrid[i] = new string(row);
        }

        return pathExists;
    }

    // Алгоритм Дейкстри для пошуку найкоротшого шляху
    private static bool Dijkstra()
    {
        var directions = new (int, int)[]
        {
            (-1, 0), (1, 0), (0, -1), (0, 1)
        };

        var dist = new int[n, n];
        var prev = new (int, int)?[n, n];
        var pq = new SortedSet<(int, int, int)>(Comparer<(int, int, int)>.Create((a, b) =>
        {
            int comp = a.Item1.CompareTo(b.Item1);
            if (comp == 0)
            {
                comp = a.Item2.CompareTo(b.Item2);
                if (comp == 0)
                {
                    comp = a.Item3.CompareTo(b.Item3);
                }
            }
            return comp;
        }));

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                dist[i, j] = int.MaxValue;
            }
        }

        dist[start.x, start.y] = 0;
        pq.Add((0, start.x, start.y));

        while (pq.Count > 0)
        {
            var (d, x, y) = pq.Min;
            pq.Remove(pq.Min);

            if (x == end.x && y == end.y)
            {
                MarkPath(prev, x, y);
                return true;
            }

            foreach (var (dx, dy) in directions)
            {
                int nx = x + dx;
                int ny = y + dy;

                if (nx >= 0 && ny >= 0 && nx < n && ny < n && (grid![nx, ny] == '.' || grid[nx, ny] == 'X'))
                {
                    int newDist = d + 1;
                    if (newDist < dist[nx, ny])
                    {
                        pq.Remove((dist[nx, ny], nx, ny));
                        dist[nx, ny] = newDist;
                        prev[nx, ny] = (x, y);
                        pq.Add((newDist, nx, ny));
                    }
                }
            }
        }

        return false;
    }

    // Функція для відмітки шляху у сітці
    private static void MarkPath((int, int)?[,] prev, int x, int y)
    {
        while (prev[x, y] != null)
        {
            grid![x, y] = '+';
            var (px, py) = prev[x, y]!.Value;
            x = px;
            y = py;
        }
        grid![start.x, start.y] = '@';
    }
}