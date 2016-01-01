using System.Security.Cryptography;

namespace EasyRead.Core.Services.LoginAuthentication
{
    public interface ISaltedHashGenerator
    {
        byte[] GenerateSaltedHash(string str, byte[] salt);

        byte[] GenerateSaltedHash(string str, out byte[] salt, int saltLength);
    }

    public class SaltedHashGenerator : ISaltedHashGenerator
    {
        public const int SALT_BYTE_SIZE = 24;
        public const int HASH_BYTE_SIZE = 24;
        public const int PBKDF2_ITERATIONS = 1000;

        public byte[] GenerateSaltedHash(string str, byte[] salt)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(str, salt)
            {
                IterationCount = PBKDF2_ITERATIONS
            };

            return pbkdf2.GetBytes(HASH_BYTE_SIZE);
        }

        public byte[] GenerateSaltedHash(string str, out byte[] salt, int saltLength)
        {
            using (var rnd = new RNGCryptoServiceProvider())
            {
                salt = new byte[SALT_BYTE_SIZE];
                rnd.GetBytes(salt);
            }

            return GenerateSaltedHash(str, salt);
        }
    }
}
