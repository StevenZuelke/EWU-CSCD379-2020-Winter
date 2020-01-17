using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace SecretSanta.Data.Tests
{
    [TestClass]
    public class GiftTests:TestBase
    {
        [TestMethod]
        public async Task AddGift_WithUser_ShouldCreateForeignRelationship()
        {
            var gift = new Gift
            {
                Title = "My Title",
                Description = "my-desc",
                Url = "UrlUrl.Url",
            };
            var user = new User
            {
                FirstName = "Inigo",
                LastName = "Montoya",
            };
            // Arrange
            using (ApplicationDbContext dbContext = new ApplicationDbContext(Options))
            {
                gift.User = user;

                dbContext.Gifts.Add(gift);

                await dbContext.SaveChangesAsync();
            }

            using (ApplicationDbContext dbContext = new ApplicationDbContext(Options))
            {
                var gifts = await dbContext.Gifts.Include(p => p.User).ToListAsync();
                //var posts = await dbContext.Posts.ToListAsync();
                Assert.AreEqual(1, gifts.Count);
                Assert.AreEqual(gift.Title, gifts[0].Title);
                Assert.AreNotEqual(0, gifts[0].Id);
                //Assert.IsNotNull(posts[0].Author);
            }
        }

    }
}
