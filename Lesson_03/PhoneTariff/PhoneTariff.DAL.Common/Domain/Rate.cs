using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneTariff.DAL.Common.Domain
{
    [Serializable]
    public class Rate
    {

        public Rate()
        {
        }

        public Rate(Tariff tariff, Zone zone, double peakRate, double offPeakRate)
        {
            this.Tariff = tariff;
            this.Zone = zone;
            this.PeakRate = peakRate;
            this.OffPeakRate = offPeakRate;
        }


        public Tariff Tariff { get; set; }
        public Zone Zone { get; set; }
        public double PeakRate { get; set; }
        public double OffPeakRate { get; set; }
    }


    public struct ZoneConsumption : IComparable
    {
        public string ZoneId { get; set; }
        public double PeakDuration { get; set; }
        public double OffPeakDuration { get; set; }

        public int CompareTo(object obj)
        {
            return this.ZoneId.CompareTo(obj);
        }
    }
}
