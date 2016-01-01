namespace EnglishEasyRead.Core.Model
{
    public class LoginAuthentication
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string LoginName { get; set; }
        public byte[] Salt { get; set; }
        public byte[] PasswordHash { get; set; }
    }
}