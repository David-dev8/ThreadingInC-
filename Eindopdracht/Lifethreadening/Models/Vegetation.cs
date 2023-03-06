using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Vegetation : SimulationElement
    {
        public Vegetation(Location location) : base(location)
        {
        }

        public override bool live()
        {
            throw new NotImplementedException();
        }
    }
}
