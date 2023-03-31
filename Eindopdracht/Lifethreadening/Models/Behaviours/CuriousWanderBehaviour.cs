using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    /// <summary>
    /// This class is used to contain behavior characteristics partaining to the behaviour of wandering aroud
    /// </summary>
    public class CuriousWanderBehaviour : WanderBehaviour
    {
        private const double MOTIVATION_FACTOR = 1 / 4;

        /// <summary>
        /// Creates a new wander behaviour
        /// </summary>
        /// <param name="animal">The anial to create this beheviour for</param>
        public CuriousWanderBehaviour(Animal animal) : base(animal)
        {
        }

        /// <summary>
        /// This method calculates the ammount of motivation the animal has to randomly wander around
        /// </summary>
        /// <returns>The motivation the animal has to wander around</returns>
        protected override int GetMotivation()
        {
            return (int)(MOTIVATION_FACTOR * Animal.Statistics.MetabolicRate);
        }
    }
}
