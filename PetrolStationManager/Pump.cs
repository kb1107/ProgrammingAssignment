using System.Timers;

namespace Assignment_2_PetrolStation
{
    class Pump
    {
        Vehicle currentVehicle = null;
        public int PumpNumber { get; private set; }

        public bool IsAvailable()
        {
            // returns TRUE if currentVehicle is NULL, meaning available
            // returns FALSE if currentVehicle is NOT NULL, meaning busy
            return currentVehicle == null;
        }

        public void AssignVehicle(Vehicle v, int pumpNum)
        {
            currentVehicle = v;
            PumpNumber = pumpNum;

            Timer timer = new Timer();
            timer.Interval = v.FuelTime;
            timer.AutoReset = false; // does not repeat
            timer.Elapsed += ReleaseVehicle;
            timer.Enabled = true;
            timer.Start();
        }

        public void ReleaseVehicle(object sender, ElapsedEventArgs e)
        {
            Data.RecordTransaction(currentVehicle, PumpNumber);
            currentVehicle = null;
        }

    }
}
