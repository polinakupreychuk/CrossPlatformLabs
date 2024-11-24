using System;
using System.IO;
using System.Collections.Generic;
using System.Numerics;

public class Program
{
    public static void Main()
    {
        // Отримуємо шлях до поточної директорії
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        
        // Піднімаємось на 4 рівні вгору до папки ResultExecution
        string resultExecutionPath = Path.GetFullPath(Path.Combine(baseDirectory, "..", "..", "..", "ResultExecution"));
        
        // Формуємо шляхи до файлів
        string inputPath = Path.Combine(resultExecutionPath, "INPUT.TXT");
        string outputPath = Path.Combine(resultExecutionPath, "OUTPUT.TXT");

        string input = File.ReadAllText(inputPath);
        BigInteger N = BigInteger.Parse(input.Trim());
        int count = CountLuckyNumbers(N);
        File.WriteAllText(outputPath, count.ToString());
    }

    public static int CountLuckyNumbers(BigInteger N)
    {
        // Генеруємо всі "щасливі числа" до \( N \) і повертаємо їх кількість
        var luckyNumbers = GenerateLuckyNumbers(N);
        return luckyNumbers.Count;
    }

    public static List<BigInteger> GenerateLuckyNumbers(BigInteger max)
    {
        var luckyNumbers = new List<BigInteger>();
        GenerateRecursive("", max, luckyNumbers);
        return luckyNumbers;
    }

    private static void GenerateRecursive(string current, BigInteger max, List<BigInteger> luckyNumbers)
    {
        if (current != "")
        {
            BigInteger number = BigInteger.Parse(current);
            if (number > max)
                return;
            luckyNumbers.Add(number);
        }

        // Якщо поточне число ще не перевищує \( max \), додаємо до нього цифри "4" або "7"
        if (current.Length == 0 || BigInteger.Parse(current) <= max)
        {
            GenerateRecursive(current + "4", max, luckyNumbers);
            GenerateRecursive(current + "7", max, luckyNumbers);
        }
    }

    public static bool IsLucky(BigInteger number)
    {
        // Перевіряємо, чи є число "щасливим"
        while (number > 0)
        {
            BigInteger digit = number % 10;
            if (digit != 4 && digit != 7)
            {
                return false;
            }
            number /= 10;
        }
        return true;
    }
}