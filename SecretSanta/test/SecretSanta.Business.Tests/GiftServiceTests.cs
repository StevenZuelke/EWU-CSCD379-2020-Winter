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
    public class GiftServiceTests : TestBase
    {
        //Test Insert
        [TestMethod]
        public async Task InsertAsync_Gift1_Success()
        {
            // Arrange
            using var dbContextInsert = new ApplicationDbContext(Options);
            IGiftService service = new GiftService(dbContextInsert, Mapper);

            var gift = SampleData.CreateGift1();

            // Act
            await service.InsertAsync(gift);

            // Assert
            Assert.IsNotNull(gift.Id);
        }
        //Test Update
        [TestMethod]
        public async Task UpdateGift_ShouldSaveIntoDatabase()
        {
            // Arrange
            using var dbContextInsert = new ApplicationDbContext(Options);
            IGiftService service = new GiftService(dbContextInsert, Mapper);

            var gift1 = SampleData.CreateGift1();
            var gift2 = SampleData.CreateGift2();

            await service.InsertAsync(gift1);
            await service.InsertAsync(gift2);

            // Act
            using var dbContextFetch = new ApplicationDbContext(Options);
            Gift gift1FromDb = await dbContextFetch.Gifts.SingleAsync(item => item.Id == gift1.Id);
            const string newTitle = "New Title";
            gift1FromDb.Title = newTitle;

            // Update Gift1 using the gift2 Id.
            await service.UpdateAsync(gift2.Id!.Value, gift1FromDb);

            // Assert
            using var dbContextAssert = new ApplicationDbContext(Options);
            gift1FromDb = await dbContextAssert.Gifts.SingleAsync(item => item.Id == gift1.Id);
            var gift2FromDb = await dbContextAssert.Gifts.SingleAsync(item => item.Id == 2);

            Assert.AreEqual(newTitle ,gift2FromDb.Title);
        }
        //Test FetchAll
        [TestMethod]
        public async Task FetchAll_ShouldReturnAllGifts()
        {
            //Arrange 
            using var dbContext = new ApplicationDbContext(Options);
            IGiftService service = new GiftService(dbContext, Mapper);

            var gift1 = SampleData.CreateGift1();
            var gift2 = SampleData.CreateGift2();
            await service.InsertAsync(gift1);
            await service.InsertAsync(gift2);
            //Act
            List<Gift> gifts = await service.FetchAllAsync();
            //Assert
            Assert.AreEqual(gift1.Title, gifts[0].Title);
            Assert.AreEqual(gift2.Title, gifts[1].Title);
        }
        //Test InsertList
        [TestMethod]
        public async Task InsertList_ShouldPutUsersInDb()
        {
            // Arrange
            using var dbContextInsert = new ApplicationDbContext(Options);
            IGiftService service = new GiftService(dbContextInsert, Mapper);
            var gift1 = SampleData.CreateGift1();
            var gift2 = SampleData.CreateGift2();
            //Act
            await service.InsertAsync(gift1, gift2);
            Gift gift1FromDb = await dbContextInsert.Gifts.SingleAsync(item => item.Id == gift1.Id);
            Gift gift2FromDb = await dbContextInsert.Gifts.SingleAsync(item => item.Id == 2);
            //Assert
            Assert.AreEqual((gift1.Title,gift2.Title),(gift1FromDb.Title,gift2FromDb.Title));
        }
        //Test FetchById
        [TestMethod]
        public async Task FetchById_ShouldReturnSuccess()
        {
            // Arrange
            using var dbContextInsert = new ApplicationDbContext(Options);
            IGiftService service = new GiftService(dbContextInsert, Mapper);
            var gift1 = SampleData.CreateGift1();
            var gift2 = SampleData.CreateGift2();
            //Act
            await service.InsertAsync(gift1, gift2);
            Gift gift1FromDb = await dbContextInsert.Gifts.SingleAsync(item => item.Id == gift1.Id);
            Gift gift2FromDb = await dbContextInsert.Gifts.SingleAsync(item => item.Id == 2);
            Gift gift1FromService = await service.FetchByIdAsync(1);
            Gift gift2FromService = await service.FetchByIdAsync(2);
            //Assert
            Assert.AreEqual((gift1FromDb.Title, gift2FromDb.Title),
                (gift1FromService.Title,gift2FromService.Title));
        }
        //Test Delete
        [TestMethod]
        public async Task Delete_ShouldReturnEmpty()
        {
            // Arrange
            using var dbContextInsert = new ApplicationDbContext(Options);
            IGiftService service = new GiftService(dbContextInsert, Mapper);
            var gift = SampleData.CreateGift1();
            //Act
            await service.InsertAsync(gift);
            Gift giftFromDb = await dbContextInsert.Gifts.SingleAsync(item => item.Id == gift.Id);
            _ = await service.DeleteAsync(giftFromDb.Id!.Value);
            List<Gift> gifts = await service.FetchAllAsync();
            //Assert
            Assert.AreEqual(0, gifts.Count);
        }
        //Test User is still contained within DB
        [TestMethod]
        public async Task AddGiftWithUser_UserReturnsWithGift()
        {
            // Arrange
            using var dbContextInsert = new ApplicationDbContext(Options);
            IGiftService service = new GiftService(dbContextInsert, Mapper);
            var gift = SampleData.CreateGift1();
            //Act
            await service.InsertAsync(gift);
            Gift giftFromDb = await service.FetchByIdAsync(gift.Id!.Value);
            //Assert
            Assert.IsNotNull(giftFromDb.User);
            Assert.AreEqual(gift.User, giftFromDb.User);
        }
    }
}
