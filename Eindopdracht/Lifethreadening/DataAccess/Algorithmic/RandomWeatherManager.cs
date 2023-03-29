using Lifethreadening.ExtensionMethods;
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
        private const double TEMPERATURE_INITIAL = 15;
        private const double WIND_SPEED_INITIAL = 40;
        private const double RAIN_FALL_INITIAL = 0;
        private const double TEMPERATURE_DEVIATION_RANGE = 1.25;
        private const double WIND_SPEED_DEVIATION_RANGE = 2.5;
        private const double RAIN_FALL_DEVIATION_RANGE = 0.5;

        private IList<Weather> _weatherHistory = new List<Weather>()
        {
            new Weather(TEMPERATURE_INITIAL, WIND_SPEED_INITIAL, RAIN_FALL_INITIAL)
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
                current.Temperature.Deviate(TEMPERATURE_DEVIATION_RANGE),
                current.WindSpeed.Deviate(WIND_SPEED_DEVIATION_RANGE),
                Math.Max(0, current.RainFall.Deviate(RAIN_FALL_DEVIATION_RANGE))
            );
        }
    }
}
