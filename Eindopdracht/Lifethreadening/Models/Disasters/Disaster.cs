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
        public string Description { get; private set; }
        public DateTime DateInitiated { get; private set; }
        protected WorldContextService ContextService { get; set; }
        public string LongDescription 
        { 
            get
            {
                return GetLongDescription();
            }
        }

        public Disaster(string description, WorldContextService contextService)
        {
            Description = description;
            ContextService = contextService;
            DateInitiated = contextService.GetContext().Date;
        }

        public abstract void Strike(IEnumerable<SimulationElement> simulationElements);

        public abstract string GetLongDescription();
    }
}
