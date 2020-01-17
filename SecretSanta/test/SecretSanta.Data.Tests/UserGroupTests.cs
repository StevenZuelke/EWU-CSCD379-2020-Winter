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


namespace SecretSanta.Data.Tests
{
    [TestClass]
    public class UserGroupTests:TestBase
    {
        [TestMethod]
        public async Task CreatePostWithManyTags()
        {
            // Arrange
            IHttpContextAccessor httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, "imontoya"));

            var gift = new Gift
            {
                Title = "My Title",
                 Description= "my-desc",
                Url = "UrlUrl.Url"
            };
            var user = new User
            {
                FirstName = "Inigo",
                LastName = "Montoya",
            };
            var group = new Group
            {
                Name = "GroupName"
            };
            var group2 = new Group
            {
                Name = "SecondGroupName"
            };

            // Act
            user.UserGroups = new List<UserGroup>();
            user.UserGroups.Add(new UserGroup { User = user, Group = group });
            user.UserGroups.Add(new UserGroup { User = user, Group = group2 });
            gift.User = user;
            using (ApplicationDbContext dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                dbContext.Gifts.Add(gift);
                await dbContext.SaveChangesAsync();
            }

            // Assert
            using (ApplicationDbContext dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var retrievedGift = await dbContext.Gifts.Where(p => p.Id == gift.Id)
                    .Include(p => p.User).ThenInclude(pt => pt.UserGroups)
                        .ThenInclude(at => at.Group).SingleOrDefaultAsync();

                Assert.IsNotNull(retrievedGift);
                Assert.AreEqual(2, retrievedGift.User.UserGroups.Count);
                Assert.IsNotNull(retrievedGift.User.UserGroups[0].Group);
                Assert.IsNotNull(retrievedGift.User.UserGroups[1].Group);
            }
        }
    }


}
