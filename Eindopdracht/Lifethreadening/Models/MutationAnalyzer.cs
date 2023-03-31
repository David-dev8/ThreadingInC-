using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Lifethreadening.Models
{
    /// <summary>
    /// This class is used to analyse mutations
    /// </summary>
    public class MutationAnalyzer
    {
        public ISet<Mutation> Mutations { get; set; }

        public int TotalAmounntOfMutations
        {
            get
            {
                return Mutations.Count;
            }
        }

        /// <summary>
        /// Creates a new mutation analyser
        /// </summary>
        public MutationAnalyzer() 
        { 
            Mutations = new HashSet<Mutation>();
        }

        /// <summary>
        /// This function adds the mutations of a set of animals to itself
        /// </summary>
        /// <param name="animals">The animals to gather the mutations from</param>
        public void RegisterMutations(IEnumerable<Animal> animals)
        {
            foreach(Animal animal in animals)
            {
                Mutations.UnionWith(animal.Mutations);
            }
        }

        /// <summary>
        /// This function analyses all gather mutations
        /// </summary>
        /// <returns>A dictionary containing info about the mutations</returns>
        public IDictionary<StatisticInfo, int> Analyze()
        {
            return Mutations.SelectMany(mutation => mutation.Affected).GroupBy(statistic => statistic.Name)
                .ToDictionary(statistic => statistic.First(), statistics => statistics.Count());
        }
    }
}
