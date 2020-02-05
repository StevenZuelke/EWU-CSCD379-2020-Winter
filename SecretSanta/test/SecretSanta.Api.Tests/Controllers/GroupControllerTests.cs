using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Business.Services;
using SecretSanta.Business.Dto;
using SecretSanta.Data;
using System;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class GroupControllerTests : BaseApiControllerTests<Business.Dto.Group,GroupInput, Data.Group>
    {
        protected override Data.Group CreateEntity()
        {
            return new Data.Group(Guid.NewGuid().ToString());
        }
        protected override Business.Dto.Group CreateDto()
        {
            return new Business.Dto.Group
            {
                Title = Guid.NewGuid().ToString()
            };
        }
        protected override Business.Dto.GroupInput CreateInput()
        {
            return new Business.Dto.GroupInput
            {
                Title = Guid.NewGuid().ToString()
            };
        }
    }


    //public class GroupInMemoryService : InMemoryEntityService<Group>, IGroupService
    //{
    //
    //}
}
