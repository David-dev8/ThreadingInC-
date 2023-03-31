using Lifethreadening.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    public class WanderBehaviour : Behaviour
    {
        private const int MAX_WANDER_DISTANCE = 1;
        private const int MOTIVATION = 10;

        public WanderBehaviour(Animal animal) : base(animal)
        {
        }

        public override Incentive Guide()
        {
            IDictionary<Location, Path> locations = Animal.DetectSurroundings(MAX_WANDER_DISTANCE);
            Path pathToWander = locations.Values.GetRandom();
            return new Incentive(() =>
            {
                Animal.MoveAlong(pathToWander);
            }, GetMotivation());
        }

        protected virtual int GetMotivation()
        {
            return MOTIVATION;
        }
    }
}
