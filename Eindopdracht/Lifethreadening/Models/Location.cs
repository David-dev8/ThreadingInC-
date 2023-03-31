using Lifethreadening.Base;
using Lifethreadening.Models.Behaviours;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    /// <summary>
    /// This class is used to represent a location in a world
    /// </summary>
    public class Location : Observable
    {
        // There is no native concurrent list in C#
        private object _simulationElementsLocker = new object();
        private IList<SimulationElement> _simulationElements;

        /// <summary>
        /// Creates a new location
        /// </summary>
        /// <param name="simulationElements">A collection of all simulation elements in this lcoation</param>
        public Location(IList<SimulationElement> simulationElements = null)
        {
            _simulationElements = simulationElements ?? new List<SimulationElement>();
        }

        public IList<Location> Neighbours { get; set; } = new List<Location>();
        public IEnumerable<SimulationElement> SimulationElements
        {
            get
            {
                IEnumerable<SimulationElement> elements;
                // Make a snapshot
                lock(_simulationElementsLocker)
                {
                    elements = _simulationElements.OrderBy(element => element.Priority).ToList();
                }
                return elements;
            }
        }

        public bool IsObstructed
        {
            get
            {
                return SimulationElements.Any(s => s is Obstruction);
            }
        }

        /// <summary>
        /// This function adds a simulation element to a location
        /// </summary>
        /// <param name="simulationElement">The element to add</param>
        public void AddSimulationElement(SimulationElement simulationElement)
        {
            lock(_simulationElementsLocker)
            {
                _simulationElements.Add(simulationElement);
            }
        }

        /// <summary>
        /// This function removes a simulation element from a location
        /// </summary>
        /// <param name="simulationElement">The element to remove</param>
        public void RemoveSimulationElement(SimulationElement simulationElement)
        {
            lock(_simulationElementsLocker)
            {
                _simulationElements.Remove(simulationElement);
            }
        }

        /// <summary>
        /// This function removes all dead and non existing simulation elements from a location
        /// </summary>
        public void RemoveNonExistingSimulationElements()
        {
            lock(_simulationElementsLocker)
            {
                for(int i = _simulationElements.Count - 1; i >= 0; i--)
                {
                    SimulationElement simulationElement = _simulationElements[i];
                    if(!_simulationElements[i].StillExistsPhysically())
                    {
                        _simulationElements.Remove(simulationElement);
                    }
                }
            }
        }
    }
}
