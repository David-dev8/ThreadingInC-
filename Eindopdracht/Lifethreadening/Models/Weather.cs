using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    /// <summary>
    /// This class is used to store data about a weather state
    /// </summary>
    public class Weather
    {
        public double Temperature { get; set; }
        public double WindSpeed { get; set; }
        public double RainFall { get; set; }

        /// <summary>
        /// Creates a new weather object
        /// </summary>
        /// <param name="temperature">The temprature</param>
        /// <param name="windSpeed">The windspeed</param>
        /// <param name="rainFall">The rainfall</param>
        public Weather(double temperature, double windSpeed, double rainFall)
        {
            Temperature = temperature;
            WindSpeed = windSpeed;
            RainFall = rainFall;
        }
    }
}
