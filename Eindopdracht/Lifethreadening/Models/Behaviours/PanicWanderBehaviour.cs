using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    /// <summary>
    /// This class is used to contain behavior characteristics partaining to the behaviour of running away in a panic
    /// </summary>
    public class PanicWanderBehaviour : WanderBehaviour
    {
        private const double MOTIVATION_FACTOR = 1 / 3;

        /// <summary>
        /// The animal to create the panic wander behaviour for
        /// </summary>
        /// <param name="animal"></param>
        public PanicWanderBehaviour(Animal animal) : base(animal)
        {
        }

        /// <summary>
        /// This method calculates the ammount of motivation this animal has to wander in panic
        /// </summary>
        /// <returns>The ammount of motivation the animal has to panic</returns>
        protected override int GetMotivation()
        {
            return (int)((Animal.Statistics.MetabolicRate + (100 - Animal.Hp)) / 2 * MOTIVATION_FACTOR);
        }
    }
}
