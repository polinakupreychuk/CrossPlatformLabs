using System;
using System.Collections.Generic;
using System.IO;

namespace ClassLibraryLab4
{
    public class Lab3
    {
        public static void ExecuteLab3(string inputFile, string outputFile)
        {
            try
            {
                if (!File.Exists(inputFile))
                    throw new FileNotFoundException($"Input file not found: {inputFile}");

                string[] lines = File.ReadAllLines(inputFile);
                var (maze, start, end, keyCosts) = ParseInput(lines);

                string result = BFS(maze, start, end, keyCosts);
                File.WriteAllText(outputFile, result);
                Console.WriteLine("Result written to output file.");
            }
            catch (Exception ex)
            {
                File.WriteAllText(outputFile, "Error: " + ex.Message);
            }
        }

        private static (char[,], (int, int), (int, int), int[]) ParseInput(string[] lines)
        {
            var dimensions = lines[0].Split();
            int R = int.Parse(dimensions[0]);
            int C = int.Parse(dimensions[1]);
            int[] keyCosts = Array.ConvertAll(lines[1].Split(), int.Parse);

            char[,] maze = new char[R, C];
            (int, int) start = (0, 0);
            (int, int) end = (0, 0);

            for (int i = 0; i < R; i++)
            {
                string row = lines[i + 2];
                for (int j = 0; j < C; j++)
                {
                    maze[i, j] = row[j];
                    if (row[j] == 'S') start = (i, j);
                    if (row[j] == 'E') end = (i, j);
                }
            }

            return (maze, start, end, keyCosts);
        }

        private static string BFS(char[,] maze, (int, int) start, (int, int) end, int[] keyCosts)
        {
            int R = maze.GetLength(0);
            int C = maze.GetLength(1);
            var queue = new Queue<(int row, int col, int keyMask, int cost)>();
            bool[,,] visited = new bool[R, C, 16];

            queue.Enqueue((start.Item1, start.Item2, 0, 0));
            visited[start.Item1, start.Item2, 0] = true;

            int[] dRow = { -1, 1, 0, 0 };
            int[] dCol = { 0, 0, -1, 1 };

            while (queue.Count > 0)
            {
                var (row, col, keyMask, cost) = queue.Dequeue();

                if ((row, col) == end)
                    return cost.ToString();

                for (int i = 0; i < 4; i++)
                {
                    int newRow = row + dRow[i];
                    int newCol = col + dCol[i];

                    if (!IsValidCell(newRow, newCol, R, C, maze))
                        continue;

                    char currentCell = maze[newRow, newCol];
                    int newKeyMask = UpdateKeyMask(keyMask, currentCell);
                    int newCost = UpdateCost(cost, keyMask, currentCell, keyCosts);

                    if (newKeyMask == keyMask && visited[newRow, newCol, newKeyMask])
                        continue;

                    visited[newRow, newCol, newKeyMask] = true;
                    queue.Enqueue((newRow, newCol, newKeyMask, newCost));
                }
            }

            return "Sleep";
        }

        private static bool IsValidCell(int row, int col, int R, int C, char[,] maze)
        {
            return row >= 0 && col >= 0 && row < R && col < C && maze[row, col] != 'X';
        }

        private static int UpdateKeyMask(int keyMask, char cell)
        {
            return cell switch
            {
                'R' => keyMask | (1 << 0),
                'G' => keyMask | (1 << 1),
                'B' => keyMask | (1 << 2),
                'Y' => keyMask | (1 << 3),
                _ => keyMask
            };
        }

        private static int UpdateCost(int cost, int keyMask, char cell, int[] keyCosts)
        {
            return cell switch
            {
                'R' when (keyMask & (1 << 0)) == 0 => cost + keyCosts[0],
                'G' when (keyMask & (1 << 1)) == 0 => cost + keyCosts[1],
                'B' when (keyMask & (1 << 2)) == 0 => cost + keyCosts[2],
                'Y' when (keyMask & (1 << 3)) == 0 => cost + keyCosts[3],
                _ => cost
            };
        }
    }
}
