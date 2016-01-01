using System;
using EasyRead.Core.Model;
using EasyRead.Core.Repositories;
using EasyRead.Core.Services;
using EasyRead.Core.Services.User;
using Moq;
using NUnit.Framework;

namespace EasyRead.Core.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        [TestCase(0, null, "email@gmail.com", ExpectedException = typeof(ServiceException<ModelValidationError>))]
        [TestCase(0, "name", null, ExpectedException = typeof(ServiceException<ModelValidationError>))]
        [TestCase(0, "", "invalid@email", ExpectedException = typeof(ServiceException<ModelValidationError>))]
        [TestCase(0, "name", "", ExpectedException = typeof(ServiceException<ModelValidationError>))]
        [TestCase(0, "Eugene Blokhin", "eugene.blohin@gmail.com")]
        [TestCase(1, "Eugene Blokhin", "eugene.blohin@gmail.com", ExpectedException = typeof(ServiceException<ModelValidationError>))]
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

