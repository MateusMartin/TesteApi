using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TesteApi.Models;

namespace TesteApi
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
