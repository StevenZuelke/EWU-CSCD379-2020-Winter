using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Business
{
    public class UserService : EntityService<User>, IUserService
    {
        public UserService(ApplicationDbContext applicationDbContext, IMapper mapper) :
            base(applicationDbContext, mapper)
        {
        }
        //Fetch by ID
        public override async Task<User> FetchByIdAsync(int id)
        {
            return await ApplicationDbContext.Set<User>().SingleAsync(item => item.Id == id);
        }
        //Fetch All
        public override async Task<List<User>> FetchAllAsync()
        {
            return await ApplicationDbContext.Set<User>().ToListAsync();
        }
        //Insert 
        public override async Task<User> InsertAsync(User entity)
        {
            await InsertAsync(new[] { entity });
            return entity;
        }
        //Insert List
        public override async Task<User[]> InsertAsync(params User[] entities)
        {
            foreach (User entity in entities)
            {
                ApplicationDbContext.Set<User>().Add(entity);
                await ApplicationDbContext.SaveChangesAsync();
            }
            return entities;
        }
        //Update
        public override async Task<User> UpdateAsync(int id, User entity)
        {
            User result = await ApplicationDbContext.Set<User>().SingleAsync(item => item.Id == id);
            Mapper.Map(entity, result);
            await ApplicationDbContext.SaveChangesAsync();
            return result;
        }
        //Delete
        public override async Task<bool> DeleteAsync(int id)
        {
            User user = await ApplicationDbContext.Set<User>().SingleAsync(item => item.Id == id);
            ApplicationDbContext.Remove<User>(user);
            await ApplicationDbContext.SaveChangesAsync();
            return true;
        }
    }
}
