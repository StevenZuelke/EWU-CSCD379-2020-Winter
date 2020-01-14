using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework.Internal;
using SecretSanta.Business;
using System.Collections.Generic;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class GiftTests
    {
        [TestMethod]
        public void Create_Gift_Success()
            {
                //arrange
                int id = 0;
                string title = "title";
                string desc = "description";
                string url = "URL";
                User user = new User(1, "fname", "lname");
                //act
                Gift gift = new Gift(id, title, desc, url, user);
                //assert
                Assert.AreEqual<int>(id, gift.Id, "id value is unexpected");
                Assert.AreEqual<string>(title, gift.Title, "title value is unexpected");
                Assert.AreEqual<string>(desc, gift.Description, "description value is unexpected");
                Assert.AreEqual<string>(url, gift.Url, "url value is unexpected");
                Assert.AreEqual<User>(user, gift.User, "User value is unexpected");
            }
        
    }
}
