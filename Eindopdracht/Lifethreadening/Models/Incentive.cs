using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    /// <summary>
    /// This class is used to represent an incentice for an inimal
    /// </summary>
    public class Incentive
    {
        public Action Action { get; set; }

        public int Motivation { get; set; }

        /// <summary>
        /// Creates a new incentive
        /// </summary>
        /// <param name="action">The action the incentive is for</param>
        /// <param name="motivation">The motivation the animal has to complete this action</param>
        public Incentive(Action action, int motivation = 0)
        {
            Action = action;
            Motivation = motivation;
        }
    }
}
