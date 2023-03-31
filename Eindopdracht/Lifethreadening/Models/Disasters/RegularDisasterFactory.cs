using Lifethreadening.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Disasters
{
    /// <summary>
    /// This class is used to create regular disasters
    /// </summary>
    public class RegularDisasterFactory : IDisasterFactory
    {
        private const double FIRE_DISASTER_CHANCE = 0.25;
        private const double FLOODING_DISASTER_CHANCE = 0.35;
        private const double EARTHQUAKE_DISASTER_CHANCE = 0.55;

        private Random _random = new Random();

        /// <summary>
        /// This method tandomly creates a new disaster
        /// </summary>
        /// <param name="worldContextService">Rhe context service</param>
        /// <returns>A random diaster for the world</returns>
        public Disaster CreateDisaster(WorldContextService worldContextService)
        {
            double randomNumber = _random.NextDouble();
            double total = 0;
            if(randomNumber < (total += FIRE_DISASTER_CHANCE))
            {
                return new LightningDisaster(worldContextService);
            }
            else if(randomNumber < (total += FLOODING_DISASTER_CHANCE))
            {
                return new FloodingDisaster(worldContextService);
            }
            else if(randomNumber < (total += EARTHQUAKE_DISASTER_CHANCE))
            {
                return new EarthquakeDisaster(worldContextService);
            }
            return null; // TODO throw exception? ook bij animal?
        }
    }
}
