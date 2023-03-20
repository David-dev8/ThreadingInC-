using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Mutation
    {
        private bool _hasAffected = false;
        private Action<Statistics> _mutateAction;

        public MutationType type { get; set; }
        public string allel { get; set; }
        public string proteinBefore { get; set; }
        public string proteinAfter { get; set; }
        public DateTime mutationDate { get; set; } // TODO
        public IList<StatisticInfo> affected { get; set; }


        public Mutation(MutationType type, string allel, string proteinBefore, string proteinAfter, Action<Statistics> mutateAction)
        {
            this.type = type;
            this.allel = allel;
            this.proteinBefore = proteinBefore;
            this.proteinAfter = proteinAfter;
            _mutateAction = mutateAction;
        }

        public void Affect(Animal animal)
        {
            if(!_hasAffected)
            {
                IDictionary<string, StatisticInfo> previousStatistics = animal.Statistics.GetData();

                _mutateAction(animal.Statistics);
                animal.Mutations.Add(this);
                
                _hasAffected = true;
                affected = CreateChangedStatistics(previousStatistics, animal.Statistics.GetData()).Values.ToList();
            }
        }

        private IDictionary<string, StatisticInfo> CreateChangedStatistics(IDictionary<string, StatisticInfo> previousStatistics, IDictionary<string, StatisticInfo> currentStatistics)
        {
            IDictionary<string, StatisticInfo> changedStatistics = new Dictionary<string, StatisticInfo>();
            foreach(KeyValuePair<string, StatisticInfo> statistic in currentStatistics)
            {
                int change = statistic.Value.value - previousStatistics[statistic.Key].value;
                if(change != 0)
                {
                    changedStatistics.Add(statistic.Key, new StatisticInfo(statistic.Key, statistic.Value.color, change));
                }
            }
            return changedStatistics;
        }
    }
}
