using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess.JSON
{
    /// <summary>
    /// This class stores data about the world locations used when saving to a JSON files
    /// </summary>
    public class JSONLocation
    {
        public string Id { get; set; }
        public IList<string> Neighbours { get; set; }
        public IEnumerable<SimulationElement> SimulationElements { get; set; }
    }
}
