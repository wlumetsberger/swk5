using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Timer
{
    public delegate void ExpireEventHandler(DateTime signaledTime);
    public class Timer
    {
        private Thread ticker;
        private const int DEFAULT_INTERVAL = 1000;
        public int Interval { get; set; }
        public event ExpireEventHandler Expired;

        public Timer()
        {
            Interval = DEFAULT_INTERVAL;
            ticker = new Thread(OnTick);
        }

        public void Start()
        {
            ticker.Start();
        }

        private void OnTick()
        {
            while (true)
            {
                Thread.Sleep(Interval);
                /*if(Expired != null)
                {
                    Expired(DateTime.Now);
                }*/
                Expired?.Invoke(DateTime.Now);
            }

        }

        


    }
}
