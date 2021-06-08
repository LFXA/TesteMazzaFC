using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesteMazzaAPI.Models;

namespace TesteMazzaAPI.Data
{
    public class TesteMazzaContext : DbContext
    {
        public TesteMazzaContext(DbContextOptions<TesteMazzaContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
    }
}
