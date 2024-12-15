using System;
using System.IO;

namespace ClassLibraryLab4
{
    public class Lab2
    {
        public static void ExecuteLab2(string inputFile, string outputFile)
        {
            try
            {
                // Читаємо вхідні дані
                string[] input = ReadInputData(inputFile).Split(new[] { ' ', '\n', '\r' }, 
                    StringSplitOptions.RemoveEmptyEntries);

                // Перевірка валідності даних
                if (input.Length != 2)
                {
                    throw new ArgumentException("Input must contain exactly two integers.");
                }

                int m = int.Parse(input[0]);
                int n = int.Parse(input[1]);

                if (!IsValidInput(m, n))
                {
                    throw new ArgumentException("Invalid input values. Ensure 2 ≤ m ≤ 50, 2 ≤ n ≤ 50, and m ≤ n.");
                }

                // Обчислюємо кількість способів покриття плитками
                long result = CountTilings(m, n);

                // Записуємо результат у файл
                File.WriteAllText(outputFile, result.ToString());

                Console.WriteLine($"Results have been successfully saved to {outputFile}");
            }
            catch (Exception ex)
            {
                File.WriteAllText(outputFile, "0");
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static string ReadInputData(string inputFile)
        {
            if (!File.Exists(inputFile))
            {
                throw new FileNotFoundException($"Input file not found: {inputFile}");
            }

            string inputData = File.ReadAllText(inputFile).Trim();
            if (string.IsNullOrEmpty(inputData))
            {
                throw new ArgumentException("Input data cannot be empty or null.");
            }

            return inputData;
        }

        public static bool IsValidInput(int m, int n)
        {
            return m >= 2 && m <= 50 && n >= 2 && n <= 50 && m <= n;
        }

        public static long CountTilings(int m, int n)
        {
            long[] dp = new long[n + 1];
            dp[0] = 1;

            for (int i = 1; i <= n; i++)
            {
                dp[i] = dp[i - 1];
                if (i >= m)
                {
                    dp[i] += dp[i - m];
                }
            }

            return dp[n];
        }
    }
}
