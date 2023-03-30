using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess
{
    /// <summary>
    /// This class manages data partaining to the weather
    /// </summary>
    public interface IWeatherManager
    {
        /// <summary>
        /// Retrieves the current weather state
        /// </summary>
        /// <returns>The current weather</returns>
        Weather GetCurrent();

        /// <summary>
        /// Updates the weather to a new set of values
        /// </summary>
        void Update();
    }
}
