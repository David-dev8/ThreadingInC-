using Lifethreadening.Base;
using Lifethreadening.Models.Behaviours;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Location : Observable
    {
        private IList<SimulationElement> _simulationElements = new List<SimulationElement>();

        public IList<Location> Neighbours { get; set; } = new List<Location>();
        public IEnumerable<SimulationElement> SimulationElements
        {
            get
            {
                return _simulationElements.OrderBy(element => element.Priority);
            }
        }

        public void AddSimulationElement(SimulationElement simulationElement)
        {
            _simulationElements.Add(simulationElement);
            simulationElement.Location = this;
        }

        public void RemoveSimulationElement(SimulationElement simulationElement)
        {
            _simulationElements.Remove(simulationElement);
            simulationElement.Location = null;
        }
    }
}
