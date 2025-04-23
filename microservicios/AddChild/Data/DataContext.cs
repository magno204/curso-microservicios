using Microsoft.EntityFrameworkCore;
using AddChild.Models;

namespace AddChild.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Child> Children { get; set; }
    }
}