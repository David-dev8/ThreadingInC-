using Lifethreadening.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    /// <summary>
    /// This class is used to contain behavior characteristics partaining to the behaviour of wandering randomly
    /// </summary>
    public class WanderBehaviour : Behaviour
    {
        private const int MAX_WANDER_DISTANCE = 1;
        private const int MOTIVATION = 10;

        /// <summary>
        /// Creates a new wandering behaviour
        /// </summary>
        /// <param name="animal">The animal to create this behaviour for</param>
        public WanderBehaviour(Animal animal) : base(animal)
        {
        }

        public override Incentive guide()
        {
            IDictionary<Location, Path> locations = Animal.DetectSurroundings(MAX_WANDER_DISTANCE);
            Path pathToWander = locations.Values.GetRandom();
            return new Incentive(() =>
            {
                Animal.MoveAlong(pathToWander);
            }, GetMotivation());
        }

        /// <summary>
        /// This method returns the ammount of motivation the animal has to wander
        /// </summary>
        /// <returns>The ammount of motivation the animal has to wander</returns>
        protected virtual int GetMotivation()
        {
            return MOTIVATION;
        }
    }
}
