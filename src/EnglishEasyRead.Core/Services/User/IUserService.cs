namespace EnglishEasyRead.Core.Services.User
{
    public interface IUserService
    {
        long CreateUser(Model.User user);
        Model.User GetUser(long userId);
    }
}
