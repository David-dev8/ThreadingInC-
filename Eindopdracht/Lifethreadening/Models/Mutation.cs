using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    /// <summary>
    /// This class stores all data partaining to mutations
    /// </summary>
    public class Mutation
    {
        private bool _hasAffected;
        private Action<Statistics> _mutateAction;

        public MutationType Type { get; set; }
        public string Allel { get; set; }
        public string ProteinBefore { get; set; }
        public string ProteinAfter { get; set; }
        public DateTime MutationDate { get; set; }
        public IList<StatisticInfo> Affected { get; set; }

        /// <summary>
        /// Creates a new mutation
        /// </summary>
        /// <param name="type">The type of mutation</param>
        /// <param name="allel">The allel that gets mutated</param>
        /// <param name="proteinBefore">The protein that gets affected</param>
        /// <param name="proteinAfter">The resulting protien</param>
        /// <param name="mutationDate">The date of the mutation</param>
        /// <param name="mutateAction"> The changes this mutation makes</param>
        public Mutation(MutationType type, string allel, string proteinBefore, string proteinAfter, DateTime mutationDate, Action<Statistics> mutateAction)
        {
            Type = type;
            Allel = allel;
            ProteinBefore = proteinBefore;
            ProteinAfter = proteinAfter;
            MutationDate = mutationDate;
            _mutateAction = mutateAction;
            Affected = new List<StatisticInfo>();
            _hasAffected = false;
        }

        /// <summary>
        /// Creates a new mutation
        /// </summary>
        /// <param name="type">The type of mutation</param>
        /// <param name="allel">The allel that gets mutated</param>
        /// <param name="proteinBefore">The protein that gets affected</param>
        /// <param name="proteinAfter">The resulting protien</param>
        /// <param name="mutationDate">The date of the mutation</param>
        /// <param name="affected"> The changes this mutation makes</param>
        [JsonConstructor]
        public Mutation(MutationType type, string allel, string proteinBefore, string proteinAfter, DateTime mutationDate, IList<StatisticInfo> affected)
        {
            Type = type;
            Allel = allel;
            ProteinBefore = proteinBefore;
            ProteinAfter = proteinAfter;
            MutationDate = mutationDate;
            Affected = affected;
            _hasAffected = true;
        }

        /// <summary>
        /// This function aplies the mutation effect to an animal
        /// </summary>
        /// <param name="animal">The animal to mutate</param>
        public void Affect(Animal animal)
        {
            if(!_hasAffected)
            {
                IDictionary<string, StatisticInfo> previousStatistics = animal.Statistics.GetData();

                _mutateAction(animal.Statistics);
                animal.Mutations.Add(this);
                
                _hasAffected = true;
                Affected = CreateChangedStatistics(previousStatistics, animal.Statistics.GetData()).Values.ToList();
            }
        }

        /// <summary>
        /// This function generates the statistics of after the mutation
        /// </summary>
        /// <param name="previousStatistics">The statistics before the mutation</param>
        /// <param name="currentStatistics">The statistics after the mutation</param>
        /// <returns>A collection of all changes</returns>
        private IDictionary<string, StatisticInfo> CreateChangedStatistics(IDictionary<string, StatisticInfo> previousStatistics, IDictionary<string, StatisticInfo> currentStatistics)
        {
            IDictionary<string, StatisticInfo> changedStatistics = new Dictionary<string, StatisticInfo>();
            foreach(KeyValuePair<string, StatisticInfo> statistic in currentStatistics)
            {
                int change = statistic.Value.Value - previousStatistics[statistic.Key].Value;
                if(change != 0)
                {
                    changedStatistics.Add(statistic.Key, new StatisticInfo(statistic.Key, statistic.Value.Color, change));
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
