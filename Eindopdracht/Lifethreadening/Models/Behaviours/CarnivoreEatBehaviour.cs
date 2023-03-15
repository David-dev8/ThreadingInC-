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

        // Get the animal that is the most attractive and eat it if it is in range
        public override Incentive guide()
        {
            IDictionary<Animal, double> targets = new Dictionary<Animal, double>();
            IDictionary<Location, Path> locations = Animal.DetectSurroundings(); // TODO cache?
            foreach(KeyValuePair<Location, Path> location in locations)
            {
                // The closer we are, the higher the priority
                double distanceFactor = 1 / Math.Sqrt(location.Value.Length);
                foreach(SimulationElement element in location.Key.SimulationElements)
                {
                    if(element is Animal animal)
                    {
                        targets.Add(animal, element.GetNutritionalValue() * distanceFactor);
                    }
                }
            }

            Animal mostRelevantTarget = targets.Keys.FirstOrDefault();
            foreach(KeyValuePair<Animal, double> target in targets)
            {
                if(target.Value > targets[mostRelevantTarget])
                {
                    mostRelevantTarget = target.Key;
                }
            }

            if(mostRelevantTarget != null)
            {
                // There is a target
                return new Incentive(() =>
                {
                    // Move towards the animal and try to attack it
                    Animal.MoveAlong(locations[mostRelevantTarget.Location]);
                    Attack(mostRelevantTarget);
                }, GetMotivation());
            }

            return null;
        }

        private void Attack(Animal otherAnimal)
        {
            // Is the animal in range? Decrease its hp
            if(CanReach(otherAnimal.Location))
            {
                otherAnimal.Hp -= GetDamageToDealTo(otherAnimal);
                // Try to consume
                Consume(otherAnimal);
            }
        }

        private int GetDamageToDealTo(Animal otherAnimal)
        {
            return Math.Min(Animal.Statistics.Aggresion - otherAnimal.Statistics.SelfDefence, MINIMUM_DAMAGE);
        }

        private int GetMotivation()
        {
            return (int)(Animal.Statistics.Aggresion + 1 / Math.Sqrt(Animal.Energy)); // TODO
        }
    }
}
