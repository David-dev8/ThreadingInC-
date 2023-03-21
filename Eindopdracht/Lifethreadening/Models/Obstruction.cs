using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Obstruction : SimulationElement
    {
        private const int DEFAULT_PRIORITY = 3;

        public Obstruction(string image, WorldContextService contextService): base(DEFAULT_PRIORITY, image, contextService)
        { 
        }

        public override int GetNutritionalValue()
        {
            return 0;
        }

        public override int DepleteNutritionalValue()
        {
            return GetNutritionalValue();
        }

        public override bool StillExistsPhysically()
        {
            return true;
        }

        protected override Action GetNextAction()
        {
            return null;
        }
    }
}
