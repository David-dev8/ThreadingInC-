using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    /// <summary>
    /// This class is used to store data about paths between two points
    /// </summary>
    public class Path
    {
        public Path PreviousPath { get; set; }
        public Location CurrentLocation { get; set; }
        public int Length
        {
            get
            {
                Path path = this;
                int i = 0;
                while (path.PreviousPath != null)
                {
                    path = path.PreviousPath;
                    i++;
                }
                return i;
            }
        }

        /// <summary>
        /// Creates a new path
        /// </summary>
        /// <param name="currentLocation">The start location</param>
        /// <param name="previousPath">The end location</param>
        public Path(Location currentLocation, Path previousPath = null)
        {
            CurrentLocation = currentLocation;
            PreviousPath = previousPath;
        }

        /// <summary>
        /// This function gets a location on a speciefied amount of steps in path
        /// </summary>
        /// <param name="index">The amount of steps into the path to check</param>
        /// <returns>The location at the sepcified amount of steps</returns>
        public Location GetLocationAt(int index)
        {
            if(index < 0 || index > Length)
            {
                return null;
            }

            Path path = this;
            for(int i = 0; i < Length - index; i++)
            {
                path = path.PreviousPath;
            }
            return path.CurrentLocation;
        }
    }
}
