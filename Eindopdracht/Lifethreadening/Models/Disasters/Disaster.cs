using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Disasters
{
    // Most of the time tries to inflict its damage repeatedly
    public abstract class Disaster
    {
        public Disaster()
        {
            // TODO lock de planten
        }

        public abstract void Strike(IEnumerable<SimulationElement> simulationElements);
    }
}
