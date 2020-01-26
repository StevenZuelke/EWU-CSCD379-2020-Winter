using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Business
{
    public class GroupService : EntityService<Group>, IGroupService
    {
        public GroupService(ApplicationDbContext applicationDbContext, IMapper mapper) :
            base(applicationDbContext, mapper)
        {
        }
        //Fetch by ID
        public override async Task<Group> FetchByIdAsync(int id)
        {
            return await ApplicationDbContext.Set<Group>().SingleAsync(item => item.Id == id);
        }
        //Fetch All
        public override async Task<List<Group>> FetchAllAsync()
        {
            return await ApplicationDbContext.Set<Group>().ToListAsync();
        }
        //Insert 
        public override async Task<Group> InsertAsync(Group entity)
        {
            await InsertAsync(new[] { entity });
            return entity;
        }
        //Insert List
        public override async Task<Group[]> InsertAsync(params Group[] entities)
        {
            foreach (Group entity in entities)
            {
                ApplicationDbContext.Set<Group>().Add(entity);
                await ApplicationDbContext.SaveChangesAsync();
            }
            return entities;
        }
        //Update
        public override async Task<Group> UpdateAsync(int id, Group entity)
        {
            Group result = await ApplicationDbContext.Set<Group>().SingleAsync(item => item.Id == id);
            Mapper.Map(entity, result);
            await ApplicationDbContext.SaveChangesAsync();
            return result;
        }
        //Delete
        public override async Task<bool> DeleteAsync(int id)
        {
            Group group = await ApplicationDbContext.Set<Group>().SingleAsync(item => item.Id == id);
            ApplicationDbContext.Remove<Group>(group);
            await ApplicationDbContext.SaveChangesAsync();
            return true;
        }
    }
}
