using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Weather
    {
        public double Temperature { get; set; }
        public double WindSpeed { get; set; }
        public double RainFall { get; set; }

        public Weather(double temperature, double windSpeed, double rainFall)
        {
            Temperature = temperature;
            WindSpeed = windSpeed;
            RainFall = rainFall;
        }
    }
}
