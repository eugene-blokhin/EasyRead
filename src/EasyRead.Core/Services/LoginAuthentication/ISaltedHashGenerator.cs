namespace EasyRead.Core.Services.LoginAuthentication
{
    public interface ISaltedHashGenerator
    {
        byte[] GenerateSaltedHash(string str, byte[] salt);

        byte[] GenerateSaltedHash(string str, out byte[] salt, int saltLength);
    }
}
