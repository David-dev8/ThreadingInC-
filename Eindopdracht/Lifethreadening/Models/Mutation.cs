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
        public string Allel { get; set; }
        public string ProteinBefore { get; set; }
        public string ProteinAfter { get; set; }
        public DateTime mutationDate { get; set; } // TODO
        public IList<StatisticInfo> Affected { get; set; }


        public Mutation(MutationType type, string allel, string proteinBefore, string proteinAfter, Action<Statistics> mutateAction)
        {
            Type = type;
            Allel = allel;
            ProteinBefore = proteinBefore;
            ProteinAfter = proteinAfter;
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

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Mutation other = (Mutation)obj;
            return Allel == other.Allel && DateTime.Equals(MutationDate, other.MutationDate);
        }

        public override int GetHashCode()
        {
            return Allel.GetHashCode() + MutationDate.GetHashCode();
        }
    }
}
