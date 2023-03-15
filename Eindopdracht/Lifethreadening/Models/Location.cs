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
    public class Location: Observable
    {
        public IList<Location> Neighbours { get; set; } = new List<Location>();
        public IList<SimulationElement> SimulationElements { get; set; } = new List<SimulationElement>();

        public void AddSimulationElement(SimulationElement simulationElement)
        {
            SimulationElements.Add(simulationElement);
            simulationElement.Location = this;
        }

        public void RemoveSimulationElement(SimulationElement simulationElement)
        {
            SimulationElements.Remove(simulationElement);
            simulationElement.Location = null;
        }
    }
}
