
using Microsoft.EntityFrameworkCore;
namespace WebApplication8.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }   
        public DbSet<User> Users { get; set; }

        
    }
}
