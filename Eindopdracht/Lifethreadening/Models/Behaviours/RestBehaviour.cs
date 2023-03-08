using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    public class RestBehaviour : Behaviour
    {
        public RestBehaviour(Animal animal) : base(animal)
        {
        }

        public override Incentive act()
        {
            throw new NotImplementedException();
        }

        public override Incentive guide()
        {
            throw new NotImplementedException();
        }

        private int GetMotivation()
        {
            return 0;
        }
    }
}
