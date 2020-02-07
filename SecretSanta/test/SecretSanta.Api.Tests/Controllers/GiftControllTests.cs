using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Business.Dto;
using SecretSanta.Business.Services;
using SecretSanta.Data;
using System;
using System.Collections.Generic;
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
            var uri = new Uri("api/Gift", UriKind.RelativeOrAbsolute);
            HttpResponseMessage response = await Client.GetAsync(uri);

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
            Assert.AreEqual(gift.UserId, gifts[15].UserId);
        }
        
        [TestMethod]
        public async Task Get_IndexOutOfBounds_NotFound()
        {
            Uri uri = new Uri("api/Gift/42", UriKind.RelativeOrAbsolute);
            HttpResponseMessage response = await Client.GetAsync(uri);
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
            Uri uri = new Uri($"api/Gift/{gift.Id}", UriKind.RelativeOrAbsolute);
            HttpResponseMessage response = await Client.GetAsync(uri);

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
            Uri uri = new Uri("api/Gift/42", UriKind.RelativeOrAbsolute);
            HttpResponseMessage response = await Client.PutAsync(uri, stringContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }


        [TestMethod]
        public async Task Post_WithValid_AddsGift()
        {
            // Arrange
            using ApplicationDbContext context = Factory.GetDbContext();
            Business.Dto.GiftInput giftInput = CreateInput();
            string jsonData = JsonSerializer.Serialize(giftInput);

            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Act
            Uri uri = new Uri("api/Gift/", UriKind.RelativeOrAbsolute);
            HttpResponseMessage response = await Client.PostAsync(uri, stringContent);

            // Assert
            response.EnsureSuccessStatusCode();
            string retunedJson = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Business.Dto.Gift returnedGift = JsonSerializer.Deserialize<Business.Dto.Gift>(retunedJson, options);

            // Assert that returnedGift matches im values
            Assert.AreEqual(giftInput.Title, returnedGift.Title);
            Assert.AreEqual(giftInput.Description, returnedGift.Description);
            Assert.AreEqual(giftInput.Url, returnedGift.Url);
            Assert.AreEqual(giftInput.UserId, returnedGift.UserId);

            // Assert that returnedGift matches database value
            Data.Gift dataGift = await context.Gifts.FindAsync(returnedGift.Id);
            Assert.AreEqual(giftInput.Title, dataGift.Title);
            Assert.AreEqual(giftInput.Description, dataGift.Description);
            Assert.AreEqual(giftInput.Url, dataGift.Url);
            Assert.AreEqual(giftInput.UserId, dataGift.UserId);
        }


        [TestMethod]
        public async Task Post_WithInvalidGift_Fails()
        {
            // Arrange
            using ApplicationDbContext context = Factory.GetDbContext();
            Business.Dto.GiftInput giftInput = CreateInput();
            giftInput.Title = null;
            string jsonData = JsonSerializer.Serialize(giftInput);
            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Act
            Uri uri = new Uri($"api/Gift", UriKind.RelativeOrAbsolute);
            HttpResponseMessage response = await Client.PostAsync(uri, stringContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Put_WithId_UpdatesGift()
        {
            // Arrange
            using ApplicationDbContext context = Factory.GetDbContext();
            Business.Dto.GiftInput giftInput = CreateInput();
            Data.Gift entityIn = await context.Gifts.FirstOrDefaultAsync();
            giftInput.Title = entityIn.Title+="changed";
            giftInput.Description = entityIn.Description += "changed";
            giftInput.Url = entityIn.Url += "changed";
            
            string jsonData = JsonSerializer.Serialize(giftInput);

            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Act
            Uri uri = new Uri($"api/Gift/{entityIn.Id}", UriKind.RelativeOrAbsolute);
            HttpResponseMessage response = await Client.PutAsync(uri, stringContent);

            // Assert
            response.EnsureSuccessStatusCode();
            string retunedJson = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Business.Dto.Gift returnedGift = JsonSerializer.Deserialize<Business.Dto.Gift>(retunedJson, options);

            // Assert that returnedGift matches im values
            Assert.AreEqual(giftInput.Title, returnedGift.Title);
            Assert.AreEqual(giftInput.Description, returnedGift.Description);
            Assert.AreEqual(giftInput.Url, returnedGift.Url);
            Assert.AreEqual(giftInput.UserId, returnedGift.UserId);
           
            // Assert that returnedGift matches database value
            Data.Gift dataGift = await context.Gifts.FindAsync(entityIn.Id);
            Assert.AreEqual(giftInput.Title, dataGift.Title);
            Assert.AreEqual(giftInput.Description, dataGift.Description);
            Assert.AreEqual(giftInput.Url, dataGift.Url);
            Assert.AreEqual(giftInput.UserId, dataGift.UserId);
        }

        [TestMethod]
        public async Task Put_WithInvalidReplacement_Fails()
        {
            // Arrange
            using ApplicationDbContext context = Factory.GetDbContext();
            Business.Dto.GiftInput giftInput = CreateInput();
            giftInput.Title = null;

            string jsonData = JsonSerializer.Serialize(giftInput);

            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Act
            Uri uri = new Uri($"api/Gift/1", UriKind.RelativeOrAbsolute);
            HttpResponseMessage response = await Client.PutAsync(uri, stringContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Delete_ValidId_Success()
        {
            //Arrange
            using ApplicationDbContext context = Factory.GetDbContext();
            List<Data.Gift> giftsBefore = await context.Gifts.ToListAsync();

            //Act
            Uri uriDelete = new Uri($"api/Gift/{giftsBefore[0].Id}", UriKind.RelativeOrAbsolute);
            HttpResponseMessage responseDelete = await Client.DeleteAsync(uriDelete);
            
            using ApplicationDbContext contextAct = Factory.GetDbContext();
            List<Data.Gift> giftsAfter = await context.Gifts.ToListAsync();
            //Assert
            Assert.AreEqual(responseDelete.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(giftsAfter.Count+1,giftsBefore.Count);
            Assert.AreNotEqual(giftsAfter[0].Id, giftsBefore[0].Id);
        }

        [TestMethod]
        public async Task Delete_InvalidId_Fails()
        {
            //Arrange
            using ApplicationDbContext context = Factory.GetDbContext();
          
            //Act
            Uri uriDelete = new Uri("api/Gift/42", UriKind.RelativeOrAbsolute);
            HttpResponseMessage responseDelete = await Client.DeleteAsync(uriDelete);

            //Assert
            Assert.AreEqual(responseDelete.StatusCode, HttpStatusCode.NotFound);
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
                UserId = 1
            };
        }
        protected override Business.Dto.GiftInput CreateInput()
        {
            return new GiftInput
            {
                Title = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Url = Guid.NewGuid().ToString(),
                UserId = 1
            };
        }
    }

    //public class GiftInMemoryService : InMemoryEntityService<Gift>, IGiftService
    //{
    //
    //}
}
