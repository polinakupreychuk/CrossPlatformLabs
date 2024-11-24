using System;
using System.IO;

public class Program
{
    // Функція для обчислення кількості способів плиткового покриття області розміру m x n
    public static long CountTilings(int m, int n)
    {
        long[] dp = new long[n + 1];
        dp[0] = 1;

        for (int i = 1; i <= n; i++)
        {
            if (i >= m)
            {
                dp[i] += dp[i - m];
            }
            if (i >= 1)
            {
                dp[i] += dp[i - 1];
            }
        }
        
        return dp[n];
    }

    // Функція для перевірки валідності вхідних даних
    public static bool IsValidInput(int m, int n)
    {
        return m >= 2 && m <= 50 && n >= 2 && n <= 50 && m <= n;
    }

    static void Main()
    {
        // Отримуємо шлях до поточної директорії
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        
        // Піднімаємось на 4 рівні вгору до папки ResultExecution
        string resultExecutionPath = Path.GetFullPath(Path.Combine(baseDirectory, "..", "..", "..", "ResultExecution"));
        
        // Формуємо шляхи до файлів
        string inputPath = Path.Combine(resultExecutionPath, "INPUT.TXT");
        string outputPath = Path.Combine(resultExecutionPath, "OUTPUT.TXT");

        try
        {
            string[] input = File.ReadAllText(inputPath).Split(new[] { ' ', '\n', '\r' }, 
                StringSplitOptions.RemoveEmptyEntries);
            
            int m = int.Parse(input[0]);
            int n = int.Parse(input[1]);
            
            if (!IsValidInput(m, n))
            {
                throw new ArgumentException("Invalid input");
            }
            
            long result = CountTilings(m, n);
            File.WriteAllText(outputPath, result.ToString());
        }
        catch (Exception)
        {
            File.WriteAllText(outputPath, "0");
        }
    }
}