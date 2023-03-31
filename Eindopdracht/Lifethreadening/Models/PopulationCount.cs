using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    /// <summary>
    /// This class is used to store populations on specific dates
    /// </summary>
    public class PopulationCount
    {
        public DateTime Date { get; set; }
        public Species Species { get; set; }
        public int AmountOfAnimals { get; set; }

        /// <summary>
        /// Creates a new pupulation count object
        /// </summary>
        /// <param name="date">The date the count was done</param>
        /// <param name="species">The spiecies that where counted</param>
        /// <param name="amountOfAnimals">The ammount of animals that where counted</param>
        public PopulationCount(DateTime date, Species species, int amountOfAnimals)
        {
            Date = date;
            Species = species;
            AmountOfAnimals = amountOfAnimals;
        }
    }
}
