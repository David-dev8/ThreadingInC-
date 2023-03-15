using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Obstruction : SimulationElement
    {
        public override bool StillExistsPhysically()
        {
            return true;
        }

        protected override Action GetNextAction(WorldContext context)
        {
            return null;
        }
    }
}
