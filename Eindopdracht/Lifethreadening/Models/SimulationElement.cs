using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    /// <summary>
    /// This class stores data about simulation elements
    /// </summary>
    public abstract class SimulationElement
    {
        public int Priority { get; private set; }
        public string Image { get; set; }
        [JsonIgnore]
        public Location Location { get; set; }
        protected Action PlannedAction { get; set; }
        public WorldContextService ContextService { get; set; }

        /// <summary>
        /// Creates a new simulation element
        /// </summary>
        /// <param name="priority">The priorit of the element</param>
        /// <param name="image">The image that represents the simulation element</param>
        /// <param name="contextService">The context service</param>
        public SimulationElement(int priority, string image, WorldContextService contextService)
        {
            Priority = priority;
            Image = image;
            ContextService = contextService;
        }

        /// <summary>
        /// This function plans an action to be executed
        /// </summary>
        public void Plan()
        {
            PlannedAction = GetNextAction();
        }

        /// <summary>
        /// This function executes the planned action
        /// </summary>
        public virtual void Act()
        {
            if(PlannedAction != null)
            {
                PlannedAction();
                PlannedAction = null;
            }
        }

        /// <summary>
        /// This function gets the next action for the simulation element
        /// </summary>
        /// <returns>The next action for the simualtion element</returns>
        protected abstract Action GetNextAction();

        /// <summary>
        /// This function checks if an elemenbt is still existing in the world
        /// </summary>
        /// <returns>A boolean value indicating wether the element still exists</returns>
        public abstract bool StillExistsPhysically();

        /// <summary>
        /// This function returns the nutritional value of the element
        /// </summary>
        /// <returns>The nutritional value of the element</returns>
        public abstract int GetNutritionalValue();

        /// <summary>
        /// This function removes nutritional value from an element
        /// </summary>
        /// <returns>The new nutritional value of the element</returns>
        public abstract int DepleteNutritionalValue();
    }
}
