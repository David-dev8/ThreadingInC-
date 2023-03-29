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
    public class Location : Observable
    {
        // There is no native concurrent list in C#
        private object _simulationElementsLocker = new object();
        private IList<SimulationElement> _simulationElements;

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

        public void AddSimulationElement(SimulationElement simulationElement)
        {
            lock(_simulationElementsLocker)
            {
                _simulationElements.Add(simulationElement);
            }
        }

        public void RemoveSimulationElement(SimulationElement simulationElement)
        {
            lock(_simulationElementsLocker)
            {
                _simulationElements.Remove(simulationElement);
            }
        }

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
