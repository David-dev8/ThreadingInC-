using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    public class CuriousWanderBehaviour : WanderBehaviour
    {
        private const double MOTIVATION_FACTOR = 1 / 4;

        public CuriousWanderBehaviour(Animal animal) : base(animal)
        {
        }

        protected override int GetMotivation()
        {
            return (int)(MOTIVATION_FACTOR * Animal.Statistics.MetabolicRate);
        }
    }
}
