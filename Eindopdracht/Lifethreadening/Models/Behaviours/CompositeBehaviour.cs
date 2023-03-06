using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    public class CompositeBehaviour: Behaviour
    {
        public IList<Behaviour> Behaviours { get; set; } = new List<Behaviour>();

        public CompositeBehaviour(Animal animal, IList<Behaviour> behaviours): base(animal)
        {
            Behaviours = behaviours;
        }

        public override Incentive guide()
        {
            IList<Incentive> incentives = new List<Incentive>();
            foreach(Behaviour behaviour in Behaviours)
            {
                incentives.Add(behaviour.guide());
            }
            return GetMostAppealing(incentives);
        }

        public override Incentive act()
        {
            IList<Incentive> incentives = new List<Incentive>();
            foreach(Behaviour behaviour in Behaviours)
            {
                incentives.Add(behaviour.act());
            }
            return GetMostAppealing(incentives);
        }

        private Incentive GetMostAppealing(IEnumerable<Incentive> incentives)
        {
            if(!incentives.Any())
            {
                return null;
            }
            var maxIncentive = incentives.First();
            foreach(Incentive incentive in incentives)
            {
                if(incentive.Motivation > maxIncentive.Motivation)
                {
                    maxIncentive = incentive;
                }
            }
            return maxIncentive;
        }
    }
}
