using EnglishEasyRead.BusinessServices;
using EnglishEasyRead.BusinessServices.NancyModules;
using EnglishEasyRead.BusinessServices.RemoteServices.DictionaryApi;
using EnglishEasyRead.BusinessServices.RemoteServices.TextService;
using EnglishEasyRead.Core.NancyModules;
using EnglishEasyRead.Dictionary.NancyModules;
using Nancy;
using Nancy.TinyIoc;
using Owin;

namespace EnglishEasyRead.Dictionary.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
        }
    }

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private class DictionaryApiModuleSettings : IDictionaryApiModuleSettings
        {
            public string BasePath { get; } = "/api/dictionary";
        }

        private class TextModuleSettings : ITextModuleSettings
        {
            public string BasePath { get; } = "api/core";
        }

        private class TextBusinessServiceModuleSettings : ITextBusinessServiceModuleSettings
        {
            public string BasePath { get; } = "api/gateway";
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            //EnglishEasyRead.Dictionary.NancyModules
            container.Register<IDictionaryApiModuleSettings>(new DictionaryApiModuleSettings());

            //EnglishEasyRead.Core.NancyModules
            container.Register<ITextModuleSettings>(new TextModuleSettings());

            //EnglishEasyRead.BusinessServices.NancyModules
            container.Register<ITextBusinessServiceModuleSettings>(new TextBusinessServiceModuleSettings());
            container.Register<ITextBusinessService, TextBusinessService>();

            //EnglishEasyRead.BusinessServices
            container.Register<IDictionaryApiClient>(new DictionaryApiClient("http://localhost:57504/api/dictionary"));
            container.Register<ITextServiceClient>(new TextServiceClient("http://localhost:57504/api/core"));
        }
    }
}