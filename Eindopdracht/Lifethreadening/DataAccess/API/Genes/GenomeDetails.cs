using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess.API.Genes
{
    public class GenomeDetails
    {
        public string PrimaryAccession { get; set; }
        [JsonPropertyName("proteinDescription")]
        public Protein Protein { get; set; }
        public IEnumerable<Gene> Genes { get; set; }
        public Description Sequence { get; set; }
    }
}
