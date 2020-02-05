using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Business;
using SecretSanta.Business.Dto;
using SecretSanta.Data;
using System;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests : BaseApiControllerTests<Business.Dto.User,UserInput,Data.User>
    {
        protected override Data.User CreateEntity()
        {
            return new Data.User(Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString());
        }
        protected override Business.Dto.User CreateDto()
        {
            return new Business.Dto.User
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString()
            };
        }
        protected override Business.Dto.UserInput CreateInput()
        {
            return new Business.Dto.UserInput
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString()
            };
        }
    }

    //public class UserInMemoryService : InMemoryEntityService<User>, IUserService
    //{
    //
    //}
}
