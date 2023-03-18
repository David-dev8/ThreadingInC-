using Lifethreadening.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Disasters
{
    public class FireDisaster : Disaster
    {
        private Random _random = new Random();

        private const int MIN_DAMAGE = 1;
        private const int MAX_DAMAGE = 5;
        private const int MIN_SPARKS = 1;
        private const int MAX_SPARKS = 5000;

        public override void Strike(IEnumerable<SimulationElement> simulationElements)
        {
            // Fire sparks and hits
            IEnumerable<int> sparks = Enumerable.Range(0, _random.Next(100))
                        .Select(r => _random.Next(10))
                        .ToList();
            foreach(int spark in sparks)
            {
                Damage(simulationElements.GetRandom(), spark);
            }
        }

        private void Damage(SimulationElement simulationElement, int damage)
        {
            if(simulationElement is Animal animal)
            {
                animal.AddHp(-damage);
            }
            else if(simulationElement is Vegetation vegetation)
            {
                vegetation.AddNutrition(-damage);
            }
        }
    }
}
