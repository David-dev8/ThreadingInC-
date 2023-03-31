using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    /// <summary>
    /// This interface is used to define a breed factory
    /// </summary>
    public interface IBreedFactory
    {
        /// <summary>
        /// This function creates ofspring
        /// </summary>
        /// <param name="father">the first parent</param>
        /// <param name="mother">The second parent</param>
        /// <param name="contextService">the contextservice</param>
        /// <returns>A collection with birthed ofspring</returns>
        IEnumerable<Animal> CreateAnimals(Animal father, Animal mother, WorldContextService contextService);
    }
}
