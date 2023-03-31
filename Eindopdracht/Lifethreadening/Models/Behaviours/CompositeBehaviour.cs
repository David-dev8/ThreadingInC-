using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    /// <summary>
    /// This class is used to contain behavior characteristics partaining to the multiple behaviours
    /// </summary>
    public class CompositeBehaviour: Behaviour
    {
        private IList<Behaviour> _behaviours = new List<Behaviour>();

        /// <summary>
        /// Creates a new composit behaviour
        /// </summary>
        /// <param name="animal">The animal to create this behaviour for</param>
        public CompositeBehaviour(Animal animal): base(animal)
        {
        }

        /// <summary>
        /// Adds a new behaviour to the composit behaviour
        /// </summary>
        /// <param name="behaviour"></param>
        public void Add(Behaviour behaviour)
        {
            _behaviours.Add(behaviour);
        }

        /// <summary>
        /// This methord teturns the most desired incentive from the collected behaviours
        /// </summary>
        /// <returns>The most desireable incentive</returns>
        public override Incentive Guide()
        {
            IList<Incentive> incentives = new List<Incentive>();
            foreach(Behaviour behaviour in _behaviours)
            {
                Incentive incentive = behaviour.Guide();
                if(incentive != null)
                {
                    incentives.Add(incentive);
                }
            }
            return GetMostAppealing(incentives);
        }

        /// <summary>
        /// This method calculates wich incentive is the most desireable for an animal
        /// </summary>
        /// <param name="incentives">A list of all posible incentives</param>
        /// <returns>The most desireable incentive</returns>
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
