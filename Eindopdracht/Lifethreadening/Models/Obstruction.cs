using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    /// <summary>
    /// This class is used to store data about obstruction elements
    /// </summary>
    public class Obstruction : SimulationElement
    {
        private const int DEFAULT_PRIORITY = 3;

        /// <summary>
        /// Creates a new obstruction
        /// </summary>
        /// <param name="image">The icon of the obstruction</param>
        /// <param name="contextService">The contextservice</param>
        public Obstruction(string image, WorldContextService contextService): base(DEFAULT_PRIORITY, image, contextService)
        { 
        }

        /// <summary>
        /// This function gets the nutritional value of the obstruction
        /// </summary>
        /// <returns>The nutritional value of the obstruction</returns>
        public override int GetNutritionalValue()
        {
            return 0;
        }

        /// <summary>
        /// This function depletes the nutritional value of the obstruction
        /// </summary>
        /// <returns>The new nutritional value of the obstruction</returns>
        public override int DepleteNutritionalValue()
        {
            return GetNutritionalValue();
        }

        /// <summary>
        /// This function check if the obstruction still exists
        /// </summary>
        /// <returns>A boolean calue indicating wether the obstruction still exists</returns>
        public override bool StillExistsPhysically()
        {
            return true;
        }

        /// <summary>
        /// This function calculates the next action for the obstruction
        /// </summary>
        /// <returns>The next action to perform</returns>
        protected override Action GetNextAction()
        {
            return null;
        }
    }
}
