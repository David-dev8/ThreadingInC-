using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    /// <summary>
    /// This class is used to contain behavior characteristics partaining to the behaviour of resting
    /// </summary>
    public class RestBehaviour : Behaviour
    {
        private const int RESILIENCE_PER_ENERGY_GAINED_FOR_RESTING = 30;

        /// <summary>
        /// Creates a new resting behaviour
        /// </summary>
        /// <param name="animal">The animal to create the beahviour for</param>
        public RestBehaviour(Animal animal) : base(animal)
        {
        }

        public override Incentive guide()
        {
            int energyGainedForResting = Animal.Statistics.Resilience / RESILIENCE_PER_ENERGY_GAINED_FOR_RESTING;
            return new Incentive(() =>
            {
                Animal.AddEnergy(energyGainedForResting); // TODO do we even rest?
            }, GetMotivation());
        }

        /// <summary>
        /// This method calculates the motivation the animal has to rest
        /// </summary>
        /// <returns>The motivation the animal has to rest</returns>
        private int GetMotivation()
        {
            return (int)(1.0 / 5.0 * (100 - Animal.Energy));
        }
    }
}
