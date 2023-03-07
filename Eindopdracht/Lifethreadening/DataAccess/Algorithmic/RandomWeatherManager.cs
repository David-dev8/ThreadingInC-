using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess.Algorithmic
{
    public class RandomWeatherManager : IWeatherManager
    {
        private const double HUMIDITY_INITIAL = 50;
        private const double WIND_SPEED_INITIAL = 20;
        private const double RAIN_FALL_INITIAL = 0;
        private const double HUMIDITY_DEVIATION_RANGE = 0.5;
        private const double WIND_SPEED_DEVIATION_RANGE = 1;
        private const double RAIN_FALL_DEVIATION_RANGE = 1;

        private Random random = new Random();
        private IList<Weather> _weatherHistory = new List<Weather>()
        {
            new Weather(HUMIDITY_INITIAL, WIND_SPEED_INITIAL, RAIN_FALL_INITIAL)
        };

        public Weather GetCurrent()
        {
            return _weatherHistory[_weatherHistory.Count - 1];
        }

        public void Update()
        {
            Weather newWeather = CreateNewWeather();
            _weatherHistory.Add(newWeather);
        }

        private Weather CreateNewWeather()
        {
            Weather current = GetCurrent();
            return new Weather(
                Deviate(current.Humidity, HUMIDITY_DEVIATION_RANGE),
                Deviate(current.WindSpeed, WIND_SPEED_DEVIATION_RANGE),
                Deviate(current.RainFall, RAIN_FALL_DEVIATION_RANGE)
            );
        }

        private double Deviate(double start, double deviationRange)
        {
            return start + (random.NextDouble() - 0.5) * deviationRange;
        }
    }
}
