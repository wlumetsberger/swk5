using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timer
{
    class Program
    {
        static void Main(string[] args)
        {
            Timer timer = new Timer();
            timer.Interval = 500;

            // timer.Expired += new ExpiredEventHandler(OnTimerExpired)
            //timer.Expired += OnTimerExpired;

            timer.Expired += signalTime => { Console.WriteLine(signalTime); };
            timer.Start();
        }

        private static void OnTimerExpired(DateTime signaledTime)
        {
            Console.WriteLine(signaledTime);
        }
    }
}
