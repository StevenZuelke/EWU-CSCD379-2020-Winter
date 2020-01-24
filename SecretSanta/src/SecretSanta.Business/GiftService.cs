using Microsoft.EntityFrameworkCore.ChangeTracking;
using SecretSanta.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Business
{
    public class GiftService
    {
        private ApplicationDbContext DbContext { get; }
        public GiftService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<Gift> InsertAsync(Gift gift)
        {
            EntityEntry<Gift> insertedEntity = DbContext.Gifts.Add(gift);
            await DbContext.SaveChangesAsync();
            return insertedEntity.Entity;
        }
        
    }
}
