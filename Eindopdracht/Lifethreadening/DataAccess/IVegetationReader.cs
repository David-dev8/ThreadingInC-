using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess
{
    /// <summary>
    /// This class is used to retrieve data on vegitation from storage
    /// </summary>
    public interface IVegetationReader
    {
        /// <summary>
        /// Gets all vegitation data for a specific ecosystem
        /// </summary>
        /// <param name="ecosystemId">The ecosystem to get the vegitation data for</param>
        /// <param name="contextService">The contaxtservices</param>
        /// <returns>A list with all the vegitation data</returns>
        IEnumerable<Vegetation> ReadByEcosystem(int ecosystemId, WorldContextService contextService);
    }
}
