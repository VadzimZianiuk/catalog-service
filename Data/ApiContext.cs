using CatalogService.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.DBContext
{
    public class ApiContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }

        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options) { }
    }
}
