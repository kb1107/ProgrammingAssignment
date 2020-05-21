using System;
using System.Timers;

namespace Assignment_2_PetrolStation
{
    class Program
    {

        static void Main(string[] args)
        {
            Data.Initialise();
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.AutoReset = true;
            timer.Elapsed += RunProgramLoop;
            timer.Enabled = true;
            timer.Start();

            Console.ReadLine();
        }

        static void RunProgramLoop(object sender, ElapsedEventArgs e)
        {
            Console.Clear();           
            Display.DrawVehicles();
            Console.WriteLine();
            Display.DrawPumps();
            Console.WriteLine();
            Data.AssignVehicleToPump();           
            Display.DrawCounters();
        }
    }
}
