using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    /// <summary>
    /// This class is used to contain behavior characteristics partaining to the behaviour of eating other animals
    /// </summary>
    public class CarnivoreEatBehaviour : EatBehaviour
    {
        private const int MINIMUM_DAMAGE = 1;

        /// <summary>
        /// Creates a new Eating behaviour
        /// </summary>
        /// <param name="animal">The animal to create this behaviour for</param>
        public CarnivoreEatBehaviour(Animal animal) : base(animal)
        {
        }

        public override Incentive Guide()
        {
            return Guide((simulationElement) => simulationElement is Animal);
        }

        /// <summary>
        /// This method lets an animal inflict damage on the animal it wants to eat, doing damage to it until it is dead and can be eaten
        /// </summary>
        /// <param name="target">The animal that gets hurt</param>
        protected override void Inflict(SimulationElement target)
        {
            Animal otherAnimal = (Animal)target;
            // Is the animal in range? Decrease its hp
            if(CanReach(otherAnimal.Location))
            {
                otherAnimal.AddHp(-GetDamageToDealTo(otherAnimal));
                // Try to consume
                Consume(otherAnimal);
            }
        }

        /// <summary>
        /// This method calculates how much damage would be done to a given animal
        /// </summary>
        /// <param name="otherAnimal">The animal to calculate the damage for</param>
        /// <returns>The damage that would be dealt against the given animal</returns>
        private int GetDamageToDealTo(Animal otherAnimal)
        {
            return Math.Max(Animal.Statistics.Aggresion - otherAnimal.Statistics.SelfDefence, MINIMUM_DAMAGE);
        }

        /// <summary>
        /// Gets the ammount of motivation this animal has to eat another animal
        /// </summary>
        /// <returns>The motivation level to eat another animal presented as an int</returns>
        protected override int GetMotivation()
        {
            return (int)(Animal.Statistics.Aggresion / 50.0 * GetHunger());
        }
    }
}
