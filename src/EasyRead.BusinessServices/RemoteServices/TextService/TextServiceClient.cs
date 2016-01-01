using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;

namespace EasyRead.BusinessServices.RemoteServices.TextService
{
    public class TextServiceClient : ITextServiceClient
    {
        private readonly RestClient _client;

        public TextServiceClient(string baseUrl)
        {
            if (baseUrl == null) throw new ArgumentNullException(nameof(baseUrl));

            _client = new RestClient(baseUrl);
        }

        public async Task<TextModel> SaveTextAsync(TextModel text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            var request = new RestRequest("text", Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(text);

            var response = await _client.ExecutePostTaskAsync<TextModel>(request);

            //TODO: check return code

            return response.Data;
        }

        public async Task<TextModel[]> GetAllTexts()
        {
            var request = new RestRequest("text", Method.GET);
            request.AddHeader("Accept", "application/json");

            var response = await _client.ExecuteGetTaskAsync<List<TextModel>>(request);
            
            //TODO: check return code

            return response.Data.ToArray();
        }

        public async Task<TextModel> GetTextAsync(long textId)
        {
            var request = new RestRequest("text/{id}", Method.GET);
            request.AddParameter("id", textId);
            request.AddHeader("Accept", "application/json");

            var response = await _client.ExecuteGetTaskAsync<TextModel>(request);

            //TODO: check return code

            return response.Data;
        }

        public async Task<string> GetTextBodyAsync(long textId)
        {
            var request = new RestRequest("text/{id}/body", Method.GET);
            request.AddParameter("id", textId);
            request.AddHeader("Accept", "plain/text");

            var response = await _client.ExecuteGetTaskAsync<string>(request);

            //TODO: check return code

            return response.Data;
        }

        public async Task<string[]> GetTextWordsAsync(long textId)
        {
            var request = new RestRequest("text/{id}/words", Method.GET);
            request.AddParameter("id", textId);
            request.AddHeader("Accept", "application/json");

            var response = await _client.ExecuteGetTaskAsync<List<string>>(request);

            //TODO: check return code

            return response.Data.ToArray();
        }
    }
}
