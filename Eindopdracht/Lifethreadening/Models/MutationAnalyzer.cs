using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Lifethreadening.Models
{
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

        public MutationAnalyzer() 
        { 
            Mutations = new HashSet<Mutation>();
        }

        public void RegisterMutations(IEnumerable<Animal> animals)
        {
            foreach(Animal animal in animals)
            {
                Mutations.UnionWith(animal.Mutations);
            }
        }

        public IDictionary<StatisticInfo, int> Analyze()
        {
            return Mutations.SelectMany(mutation => mutation.Affected).GroupBy(statistic => statistic.Name)
                .ToDictionary(statistic => statistic.First(), statistics => statistics.Count());
        }
    }
}
