using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    public abstract class EatBehaviour : Behaviour
    {
        private const double HP_INCREASE_BY_NUTRITION_FACTOR = 1.0 / 3.0;

        public EatBehaviour(Animal animal) : base(animal)
        {
        }

        protected void Consume(SimulationElement element)
        {
            int nutrition = element.DepleteNutritionalValue();
            Animal.AddHp((int)(nutrition * HP_INCREASE_BY_NUTRITION_FACTOR));
            Animal.AddEnergy(nutrition);
        }

        private IDictionary<SimulationElement, double> FindTargets(IDictionary<Location, Path> locations, Func<SimulationElement, bool> filter)
        {
            IDictionary<SimulationElement, double> targets = new Dictionary<SimulationElement, double>();
            foreach(KeyValuePair<Location, Path> location in locations)
            {
                // The closer the target is, the higher the priority
                double distanceFactor = 1 / Math.Sqrt(location.Value.Length);
                foreach(SimulationElement element in location.Key.SimulationElements)
                {
                    if(element != Animal && filter(element))
                    {
                        targets.Add(element, element.GetNutritionalValue() * distanceFactor);
                    }
                }
            }
            return targets;
        }

        private SimulationElement FindMostRelevantTarget(IDictionary<SimulationElement, double> targets)
        {
            SimulationElement mostRelevantTarget = targets.Keys.FirstOrDefault();
            foreach(KeyValuePair<SimulationElement, double> target in targets)
            {
                if(target.Value > targets[mostRelevantTarget])
                {
                    mostRelevantTarget = target.Key;
                }
            }
            return mostRelevantTarget;
        }

        protected Incentive guide(Func<SimulationElement, bool> filter)
        {
            IDictionary<Location, Path> locations = Animal.DetectSurroundings(); // TODO cache?
            SimulationElement mostRelevantTarget = FindMostRelevantTarget(FindTargets(locations, filter));

            if(mostRelevantTarget != null)
            {
                Path pathToFollow = locations[mostRelevantTarget.Location];
                // There is a target
                return new Incentive(() =>
                {
                    // Move towards the element and try to eat it
                    Animal.MoveAlong(pathToFollow);
                    Inflict(mostRelevantTarget);
                }, GetMotivation());
            }

            return null;
        }

        protected int GetHunger()
        {
            return Math.Max(0, (int)((((100 - Animal.Hp) * 1.0 / 3.0) + 100 - Animal.Energy) / (1.0 + 1.0 / 3.0)));
        }

        protected abstract void Inflict(SimulationElement target);
        protected abstract int GetMotivation();
    }
}
