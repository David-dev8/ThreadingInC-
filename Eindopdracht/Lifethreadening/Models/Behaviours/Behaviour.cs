using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    public abstract class Behaviour
    {
        public Animal Animal { get; set; }

        public Behaviour(Animal animal) 
        { 
            Animal = animal;
        }

        public abstract Incentive act();
        public abstract Incentive guide();
    }
}
