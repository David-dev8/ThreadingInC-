using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess
{
    public interface ICustomSpeciesWriter
    {
        void Create(Species species);
        Task CreateMultiple(IEnumerable<Species> species); // TODO deze methode aan het einde van het project weghalen, dit is voor nu een template voor het bulk inserten
    }
}
