using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    /// <summary>
    /// This class is used to contain data partaining to the behaviour of animals
    /// </summary>
    public abstract class Behaviour
    {
        [JsonIgnore]
        public Animal Animal { get; set; }

        /// <summary>
        /// Creates a new behaviour
        /// </summary>
        /// <param name="animal">The animal to create the behaviour for</param>
        public Behaviour(Animal animal) 
        { 
            Animal = animal;
        }

        /// <summary>
        /// The guide method calculates points of interests for the behaviour
        /// </summary>
        /// <returns>A incentive to reach the calculated point of interest and execute an action</returns>
        public abstract Incentive Guide();

        /// <summary>
        /// Calculates wether it is posible to reach a location
        /// </summary>
        /// <param name="location">The locaction to reach</param>
        /// <returns>A boolean value indicating if the given location can be reached</returns>
        protected bool CanReach(Location location)
        {
            return Animal.Location == location || Animal.Location.Neighbours.Contains(location);
        }
    }
}
