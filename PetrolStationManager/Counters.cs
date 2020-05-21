using System.Collections.Generic;
using System.Linq;

namespace Assignment_2_PetrolStation
{
    public static class Counters
    {
        static List<string> transactions;
        public static double[] FuelDispensed { get; private set; } // counter 1- array containing total amount of fuel dispensed for each fuel type
        public static float TotalIncome { get; private set; } //counter 2- amount of money counter 1 equates to (see below for counter 3)
        public static int VehiclesThatLeft { get; private set; } //counter 5- no. of vehicles that left forecourt without fueling

        public static void Initialise(int fuelTypes)
        {
            FuelDispensed = new double[fuelTypes];
            transactions = new List<string>();
        }

        public static void AddTransaction(string transaction, float cost)
        {
            transactions.Add(transaction);
            TotalIncome += cost;
        }

        public static void UpdateFuel(int fuelType, double litres)
        {
            FuelDispensed[fuelType] += litres;
        }

        public static int GetTotalVehiclesServiced()
        {
            return transactions.Count();  //counter 4- the number of vehicles serviced
        }


        /// <summary>
        /// Totals all elements of array containing amount of fuel dispensed.
        /// </summary>
        /// <returns> double value containing total fuel dispensed in litres </returns>
        public static double GetTotalFuelDispensed()
        {
            return FuelDispensed.Sum();
        }

        /// <summary>
        /// Counter- 3
        /// calculates and returns the commission based on total fuel sales
        /// at the rate of 1%
        /// </summary>
        /// <returns> float value containing commission earned in (£)</returns>
        public static float GetCommissionEarned()
        {
            return TotalIncome / 100;
        }

        public static void IncrementVehiclesThatLeft()
        {
            VehiclesThatLeft++;
        }

    }

}
