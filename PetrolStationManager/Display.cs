using System;
using System.IO;

namespace Assignment_2_PetrolStation
{
	public class Display
	{
		public static void DrawVehicles()
		{
			Vehicle v;

			Console.WriteLine(" Vehicles Queue [ID-Type-Fuel]:");

			for (int i = 0; i < Data.vehicles.Count; i++)
			{
				v = Data.vehicles[i];
				Console.Write(" #{0}-{1}-{2} | ", v.CarID, v.VehType, v.FuelType);
			}
		}

		public static void DrawPumps()
		{
			Pump p;

			Console.WriteLine("\n Pumps Status:");

			for (int i = 0; i < 9; i++)
			{
				p = Data.pumps[i];
				if (i % 3 == 0) { Console.WriteLine(" LANE {0}--------------------------------------------------->", i / 3 + 1); }
				Console.Write("         #{0} ", i + 1);
				if (p.IsAvailable())
				{
					Console.ForegroundColor = ConsoleColor.Green; // green font when pump is free
					Console.Write("FREE");
					Console.ForegroundColor = ConsoleColor.White;
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red; // red when busy
					Console.Write("BUSY");
					Console.ForegroundColor = ConsoleColor.White;
				}
				Console.Write(" | ");

				// modulus -> remainder of a division operation
				// 0 % 3 => 0 (0 / 3 = 0 R=0)
				// 1 % 3 => 1 (1 / 3 = 0 R=1)
				// 2 % 3 => 2 (2 / 3 = 0 R=2)
				// 3 % 3 => 0 (3 / 3 = 1 R=0)
				// 4 % 3 => 1 (4 / 3 = 1 R=1)
				// 5 % 3 => 2 (5 / 3 = 1 R=2)
				// 6 % 3 => 0 (6 / 3 = 2 R=0)
				// ...
				if (i % 3 == 2)
				{
					Console.WriteLine();
					Console.WriteLine();
				}

			}
		}

		public static void DrawCounters()
		{
			Console.WriteLine(" Fuel Dispensed:");
			Console.WriteLine(" {0, -15} {1:0.00} litres		@ £{2:0.00} /litre", "Unleaded:", Counters.FuelDispensed[Data.UNLEADED], Data.fuelPrice[Data.UNLEADED]);
			Console.WriteLine(" {0, -15} {1:0.00} litres		@ £{2:0.00} /litre", "Diesel:", Counters.FuelDispensed[Data.DIESEL], Data.fuelPrice[Data.DIESEL]);
			Console.WriteLine(" {0, -15} {1:0.00} litres		@ £{2:0.00} /litre", "LPG:", Counters.FuelDispensed[Data.LPG], Data.fuelPrice[Data.LPG]);
			Console.WriteLine(" {0, -15} {1:0.00} litres", "Total:", Counters.GetTotalFuelDispensed());
			
			Console.WriteLine("\n {0, -15}£{1:0.00}", "Total Income:", Counters.TotalIncome);
			Console.WriteLine(" {0, -15}£{1:0.00}", "Commission:", Counters.GetCommissionEarned());
			
			Console.WriteLine("\n Vehicles:");
			Console.WriteLine(" {0, -20} {1}", "Serviced:", Counters.GetTotalVehiclesServiced());
			Console.WriteLine(" {0, -20} {1}", "Left without fuel:", Counters.VehiclesThatLeft);

			Console.WriteLine("\n Press the return key to exit at any point");
		}

		/// <summary>
		///  Writes the transaction details to a new line of a text file called 'transactions.txt'
		///  Creates new file if does not already exist
		/// </summary>
		/// <param name="lastTransaction"></param>
		public static void PrintTransaction(string lastTransaction)
		{
			TextWriter tw = new StreamWriter("transactions.txt", true);
			tw.WriteLine(lastTransaction);
			tw.Close();
		}
	}
}
