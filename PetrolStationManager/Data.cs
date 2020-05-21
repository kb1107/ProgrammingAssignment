using System.Collections.Generic;
using System.Timers;
using System;

namespace Assignment_2_PetrolStation
{
    class Data
    {
        private static Timer timer;
        public static List<Vehicle> vehicles;
        public static List<Pump> pumps;

        static string[] vehicleType = { "HGV", "Van", "Car" }; //all available types of vehicle
        static int[] vehicleTankCapacity = { 150000, 80000, 40000 }; //amounts (ml) in corresponding positions in array
                                                                     //position of each vehicle type in array
                                                                     //HGV = 0
                                                                     //Van = 1
                                                                     //Car = 2

        static string[] fuelType = { "Diesel", "LPG", "Unleaded" }; //order of fuel types is relative to vehicle type. A vehicle may be created with any fuel type equal to or less than their array index.
        public readonly static float[] fuelPrice = { 1.50f, 1.00f, 1.10f };

        public const int DIESEL = 0; //refers to position in array
        public const int LPG = 1;
        public const int UNLEADED = 2;

        const float PUMP_SPEED = 3.5f; //litres per second the pump is capable of dispensing
        private static Random rand = new Random(); //needed to randomise vehicle creation and fuel time

        public static void Initialise()
        {
            InitialisePumps();
            InitialiseVehicles();
            Counters.Initialise(fuelType.Length);
        }

        private static void InitialiseVehicles()
        {
            vehicles = new List<Vehicle>();

            timer = new Timer();
            timer.Interval = rand.Next(1500, 2200); //randomises first vehicle creation between 1.5 - 2.2 seconds
            timer.AutoReset = true; // keep repeating
            timer.Elapsed += CreateVehicle;
            timer.Enabled = true;
            timer.Start();
        }

        private static void CreateVehicle(object sender, ElapsedEventArgs e)
        {
            timer.Interval = rand.Next(1500, 2200); //update interval between vehicle creations
            if (vehicles.Count > 4) { return; }  // check amount of vehicles in queue. Max length 5
            int waitTime = rand.Next(5000, 6000); // random time between 5-6 seconds for how long vehicle will wait before being serviced
            int vehicleIndex = rand.Next(vehicleType.Length);
            string vtype = vehicleType[vehicleIndex];
            int fuelIndex = rand.Next(vehicleIndex + 1); // order of vehicles in the array and the corresponding fuel types accessible to them
            string fuel = fuelType[fuelIndex];
            // vehicle tank capacity (ml) / 4 = quarter tank (upper limit of fuel in tank)
            // divided by litres/ sec pump dispenses fuel
            // = vehicle fuel time in ms.
            double fuelTime = (vehicleTankCapacity[vehicleIndex] - rand.Next(vehicleTankCapacity[vehicleIndex] / 4)) / PUMP_SPEED;
            Vehicle v = new Vehicle(vtype, fuel, fuelTime, waitTime);
            vehicles.Add(v);
        }

        private static void InitialisePumps()
        {
            pumps = new List<Pump>();

            Pump p;

            for (int i = 0; i < 9; i++)
            {
                p = new Pump();
                pumps.Add(p);
            }
        }

        /// <summary>
        /// Checks for available pumps. Removes first  vehicle from queue and assigns it to pump.
        /// Contains the logic to 'block' a lane, meaning a busy pump will prevent any further pumps in that lane from being accessed.
        /// Assign vehicle to last possible pump in the lane.
        /// </summary>
        public static void AssignVehicleToPump()
        {
            Vehicle v;
            Pump p;
            int pumpNum;


            if (vehicles.Count == 0) { return; }

            for (int i = 0; i < 9; i += 3) //checks if first pump in each lane is free
            {
                p = pumps[i];
                pumpNum = i + 1;

                if (vehicles.Count == 0) { break; }

                if (p.IsAvailable())
                {
                    v = vehicles[0]; // get first vehicle
                    vehicles.RemoveAt(0); // remove vehicles from queue
                    v.GoToPump(); // stops waiting time timer

                    if (pumps[i + 1].IsAvailable()) //if previous pump is free, checks if next pump along is available (so the last pump in line is used when free)
                    {
                        if (pumps[i + 2].IsAvailable()) //if first two pumps are free, checks if last in row is available
                        {
                            p = pumps[i + 2];
                            pumpNum = i + 3;
                            p.AssignVehicle(v, pumpNum); // assign it to the pump
                            break;
                        }
                        else
                        {
                            p = pumps[i + 1];
                            pumpNum = i + 2;
                            p.AssignVehicle(v, pumpNum);
                            break;
                        }
                    }
                    else
                    {
                        p.AssignVehicle(v, pumpNum);
                        break;
                    }

                }

                if (vehicles.Count == 0) { break; }
            }
        }
        /// <summary>
        /// Collates data from the fuel pump transactions which is then saved in a list and written to a text file.
        /// Updates counters for fuel, vehicle type etc.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="pumpNum"></param>
        public static void RecordTransaction(Vehicle v, int pumpNum)
        {
            double litres = (v.FuelTime / 1000) * PUMP_SPEED;
            int fuel = Array.IndexOf(fuelType, v.FuelType);
            float cost = (float)litres * fuelPrice[fuel];
            string transactionDetails = $"{DateTime.Now} \nVehicle ID: {v.CarID}    Vehicle Type: {v.VehType}    Fuel: {v.FuelType}    Litres: {((v.FuelTime / 1000) * PUMP_SPEED):0.00}    Pump: {pumpNum}    Cost: £{cost:0.00}\n";
            Counters.UpdateFuel(fuel, litres);
            Counters.AddTransaction(transactionDetails, cost);
            Display.PrintTransaction(transactionDetails);
        }

    }
}