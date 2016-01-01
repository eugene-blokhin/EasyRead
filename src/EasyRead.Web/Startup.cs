using System;
using System.Collections.Generic;
using EasyRead.BusinessServices;
using EasyRead.BusinessServices.RemoteServices.DictionaryApi;
using EasyRead.BusinessServices.RemoteServices.TextService;
using EasyRead.Core.DataAccess;
using EasyRead.Core.Repositories;
using EasyRead.Core.Services.LoginAuthentication;
using EasyRead.Core.Services.User;
using EasyRead.Dictionary;
using EasyRead.NancyModules.BusinessServices;
using EasyRead.NancyModules.Core;
using EasyRead.NancyModules.Core.LoginAuthentication;
using EasyRead.NancyModules.Core.User;
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

    internal abstract class DependencyContainerSetup
    {
        public virtual void ConfigureApplicationContainer(TinyIoCContainer container) { }
        public virtual void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context) { }
    }

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private readonly List<DependencyContainerSetup> _containerSetups = new List<DependencyContainerSetup>
        {
            new UserServiceSetup(),
            new DictionaryServiceSetup(), 
            new TextServiceSetup(),
            new TextBusinessServiceSetup(),
            new LoginAuthenticationServiceSetup(),
            new DatabaseContextServiceSetup()
        };

        protected override void ConfigureApplicationContainer(TinyIoCContainer container) =>
            _containerSetups.ForEach(setup => setup.ConfigureApplicationContainer(container));

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context) =>
            _containerSetups.ForEach(setup => setup.ConfigureRequestContainer(container, context));
        
        private class UserServiceSetup : DependencyContainerSetup
        {
            private class UserModuleSettings : IUserModuleSettings
            {
                public string ModulePath { get; } = "api/core";
            }

            public override void ConfigureApplicationContainer(TinyIoCContainer container)
            {
                container.Register<IUserModuleSettings>(new UserModuleSettings());
            }

            public override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
            {
                container.Register<IUserService, UserService>();
                container.Register<IUserRepository, UserRepository>();
            }
        }

        private class DictionaryServiceSetup : DependencyContainerSetup
        {
            private class DictionaryApiModuleSettings : IDictionaryApiModuleSettings
            {
                public string BasePath { get; } = "/api/dictionary";
            }

            public override void ConfigureApplicationContainer(TinyIoCContainer container)
            {
                container.Register<IDictionaryApiModuleSettings>(new DictionaryApiModuleSettings());
                container.Register<ITextAnalysisService, TextAnalysisService>();

                var databasePath = @"C:\WordNet\2.1\dict";
                container.Register<IMorpher>(new Morpher(databasePath));
                container.Register<IWordNetFacade>(new WordNetFacade(databasePath));
            }
        }

        private class TextServiceSetup : DependencyContainerSetup
        {
            private class TextModuleSettings : ITextModuleSettings
            {
                public string BasePath { get; } = "api/core";
            }

            public override void ConfigureApplicationContainer(TinyIoCContainer container)
            {
                container.Register<ITextModuleSettings>(new TextModuleSettings());
            }
        }

        private class TextBusinessServiceSetup : DependencyContainerSetup
        {
            private class TextBusinessServiceModuleSettings : ITextBusinessServiceModuleSettings
            {
                public string BasePath { get; } = "api/gateway";
            }

            public override void ConfigureApplicationContainer(TinyIoCContainer container)
            {
                container.Register<ITextBusinessServiceModuleSettings>(new TextBusinessServiceModuleSettings());
                container.Register<ITextBusinessService, TextBusinessService>();

                container.Register<IDictionaryApiClient>(new DictionaryApiClient("http://localhost:57504/api/dictionary"));
                container.Register<ITextServiceClient>(new TextServiceClient("http://localhost:57504/api/core"));
            }
        }

        private class DatabaseContextServiceSetup : DependencyContainerSetup
        {
            public override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
            {
                var dbContext = new EasyReadDbContext("EasyReadDb");
                container.Register<IDbContextFactory>(new DbContectFactory(dbContext));

                //IDisposable items added to this collection are disposed automatically when the request has been processed.
                context.Items["EasyReadDbContext"] = dbContext;
            }
        }

        private class LoginAuthenticationServiceSetup : DependencyContainerSetup
        {
            private class LoginAuthenticationModuleSettings : ILoginAuthenticationModuleSettings
            {
                public string ModulePath { get; } = "api/core";
            }

            public override void ConfigureApplicationContainer(TinyIoCContainer container)
            {
                container.Register<ILoginAuthenticationModuleSettings, LoginAuthenticationModuleSettings>();
                container.Register<IPasswordValidator, PasswordValidator>();
                container.Register<ISaltedHashGenerator, SaltedHashGenerator>();
            }

            public override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
            {
                container.Register<ILoginAuthenticationService, LoginAuthenticationService>();
                container.Register<ILoginAuthenticationRepository, LoginAuthenticationRepository>();
            }
        }
    }
    
    internal class DbContectFactory : IDbContextFactory
    {
        private readonly EasyReadDbContext _context;

        public DbContectFactory(EasyReadDbContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            _context = context;
        }

        public EasyReadDbContext GetContext()
        {
            return _context;
        }
    }
}
