using EnglishEasyRead.BusinessServices;
using EnglishEasyRead.BusinessServices.RemoteServices.DictionaryApi;
using EnglishEasyRead.BusinessServices.RemoteServices.TextService;
using EnglishEasyRead.Dictionary;
using EnglishEasyRead.NancyModules.BusinessServices;
using EnglishEasyRead.NancyModules.Core;
using EnglishEasyRead.NancyModules.Dictionary;
using EnglishEasyRead.WordNet;
using Nancy;
using Nancy.TinyIoc;
using Owin;

namespace EnglishEasyRead.Web
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
            container.Register<ITextAnalysisService, TextAnalysisService>();

            //EnglishEasyRead.Core.NancyModules
            container.Register<ITextModuleSettings>(new TextModuleSettings());

            //EnglishEasyRead.BusinessServices.NancyModules
            container.Register<ITextBusinessServiceModuleSettings>(new TextBusinessServiceModuleSettings());
            container.Register<ITextBusinessService, TextBusinessService>();

            //EnglishEasyRead.BusinessServices
            container.Register<IDictionaryApiClient>(new DictionaryApiClient("http://localhost:57504/api/dictionary"));
            container.Register<ITextServiceClient>(new TextServiceClient("http://localhost:57504/api/core"));

            //EnglishEasyRead.Dictionary
            var databasePath = @"C:\WordNet\2.1\dict";
            container.Register<IMorpher>(new Morpher(databasePath));
            container.Register<IWordNetFacade>(new WordNetFacade(databasePath));
        }
    }
}