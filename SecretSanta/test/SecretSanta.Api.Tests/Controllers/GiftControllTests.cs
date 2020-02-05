using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Business.Dto;
using SecretSanta.Business.Services;
using SecretSanta.Data;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class GiftControllTests : BaseApiControllerTests<Business.Dto.Gift,GiftInput, Data.Gift>
    {
        [TestMethod]
        public async Task Get_ReturnsGifts()
        {
            // Arrange
            using ApplicationDbContext context = Factory.GetDbContext();
            Data.Gift gift = CreateEntity();
            context.Gifts.Add(gift);
            context.SaveChanges();

            // Act
            HttpResponseMessage response = await Client.GetAsync("api/Gift");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            string jsonData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Business.Dto.Gift[] gifts =
                JsonSerializer.Deserialize<Business.Dto.Gift[]>(jsonData, options);
            Assert.AreEqual(16, gifts.Length);

            Assert.AreEqual(gift.Id, gifts[15].Id);
            Assert.AreEqual(gift.Title, gifts[15].Title);
            Assert.AreEqual(gift.Description, gifts[15].Description);
            Assert.AreEqual(gift.Url, gifts[15].Url);
        }
        
        [TestMethod]
        public async Task Get_IndexOutOfBounds_NotFound()
        {
            HttpResponseMessage response = await Client.GetAsync("api/Gift/42");
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Get_IndexInBounds_ReturnsGift()
        {
            // Arrange
            using ApplicationDbContext context = Factory.GetDbContext();
            Data.Gift gift = CreateEntity();
            context.Gifts.Add(gift);
            context.SaveChanges();

            // Act
            HttpResponseMessage response = await Client.GetAsync("api/Gift/16");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            string jsonData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Business.Dto.Gift giftReturned =
                JsonSerializer.Deserialize<Business.Dto.Gift>(jsonData, options);
            Assert.AreEqual(gift.Id, giftReturned.Id);
            Assert.AreEqual(gift.Title, giftReturned.Title);
            Assert.AreEqual(gift.Description, giftReturned.Description);
            Assert.AreEqual(gift.Url, giftReturned.Url);
        }
        [TestMethod]
        public async Task Put_WithMissingId_NotFound()
        {
            // Arrange
            Business.Dto.GiftInput gift = CreateInput();
            string jsonData = JsonSerializer.Serialize(gift);

            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await Client.PutAsync("api/Gift/42", stringContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Put_WithId_UpdatesGift()
        {
            // Arrange
            using ApplicationDbContext context = Factory.GetDbContext();
            Business.Dto.GiftInput giftIn = CreateInput();
            giftIn.Title += "changed";
            giftIn.Description += "changed";
            giftIn.Url += "changed";

            string jsonData = JsonSerializer.Serialize(giftIn);

            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await Client.PutAsync($"api/Gift/1", stringContent);

            // Assert
            response.EnsureSuccessStatusCode();
            string retunedJson = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Business.Dto.Gift returnedGift = JsonSerializer.Deserialize<Business.Dto.Gift>(retunedJson, options);

            // Assert that returnedGift matches im values
            Assert.AreEqual(giftIn.Title, returnedGift.Title);
            Assert.AreEqual(giftIn.Description, returnedGift.Description);
            Assert.AreEqual(giftIn.Url, returnedGift.Url);
            // Assert that returnedGift matches database value
            

        }

        [TestMethod]
        public async Task Delete_FirstElementGone()
        {
            //Arrange
            using ApplicationDbContext context = Factory.GetDbContext();
            HttpResponseMessage response = await Client.GetAsync("api/Gift");
            string jsonData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Business.Dto.Gift[] giftsBefore =
                JsonSerializer.Deserialize<Business.Dto.Gift[]>(jsonData, options);
            //Act
            HttpResponseMessage responseDelete = await Client.DeleteAsync($"api/Gift/1");
            HttpResponseMessage responseAfter = await Client.GetAsync("api/Gift");
            string jsonDataAfter = await responseAfter.Content.ReadAsStringAsync();
            var optionsAfter = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Business.Dto.Gift[] giftsAfter =
                JsonSerializer.Deserialize<Business.Dto.Gift[]>(jsonData, options);
            //Assert
            Assert.AreEqual(responseDelete.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(giftsAfter.Length+1,giftsBefore.Length);
            Assert.AreNotEqual(giftsAfter[0], giftsBefore[0]);
        }
        //protected override BaseApiController<Gift> CreateController(GiftInMemoryService service)
        //    => new GiftController(service);

        protected override Data.Gift CreateEntity()
        {
            return new Data.Gift(Guid.NewGuid().ToString(),
                           Guid.NewGuid().ToString(),
                           Guid.NewGuid().ToString(),
                           new Data.User(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));
        }
        protected override Business.Dto.Gift CreateDto()
        {
            return new Business.Dto.Gift
            {
                Title = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Url = Guid.NewGuid().ToString(),
                User = new Business.Dto.User
                {
                    FirstName = Guid.NewGuid().ToString(),
                    LastName = Guid.NewGuid().ToString()
                }
            };
        }
        protected override Business.Dto.GiftInput CreateInput()
        {
            return new GiftInput
            {
                Title = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Url = Guid.NewGuid().ToString(),
                User = new Business.Dto.User
                {
                    FirstName = Guid.NewGuid().ToString(),
                    LastName = Guid.NewGuid().ToString()
                }
            };
        }
    }

    //public class GiftInMemoryService : InMemoryEntityService<Gift>, IGiftService
    //{
    //
    //}
}
