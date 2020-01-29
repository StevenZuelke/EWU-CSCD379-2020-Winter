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
    public class GiftControllerTests
    {
        [TestMethod]
        public void Create_GiftController_Success()
        {
            //Arrange
            var service = new Mock<GiftService>();

            //Act
            _ = new Mock<GiftController>(service);

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_WithoutService_Fails()
        {
            //Arrange

            //Act
            _ = new GiftController(null!);

            //Assert
        }

        [TestMethod]
        public async Task GetById_WithExistingGift_Success()
        {
            // Arrange
            var service = new Mock<IGiftService>();
            Gift gift = SampleData.CreateGift1();

            service.Setup(m => m.InsertAsync(gift)).ReturnsAsync(gift);
            int idnum = gift.Id;
            service.Setup(m => m.FetchByIdAsync(idnum)).ReturnsAsync(FetchHelper(idnum));
           var controller = new GiftController(service.Object);
            await service.Object.InsertAsync(gift);
            // Act
            ActionResult<Gift> rv = await controller.Get(gift.Id);

            // Assert
            Assert.IsTrue(rv.Result is OkObjectResult);
        }

        [TestMethod]
        public async Task Post_Gift_Success()
        {
            // Arrange
            var service = new Mock<IGiftService>();
            Gift gift = SampleData.CreateGift1();
            service.Setup(m => m.InsertAsync(gift)).ReturnsAsync(gift);
            var controller = new GiftController(service.Object);
            // Act
            gift = await controller.Post(gift);
            // Assert
            Assert.IsNotNull(gift.Id);
        }

        [TestMethod]
        public async Task FetchAll_Gift_Success()
        {
            //Arrange
            var service = new Mock<IGiftService>();
            Gift gift = SampleData.CreateGift1();
            Gift gift2 = SampleData.CreateGift2();
            service.Setup(m => m.FetchAllAsync()).ReturnsAsync(FetchAllHelper(gift,gift2));
            var controller = new GiftController(service.Object);
            //Act
            List<Gift> gifts = (List<Gift>)await controller.Get();
            //Assert
            Assert.AreEqual(gifts.Count, 2);
        }
        
        [TestMethod]
        public async Task Put_Gift_Success()
        {
            // Arrange
            var service = new Mock<IGiftService>();
            Gift gift = SampleData.CreateGift1();
            int id = 2;
            service.Setup(m => m.UpdateAsync(id, gift)).ReturnsAsync(PutHelper(id,gift));
            var controller = new GiftController(service.Object);
            // Act
            ActionResult<Gift> ar = await controller.Put(id, gift);
            // Assert
            Assert.IsTrue(ar.Result is OkObjectResult);
            Gift newGift = ar.Value;
            Assert.AreEqual(newGift.Id,id);
        }
        //Update gift and return new Val
        private Gift PutHelper(int id, Gift gift)
        {
            Gift testGift = new TestGift(gift, id);
            return testGift;
        }
        //Create new gift by specified ID
        private Gift FetchHelper(int id)
        {
            Gift newGift = new Gift("Title", "Desc", "Url",SampleData.CreateInigoMontoya());
            Gift testGift = new TestGift(newGift, id);
            return testGift;
        }
        //Create gift list from every input gift
        private List<Gift> FetchAllHelper( params Gift[] gift)
        {
            List<Gift> gifts = new List<Gift>();
            foreach (Gift g in gift)
            {
                gifts.Add(g);
            }
            return gifts;
        }

    }

    /*
    public class GiftService : IGiftService
    {
        private Dictionary<int, Gift> Items { get; } = new Dictionary<int, Gift>();

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Gift>> FetchAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Gift?> FetchByIdAsync(int id)
        {
            if (Items.TryGetValue(id, out Gift? gift))
            {
                Task<Gift?> t1 = Task.FromResult<Gift?>(gift);
                return t1;
            }
            Task<Gift?> t2 = Task.FromResult<Gift?>(null);
            return t2;
        }

        public Task<Gift> InsertAsync(Gift entity)
        {
            int id = Items.Count + 1;
            Items[id] = new TestGift(entity, id);
            return Task.FromResult(Items[id]);
        }

        public Task<Gift[]> InsertAsync(params Gift[] entity)
        {
            throw new NotImplementedException();
        }

        public Task<Gift?> UpdateAsync(int id, Gift entity)
        {
            throw new NotImplementedException();
        }
    }*/
    public class TestGift : Gift
    {
        public TestGift(Gift gift, int id)
            : base(gift.Title, gift.Description, gift.Url, gift.User)
        {
            Id = id;
        }
    }
    
}
