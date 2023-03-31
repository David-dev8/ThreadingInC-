using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    /// <summary>
    /// This class is used to contain behavior characteristics partaining to the behaviour of eating vegitation
    /// </summary>
    public class HerbivoreEatBehaviour : EatBehaviour
    {
        private const double HUNGER_MOTIVATION_FACTOR = 1 / 2;

        /// <summary>
        /// Creates a new eat behaviour
        /// </summary>
        /// <param name="animal"></param>
        public HerbivoreEatBehaviour(Animal animal) : base(animal)
        {
        }

        public override Incentive Guide()
        {
            return Guide((simulationElement) => simulationElement is Vegetation);
        }

        /// <summary>
        /// This method calculates the effect of concumption on the vegitation and aplies the effect to it
        /// </summary>
        /// <param name="target">The vegitation to consume</param>
        protected override void Inflict(SimulationElement target)
        {
            Vegetation vegetation = (Vegetation)target;
            if(CanReach(vegetation.Location))
            {
                // Try to consume
                Consume(vegetation);
            }
        }

        /// <summary>
        /// This method calculates the motivation for the animal to eat vegitation
        /// </summary>
        /// <returns>The ammount of motivation this animal has to eat vegitation</returns>
        protected override int GetMotivation()
        {
            return (int)(HUNGER_MOTIVATION_FACTOR * GetHunger());
        }
    }
}
