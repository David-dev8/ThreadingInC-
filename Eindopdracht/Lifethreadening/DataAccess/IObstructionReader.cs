using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess
{
    /// <summary>
    /// This class is used to retrieve data about obstructions from storage
    /// </summary>
    public interface IObstructionReader
    {
        /// <summary>
        /// Retrieves data about obstructions of an ecosystem from storage
        /// </summary>
        /// <param name="ecosystemId">The ecosystem to get the obstructions for</param>
        /// <param name="contextService">The context service</param>
        /// <returns>A list of all obstructions that are assigned to the given ecosystem</returns>
        IEnumerable<Obstruction> ReadByEcosystem(int ecosystemId, WorldContextService contextService);
    }
}
