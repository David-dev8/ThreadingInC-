using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Disasters
{
    /// <summary>
    /// This class is used to create Disasters
    /// </summary>
    public interface IDisasterFactory
    {
        /// <summary>
        /// Creates a new disaster
        /// </summary>
        /// <param name="worldContextService">The context service</param>
        /// <returns>A new disaster</returns>
        Disaster CreateDisaster(WorldContextService worldContextService);
    }
}
