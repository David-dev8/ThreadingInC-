using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    public abstract class EatBehaviour : Behaviour
    {
        private const double HP_INCREASE_BY_NUTRITION_FACTOR = 1 / 3;

        public EatBehaviour(Animal animal) : base(animal)
        {
        }

        protected void Consume(SimulationElement element)
        {
            int nutrition = element.DepleteNutritionalValue();
            Animal.Hp += (int)(nutrition * HP_INCREASE_BY_NUTRITION_FACTOR);
            Animal.Energy += nutrition;
        }
    }
}
