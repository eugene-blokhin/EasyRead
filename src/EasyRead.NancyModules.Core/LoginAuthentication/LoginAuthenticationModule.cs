using System;
using EasyRead.Core.Services.LoginAuthentication;
using Nancy;
using Nancy.ModelBinding;

namespace EasyRead.NancyModules.Core.LoginAuthentication
{
    public interface ILoginAuthenticationModuleSettings
    {
        string ModulePath { get; }
    }

    public class CreateLoginAuthenticationRequest
    {
        //TODO: make sure HTTPS is used
        public long UserId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class CreateLoginAuthenticationResponse
    {
        public long UserId { get; set; }
    }

    public class LoginAuthenticationModule : NancyModule
    {
        private readonly ILoginAuthenticationService _loginAuthenticationService;

        public LoginAuthenticationModule(ILoginAuthenticationModuleSettings settings, ILoginAuthenticationService loginAuthenticationService)
            : base(settings.ModulePath)
        {
            if (loginAuthenticationService == null) throw new ArgumentNullException(nameof(loginAuthenticationService));

            _loginAuthenticationService = loginAuthenticationService;

            Get["ValidateLoginAuthentication", "/login-authenticator"] = _ =>
            {
                var login = Request.Query["login"];
                var password = Request.Query["password"];

                long? userId = _loginAuthenticationService.GetUserId(login, password);

                if (userId.HasValue)
                {
                    return new CreateLoginAuthenticationResponse {UserId = userId.Value};
                }
                else
                {
                    return HttpStatusCode.NotFound;
                }
            };


            //TODO: Current password must be required in order to change the old one. Otherwise, it's not safe.
            Post["CreateLoginAuthentication", "/login-authenticator"] = _ =>
            {
                var request = this.Bind<CreateLoginAuthenticationRequest>();
                _loginAuthenticationService.SetPassword(request.UserId, request.Login, request.Password);

                return 200;
            };
        }
    }
}
