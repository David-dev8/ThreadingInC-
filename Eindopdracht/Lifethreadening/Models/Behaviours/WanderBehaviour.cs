using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    public class WanderBehaviour : Behaviour
    {
        public WanderBehaviour(Animal animal) : base(animal)
        {
        }

        public override Incentive guide()
        {
            // There is a target
            return new Incentive(() =>
            {
                Animal.MoveAlong(new Path(Animal.Location.Neighbours.First()));
            }, 20);
        }
    }
}
