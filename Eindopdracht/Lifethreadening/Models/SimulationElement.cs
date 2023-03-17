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
        protected WorldContextService ContextService { get; set; }

        public SimulationElement(int priority, WorldContextService contextService)
        {
            Priority = priority;
            ContextService = contextService;
        }

        public void Plan()
        {
            PlannedAction = GetNextAction();
        }

        public virtual void Act()
        {
            if(PlannedAction != null)
            {
                PlannedAction();
                PlannedAction = null;
            }
        }

        protected abstract Action GetNextAction();
        public abstract bool StillExistsPhysically();
        public abstract int GetNutritionalValue();
        public abstract int DepleteNutritionalValue();
    }
}
