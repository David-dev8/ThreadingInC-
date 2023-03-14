using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    public class BreedBehaviour : Behaviour
    {
        public BreedBehaviour(Animal animal, IBreedFactory breedFactory) : base(animal)
        {
        }

        public override Incentive guide()
        {
            throw new NotImplementedException();
        }
    }
}
