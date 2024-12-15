using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace ClassLibraryLab4
{
    public class Lab1
    {
        public static void ExecuteLab4(string inputFile, string outputFile)
        {
            try
            {
                string input = ReadInputData(inputFile);

                if (!BigInteger.TryParse(input, out BigInteger N) || N <= 0)
                {
                    throw new ArgumentException("Input should be a positive integer.");
                }

                int count = CountLuckyNumbers(N);

                File.WriteAllText(outputFile, count.ToString());

                Console.WriteLine($"Result has been successfully saved to {outputFile}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static string ReadInputData(string inputFile)
        {
            if (!File.Exists(inputFile))
            {
                throw new FileNotFoundException($"Input file not found: {inputFile}");
            }

            string input = File.ReadAllText(inputFile).Trim();
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException("Input string cannot be empty or null.");
            }

            return input;
        }

        public static int CountLuckyNumbers(BigInteger N)
        {
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

            if (current.Length == 0 || BigInteger.Parse(current) <= max)
            {
                GenerateRecursive(current + "4", max, luckyNumbers);
                GenerateRecursive(current + "7", max, luckyNumbers);
            }
        }

        public static bool IsLucky(BigInteger number)
        {
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
}
