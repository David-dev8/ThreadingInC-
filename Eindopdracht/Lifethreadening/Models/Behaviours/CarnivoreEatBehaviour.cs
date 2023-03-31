using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    public class CarnivoreEatBehaviour : EatBehaviour
    {
        private const int MINIMUM_DAMAGE = 1;

        public CarnivoreEatBehaviour(Animal animal) : base(animal)
        {
        }

        public override Incentive Guide()
        {
            return Guide((simulationElement) => simulationElement is Animal);
        }

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

        private int GetDamageToDealTo(Animal otherAnimal)
        {
            return Math.Max(Animal.Statistics.Aggresion - otherAnimal.Statistics.SelfDefence, MINIMUM_DAMAGE);
        }

        protected override int GetMotivation()
        {
            return (int)(Animal.Statistics.Aggresion / 50.0 * GetHunger());
        }
    }
}
