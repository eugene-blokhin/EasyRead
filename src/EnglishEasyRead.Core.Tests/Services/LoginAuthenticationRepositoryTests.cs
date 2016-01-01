using System;
using System.Linq;
using System.Text;
using EasyReader.Common;
using EnglishEasyRead.Core.Model;
using EnglishEasyRead.Core.Repositories;
using EnglishEasyRead.Core.Services.LoginAuthentication;
using Moq;
using NUnit.Framework;

namespace EnglishEasyRead.Core.Tests.Services
{
    [TestFixture]
    public class LoginAuthenticationRepositoryTests
    {
        [TestCase(-1, "login", "accesS!@#", ExpectedException = typeof(ArgumentOutOfRangeException))]
        [TestCase(0, "login", "accesS!@#", ExpectedException = typeof(ArgumentOutOfRangeException))]

        [TestCase(long.MaxValue, null, "accesS!@#", ExpectedException = typeof(ArgumentNullException))]
        [TestCase(long.MaxValue, "goodlogin", null, ExpectedException = typeof(ArgumentNullException))]

        [TestCase(long.MaxValue, " ", "accesS!@#", ExpectedException = typeof(Exception<LoginDoesNotMeetRequrements>))]
        [TestCase(long.MaxValue, "asd", "accesS!@#", ExpectedException = typeof(Exception<LoginDoesNotMeetRequrements>))] // Too short
        [TestCase(long.MaxValue, "F!@k", "accesS!@#", ExpectedException = typeof(Exception<LoginDoesNotMeetRequrements>))]
        [TestCase(long.MaxValue, "tooo-loooooooooooooooooooong", "accesS!@#", ExpectedException = typeof(Exception<LoginDoesNotMeetRequrements>))]

        [TestCase(long.MaxValue, "goodlogin", "accesS!@#")]
        [TestCase(long.MaxValue, "good-login", "accesS!@#")]
        [TestCase(long.MaxValue, "good-login1", "accesS!@#")]
        public void SetPassword_General(long userId, string login, string password)
        {
            var repositoryMock = new Mock<ILoginAuthenticationRepository>(MockBehavior.Loose);
            var passwordValidatorMock = new Mock<IPasswordValidator>(MockBehavior.Loose);
            var saltedHashGenerator = new SaltedHashGenerator();

            var service = new LoginAuthenticationService(repositoryMock.Object, passwordValidatorMock.Object, saltedHashGenerator);
            
            service.SetPassword(userId, login, password);

            byte[] salt;
            var expectedPasswordHash = saltedHashGenerator.GenerateSaltedHash(password, out salt, LoginAuthenticationService.SALT_LENGTH);

            passwordValidatorMock.Verify(validator => validator.Validate(password));
            repositoryMock.Verify(repository => repository.GetByUserId(userId));
            repositoryMock.Verify(repository => repository.Save(It.Is<LoginAuthentication>(
                authentication =>
                    authentication.Id == 0
                    && authentication.UserId == userId
                    && authentication.LoginName == login
                    && authentication.Salt.SequenceEqual(salt)
                    && authentication.PasswordHash.SequenceEqual(expectedPasswordHash)
            )));
        }


        [TestCase(null, "pass", ExpectedException = typeof(ArgumentNullException))]
        [TestCase("login", null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("login", "pass")]
        public void GetUserId_General(string login, string password)
        {
            var repositoryMock = new Mock<ILoginAuthenticationRepository>(MockBehavior.Loose);
            var passwordValidatorMock = new Mock<IPasswordValidator>(MockBehavior.Loose);
            var saltedHashGenerator = new SaltedHashGenerator();

            var service = new LoginAuthenticationService(repositoryMock.Object, passwordValidatorMock.Object, saltedHashGenerator);
            
            var userId = service.GetUserId(login, password);

            Assert.IsNull(userId);
            repositoryMock.Verify(repository => repository.GetByLogin(login));
        }

        [TestCase(false, false)]
        [TestCase(true, false)]
        [TestCase(true, true)]
        public void GetUserId_CheckPassword(bool loginCorrect, bool passwordCorrect)
        {
            const string login = "login";
            var inputPass = "access";
            var dbPass = passwordCorrect ? inputPass : "somethingElse";
            
            var repositoryMock = new Mock<ILoginAuthenticationRepository>(MockBehavior.Loose);
            var passwordValidatorMock = new Mock<IPasswordValidator>(MockBehavior.Loose);
            var saltedHashGenerator = new SaltedHashGenerator();

            byte[] salt;
            var dbHash = saltedHashGenerator.GenerateSaltedHash(dbPass, out salt, 32);

            var loginAuthentication = new LoginAuthentication
            {
                Id = long.MaxValue - 1,
                UserId = long.MaxValue,
                Salt = salt,
                PasswordHash = dbHash
            };

            repositoryMock.Setup(repository => repository.GetByLogin(login)).Returns(loginCorrect ? loginAuthentication : null);

            var service = new LoginAuthenticationService(repositoryMock.Object, passwordValidatorMock.Object, saltedHashGenerator);
            var userId = service.GetUserId(login, inputPass);

            if (loginCorrect && passwordCorrect)
            {
                Assert.NotNull(userId);
                Assert.AreEqual(loginAuthentication.UserId, userId.Value);
            }
            else
            {
                Assert.IsNull(userId);
            }
        }

        private class SaltedHashGenerator : ISaltedHashGenerator
        {
            public byte[] GenerateSaltedHash(string str, byte[] salt)
            {
                return Encoding.UTF8.GetBytes(str).Union(salt).ToArray();
            }

            public byte[] GenerateSaltedHash(string str, out byte[] salt, int saltLength)
            {
                salt = new byte[saltLength];
                return GenerateSaltedHash(str, salt);
            }
        }

    }
}