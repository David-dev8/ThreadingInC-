using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Obstruction : SimulationElement
    {
        public Obstruction(string image)
        { 
        }

        public override int GetNutritionalValue()
        {
            return 0;
        }

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
