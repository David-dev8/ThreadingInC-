using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess
{
    /// <summary>
    /// This class is used to retrieve data about ecosystems from storage
    /// </summary>
    public interface IEcosystemReader
    {

        /// <summary>
        /// Retieves all ecosystems from storage
        /// </summary>
        /// <returns>A list contaning all ecosystems</returns>
        IEnumerable<Ecosystem> ReadAll();
    }
}
