using System;
using System.Linq;

namespace ClassLibraryLab5
{
    public class LibLab2
    {
        public static string ExecuteLab2(string inputData)
        {
            try
            {
                // Розраховуємо вартість
                int totalCost = CalculateCostFromInput(inputData);

                // Повертаємо результат як строку
                return $"Total cost is: {totalCost}";
            }
            catch (Exception ex)
            {
                return $"An error occurred: {ex.Message}";
            }
        }

        private static int CalculateCostFromInput(string inputData)
        {
            // Припускаємо, що inputData - це один рядок, який містить всі необхідні дані.
            // Приклад: "10 3 5 100 4 200 8 150" для n=10, m=3 і постачальників (5, 100), (4, 200), (8, 150)

            string[] data = inputData.Split();

            // Перші два значення - це n і m
            int n = int.Parse(data[0]);
            int m = int.Parse(data[1]);

            // Далі йдуть пари постачальників
            (int Pairs, int Price)[] suppliers = new (int, int)[m];
            for (int i = 0; i < m; i++)
            {
                int ai = int.Parse(data[2 + i * 2]);
                int bi = int.Parse(data[3 + i * 2]);
                suppliers[i] = (ai, bi);
            }

            // Сортуємо постачальників по найменшій вартості за пару
            var sortedSuppliers = suppliers
                .Select(s => new { s.Pairs, s.Price, PricePerPair = (double)s.Price / s.Pairs })
                .OrderBy(s => s.PricePerPair)
                .ToArray();

            int totalPairs = 0;
            int totalCost = 0;

            // Підраховуємо загальну вартість
            foreach (var supplier in sortedSuppliers)
            {
                if (totalPairs >= n)
                    break;

                int neededPairs = n - totalPairs;
                int packsToBuy = Math.Min(neededPairs / supplier.Pairs, 1);

                totalPairs += packsToBuy * supplier.Pairs;
                totalCost += packsToBuy * supplier.Price;
            }

            return totalCost;
        }
    }
}
