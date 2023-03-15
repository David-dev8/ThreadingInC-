using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    public class CompositeBehaviour: Behaviour
    {
        private IList<Behaviour> _behaviours = new List<Behaviour>();

        public CompositeBehaviour(Animal animal): base(animal)
        {
        }

        public void Add(Behaviour behaviour)
        {
            _behaviours.Add(behaviour);
        }

        public override Incentive guide()
        {
            IList<Incentive> incentives = new List<Incentive>();
            foreach(Behaviour behaviour in _behaviours)
            {
                Incentive incentive = behaviour.guide();
                if(incentive != null)
                {
                    incentives.Add(incentive);
                }
            }
            return GetMostAppealing(incentives);
        }

        private Incentive GetMostAppealing(IEnumerable<Incentive> incentives)
        {
            if(!incentives.Any())
            {
                return null;
            }
            var maxIncentive = incentives.FirstOrDefault();
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
