using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    public class RestBehaviour : Behaviour
    {
        private const int ENERGY_GAINED_FOR_RESTING = 2;

        public RestBehaviour(Animal animal) : base(animal)
        {
        }

        public override Incentive guide()
        {
            return new Incentive(() =>
            {
                Animal.AddEnergy(ENERGY_GAINED_FOR_RESTING); // TODO do we even rest?
            }, GetMotivation());
        }

        private int GetMotivation()
        {
            return (int)(1 / 3 * (100 - Animal.Energy));
        }
    }
}
