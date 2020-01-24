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

        public override async Task<Group> FetchByIdAsync(int id)
        {
            return await ApplicationDbContext.Set<Group>().SingleAsync(item => item.Id == id);
        }
    }
}
