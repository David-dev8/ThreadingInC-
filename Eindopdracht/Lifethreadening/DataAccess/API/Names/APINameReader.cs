using Lifethreadening.DataAccess.API.Genes;
using Lifethreadening.ExtensionMethods;
using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess.API.Names
{
    public class APINameReader : APICaller, INameReader // TODO onnodige folder?
    {
        private const string NAME_API_BASE_URL = "https://names.drycodes.com";
        private const string CASING = "title";
        private const string SEPARATOR = "space";
        private const int CACHE_AMOUNT = 50;
        private static readonly IDictionary<Sex, string> _options = new Dictionary<Sex, string>()
        {
            { Sex.MALE, "boy_names" },
            { Sex.FEMALE, "girl_names" },
        };

        private IDictionary<Sex, Queue<string>> _cache = new Dictionary<Sex, Queue<string>>()
        {
            { Sex.MALE, new Queue<string>() },
            { Sex.FEMALE, new Queue<string>() },
        };

        private IDictionary<Sex, List<string>> _backup = new Dictionary<Sex, List<string>>()
        {
            { Sex.MALE, new List<string>() { "Harold", "John", "Carl", "Hank" } },
            { Sex.FEMALE, new List<string>() { "Janice", "Selene", "Vivian", "Lenora" } },
        }; // TODO goed idee?

        public async Task<string> GetName(Sex sex)
        {
            if(!_cache[sex].Any())
            {
                await RetrieveNextBatch(sex);
            }
            return _cache[sex].Dequeue();
            
        }

        private async Task RetrieveNextBatch(Sex sex)
        {
            try
            {
                var result = await _apiHandler.Fetch<IEnumerable<string>>(NAME_API_BASE_URL, CACHE_AMOUNT,
                new Dictionary<string, object>()
                {
                    { "case", CASING },
                    { "separator", SEPARATOR },
                    { "nameOptions", _options[sex] }
                });
                _cache[sex].EnqueueMultiple(result.Value.Select(name => name.Split(" ").First()));
            }
            catch(Exception)
            {
                _cache[sex].EnqueueMultiple(_backup[sex]); // TODO
            }
        }
    }
}
