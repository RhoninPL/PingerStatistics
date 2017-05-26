using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using PingerLibrary;
namespace PingerLibrary
{
    public class PingerLibrary
    {
        private string address;
        //public static List<long> pingTimes = new List<long>();
        public static PingsList<long> pingTimes;
        public static DateTime startTime;
        public static bool sent;
        public static int failed = 0;

        public PingerLibrary(string address = "www.google.pl", TimeSpan? recordTime = null)
        {
            this.address = address != "" ? address : "www.google.pl";

            pingTimes = new PingsList<long>(1000, recordTime ?? TimeSpan.FromMinutes(15));
        }

        public void Run()
        {
            startTime = DateTime.Now;
            Timer timer = new Timer(1000);
            timer.Elapsed += (sender, e) => SendPingAsync(address);
            timer.Start();

            Timer timer2 = new Timer(3000);
            timer2.Elapsed += (sender, e) => StartDisplayInfo();
            timer2.Start();
        }

        public static void StartDisplayInfo()
        {
            if (sent)
                Console.WriteLine($"Avg: {Math.Round(pingTimes.Average(), 2)} ms, Time: {(DateTime.Now - startTime).ToString("mm\\:ss")} minutes. Lost: {failed}. Total: {pingTimes.Count}");
        }

        public static async void SendPingAsync(string address)
        {
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            Ping ping = new Ping();
            ping.PingCompleted += new PingCompletedEventHandler(DisplayReply);
            await ping.SendPingAsync(address, 2500, buffer, new PingOptions(64, true));

        }

        public static void DisplayReply(object sender, PingCompletedEventArgs e)
        {
            PingReply pingReply = e.Reply;
            if (pingReply.Status == IPStatus.Success)
            {
                pingTimes.Add(pingReply.RoundtripTime);
                sent = true;
            }
            else
                failed++;
        }
    }
}
