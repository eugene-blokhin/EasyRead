using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;

namespace EasyRead.BusinessServices.RemoteServices.DictionaryApi
{
    public class DictionaryApiClient : IDictionaryApiClient
    {
        private readonly RestClient _client;

        public DictionaryApiClient(string baseUrl)
        {
            if (baseUrl == null) throw new ArgumentNullException(nameof(baseUrl));

            _client = new RestClient(baseUrl);
        }

        public async Task<string[]> ExtractLemmasAsync(string text)
        {
            var request = new RestRequest("/extract-lemmas", Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "text/plain");
            request.AddParameter("text/xml", text, ParameterType.RequestBody);

            var response = await _client.ExecutePostTaskAsync<List<string>>(request);
            
            //TODO: Handle errors

            return response.Data.ToArray();
        }
    }
}
