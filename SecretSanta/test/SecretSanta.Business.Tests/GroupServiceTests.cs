using SecretSanta.Data;
using SecretSanta.Data.Tests;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class GroupServiceTests : TestBase
    {
        //Test Insert
        [TestMethod]
        public async Task InsertAsync_Group1_Success()
        {
            // Arrange
            using var dbContextInsert = new ApplicationDbContext(Options);
            IGroupService service = new GroupService(dbContextInsert, Mapper);

            var group1 = SampleData.CreateGroup1();

            // Act
            await service.InsertAsync(group1);

            // Assert
            Assert.IsNotNull(group1.Id);
        }
        //Test Update
        [TestMethod]
        public async Task UpdateGroup_ShouldSaveIntoDatabase()
        {
            // Arrange
            using var dbContextInsert = new ApplicationDbContext(Options);
            IGroupService service = new GroupService(dbContextInsert, Mapper);

            var group1 = SampleData.CreateGroup1();
            var group2 = SampleData.CreateGroup2();

            await service.InsertAsync(group1);
            await service.InsertAsync(group2);

            // Act
            using var dbContextFetch = new ApplicationDbContext(Options);
            Group group1FromDb = await dbContextFetch.Groups.SingleAsync(item => item.Id == group1.Id);
            const string newTitle = "New Title";
            group1FromDb.Title = newTitle;

            // Update group1 using the group2 Id.
            await service.UpdateAsync(group2.Id!.Value, group1FromDb);

            // Assert
            using var dbContextAssert = new ApplicationDbContext(Options);
            group1FromDb = await dbContextAssert.Groups.SingleAsync(item => item.Id == group1.Id);
            var group2FromDb = await dbContextAssert.Groups.SingleAsync(item => item.Id == 2);

            Assert.AreEqual(
                (SampleData.GroupTitle1,newTitle), (group1FromDb.Title, group2FromDb.Title));
        }
        //Test FetchAll
        [TestMethod]
        public async Task FetchAll_ShouldReturnAllGroups()
        {
            //Arrange 
            using var dbContext = new ApplicationDbContext(Options);
            IGroupService service = new GroupService(dbContext, Mapper);

            var group1 = SampleData.CreateGroup1();
            var group2 = SampleData.CreateGroup2();
            await service.InsertAsync(group1);
            await service.InsertAsync(group2);
            //Act
            List<Group> groups = await service.FetchAllAsync();
            //Assert
            Assert.AreEqual(group1.Title, groups[0].Title);
            Assert.AreEqual(group2.Title, groups[1].Title);
        }
        //Test InsertList
        [TestMethod]
        public async Task InsertList_ShouldPutGroupsInDb()
        {
            // Arrange
            using var dbContextInsert = new ApplicationDbContext(Options);
            IGroupService service = new GroupService(dbContextInsert, Mapper);
            var group1 = SampleData.CreateGroup1();
            var group2 = SampleData.CreateGroup2();
            //Act
            await service.InsertAsync(group1, group2);
            Group group1FromDb = await dbContextInsert.Groups.SingleAsync(item => item.Id == group1.Id);
            Group group2FromDb = await dbContextInsert.Groups.SingleAsync(item => item.Id == 2);
            //Assert
            Assert.AreEqual((group1.Title,group2.Title),(group1FromDb.Title,group2FromDb.Title));
        }
        //Test FetchById
        [TestMethod]
        public async Task FetchById_ShouldReturnSuccess()
        {
            // Arrange
            using var dbContextInsert = new ApplicationDbContext(Options);
            IGroupService service = new GroupService(dbContextInsert, Mapper);
            var group1 = SampleData.CreateGroup1();
            var group2 = SampleData.CreateGroup2();
            //Act
            await service.InsertAsync(group1, group2);
            Group group1FromDb = await dbContextInsert.Groups.SingleAsync(item => item.Id == group1.Id);
            Group group2FromDb = await dbContextInsert.Groups.SingleAsync(item => item.Id == 2);
            Group group1FromService = await service.FetchByIdAsync(1);
            Group group2FromService = await service.FetchByIdAsync(2);
            //Assert
            Assert.AreEqual((group1FromDb.Title,group2FromDb.Title),
                (group1FromService.Title,group2FromService.Title));
        }
        //Test Delete
        [TestMethod]
        public async Task Delete_ShouldReturnEmpty()
        {
            // Arrange
            using var dbContextInsert = new ApplicationDbContext(Options);
            IGroupService service = new GroupService(dbContextInsert, Mapper);
            var group = SampleData.CreateGroup1();
            //Act
            await service.InsertAsync(group);
            Group groupFromDb = await dbContextInsert.Groups.SingleAsync(item => item.Id == group.Id);
            //Turned on warning because the Id from the object couldn't be used as paramater
            _ = await service.DeleteAsync(groupFromDb.Id!.Value);
            List<Group> groups = await service.FetchAllAsync();
            //Assert
            Assert.AreEqual(0, groups.Count);
        }
    }
}
