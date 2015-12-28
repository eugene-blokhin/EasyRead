using System.IO;
using System.Threading.Tasks;
using Nancy;
using Nancy.Responses.Negotiation;

namespace EnglishEasyRead.Dictionary.NancyModules
{
    public class DictionaryApiModule : NancyModule
    {
        public DictionaryApiModule(IDictionaryApiModuleSettings settings) : base(settings.BasePath)
        {
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
                .WithModel(text.Split(' '))
                .WithStatusCode(HttpStatusCode.OK);
        }
    }
}
