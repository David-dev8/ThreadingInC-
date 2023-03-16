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
        public int Priority { get; private set; }
        protected Action PlannedAction { get; set; }

        public SimulationElement(int priority)
        {
            Priority = priority;
        }

        public void Plan(WorldContext context)
        {
            PlannedAction = GetNextAction(context);
        }

        public void Act()
        {
            if(PlannedAction != null)
            {
                PlannedAction();
                PlannedAction = null;
            }
        }

        protected abstract Action GetNextAction(WorldContext context);
        public abstract bool StillExistsPhysically();
        public abstract int GetNutritionalValue();
        public abstract int DepleteNutritionalValue();
    }
}
