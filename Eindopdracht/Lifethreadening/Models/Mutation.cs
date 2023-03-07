using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Mutation
    {
        public MutationType type { get; set; }
        public string allel { get; set; }
        public string proteinBefore { get; set; }
        public string proteinAfter { get; set; }
        public DateTime mutationDate { get; set; }
        public Dictionary<string, StatisticInfo> affected { get; set; }

        public Mutation(MutationType type, string allel, string proteinBefore, string proteinAfter, DateTime mutationdate, Dictionary<string, StatisticInfo> affected)
        {
            this.type = type;
            this.allel = allel;
            this.proteinBefore = proteinBefore;
            this.proteinAfter = proteinAfter;
            this.affected = affected;
            this.mutationDate= mutationdate;
        }
    }
}
