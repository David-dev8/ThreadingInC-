using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public abstract class SimulationElement
    {
        public Location Location { get; set; }

        public SimulationElement(Location location) 
        { 
            this.Location = location;
        }

        public abstract bool live();
    }
}
