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

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            container.Register<IDictionaryApiModuleSettings>(new DictionaryApiModuleSettings());
            container.Register<ITextModuleSettings>(new TextModuleSettings());
        }
    }
}