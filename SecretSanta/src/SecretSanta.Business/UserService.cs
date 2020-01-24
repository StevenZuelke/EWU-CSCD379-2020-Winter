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

        public override async Task<User> FetchByIdAsync(int id) =>
            await ApplicationDbContext.Set<User>().SingleAsync(item => item.Id == id);
    }
}
