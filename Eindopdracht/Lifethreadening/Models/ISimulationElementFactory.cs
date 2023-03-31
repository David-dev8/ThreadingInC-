using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    /// <summary>
    /// This interface is used to define a simulation element factory
    /// </summary>
    public interface ISimulationElementFactory
    {
        /// <summary>
        /// This function generates a random simulation element
        /// </summary>
        /// <param name="contextService">The context service</param>
        /// <returns>A random simulation element</returns>
        SimulationElement CreateRandomElement(WorldContextService contextService);
        
        /// <summary>
        /// This function creates an animal
        /// </summary>
        /// <param name="contextService">The contextservice</param>
        /// <returns>An animal</returns>
        Animal CreateAnimal(WorldContextService contextService);

        /// <summary>
        /// This function creates a random vegitation element
        /// </summary>
        /// <param name="contextService">The contextservice</param>
        /// <returns>A vegitation element</returns>
        Vegetation CreateVegetation(WorldContextService contextService);

        /// <summary>
        /// This function creates a random obstrucion element
        /// </summary>
        /// <param name="contextService">The contextservice</param>
        /// <returns>A random obstruction element</returns>
        Obstruction CreateObstruction(WorldContextService contextService);
    }
}
