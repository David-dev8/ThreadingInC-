using Lifethreadening.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    /// <summary>
    /// This class is used to reoresent an empty simulation world
    /// </summary>
    internal class EmptyWorld : World
    {
        /// <summary>
        /// Creates an empty world
        /// </summary>
        /// <param name="ecosystem">The ecosystem of the world</param>
        /// <param name="currentDate">the date that should be used as startpoint when creating the world</param>
        /// <param name="weatherManager">The weathermanager of the world</param>
        public EmptyWorld(Ecosystem ecosystem, DateTime currentDate, IWeatherManager weatherManager) : base(ecosystem, weatherManager)
        {
            CurrentDate = currentDate;
        }


        /// <summary>
        /// This function does nothing 
        /// </summary>
        public override void CreateWorld()
        {
        }

        /// <summary>
        /// This function gets all locations within the world
        /// </summary>
        /// <returns>A collection with all the location in the world</returns>
        public override IEnumerable<Location> GetLocations()
        {
            return new List<Location>();
        }
    }
}
