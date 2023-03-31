using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    /// <summary>
    /// This interface is used to define a mutation factory
    /// </summary>
    public interface IMutationFactory
    {
        /// <summary>
        /// This function creates a new mutation
        /// </summary>
        /// <param name="currentDate">The date of the mutation</param>
        /// <returns>A new mutation</returns>
        Task<Mutation> CreateMutation(DateTime currentDate);
    }
}
