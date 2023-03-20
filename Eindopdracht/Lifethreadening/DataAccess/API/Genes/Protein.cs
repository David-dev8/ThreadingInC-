using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess.API.Genes
{
    public class Protein
    {
        public IEnumerable<ProteinName> SubmissionNames { get; set; }
    }
}
