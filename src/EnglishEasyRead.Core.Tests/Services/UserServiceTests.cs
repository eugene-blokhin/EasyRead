using System;
using EnglishEasyRead.Core.Model;
using EnglishEasyRead.Core.Repositories;
using EnglishEasyRead.Core.Services.User;
using Moq;
using NUnit.Framework;

namespace EnglishEasyRead.Core.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        [TestCase(0, null, "email@gmail.com", ExpectedException = typeof(ArgumentException))]
        [TestCase(0, "name", null, ExpectedException = typeof(ArgumentException))]
        [TestCase(0, "name", "email@gmail.com", ExpectedException = typeof(ArgumentException))]
        [TestCase(0, "", "invalid@email", ExpectedException = typeof(ArgumentException))]
        [TestCase(0, "name", "", ExpectedException = typeof(ArgumentException))]
        [TestCase(0, "Eugene Blokhin", "eugene.blohin@gmail.com")]
        [TestCase(1, "Eugene Blokhin", "eugene.blohin@gmail.com", ExpectedException = typeof(ArgumentException))]
        public void CreateUser_General(long id, string name, string email)
        {
            var repositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            repositoryMock.Setup(repository => repository.Create(It.IsAny<User>()))
                .Returns(long.MaxValue);

            var newUser = new User
            {
                Id = id,
                Name = name,
                Email = email
            };

            var service = new UserService(repositoryMock.Object);
            var createdUserId = service.CreateUser(newUser);
            Assert.AreEqual(long.MaxValue, createdUserId);

            repositoryMock.Verify(repository => repository.Create(newUser));
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void CreateUser_UserIsNull()
        {
            var service = new UserService(Mock.Of<IUserRepository>());
            service.CreateUser(null);
        }

        [Test]
        public void CreateUser_EmailOrUsernameAlreadyExist()
        {
            throw new NotImplementedException();
        }

        [TestCase(-1, ExpectedException = typeof(ArgumentOutOfRangeException))]
        [TestCase(0, ExpectedException = typeof(ArgumentOutOfRangeException))]
        [TestCase(long.MaxValue)]
        public void GetUser_General(long id)
        {
            var user = new User
            {
                Id = id,
                Name = "name",
                Email = "my-email@gmail.com"
            };

            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(repository => repository.GetById(id)).Returns(user);

            var service = new UserService(repositoryMock.Object);
            var returnedUser = service.GetUser(id);

            Assert.AreSame(user, returnedUser);
        }
        
        [Test]
        public void GetUser_UserDoesNotExists()
        {
            var service = new UserService(new Mock<IUserRepository>(MockBehavior.Loose).Object);
            var returnedUser = service.GetUser(1);
            Assert.AreSame((User)null, returnedUser);
        }
    }
}
