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
    /// <summary>
    /// This class stores function to help with interactiong with API's
    /// </summary>
    public class APIHelper
    {
        // Reuse the same client (HttpClient is thread safe for all used operations)
        private static HttpClient s_httpClient = new HttpClient();

        private static JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
        };

        /// <summary>
        /// Retrieves data from a specific endpoint
        /// </summary>
        /// <typeparam name="T">The type of data to retrieve</typeparam>
        /// <param name="endpoint">The endpoint to get the data for</param>
        /// <param name="queryParameters">The parameters to give the endpoint</param>
        /// <param name="headersToRetrieve">The headers that need to be retrieved</param>
        /// <returns>The data that was retrieved</returns>
        public async Task<APIResult<T>> Fetch<T>(string endpoint, IDictionary<string, object> queryParameters = null, string[] headersToRetrieve = null)
        {
            return await GetFromAPI<T>(endpoint, queryParameters, headersToRetrieve);
        }

        /// <summary>
        /// Retrieves data from a variable endpoint
        /// </summary>
        /// <param name="id">The ID of the endpoint suffix</param>
        /// <typeparam name="T">The type of data to retrieve</typeparam>
        /// <param name="endpoint">The endpoint to get the data for</param>
        /// <param name="queryParameters">The parameters to give the endpoint</param>
        /// <param name="headersToRetrieve">The headers that need to be retrieved</param>
        /// <returns>The data that was retrieved</returns>
        public async Task<APIResult<T>> Fetch<T>(string endpoint, int id, IDictionary<string, object> queryParameters = null, string[] headersToRetrieve = null)
        {
            return await GetFromAPI<T>(endpoint + "/" + id, queryParameters, headersToRetrieve);
        }

        /// <summary>
        /// Retrieves a objects file from an endpoint
        /// </summary>
        /// <typeparam name="T">The type of data to retrieve</typeparam>
        /// <param name="endpoint">he endpoint to retrieve from</param>
        /// <param name="queryParameters">The parameters for the endpoint</param>
        /// <param name="headersToRetrieve">The headers to retrieve</param>
        /// <returns></returns>
        private async Task<APIResult<T>> GetFromAPI<T>(string endpoint, IDictionary<string, object> queryParameters = null, IEnumerable<string> headersToRetrieve = null)
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

        /// <summary>
        /// Deserializes a JSON response into an object
        /// </summary>
        /// <typeparam name="T">The object to deserialize into</typeparam>
        /// <param name="response">The Json response to deserialize</param>
        /// <returns>The deserialized JSON in the format you provided</returns>
        private async Task<T> FromJSON<T>(HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, _serializerOptions);
        }
    }

    /// <summary>
    /// This class stores data partaining to an API result
    /// </summary>
    /// <typeparam name="T">The type of data the result represents</typeparam>
    public class APIResult<T>
    {
        public T Value { get; set; }
        public IDictionary<string, string> Headers { get; set; }
    }
}
