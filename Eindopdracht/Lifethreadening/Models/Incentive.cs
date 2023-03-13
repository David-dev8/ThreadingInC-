using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Incentive
    {
        public Action Action { get; set; }

        public int Motivation { get; set; }

        public Incentive(Action action, int motivation = 0)
        {
            Action = action;
            Motivation = motivation;
        }
    }
}
