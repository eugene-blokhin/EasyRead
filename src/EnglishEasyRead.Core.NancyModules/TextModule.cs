using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nancy;
using Nancy.ModelBinding;

namespace EnglishEasyRead.Core.NancyModules
{
    public class TextModule : NancyModule
    {
        private static List<TextModel> _texts = new List<TextModel>();

        public TextModule(ITextModuleSettings settings) : base(settings.BasePath)
        {
            Get["text/{id:long}", true] = async (param, token) => await GetText((long) param.id, token);
            Post["text/", true] = async (_, token) => await SaveText(token);
            Get["text/{id:long}/body", true] = async (param, token) => await GetBody((long)param.id, token);
            Get["text/{id:long}/words", true] = async (param, token) => await GetWords((long)param.id, token);
        }

        private async Task<object> GetWords(long id, CancellationToken token)
        {
            var text = _texts.FirstOrDefault(t => t.Id == id);
            return text != null
                ? text.Words
                : (object)HttpStatusCode.NotFound;
        }

        private async Task<object> GetBody(long id, CancellationToken token)
        {
            var text = _texts.FirstOrDefault(t => t.Id == id);
            return text != null
                ? text.Body
                : (object) HttpStatusCode.NotFound;
        }

        private async Task<object> SaveText(CancellationToken token)
        {
            var text = this.Bind<TextModel>();
            text.Id = _texts.Count;
            _texts.Add(text);
            return text;
        }

        private async Task<object> GetText(long id, CancellationToken token)
        {
            var text = _texts.FirstOrDefault(t => t.Id == id);
            return text ?? (object)HttpStatusCode.NotFound;
        }
    }
}