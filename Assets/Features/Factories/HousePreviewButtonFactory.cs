using System;
using System.Collections.Generic;
using StarletBooking.Data;

namespace Extensions.Data.InMemoryData.UI
{
    /// <summary>
    /// Фабрика списка домов
    /// </summary>
    public class HousePreviewButtonFactory : GenericDataPreviewButtonFactory<HousesDataContainer, HouseData>
    {
        /// <summary>
        /// Сортировка домов (по номерам)
        /// </summary>
        protected override void SortData(List<HouseData> data)
        {
            data.Sort(CompareHouses);
        }

        private int CompareHouses(HouseData first, HouseData second)
        {
            if (first == null && second == null) return 0;
            if (first == null) return 1;
            if (second == null) return -1;

            bool firstNumberParsed = int.TryParse(first.Number, out int firstNumber);
            bool secondNumberParsed = int.TryParse(second.Number, out int secondNumber);

            if (firstNumberParsed && secondNumberParsed)
            {
                int numberComparison = firstNumber.CompareTo(secondNumber);
                if (numberComparison != 0) return numberComparison;
            }

            int stringNumberComparison = string.Compare(first.Number, second.Number, StringComparison.CurrentCultureIgnoreCase);
            if (stringNumberComparison != 0) return stringNumberComparison;

            return string.Compare(first.Name, second.Name, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}