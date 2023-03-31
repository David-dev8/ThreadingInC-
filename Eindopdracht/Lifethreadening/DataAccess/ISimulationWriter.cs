using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess
{
    /// <summary>
    /// This class writes data about the simulation to storage
    /// </summary>
    public interface ISimulationWriter
    {
        /// <summary>
        /// Wrotes a simulation to storage
        /// </summary>
        /// <param name="simulation">The simulation to save to storage</param>
        /// <returns>A taks containing the executing save command</returns>
        Task Write(Simulation simulation);
    }
}
