namespace EasyRead.Core.Services.LoginAuthentication
{
    public interface ILoginAuthenticationService
    {
        long? GetUserId(string login, string password);
        void SetPassword(long userId, string login, string password);
    }
}