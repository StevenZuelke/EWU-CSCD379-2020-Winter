using AutoMapper;
using SecretSanta.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Business;

namespace SecretSanta.Business.Tests
{
    //Chose to not refactor sample data for this class (Didn't think it applied)
    [TestClass]
    public class AutomapperProfileConfigurationTests
    {
        class MockGift : Gift
        {
            public MockGift(int id, string title, string description,
                string url) :
                base(title, description, url, new User("firstName", "lastName"))
            {
                base.Id = id;
            }
        }

        [TestMethod]
        public void Map_Gift_SuccessWithNoIdMapped()
        {
            (Gift source, Gift target) = (
                new MockGift(42, "Title", "Description", "Url"), new MockGift(0, "Invalid", "Invalid", "Invalid"));
            IMapper mapper = AutomapperProfileConfiguration.CreateMapper();
            mapper.Map(source, target);
            Assert.AreNotEqual<int?>(source.Id, target.Id);
            Assert.AreEqual<string>(source.Title, target.Title);
        }
    }
}