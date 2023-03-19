using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess.API
{
    public class APIGeneReader : APICaller, IGeneReader
    {
        private const string GENE_API_BASE_URL = "https://rest.uniprot.org/uniprotkb/search";

        // Store some genes to return to reduce the amount of API calls
        private List<GenomeDetails> _cache = new List<GenomeDetails>();
        // Pagination
        private string _nextPage;
        private const int MINIMUM_BATCH_SIZE = 20;

        public async Task<string> GetRandomGene()
        {
            await MakeSureToHaveEnough();
            return _cache.First().Genes.First().Name.Value;
        }

        public async Task<IEnumerable<string>> GetRandomProteins(int amount)
        {
            await MakeSureToHaveEnough(amount);
            return _cache.Take(amount).Select(genomeDetails => genomeDetails.Protein.SubmissionNames.First().FullName.Value).ToList();
        }

        private async Task MakeSureToHaveEnough(int amount = 1)
        {
            if(_cache.Count < amount)
            {
                await RetrieveNextBatch(Math.Max(MINIMUM_BATCH_SIZE, amount));
            }
        }

        private async Task RetrieveNextBatch(int amount = 1)
        {
            var fields = new string[] { "protein_name", "gene_names", "sequence" };
            var t = await _apiHandler.Fetch<UniProtResult<GenomeDetails>>("https://rest.uniprot.org/uniprotkb/search",
                new string[] { "link" }, 
                new Dictionary<string, object>()
                {
                    {"query", "(organism_id%3A9606)" },
                    {"size", 20 },
                    {"fields", string.Join(",", fields) },
                });
            _nextPage = t.Headers["link"];
            _cache.AddRange(t.Value.Results);
        }
    }

    public class UniProtResult<T>
    {
        public IEnumerable<T> Results { get; set; } 
    }

    public class GenomeDetails
    {
        public string PrimaryAccession { get; set; }
        [JsonPropertyName("proteinDescription")]
        public Protein Protein { get; set; }
        public IEnumerable<Gene> Genes { get; set; }
        public Description Sequence { get; set; }
    }

    public class Gene
    {
        [JsonPropertyName("geneName")]
        public Description Name { get; set; }
    }

    public class Protein
    {
        public IEnumerable<ProteinName> SubmissionNames { get; set; }
    }

    public class ProteinName
    {
        public Description FullName { get; set; }
    }

    public class Description
    {
        public string Value { get; set; }
    }
}
