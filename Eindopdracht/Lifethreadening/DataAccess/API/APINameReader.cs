using Lifethreadening.DataAccess.API.GeneDTOs;
using Lifethreadening.ExtensionMethods;
using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess.API
{
    /// <summary>
    /// This class is used to retrieve Names from an random name generator API
    /// </summary>
    public class APINameReader : APICaller, INameReader
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
        };

        /// <summary>
        /// Initializes the conection to the API
        /// </summary>
        /// <returns></returns>
        public async Task Initialize()
        {
            try
            {
                var results = await Task.WhenAll(_options.Keys.Select(RetrieveBatch));
                _cache = results.ToDictionary(keyValue => keyValue.Key, keyValue => keyValue.Value);
            }
            catch(Exception)
            {
                _cache = _backup.ToDictionary(keyValue => keyValue.Key, keyValue => new Queue<string>(keyValue.Value));
            }
        }


        /// <summary>
        /// Retrieves a name from the API
        /// </summary>
        /// <param name="sex">The sex to get the name for</param>
        /// <returns>A sex apropriate name</returns>
        public string GetName(Sex sex)
        {
            string nextName = _cache[sex].Dequeue();
            _cache[sex].Enqueue(nextName);
            return nextName;
        }

        /// <summary>
        /// Retrieve multiple names at once
        /// </summary>
        /// <param name="sex">The sex to get the names for</param>
        /// <returns>A list containg sex apropriate names</returns>
        private async Task<KeyValuePair<Sex, Queue<string>>> RetrieveBatch(Sex sex)
        {
            var result = await _apiHandler.Fetch<IEnumerable<string>>(NAME_API_BASE_URL, CACHE_AMOUNT,
                new Dictionary<string, object>()
                {
                    { "case", CASING },
                    { "separator", SEPARATOR },
                    { "nameOptions", _options[sex] }
                });
            return new KeyValuePair<Sex, Queue<string>>(sex, new Queue<string>(result.Value));
        }
    }
}
