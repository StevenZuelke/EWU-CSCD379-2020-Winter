using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Business;
using SecretSanta.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using SecretSanta.Business.Services;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public abstract class BaseApiControllerTests<TDto,TInputDto,TEntity>
        where TInputDto : class
        where TDto : class, TInputDto
        where TEntity : EntityBase
        //where TService : InMemoryEntityService<TDto,TInputDto>, new()
    {
        protected abstract TEntity CreateEntity();
        protected abstract TDto CreateDto();
        protected abstract TInputDto CreateInput();
//These are both initialized inside the TestSetup() 
#nullable disable
        protected SecretSantaWebApplicationFactory Factory { get; set; }
        protected HttpClient Client { get; set; }
#nullable enable
        protected IMapper Mapper { get; } = AutomapperConfigurationProfile.CreateMapper();

        [TestInitialize]
        public void TestSetup()
        {
            Factory = new SecretSantaWebApplicationFactory();

            using ApplicationDbContext context = Factory.GetDbContext();
            context.Database.EnsureCreated();

            Client = Factory.CreateClient();

            SeedData();
            
        }
        private void SeedData()
        {
            using ApplicationDbContext context = Factory.GetDbContext();
            for(int i = 0; i < 15; i++)
            {
                TEntity entity = CreateEntity();
                context.Add(entity);
                context.SaveChanges();
                Console.WriteLine(context.Entry(entity).Entity.Id);
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Factory.Dispose();
        }

        
        /*
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_RequiresService()
        {
            new ThrowingController();
        }

        [TestMethod]
        public async Task Get_FetchesAllItems()
        {
            TService service = new TService();
            service.Items.Add(CreateEntity());
            service.Items.Add(CreateEntity());
            service.Items.Add(CreateEntity());

            BaseApiController<TDto,TInputDto> controller = CreateController(service);

            IEnumerable<TDto> items = await controller.Get();

            CollectionAssert.AreEqual(service.Items.ToList(), items.ToList());
        }

        [TestMethod]
        public async Task Get_WhenEntityDoesNotExist_ReturnsNotFound()
        {
            TService service = new TService();
            BaseApiController<TDto,TInputDto> controller = CreateController(service);

            IActionResult result = await controller.Get(1);

            Assert.IsTrue(result is NotFoundResult);
        }


        [TestMethod]
        public async Task Get_WhenEntityExists_ReturnsItem()
        {
            TService service = new TService();
            TDto entity = CreateEntity();
            service.Items.Add(entity);
            BaseApiController<TDto,TInputDto> controller = CreateController(service);

            IActionResult result = await controller.Get(entity.Id);

            var okResult = result as OkObjectResult;
            
            Assert.AreEqual(entity, okResult?.Value);
        }

        [TestMethod]
        public async Task Put_UpdatesItem()
        {
            TService service = new TService();
            TDto entity1 = CreateEntity();
            service.Items.Add(entity1);
            TDto entity2 = CreateEntity();
            BaseApiController<TDto,TInputDto> controller = CreateController(service);

            TDto? result = await controller.Put(entity1.Id, entity2);

            Assert.AreEqual(entity2, result);
            Assert.AreEqual(entity2, service.Items.Single());
        }

        [TestMethod]
        public async Task Post_InsertsItem()
        {
            TService service = new TService();
            TDto entity = CreateEntity();
            BaseApiController<TDto,TInputDto> controller = CreateController(service);

            TDto? result = await controller.Post(entity);

            Assert.AreEqual(entity, result);
            Assert.AreEqual(entity, service.Items.Single());
        }

        [TestMethod]
        public async Task Delete_WhenItemDoesNotExist_ReturnsNotFound()
        {
            TService service = new TService();
            BaseApiController<TDto,TInputDto> controller = CreateController(service);

            IActionResult result = await controller.Delete(1);

            Assert.IsTrue(result is NotFoundResult);
        }

        [TestMethod]
        public async Task Delete_WhenItemExists_ReturnsOk()
        {
            TService service = new TService();
            TDto entity = CreateEntity();
            service.Items.Add(entity);
            BaseApiController<TDto,TInputDto> controller = CreateController(service);

            IActionResult result = await controller.Delete(entity.Id);

            Assert.IsTrue(result is OkResult);
        }

        private class ThrowingController : BaseApiController<TDto,TInputDto>
        {
            public ThrowingController() : base(null!)
            { }
        }
        */
    }
    /*
    public class InMemoryEntityService<TDto,TInputDto> : IEntityService<TDto,TInputDto>
        where TDto : class, TInputDto
        where TInputDto : Business.Dto.EntityBase
    {
        public IList<TDto> Items { get; } = new List<TDto>();
        private IMapper Mapper { get; } = AutomapperConfigurationProfile.CreateMapper();

        public Task<bool> DeleteAsync(int id)
        {
            if (Items.FirstOrDefault(x => x.Id == id) is { } found)
            {
                return Task.FromResult(Items.Remove(found));
            }
            return Task.FromResult(false);
        }

        public Task<List<TDto>> FetchAllAsync()
        {
            return Task.FromResult(Items.ToList());
        }

        public Task<TDto> FetchByIdAsync(int id)
        {
            return Task.FromResult(Items.FirstOrDefault(x => x.Id == id));
        }

        public Task<TDto> InsertAsync(TInputDto entity)
        {
            TDto mappedEntity = Mapper.Map<TInputDto, TDto>(entity);
            mappedEntity.Id = Items.Count;
            Items.Add(mappedEntity);
            return Task.FromResult(mappedEntity);
        }

        public Task<TDto?> UpdateAsync(int id, TInputDto entity)
        {
            if (Items.FirstOrDefault(x => x.Id == id) is TDto found)
            {
                TDto mappedEntity = Mapper.Map<TInputDto, TDto>(entity);
                mappedEntity.Id = Items.IndexOf(found);
                Items[Items.IndexOf(found)] = mappedEntity;
                return Task.FromResult<TDto?>(mappedEntity);
            }
            return Task.FromResult(default(TDto));
        }
    }
    */
}
