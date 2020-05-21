using System.Timers;

namespace Assignment_2_PetrolStation
{

    class Vehicle
    {
        static int nextCarID = 1;
        public int CarID { get; private set; }
        public string FuelType { get; private set; }
        public double FuelTime { get; private set; }
        public float CostOfFueling { get; private set; }
        public string VehType { get; private set; }
        public int WaitingTime { get; private set; } //holds random number between 1000-2000ms. Simulates amount of time vehicle will wait to be serviced before driving off.

        private Timer timer = new Timer(); //times how long vehicle has been waiting

        public Vehicle(string type, string fuel, double time, int waitTime)
        {
            CarID = nextCarID++;
            VehType = type;
            WaitingTime = waitTime;
            FuelType = fuel;
            FuelTime = time;

            timer.Interval = (double)WaitingTime; //time willing to wait to be serviced
            timer.AutoReset = false;
            timer.Elapsed += DriveOff; //simulates drivers leaving forecourt if not serviced in time
            timer.Enabled = true;
            timer.Start();
        }
        /// <summary>
        /// This is called whenever a vehicle is assigned to a pump.
        /// The timer is stopped to prevent DriveOff being called and incrementing
        /// the counter wrongly.
        /// </summary>
        public void GoToPump()
        {
            timer.Stop();
        }


        /// <summary>
        /// called when the waiting timer has elapsed.
        /// Removes vehicle object from list to simulate driving off without fueling.
        /// </summary>
        /// <param name = "sender" ></ param >
        /// < param name="e"></param>
        private void DriveOff(object sender, ElapsedEventArgs e)
        {
            Data.vehicles.Remove(this);
            Counters.IncrementVehiclesThatLeft();
        }

    }
}
