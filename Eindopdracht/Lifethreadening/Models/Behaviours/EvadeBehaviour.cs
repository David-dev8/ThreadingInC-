using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    public class EvadeBehaviour : Behaviour
    {
        public EvadeBehaviour(Animal animal) : base(animal)
        {
        }

        public override Incentive guide()
        {
            throw new NotImplementedException();
        }
    }
}
