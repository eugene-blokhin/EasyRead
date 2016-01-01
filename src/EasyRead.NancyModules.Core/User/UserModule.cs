using System;
using EasyRead.Core.Services;
using EasyRead.Core.Services.User;
using Nancy;
using Nancy.Metadata.Modules;
using Nancy.Metadata.Swagger.Core;
using Nancy.Metadata.Swagger.Fluent;
using Nancy.ModelBinding;

namespace EasyRead.NancyModules.Core.User
{
    public interface IUserModuleSettings
    {
        string ModulePath { get; }
    }

    public class UserModule : NancyModule
    {
        private readonly IUserService _userService;

        public UserModule(IUserModuleSettings modueSettings, IUserService userService) : base(modueSettings.ModulePath)
        {
            if (userService == null) throw new ArgumentNullException(nameof(userService));

            _userService = userService;

            Get["Get", "/users/{id:long}"] = param =>
            {
                var user = _userService.GetUser(param.id);
                return user ?? 404;
            };

            Post["Create", "/users"] = param =>
            {
                try
                {
                    var user = this.Bind<EasyRead.Core.Model.User>();
                    var userId = _userService.CreateUser(user);
                    user.Id = userId;

                    return user;
                }
                catch (ModelBindingException ex)
                {
                    return 400;
                }
                catch (Exception ex) when (ex.GetType().GetGenericTypeDefinition() == typeof(ServiceException<>))
                {
                    return 400;
                }
            };

            After.InsertBefore("SetDefaultAcceptHeader", context =>
            {
                //TODO:add default accept header
            });
        }
    }

    public class UserMetadataModule : MetadataModule<SwaggerRouteMetadata>
    {
        public UserMetadataModule()
        {
            Describe["Get"] = desc => new SwaggerRouteMetadata(desc).With(i => i
                .WithDescription("Returns an instance of User with by specified Id.")
                .WithRequestParameter("id", "long", description: "Id of user")
                .WithResponseModel("200", typeof(EasyRead.Core.Model.User))
                .WithResponseModel("404", typeof(void), "The user with the given Id has not been found.")
            );

            Describe["Create"] = desc => new SwaggerRouteMetadata(desc).With(i => i
                .WithDescription("Creates a new user with parameters from the request.")
                .WithRequestModel(typeof(EasyRead.Core.Model.User))
                .WithResponseModel("201", typeof(EasyRead.Core.Model.User), "Returns a created user with the Id value specified.")
                .WithResponseModel("400", typeof(void), "There is an error with the request")
            );
        }
    }
}
