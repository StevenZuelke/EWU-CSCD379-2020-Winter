using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework.Internal;
using SecretSanta.Business;
using System.Collections.Generic;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    class UserTests
    {
        [TestMethod]
        public void Create_User_Success()
        {
            //arrange
            int id = 0;
            string fname = "fname";
            string lname = "lname";
            IList<Gift> list = new List<Gift>
            {
                new Gift(0, "zero", "first", "url0", null),
                new Gift(1, "one", "second", "url1", null)
            };
            //act
            User user = new User(id, fname, lname, list);

            //assert
            Assert.AreEqual<int>(id, user.Id, "id value is unexpected");
            Assert.AreEqual<string>(fname, user.FirstName, "fname value is unexpected");
            Assert.AreEqual<string>(lname, user.LastName, "lname value is unexpected");
            Assert.AreEqual<IList<Gift>>(list, user.Gifts, "gift value is unexpected");
        }
    }
}
