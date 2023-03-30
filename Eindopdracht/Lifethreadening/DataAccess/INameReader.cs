using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess
{
    /// <summary>
    /// This class is used to retrieve names
    /// </summary>
    public interface INameReader
    {
        /// <summary>
        /// Retrieves a name
        /// </summary>
        /// <param name="sex"> Indicate wether the name should be masculin or feminine</param>
        /// <returns>An apropiate name for the given sex</returns>
        string GetName(Sex sex);
    }
}
