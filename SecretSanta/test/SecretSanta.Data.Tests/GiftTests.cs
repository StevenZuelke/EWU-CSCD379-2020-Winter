using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Data.Tests
{
    //REFACTORED IN SAMPLEDATA
    [TestClass]
    public class GiftTests : TestBase
    {
        //C for Create and 
        //R for Retrieve
        [TestMethod]
        public async Task Gift_CanBeSavedToDatabase()
        {
            // Arrange
            using (var dbContext = new ApplicationDbContext(Options))
            {
                dbContext.Gifts.Add(SampleData.CreateGift1());
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            // Act
            // Assert
            using (var dbContext = new ApplicationDbContext(Options))
            {
                var gifts = await dbContext.Gifts.ToListAsync();

                Assert.AreEqual(1, gifts.Count);
                Assert.AreEqual(SampleData.Title1, gifts[0].Title);
                Assert.AreEqual(SampleData.Url1, gifts[0].Url);
                Assert.AreEqual(SampleData.Desc1, gifts[0].Description);
            }
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Gift_SetTitleToNull_ThrowsArgumentNullException()
        {
            _ = new Gift
            (
                null!, "description", "url", new User("firstName", "lastName")
            );
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Gift_SetDescriptionToNull_ThrowsArgumentNullException()
        {
            _ = new Gift
            (
                "title", null!, "url", new User("firstName", "lastName")
            );
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Gift_SetUrlToNull_ThrowsArgumentNullException()
        {
            _ = new Gift
            (
               "title", "description", null!, new User("firstname", "lastName")
            );
        }

        //U for Update
        //Assuming if can update title, then can update the other strings.
        [TestMethod]
        public async Task Gift_CanUpdateTitle()
        {
            // Arrange
            using (var dbContext = new ApplicationDbContext(Options))
            {
                dbContext.Gifts.Add(SampleData.CreateGift1());
                await dbContext.SaveChangesAsync().ConfigureAwait(false);

            }
            // Act
            using (var dbContext = new ApplicationDbContext(Options))
            {
                //Change Title
                var gift = await dbContext.Gifts.SingleOrDefaultAsync();
                Assert.AreEqual(SampleData.Title1, gift.Title);
                Assert.AreEqual(SampleData.Desc1, gift.Description);
                Assert.AreEqual(SampleData.Url1, gift.Url);

                gift.Title = "New Title";
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            // Assert
            using (var dbContext = new ApplicationDbContext(Options))
            {
                var gift = await dbContext.Gifts.SingleOrDefaultAsync();
                Assert.AreEqual("New Title", gift.Title);
            }
        }
        [TestMethod]
        public async Task Gift_CanUpdateUser()
        {
            // Arrange
            using (var dbContext = new ApplicationDbContext(Options))
            {
                dbContext.Gifts.Add(SampleData.CreateGift1());
                await dbContext.SaveChangesAsync().ConfigureAwait(false);

            }
            // Act
            using (var dbContext = new ApplicationDbContext(Options))
            {
                //Change User
                var gift = await dbContext.Gifts.Include(g => g.User).SingleOrDefaultAsync();
                Assert.AreEqual(SampleData.Title1, gift.Title);
                Assert.AreEqual(SampleData.Desc1, gift.Description);
                Assert.AreEqual(SampleData.Url1, gift.Url);
                Assert.AreEqual(SampleData.Inigo, gift.User.FirstName);
                gift.User.FirstName = "New Name";
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            // Assert
            using (var dbContext = new ApplicationDbContext(Options))
            {
                var gift = await dbContext.Gifts.Include(g => g.User).SingleOrDefaultAsync();
                Assert.AreEqual("New Name", gift.User.FirstName);
            }
        }

        //D for Delete
        [TestMethod]
        public async Task Gift_CanBeDeleted()
        {
            // Arrange
            using (var dbContext = new ApplicationDbContext(Options))
            {
                dbContext.Gifts.Add(SampleData.CreateGift1());
                await dbContext.SaveChangesAsync().ConfigureAwait(false);

            }
            //Act
            using (var dbContext = new ApplicationDbContext(Options))
            {
                var gift = await dbContext.Gifts.SingleOrDefaultAsync();
                dbContext.Gifts.Remove(gift);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            //Assert
            using (var dbContext = new ApplicationDbContext(Options))
            {
                var gifts = await dbContext.Gifts.ToListAsync();
                Assert.AreEqual(0, gifts.Count);
            }
        }
    }
}
