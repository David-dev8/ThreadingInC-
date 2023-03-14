using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Weather
    {
        public double Humidity { get; set; }
        public double WindSpeed { get; set; }
        public double RainFall { get; set; }

        public Weather(double humidity, double windSpeed, double rainFall)
        {
            Humidity = humidity;
            WindSpeed = windSpeed;
            RainFall = rainFall;
        }
    }
}
