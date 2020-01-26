using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Business
{
    public class GiftService : EntityService<Gift>, IGiftService
    {
        public GiftService(ApplicationDbContext applicationDbContext, IMapper mapper) :
            base(applicationDbContext, mapper)
        {
        }
        //Fetch by ID
        public override async Task<Gift> FetchByIdAsync(int id)
        {
            return await ApplicationDbContext.Set<Gift>().Include(nameof(Gift.User)).SingleAsync(item => item.Id == id);
        }
        //Fetch All
        public override async Task<List<Gift>> FetchAllAsync()
        {
            return await ApplicationDbContext.Set<Gift>().Include(nameof(Gift.User)).ToListAsync();
        }
        //Insert 
        public override async Task<Gift> InsertAsync(Gift entity)
        {
            await InsertAsync(new[] { entity });
            return entity;
        }
        //Insert List
        public override async Task<Gift[]> InsertAsync(params Gift[] entities)
        {
            foreach(Gift entity in entities)
            {
                ApplicationDbContext.Set<Gift>().Add(entity);
                await ApplicationDbContext.SaveChangesAsync();
            }
            return entities;
        }
        //Update
        public override async Task<Gift> UpdateAsync(int id, Gift entity)
        {
            Gift result = await ApplicationDbContext.Set<Gift>().Include(nameof(Gift.User)).SingleAsync(item => item.Id == id);
            Mapper.Map(entity, result);
            await ApplicationDbContext.SaveChangesAsync();
            return result;
        }
        //Delete
        public override async Task<bool> DeleteAsync(int id)
        {
            Gift gift = await ApplicationDbContext.Set<Gift>().Include(nameof(Gift.User)).SingleAsync(item => item.Id == id);
            ApplicationDbContext.Remove<Gift>(gift);
            await ApplicationDbContext.SaveChangesAsync();
            return true;
        }
    }
}
