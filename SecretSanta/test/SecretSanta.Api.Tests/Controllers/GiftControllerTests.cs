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
            var service = new Mock<IGiftService>();

            //Act
            _ = new GiftController(service.Object);

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
            ActionResult<Gift> result = await controller.Get(gift.Id);
            OkObjectResult okResult = (OkObjectResult)result.Result;
            Gift giftResult = (Gift)okResult.Value;
            // Assert
            Assert.AreEqual(gift.Description, giftResult.Description);
            Assert.IsTrue(result.Result is OkObjectResult);
        }

        [TestMethod]
        public async Task GetById_WithoutGifts_Fail()
        {
            // Arrange
            var service = new Mock<IGiftService>();
            int id = 0;
#nullable disable //Well aware that Gift can't be null this part is suppposed to cause failure
            _ = service.Setup(m => m.FetchByIdAsync(id)).ReturnsAsync((Gift)null);
#nullable enable
            var controller = new GiftController(service.Object);
            // Act
            ActionResult<Gift> result = await controller.Get(id);
            // Assert
            Assert.IsTrue(result.Result is NotFoundResult);
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
            service.Setup(m => m.UpdateAsync(id, gift)).ReturnsAsync(PutHelper(id, gift));
            service.Setup(m => m.FetchByIdAsync(id)).ReturnsAsync(FetchHelper(id));
            var controller = new GiftController(service.Object);
            // Act
            ActionResult<Gift> result = await controller.Put(id, gift);
            OkObjectResult okResult = (OkObjectResult)result.Result;
            Gift giftResult = (Gift)okResult.Value;
            // Assert
            Assert.IsTrue(result.Result is OkObjectResult);
            Assert.AreEqual(giftResult.Id,id);
        }

        [TestMethod]
        public async Task Put_IndexOutofBounds_Fail()
        {
            // Arrange
            var service = new Mock<IGiftService>();
            int id = 2;
            Gift gift = SampleData.CreateGift1();
#nullable disable //Well aware that Gift can't be null this part is suppposed to cause failure
            _ = service.Setup(m => m.FetchByIdAsync(id)).ReturnsAsync((Gift)null);
#nullable enable
            service.Setup(m => m.UpdateAsync(id, gift)).ReturnsAsync(PutHelper(id, gift));
            var controller = new GiftController(service.Object);
            // Act
            ActionResult<Gift> result = await controller.Put(id,gift);
            // Assert
            Assert.IsTrue(result.Result is NotFoundResult);
        }

        [TestMethod]
        public async Task Delete_Gift_Success()
        {
            // Arrange
            var service = new Mock<IGiftService>();
            int id = 2;
            service.Setup(m => m.DeleteAsync(id)).ReturnsAsync(true);
            service.Setup(m => m.FetchByIdAsync(id)).ReturnsAsync(FetchHelper(id));
            var controller = new GiftController(service.Object);
            // Act
            ActionResult<bool> result = await controller.Delete(id);
            OkObjectResult okResult = (OkObjectResult)result.Result;
            bool boolResult = (bool)okResult.Value;
            // Assert
            Assert.IsTrue(result.Result is OkObjectResult);
            Assert.AreEqual(boolResult, true);
        }

        [TestMethod]
        public async Task Delete_GiftOutOfBounds_Fail()
        {
            // Arrange
            var service = new Mock<IGiftService>();
            int id = 2;
            service.Setup(m => m.DeleteAsync(id)).ReturnsAsync(true);
#nullable disable //Well aware that Gift can't be null this part is suppposed to cause failure
            _ = service.Setup(m => m.FetchByIdAsync(id)).ReturnsAsync((Gift)null);
#nullable enable
            var controller = new GiftController(service.Object);
            // Act
            ActionResult<bool> result = await controller.Delete(id);
            // Assert
            Assert.IsTrue(result.Result is NotFoundResult);
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
            Gift newGift = SampleData.CreateGift1();
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

    
    public class TestGift : Gift
    {
        public TestGift(Gift gift, int id)
            : base((gift?? throw new ArgumentNullException(nameof(gift))).Title,  gift.Url, gift.Description, gift.User)
        {
            Id = id;
        }
    }
    
}
