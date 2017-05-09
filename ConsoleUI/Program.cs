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
        public static List<long> pingTimes = new List<long>();
        public static DateTime startTime;
        public static bool sent;

        static void Main(string[] args)
        {
            startTime = DateTime.Now;
            Timer timer = new Timer(5000);
            timer.Elapsed += async (sender, e) => SendPingAsync();
            timer.Start();

            Timer timer2 = new Timer(3000);
            timer2.Elapsed += async (sender, e) => StartDisplayInfo();
            timer2.Start();

            Console.ReadKey();
        }

        public static void StartDisplayInfo()
        {
            if (sent)
                Console.WriteLine($"Avg: {Math.Round(pingTimes.Average(), 2)}, Time: {(DateTime.Now - startTime).ToString("mm\\:ss")}");
        }

        public static async void SendPingAsync()
        {
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            String url = "www.google.pl";
            Ping ping = new Ping();
            ping.PingCompleted += new PingCompletedEventHandler(DisplayReply);
            await ping.SendPingAsync(url, 2500, buffer, new PingOptions(64, true));

        }

        public static void DisplayReply(object sender, PingCompletedEventArgs e)
        {
            PingReply pingReply = e.Reply;
            if (pingReply.Status == IPStatus.Success)
            {
                pingTimes.Add(pingReply.RoundtripTime);
                Console.WriteLine($"IP: {pingReply.Address}, Status: {pingReply.Status}, Time: {pingReply.RoundtripTime}");
                sent = true;
            }
        }
    }
}
