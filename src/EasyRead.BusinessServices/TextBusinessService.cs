using System;
using System.Linq;
using System.Threading.Tasks;
using EasyRead.BusinessServices.RemoteServices.DictionaryApi;
using EasyRead.BusinessServices.RemoteServices.TextService;

namespace EasyRead.BusinessServices
{
    public class TextBusinessService : ITextBusinessService
    {
        private readonly ITextServiceClient _textServiceClient;
        private readonly IDictionaryApiClient _dictionaryApiClient;

        public TextBusinessService(ITextServiceClient textServiceClient, IDictionaryApiClient dictionaryApiClient)
        {
            if (textServiceClient == null) throw new ArgumentNullException(nameof(textServiceClient));
            if (dictionaryApiClient == null) throw new ArgumentNullException(nameof(dictionaryApiClient));

            _textServiceClient = textServiceClient;
            _dictionaryApiClient = dictionaryApiClient;
        }

        public async Task<long> SubmitTextAsync(TextModel text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            
            //TODO validate the model

            var lemmas = await _dictionaryApiClient.ExtractLemmasAsync(text.Body);

            var textModel = new RemoteServices.TextService.TextModel
            {
                Body = text.Body,
                Name = text.Name,
                Words = lemmas.ToList()
            };

            var savedText = await _textServiceClient.SaveTextAsync(textModel);

            return savedText.Id;
        }

        public async Task<TextInfo[]> GetAvailableAsync()
        {
            var texts = await _textServiceClient.GetAllTexts();
            return texts.Select(t => new TextInfo {Id = t.Id, Name = t.Name}).ToArray();
        }
    }
}
