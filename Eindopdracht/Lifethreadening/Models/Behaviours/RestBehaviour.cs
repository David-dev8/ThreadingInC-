using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    public class RestBehaviour : Behaviour
    {
        private const int RESILIENCE_PER_ENERGY_GAINED_FOR_RESTING = 30;

        public RestBehaviour(Animal animal) : base(animal)
        {
        }

        public override Incentive Guide()
        {
            int energyGainedForResting = Animal.Statistics.Resilience / RESILIENCE_PER_ENERGY_GAINED_FOR_RESTING;
            return new Incentive(() =>
            {
                Animal.AddEnergy(energyGainedForResting);
            }, GetMotivation());
        }

        private int GetMotivation()
        {
            return (int)(1.0 / 5.0 * (100 - Animal.Energy));
        }
    }
}
