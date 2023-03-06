using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Incentive
    {
        private readonly Action _action;

        public int Motivation { get; set; }

        public Incentive(Action action)
        {
            _action = action;
        }

        public void execute()
        {
            _action();
        }
    }
}
