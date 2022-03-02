using System;
using System.IO;

namespace UpdateWaterFM
{
    class Program
    {
        static string WaterFMDir = @"C:\Ximple\WaterFMEntry";
        static void Main(string[] args)
        {
            checkEnvironment();

            Console.WriteLine("Hello World!");

            bool checkEnvironment()
            {
                if (!Directory.Exists(WaterFMDir))
                    return false;

                Directory.CreateDirectory(Path.Combine(WaterFMDir, "Updates"));

                return true;
            }
        }
    }
}
