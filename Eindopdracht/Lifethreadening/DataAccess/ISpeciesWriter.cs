using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess
{
    /// <summary>
    /// This class is used to store data about species in storage
    /// </summary>
    public interface ISpeciesWriter
    {
        /// <summary>
        /// Creates a new species in storage
        /// </summary>
        /// <param name="species">The species to store</param>
        /// <param name="ecosystemId">The ID of the Ecosystem the animal lives in</param>
        void Create(Species species, int ecosystemId);
    }
}
