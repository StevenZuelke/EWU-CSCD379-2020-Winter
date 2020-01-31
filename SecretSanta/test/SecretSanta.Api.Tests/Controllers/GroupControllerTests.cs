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
    public class GroupControllerTests
    {
        [TestMethod]
        public void Create_GroupController_Success()
        {
            //Arrange
            var service = new Mock<IGroupService>();

            //Act
            _ = new GroupController(service.Object);

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_WithoutService_Fails()
        {
            //Arrange

            //Act
            _ = new GroupController(null!);

            //Assert
        }

        [TestMethod]
        public async Task GetById_WithExistingGroup_Success()
        {
            // Arrange
            var service = new Mock<IGroupService>();
            Group group = SampleData.CreateGroup1();

            service.Setup(m => m.InsertAsync(group)).ReturnsAsync(group);
            int idnum = group.Id;
            service.Setup(m => m.FetchByIdAsync(idnum)).ReturnsAsync(FetchHelper(idnum));
            var controller = new GroupController(service.Object);
            await service.Object.InsertAsync(group);
            // Act
            ActionResult<Group> result = await controller.Get(group.Id);
            OkObjectResult okResult = (OkObjectResult)result.Result;
            Group groupResult = (Group)okResult.Value;
            // Assert
            Assert.AreEqual(group.Title, groupResult.Title);
            Assert.IsTrue(result.Result is OkObjectResult);
        }

        [TestMethod]
        public async Task GetById_WithoutGroups_Fail()
        {
            // Arrange
            var service = new Mock<IGroupService>();
            int id = 0;
#nullable disable //Well aware that Group can't be null this part is suppposed to cause failure
            _ = service.Setup(m => m.FetchByIdAsync(id)).ReturnsAsync((Group)null);
#nullable enable
            var controller = new GroupController(service.Object);
            // Act
            ActionResult<Group> result = await controller.Get(id);
            // Assert
            Assert.IsTrue(result.Result is NotFoundResult);
        }

        [TestMethod]
        public async Task Post_Group_Success()
        {
            // Arrange
            var service = new Mock<IGroupService>();
            Group group = SampleData.CreateGroup1();
            service.Setup(m => m.InsertAsync(group)).ReturnsAsync(group);
            var controller = new GroupController(service.Object);
            // Act
            group = await controller.Post(group);
            // Assert
            Assert.IsNotNull(group.Id);
        }

        [TestMethod]
        public async Task FetchAll_Group_Success()
        {
            //Arrange
            var service = new Mock<IGroupService>();
            Group group = SampleData.CreateGroup1();
            Group group2 = SampleData.CreateGroup2();
            service.Setup(m => m.FetchAllAsync()).ReturnsAsync(FetchAllHelper(group, group2));
            var controller = new GroupController(service.Object);
            //Act
            List<Group> groups = (List<Group>)await controller.Get();
            //Assert
            Assert.AreEqual(groups.Count, 2);
        }

        [TestMethod]
        public async Task Put_Group_Success()
        {
            // Arrange
            var service = new Mock<IGroupService>();
            Group group = SampleData.CreateGroup1();
            int id = 2;
            service.Setup(m => m.UpdateAsync(id, group)).ReturnsAsync(PutHelper(id, group));
            service.Setup(m => m.FetchByIdAsync(id)).ReturnsAsync(FetchHelper(id));
            var controller = new GroupController(service.Object);
            // Act
            ActionResult<Group> result = await controller.Put(id, group);
            OkObjectResult okResult = (OkObjectResult)result.Result;
            Group groupResult = (Group)okResult.Value;
            // Assert
            Assert.IsTrue(result.Result is OkObjectResult);
            Assert.AreEqual(groupResult.Id, id);
        }

        [TestMethod]
        public async Task Put_IndexOutofBounds_Fail()
        {
            // Arrange
            var service = new Mock<IGroupService>();
            int id = 2;
            Group group = SampleData.CreateGroup1();
#nullable disable //Well aware that Group can't be null this part is suppposed to cause failure
            _ = service.Setup(m => m.FetchByIdAsync(id)).ReturnsAsync((Group)null);
#nullable enable
            service.Setup(m => m.UpdateAsync(id, group)).ReturnsAsync(PutHelper(id, group));
            var controller = new GroupController(service.Object);
            // Act
            ActionResult<Group> result = await controller.Put(id, group);
            // Assert
            Assert.IsTrue(result.Result is NotFoundResult);
        }

        [TestMethod]
        public async Task Delete_Group_Success()
        {
            // Arrange
            var service = new Mock<IGroupService>();
            int id = 2;
            service.Setup(m => m.DeleteAsync(id)).ReturnsAsync(true);
            service.Setup(m => m.FetchByIdAsync(id)).ReturnsAsync(FetchHelper(id));
            var controller = new GroupController(service.Object);
            // Act
            ActionResult<bool> result = await controller.Delete(id);
            OkObjectResult okResult = (OkObjectResult)result.Result;
            bool boolResult = (bool)okResult.Value;
            // Assert
            Assert.IsTrue(result.Result is OkObjectResult);
            Assert.AreEqual(boolResult, true);
        }

        [TestMethod]
        public async Task Delete_GroupOutOfBounds_Fail()
        {
            // Arrange
            var service = new Mock<IGroupService>();
            int id = 2;
            service.Setup(m => m.DeleteAsync(id)).ReturnsAsync(true);
#nullable disable //Well aware that Group can't be null this part is suppposed to cause failure
            _ = service.Setup(m => m.FetchByIdAsync(id)).ReturnsAsync((Group)null);
#nullable enable
            var controller = new GroupController(service.Object);
            // Act
            ActionResult<bool> result = await controller.Delete(id);
            // Assert
            Assert.IsTrue(result.Result is NotFoundResult);
        }
        //Update group and return new Val
        private Group PutHelper(int id, Group group)
        {
            Group testGroup = new TestGroup(group, id);
            return testGroup;
        }
        //Create new group by specified ID
        private Group FetchHelper(int id)
        {
            Group newGroup = SampleData.CreateGroup1();
            Group testGroup = new TestGroup(newGroup, id);
            return testGroup;
        }
        //Create group list from every input group
        private List<Group> FetchAllHelper(params Group[] group)
        {
            List<Group> groups = new List<Group>();
            foreach (Group g in group)
            {
                groups.Add(g);
            }
            return groups;
        }


    }


    public class TestGroup : Group
    {
        public TestGroup(Group group, int id)
            : base((group ?? throw new ArgumentNullException(nameof(group))).Title)
        {
            Id = id;
        }
    }

}
