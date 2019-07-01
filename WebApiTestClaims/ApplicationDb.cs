using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTestClaims
{
    public class ApplicationDb : DbContext
    {
        public ApplicationDb(DbContextOptions<ApplicationDb> options) : base (options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRoles> UserRoles { get;set; }
    }
}
