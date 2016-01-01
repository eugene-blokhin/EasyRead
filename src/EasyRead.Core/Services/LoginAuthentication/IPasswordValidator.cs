using System;

namespace EasyRead.Core.Services.LoginAuthentication
{
    public interface IPasswordValidator
    {
        void Validate(string password);
    }

    public class PasswordValidator : IPasswordValidator
    {
        public void Validate(string password)
        {
            //TODO: fix exception handling
            if (password.Length < 6) throw new ArgumentException("Password too shord.");
            if (password.Length > 12) throw new ArgumentException("Password too long.");
        }
    }


}
