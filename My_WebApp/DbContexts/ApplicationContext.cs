using Microsoft.EntityFrameworkCore;
using My_WebApp.Models.Users;

namespace My_WebApp.DbContexts
{
    public class ApplicationContext:DbContext
    {
        public DbSet<User> Users { get; set; } = null;
        public ApplicationContext()
        {
            //Database.EnsureCreated(); //????
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql("Host = localhost;Port=5432;Database=My_AppDb;Username=postgres;Password=12345");
        }
    }
}
