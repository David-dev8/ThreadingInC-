using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    public abstract class EatBehaviour : Behaviour
    {
        public EatBehaviour(Animal animal) : base(animal)
        {
        }
    }
}
