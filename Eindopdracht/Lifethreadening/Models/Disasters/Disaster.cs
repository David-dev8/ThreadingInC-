using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Disasters
{
    /// <summary>
    /// This class is used to contain data about disasters
    /// </summary>
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

        /// <summary>
        /// Creates a new disaster
        /// </summary>
        /// <param name="description">The discription of the disaster</param>
        /// <param name="contextService">The contextservice</param>
        public Disaster(string description, WorldContextService contextService)
        {
            Description = description;
            ContextService = contextService;
            DateInitiated = contextService.GetContext().Date;
        }

        /// <summary>
        /// This method lets a disaster affect the game, inflicting certain types of changes on certeain types of elements
        /// </summary>
        /// <param name="simulationElements">All the simulation elements that are affected by this disaster</param>
        public abstract void Strike(IEnumerable<SimulationElement> simulationElements);


        /// <summary>
        /// This mehod retrieves the entire disaster description
        /// </summary>
        /// <returns>The entire disaster description</returns>
        public abstract string GetLongDescription();
    }
}
