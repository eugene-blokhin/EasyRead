using System.Threading;
using System.Threading.Tasks;
using Nancy;
using Nancy.ModelBinding;

namespace EnglishEasyRead.BusinessServices.NancyModules
{
    public class TextBusinessServiceModule : NancyModule
    {
        private readonly ITextBusinessService _textBusinessService;

        public TextBusinessServiceModule(ITextBusinessServiceModuleSettings settings, ITextBusinessService textBusinessService)
            : base(settings.BasePath)
        {
            _textBusinessService = textBusinessService;

            Get["GetAvailableAsync", "text", true] = async (_, token) => await GetAvailableAsync(token);
            Post["SubmitTextAsync", "text", true] = async (_, token) => await SubmitTextAsync(token);
        }

        private async Task<object> SubmitTextAsync(CancellationToken token)
        {
            var text = this.Bind<TextModel>();
            return await _textBusinessService.SubmitTextAsync(text);
        }

        private async Task<object> GetAvailableAsync(CancellationToken token)
        {
            var texts = await _textBusinessService.GetAvailableAsync();
            return texts;
        }
    }
}