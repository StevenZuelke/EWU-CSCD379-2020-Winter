using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Data
{
    public class ApplicationDbContext:DbContext
    {
        //DbSets for user, gift, and group
        public DbSet<User> Users { get; set; }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<Group> Groups { get; set; }
        //Member variable for IHttpContextAccessor
        public IHttpContextAccessor HttpContextAccessor { get; set; }
        //Constructor with only options
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        //Constructor with both options and httpAccessor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }
    }
}
