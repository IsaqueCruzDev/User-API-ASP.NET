using BukiApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BukiApi.Data
{
    public class ApplicationDbContext : DbContext
    {
       public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }
}
