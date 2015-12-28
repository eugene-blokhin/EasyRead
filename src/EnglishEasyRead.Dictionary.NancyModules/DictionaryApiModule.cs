using System.IO;
using System.Threading.Tasks;
using Nancy;
using Nancy.Responses.Negotiation;

namespace EnglishEasyRead.Dictionary.NancyModules
{
    public class DictionaryApiModule : NancyModule
    {
        private readonly ITextAnalysisService _textAnalysisService;

        public DictionaryApiModule(IDictionaryApiModuleSettings settings, ITextAnalysisService textAnalysisService) : base(settings.BasePath)
        {
            _textAnalysisService = textAnalysisService;
            Post["ExtractLemmas", "/extract-lemmas", true] = async (_, ct) =>
            {
                using (var r = new StreamReader(Request.Body))
                {
                    var bodyText = await r.ReadToEndAsync();
                    return await ExtractLemmasAsync(bodyText);
                }
            };
        }

        private async Task<Negotiator> ExtractLemmasAsync(string text)
        {
            return Negotiate
                .WithModel(_textAnalysisService.ExtractLemmasFromText(text))
                .WithStatusCode(HttpStatusCode.OK);
        }
    }
}
