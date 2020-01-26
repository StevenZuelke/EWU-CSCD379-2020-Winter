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
    public class UserServiceTests : TestBase
    {
        //Test Insert
        [TestMethod]
        public async Task InsertAsync_Inigo_Success()
        {
            // Arrange
            using var dbContextInsert = new ApplicationDbContext(Options);
            IUserService service = new UserService(dbContextInsert, Mapper);

            var inigo = SampleData.CreateInigoMontoya();

            // Act
            await service.InsertAsync(inigo);

            // Assert
            Assert.IsNotNull(inigo.Id);
        }
        //Test Update
        [TestMethod]
        public async Task UpdateUser_ShouldSaveIntoDatabase()
        {
            // Arrange
            using var dbContextInsert = new ApplicationDbContext(Options);
            IUserService service = new UserService(dbContextInsert, Mapper);

            var inigo = SampleData.CreateInigoMontoya();
            var princess = SampleData.CreatePrincessButtercup();

            await service.InsertAsync(inigo);
            await service.InsertAsync(princess);

            // Act
            using var dbContextFetch = new ApplicationDbContext(Options);
            User inigoFromDb = await dbContextFetch.Users.SingleAsync(item => item.Id == inigo.Id);
            const string montoyaThe3rd = "Montoya The 3rd";
            inigoFromDb.LastName = montoyaThe3rd;

            // Update Inigo Montoya using the princesses Id.
            await service.UpdateAsync(princess.Id!.Value, inigoFromDb);

            // Assert
            using var dbContextAssert = new ApplicationDbContext(Options);
            inigoFromDb = await dbContextAssert.Users.SingleAsync(item => item.Id == inigo.Id);
            var princessFromDb = await dbContextAssert.Users.SingleAsync(item => item.Id == 2);

            Assert.AreEqual(
                (SampleData.Inigo, montoyaThe3rd), (princessFromDb.FirstName, princessFromDb.LastName));

            Assert.AreEqual(
                (SampleData.Inigo, SampleData.Montoya), (inigoFromDb.FirstName, inigoFromDb.LastName));
        }
        //Test FetchAll
        [TestMethod]
        public async Task FetchAll_ShouldReturnAllUsers()
        {
            //Arrange 
            using var dbContext = new ApplicationDbContext(Options);
            IUserService service = new UserService(dbContext, Mapper);

            var inigo = SampleData.CreateInigoMontoya();
            var princess = SampleData.CreatePrincessButtercup();
            await service.InsertAsync(inigo);
            await service.InsertAsync(princess);
            //Act
            List<User> users = await service.FetchAllAsync();
            //Assert
            Assert.AreEqual(inigo.FirstName, users[0].FirstName);
            Assert.AreEqual(princess.FirstName, users[1].FirstName);
        }
        //Test InsertList
        [TestMethod]
        public async Task InsertList_ShouldPutUsersInDb()
        {
            // Arrange
            using var dbContextInsert = new ApplicationDbContext(Options);
            IUserService service = new UserService(dbContextInsert, Mapper);
            var inigo = SampleData.CreateInigoMontoya();
            var princess = SampleData.CreatePrincessButtercup();
            //Act
            await service.InsertAsync(inigo,princess);
            User inigoFromDb = await dbContextInsert.Users.SingleAsync(item => item.Id == inigo.Id);
            User princessFromDb = await dbContextInsert.Users.SingleAsync(item => item.Id ==  2);
            //Assert
            Assert.AreEqual((inigo.FirstName, inigo.LastName, princess.FirstName, princess.LastName),
                (inigoFromDb.FirstName, inigoFromDb.LastName, princessFromDb.FirstName, princessFromDb.LastName));
        }
        //Test FetchById
        [TestMethod]
        public async Task FetchById_ShouldReturnSuccess()
        {
            // Arrange
            using var dbContextInsert = new ApplicationDbContext(Options);
            IUserService service = new UserService(dbContextInsert, Mapper);
            var inigo = SampleData.CreateInigoMontoya();
            var princess = SampleData.CreatePrincessButtercup();
            //Act
            await service.InsertAsync(inigo, princess);
            User inigoFromDb = await dbContextInsert.Users.SingleAsync(item => item.Id == inigo.Id);
            User princessFromDb = await dbContextInsert.Users.SingleAsync(item => item.Id == 2);
            User inigoFromService = await service.FetchByIdAsync(1);
            User princessFromService = await service.FetchByIdAsync(2);
            //Assert
            Assert.AreEqual((inigoFromDb.FirstName, princessFromDb.FirstName),
                (inigoFromService.FirstName, princessFromService.FirstName));
        }
        //Test Delete
        [TestMethod]
        public async Task Delete_ShouldReturnEmpty()
        {
            // Arrange
            using var dbContextInsert = new ApplicationDbContext(Options);
            IUserService service = new UserService(dbContextInsert, Mapper);
            var inigo = SampleData.CreateInigoMontoya();
            //Act
            await service.InsertAsync(inigo);
            User inigoFromDb = await dbContextInsert.Users.SingleAsync(item => item.Id == inigo.Id);
            _ = await service.DeleteAsync(inigoFromDb.Id!.Value);
            List<User> users = await service.FetchAllAsync();
            //Assert
            Assert.AreEqual(0, users.Count);
        }
    }
}
