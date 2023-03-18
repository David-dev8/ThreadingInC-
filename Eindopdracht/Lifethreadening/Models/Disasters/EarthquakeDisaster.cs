using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Disasters
{
    public class EarthquakeDisaster : Disaster
    {
        private Random _random = new Random();

        public override void Strike(IEnumerable<SimulationElement> simulationElements)
        {
            // Repeated shockwaves
            for(int i = 1; i < 10000; i++)
            {
                double maxIntensity = (1 / (20 * Math.Sqrt(Math.Log10(i * Math.Sqrt(i + 1)))));
                foreach(SimulationElement simulationElement in simulationElements)
                {
                    if(_random.NextDouble() < maxIntensity)
                    {
                        Damage(simulationElement);
                    }
                }
            }
        }

        private void Damage(SimulationElement simulationElement)
        {
            if(simulationElement is Animal animal)
            {
                animal.AddHp(-1);
            }
        }
    }
}
