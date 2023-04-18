using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitachiTask
{
    internal class WeatherParameterData
    {
        public double AverageValue { get; set; }
        public double MaxValue { get; set; }
        public double MinValue { get; set; }
        public List<double> MedianValues { get; set; }
        public double MedianValue { get; set; }
        public double MostAppropriateValue { get; set; }

        public WeatherParameterData()
        {
            MedianValues = new List<double>();
        }
    }
}
