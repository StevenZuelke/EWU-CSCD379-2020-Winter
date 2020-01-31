using SecretSanta.Api.Controllers;
using SecretSanta.Business;
using SecretSanta.Data;
using SecretSanta.Data.Tests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SecretSanta.Business.Services;
using Moq.Protected;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests
    {
        [TestMethod]
        public void Create_UserController_Success()
        {
            //Arrange
            var service = new Mock<IUserService>();

            //Act
            _ = new UserController(service.Object);

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_WithoutService_Fails()
        {
            //Arrange

            //Act
            _ = new UserController(null!);

            //Assert
        }

        [TestMethod]
        public async Task GetById_WithExistingUser_Success()
        {
            // Arrange
            var service = new Mock<IUserService>();
            User user = SampleData.CreateInigoMontoya();

            service.Setup(m => m.InsertAsync(user)).ReturnsAsync(user);
            int idnum = user.Id;
            service.Setup(m => m.FetchByIdAsync(idnum)).ReturnsAsync(FetchHelper(idnum));
            var controller = new UserController(service.Object);
            await service.Object.InsertAsync(user);
            // Act
            ActionResult<User> result = await controller.Get(user.Id);
            OkObjectResult okResult = (OkObjectResult)result.Result;
            User userResult = (User)okResult.Value;
            // Assert
            Assert.AreEqual(user.FirstName, userResult.FirstName);
            Assert.IsTrue(result.Result is OkObjectResult);
        }

        [TestMethod]
        public async Task GetById_WithoutUsers_Fail()
        {
            // Arrange
            var service = new Mock<IUserService>();
            int id = 0;
#nullable disable //Well aware that User can't be null this part is suppposed to cause failure
            _ = service.Setup(m => m.FetchByIdAsync(id)).ReturnsAsync((User)null);
#nullable enable
            var controller = new UserController(service.Object);
            // Act
            ActionResult<User> result = await controller.Get(id);
            // Assert
            Assert.IsTrue(result.Result is NotFoundResult);
        }

        [TestMethod]
        public async Task Post_User_Success()
        {
            // Arrange
            var service = new Mock<IUserService>();
            User user = SampleData.CreateInigoMontoya();
            service.Setup(m => m.InsertAsync(user)).ReturnsAsync(user);
            var controller = new UserController(service.Object);
            // Act
            user = await controller.Post(user);
            // Assert
            Assert.IsNotNull(user.Id);
        }

        [TestMethod]
        public async Task FetchAll_User_Success()
        {
            //Arrange
            var service = new Mock<IUserService>();
            User user = SampleData.CreateInigoMontoya();
            User user2 = SampleData.CreatePrincessButtercup();
            service.Setup(m => m.FetchAllAsync()).ReturnsAsync(FetchAllHelper(user, user2));
            var controller = new UserController(service.Object);
            //Act
            List<User> users = (List<User>)await controller.Get();
            //Assert
            Assert.AreEqual(users.Count, 2);
        }

        [TestMethod]
        public async Task Put_User_Success()
        {
            // Arrange
            var service = new Mock<IUserService>();
            User user = SampleData.CreateInigoMontoya();
            int id = 2;
            service.Setup(m => m.UpdateAsync(id, user)).ReturnsAsync(PutHelper(id, user));
            service.Setup(m => m.FetchByIdAsync(id)).ReturnsAsync(FetchHelper(id));
            var controller = new UserController(service.Object);
            // Act
            ActionResult<User> result = await controller.Put(id, user);
            OkObjectResult okResult = (OkObjectResult)result.Result;
            User userResult = (User)okResult.Value;
            // Assert
            Assert.IsTrue(result.Result is OkObjectResult);
            Assert.AreEqual(userResult.Id, id);
        }

        [TestMethod]
        public async Task Put_IndexOutofBounds_Fail()
        {
            // Arrange
            var service = new Mock<IUserService>();
            int id = 2;
            User user = SampleData.CreateInigoMontoya();
#nullable disable //Well aware that User can't be null this part is suppposed to cause failure
            _ = service.Setup(m => m.FetchByIdAsync(id)).ReturnsAsync((User)null);
#nullable enable
            service.Setup(m => m.UpdateAsync(id, user)).ReturnsAsync(PutHelper(id, user));
            var controller = new UserController(service.Object);
            // Act
            ActionResult<User> result = await controller.Put(id, user);
            // Assert
            Assert.IsTrue(result.Result is NotFoundResult);
        }

        [TestMethod]
        public async Task Delete_User_Success()
        {
            // Arrange
            var service = new Mock<IUserService>();
            int id = 2;
            service.Setup(m => m.DeleteAsync(id)).ReturnsAsync(true);
            service.Setup(m => m.FetchByIdAsync(id)).ReturnsAsync(FetchHelper(id));
            var controller = new UserController(service.Object);
            // Act
            ActionResult<bool> result = await controller.Delete(id);
            OkObjectResult okResult = (OkObjectResult)result.Result;
            bool boolResult = (bool)okResult.Value;
            // Assert
            Assert.IsTrue(result.Result is OkObjectResult);
            Assert.AreEqual(boolResult, true);
        }

        [TestMethod]
        public async Task Delete_UserOutOfBounds_Fail()
        {
            // Arrange
            var service = new Mock<IUserService>();
            int id = 2;
            service.Setup(m => m.DeleteAsync(id)).ReturnsAsync(true);
#nullable disable //Well aware that User can't be null this part is suppposed to cause failure
            _ = service.Setup(m => m.FetchByIdAsync(id)).ReturnsAsync((User)null);
#nullable enable
            var controller = new UserController(service.Object);
            // Act
            ActionResult<bool> result = await controller.Delete(id);
            // Assert
            Assert.IsTrue(result.Result is NotFoundResult);
        }
        //Update user and return new Val
        private User PutHelper(int id, User user)
        {
            User testUser = new TestUser(user, id);
            return testUser;
        }
        //Create new user by specified ID
        private User FetchHelper(int id)
        {
            User newUser = SampleData.CreateInigoMontoya();
            User testUser = new TestUser(newUser, id);
            return testUser;
        }
        //Create user list from every input user
        private List<User> FetchAllHelper(params User[] user)
        {
            List<User> users = new List<User>();
            foreach (User g in user)
            {
                users.Add(g);
            }
            return users;
        }


    }


    public class TestUser : User
    {
        public TestUser(User user, int id)
            : base((user ?? throw new ArgumentNullException(nameof(user))).FirstName, user.LastName)
        {
            Id = id;
        }
    }

}
