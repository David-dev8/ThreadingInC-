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
        /// <param name="EcoID">The ID of the Ecosystem the animal lives in</param>
        void Create(Species species, int EcoID);

        /// <summary>
        /// Create multiple spiecies in storage at the same time
        /// </summary>
        /// <param name="species">A list of all the spiecies to create in storage</param>
        /// <returns>Non</returns>
        Task CreateMultiple(IEnumerable<Species> species); // TODO deze methode aan het einde van het project weghalen, dit is voor nu een template voor het bulk inserten
    }
}
