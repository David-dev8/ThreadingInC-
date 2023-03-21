using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Mutation
    {
        public MutationType Type { get; set; }
        public string Allel { get; set; }
        public string ProteinBefore { get; set; }
        public string ProteinAfter { get; set; }
        public DateTime MutationDate { get; set; }
        public ISet<StatisticInfo> Affected { get; set; }

        public Mutation(MutationType type, string allel, string proteinBefore, string proteinAfter, DateTime mutationDate, ISet<StatisticInfo> affected)
        {
            this.Type = type;
            this.Allel = allel;
            this.ProteinBefore = proteinBefore;
            this.ProteinAfter = proteinAfter;
            this.Affected = affected;
            this.MutationDate= mutationDate;
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
