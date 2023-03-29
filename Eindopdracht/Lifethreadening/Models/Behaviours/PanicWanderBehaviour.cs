using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    public class PanicWanderBehaviour : WanderBehaviour
    {
        private const double MOTIVATION_FACTOR = 1 / 3;

        public PanicWanderBehaviour(Animal animal) : base(animal)
        {
        }

        protected override int GetMotivation()
        {
            return (int)((Animal.Statistics.MetabolicRate + (100 - Animal.Hp)) / 2 * MOTIVATION_FACTOR);
        }
    }
}
