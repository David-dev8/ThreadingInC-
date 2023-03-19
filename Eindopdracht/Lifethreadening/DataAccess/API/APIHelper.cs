using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net;

namespace Lifethreadening.DataAccess.API
{
    public class APIHelper
    {
        // Reuse the same client (HttpClient is thread safe for all used operations)
        private static HttpClient s_httpClient = new HttpClient();

        private static JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
        };

        public APIHelper()
        {
        }

        public async Task<APIResult<T>> Fetch<T>(string endpoint, string[] headersToRetrieve = null, IDictionary<string, object> queryParameters = null)
        {
            return await GetFromAPI<T>(endpoint, headersToRetrieve, queryParameters);
        }

        public async Task<APIResult<T>> Fetch<T>(string endpoint, int id, string[] headersToRetrieve = null, IDictionary<string, object> queryParameters = null)
        {
            return await GetFromAPI<T>(endpoint + "/" + id, headersToRetrieve, queryParameters);
        }

        private async Task<APIResult<T>> GetFromAPI<T>(string endpoint, IEnumerable<string> headersToRetrieve = null, IDictionary<string, object> queryParameters = null)
        {
            if(headersToRetrieve == null)
            {
                headersToRetrieve = Enumerable.Empty<string>();
            }
            if(queryParameters != null)
            {
                endpoint += "?" + string.Join("&", queryParameters.Select(parameter => parameter.Key + "=" + parameter.Value.ToString()));
            }
            using(HttpResponseMessage response = await s_httpClient.GetAsync(endpoint))
            {
                response.EnsureSuccessStatusCode();
                return new APIResult<T>() { Value = await FromJSON<T>(response), Headers = headersToRetrieve.ToDictionary(header => header, header => response.Headers.GetValues(header).First()) };
            }
        }

        private async Task<T> FromJSON<T>(HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, _serializerOptions);
        }
    }

    public class APIResult<T>
    {
        public T Value { get; set; }
        public IDictionary<string, string> Headers { get; set; }
    }
}
