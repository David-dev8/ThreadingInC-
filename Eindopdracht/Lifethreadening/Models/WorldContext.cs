using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    /// <summary>
    /// This class is used to store contextdata for a simulation world
    /// </summary>
    public class WorldContext
    {
        public Ecosystem Ecosystem { get; set; }
        public Weather Weather { get; set; }
        public DateTime Date { get; set; }

        /// <summary>
        /// Create a world context
        /// </summary>
        /// <param name="ecosystem">The exosystem of the world</param>
        /// <param name="weather">The weather of the world</param>
        /// <param name="date">The current date</param>
        public WorldContext(Ecosystem ecosystem, Weather weather, DateTime date)
        {
            Ecosystem = ecosystem;
            Weather = weather;
            Date = date;
        }
    }
}
