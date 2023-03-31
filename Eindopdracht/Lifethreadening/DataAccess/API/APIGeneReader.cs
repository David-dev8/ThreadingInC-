using Lifethreadening.DataAccess.API.GeneDTOs;
using Lifethreadening.ExtensionMethods;
using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess.API
{
    // TODO initialize? backup? Werkt deze eigenlijk nog wel?
    /// <summary>
    /// This class is used to read genes from an API
    /// </summary>
    public class APIGeneReader : APICaller, IGeneReader
    {
        private const string GENE_API_BASE_URL = "https://rest.uniprot.org/uniprotk/";
        private const string API_QUERY = "(organism_id%3A9606)";
        private static readonly string[] FIELDS_TO_SELECT = new string[] { "protein_name", "gene_names", "sequence" };

        private Queue<GenomeDetails> _cache = new Queue<GenomeDetails>();
        private IList<GenomeDetails> _backup = new List<GenomeDetails>()
        {
            new GenomeDetails() 
            {
                Protein = new Protein() { Name = new ProteinName() { FullName = new Description() { Value = "Peptase" } } },
                Genes = new List<Gene>() { new Gene() { Name = new Description() { Value = "TP56" } } },
                Sequence = new Description() { Value = "AAAAAABBTYKPWNA" }
            },
            new GenomeDetails()
            {
                Protein = new Protein() { Name = new ProteinName() { FullName = new Description() { Value = "Mycitylinose" } } },
                Genes = new List<Gene>() { new Gene() { Name = new Description() { Value = "X-4" } } },
                Sequence = new Description() { Value = "HAOQ" }
            },
            new GenomeDetails()
            {
                Protein = new Protein() { Name = new ProteinName() { FullName = new Description() { Value = "Tryptofan" } } },
                Genes = new List<Gene>() { new Gene() { Name = new Description() { Value = "Gn-(2)15" } } },
                Sequence = new Description() { Value = "PPPPAQWWWW" }
            }
        };

        // For pagination
        private string _nextPage;
        private const int MINIMUM_CACHE_AMOUNT = 100;

        public async Task<string> GetRandomGene()
        {
            await MakeSureToHaveEnough();
            return _cache.Dequeue().Genes.First().Name.Value;
        }

        public async Task<IEnumerable<string>> GetRandomProteins(int amount)
        {
            await MakeSureToHaveEnough(amount);
            return _cache.DequeueMultiple(amount).Select(genomeDetails => genomeDetails.Protein.Name.FullName.Value).ToList();
        }

        /// <summary>
        /// Checks if there are enough genes cashed, retrieves a new batch if not
        /// </summary>
        /// <param name="amount">The minimu ammount that should be pressent</param>
        /// <returns></returns>
        private async Task MakeSureToHaveEnough(int amount = 1)
        {
            if(_cache.Count < amount)
            {
                await RetrieveNextBatch(Math.Max(MINIMUM_CACHE_AMOUNT, amount));
            }
        }

        /// <summary>
        /// Retrieves more random data from the API
        /// </summary>
        /// <param name="amount">The ammount of data to retrieve</param>
        /// <returns>A task containing the executing API call</returns>
        private async Task RetrieveNextBatch(int amount)
        {
            Dictionary<string, object> queryParameters = new Dictionary<string, object>()
            {
                {"query", API_QUERY},
                {"size", amount },
                {"fields", string.Join(",", FIELDS_TO_SELECT) },
            };
            if(_nextPage != null)
            {
                queryParameters.Add("cursor", _nextPage);
            }

            try
            {
                var result = await _apiHandler.Fetch<UniProtResult<GenomeDetails>>(
                    GENE_API_BASE_URL + "search",
                    queryParameters,
                    new string[] { "link" }
                );
                _nextPage = result.Headers["link"];
                _cache.EnqueueMultiple(result.Value.Results);
            }
            catch(Exception)
            {
                _cache.EnqueueMultiple(_backup.RepeatUntilLength(amount));
            }
        }
    }
}
