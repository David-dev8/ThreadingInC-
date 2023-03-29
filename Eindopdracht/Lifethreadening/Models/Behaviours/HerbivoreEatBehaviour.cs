using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    public class HerbivoreEatBehaviour : EatBehaviour
    {
        private const double HUNGER_MOTIVATION_FACTOR = 1 / 2;

        public HerbivoreEatBehaviour(Animal animal) : base(animal)
        {
        }

        public override Incentive guide()
        {
            return guide((simulationElement) => simulationElement is Vegetation);
        }

        protected override void Inflict(SimulationElement target)
        {
            Vegetation vegetation = (Vegetation)target;
            if(CanReach(vegetation.Location))
            {
                // Try to consume
                Consume(vegetation);
            }
        }

        protected override int GetMotivation()
        {
            return (int)(HUNGER_MOTIVATION_FACTOR * GetHunger());
        }
    }
}
