using PingerLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Timers;
namespace ConsoleUI
{
    class Program
    {
        

        static void Main(string[] args)
        {
            Console.WriteLine("Enter address to ping: (default is www.google.pl)");
            var address = Console.ReadLine();

            var pinger = new PingerLibrary.PingerLibrary(address);
            pinger.Run();

            Console.ReadKey();
        }

        
    }
}
