using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    public class HerbivoreEatBehaviour : EatBehaviour
    {
        public HerbivoreEatBehaviour(Animal animal) : base(animal)
        {
        }

        public override Incentive guide()
        {
            IDictionary<Vegetation, double> targets = new Dictionary<Vegetation, double>();
            IDictionary<Location, Path> locations = Animal.DetectSurroundings(); // TODO cache?
            foreach(KeyValuePair<Location, Path> location in locations)
            {
                // The closer we are, the higher the priority
                double distanceFactor = 1 / Math.Sqrt(location.Value.Length);
                foreach(SimulationElement element in location.Key.SimulationElements)
                {
                    if(element is Vegetation animal)
                    {
                        targets.Add(animal, element.GetNutritionalValue() * distanceFactor);
                    }
                }
            }

            Vegetation mostRelevantTarget = targets.Keys.FirstOrDefault();
            foreach(KeyValuePair<Vegetation, double> target in targets)
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

        private void Attack(Vegetation vegetation)
        {
            // Is the animal in range? Decrease its hp
            if(CanReach(vegetation.Location))
            {
                // Try to consume
                Consume(vegetation);
            }
        }

        private int GetMotivation()
        {
            return (int)(1 / Math.Sqrt(Animal.Energy));
        }
    }
}
