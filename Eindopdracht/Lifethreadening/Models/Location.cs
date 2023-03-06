﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Location
    {
        public IList<Location> Neighbours { get; set; } = new List<Location>();
        public IList<SimulationElement> SimulationElements { get; set; } = new List<SimulationElement>();

        public IEnumerable<Location> GetAdjacent(int range)
        {
            ISet<Location> adjacentLocations = new HashSet<Location>() { this };
            for(int i = 0; i < range; i++)
            {
                foreach(Location location in adjacentLocations)
                {
                    foreach(Location newAdjacentLocation in location.Neighbours)
                    {
                        adjacentLocations.Add(newAdjacentLocation);
                    }
                }
            }
            return adjacentLocations;
        }
    }
}
