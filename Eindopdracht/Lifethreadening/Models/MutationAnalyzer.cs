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
        public ISet<Mutation> Mutations { get; private set; }

        public MutationAnalyzer() 
        { 
            ISet<StatisticInfo> statistics = new HashSet<StatisticInfo>();
            statistics.Add(new StatisticInfo("attack", Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#59ff00"), 5));
            statistics.Add(new StatisticInfo("defence", Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#FF0000"), 5));
            
            Mutations = new HashSet<Mutation>();
            Mutations.Add(new Mutation(MutationType.ADDITION, "q", "q", "q", DateTime.Now, statistics));
            Mutations.Add(new Mutation(MutationType.ADDITION, "q", "q", "q", DateTime.Now, statistics));
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
