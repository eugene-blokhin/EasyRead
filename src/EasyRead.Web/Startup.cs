using EasyRead.BusinessServices;
using EasyRead.BusinessServices.RemoteServices.DictionaryApi;
using EasyRead.BusinessServices.RemoteServices.TextService;
using EasyRead.Dictionary;
using EasyRead.NancyModules.BusinessServices;
using EasyRead.NancyModules.Core;
using EasyRead.NancyModules.Dictionary;
using EasyRead.WordNet;
using Nancy;
using Nancy.TinyIoc;
using Owin;

namespace EasyRead.Web
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
            //EasyRead.Dictionary.NancyModules
            container.Register<IDictionaryApiModuleSettings>(new DictionaryApiModuleSettings());
            container.Register<ITextAnalysisService, TextAnalysisService>();

            //EasyRead.Core.NancyModules
            container.Register<ITextModuleSettings>(new TextModuleSettings());

            //EasyRead.BusinessServices.NancyModules
            container.Register<ITextBusinessServiceModuleSettings>(new TextBusinessServiceModuleSettings());
            container.Register<ITextBusinessService, TextBusinessService>();

            //EasyRead.BusinessServices
            container.Register<IDictionaryApiClient>(new DictionaryApiClient("http://localhost:57504/api/dictionary"));
            container.Register<ITextServiceClient>(new TextServiceClient("http://localhost:57504/api/core"));

            //EasyRead.Dictionary
            var databasePath = @"C:\WordNet\2.1\dict";
            container.Register<IMorpher>(new Morpher(databasePath));
            container.Register<IWordNetFacade>(new WordNetFacade(databasePath));
        }
    }
}
