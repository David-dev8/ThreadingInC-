using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    public class CarnivoreEatBehaviour : EatBehaviour
    {
        private const double HP_INCREASE_BY_NUTRITION_FACTOR = 1 / 3;

        public CarnivoreEatBehaviour(Animal animal) : base(animal)
        {
        }

        // Get the animal that is the most attractive and eat it if it is in range
        public override Incentive guide()
        {
            IDictionary<Animal, double> targets = new Dictionary<Animal, double>();
            IDictionary<Location, Path> locations = DetectSurroundings(); // TODO cache?
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
                    MoveAlong(locations[mostRelevantTarget.Location]);
                    Attack(mostRelevantTarget);
                }, GetMotivation());
            }

            return null;
        }

        private void Attack(Animal animal)
        {
            // Is the animal in range? Decrease its hp
            if(AreLocationsCloseEnough(Animal.Location, animal.Location))
            {
                animal.Hp -= GetDamageToDealTo(animal);

                // Did it die? If so, the acting animal consumes its nutritional value towards our energy and partially towards hp
                if(!animal.StillExistsPhysically())
                {
                    int nutrition = animal.GetNutritionalValue();
                    Animal.Hp += (int)(nutrition * HP_INCREASE_BY_NUTRITION_FACTOR);
                    Animal.Energy += nutrition;
                }
            }
        }

        private int GetDamageToDealTo(Animal animal)
        {
            return 0;
        }

        private int GetMotivation()
        {
            return Animal.Energy;
        }
    }
}
