using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace ClassLibraryLab5
{
    public class LibLab1
    {
        public static string ExecuteLab5(string input)
        {
            try
            {
                if (!BigInteger.TryParse(input, out BigInteger N) || N <= 0)
                {
                    throw new ArgumentException("Input should be a positive integer.");
                }

                int result = GenerateSequence((int)N);

                return $"The {N}th prime or multiple of 7 is: {result}";
            }
            catch (Exception ex)
            {
                return $"An error occurred: {ex.Message}";
            }
        }

        public static bool IsPrime(int number)
        {
            if (number < 2) return false;
            for (int i = 2; i * i <= number; i++)
            {
                if (number % i == 0) return false;
            }
            return true;
        }

        public static int GenerateSequence(int N)
        {
            int count = 0;
            int current = 7;

            while (true)
            {
                if (IsPrime(current) || current % 7 == 0)
                {
                    count++;
                    if (count == N)
                    {
                        return current;
                    }
                }
                current++;
            }
        }

        public static string ExecuteLab5File(string inputFile, string outputFile)
        {
            try
            {
                string input = ReadInputData(inputFile);

                if (!BigInteger.TryParse(input, out BigInteger N) || N <= 0)
                {
                    throw new ArgumentException("Input should be a positive integer.");
                }

                int result = GenerateSequence((int)N);

                File.WriteAllText(outputFile, result.ToString());

                return $"Result has been successfully saved to {outputFile}";
            }
            catch (Exception ex)
            {
                return $"An error occurred: {ex.Message}";
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
    }
}
