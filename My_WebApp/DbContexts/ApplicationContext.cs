using Microsoft.EntityFrameworkCore;
using My_WebApp.Models.Departments;
using My_WebApp.Models.Employees;
using My_WebApp.Models.Users;

namespace My_WebApp.DbContexts
{
    public class ApplicationContext:DbContext
    {
        public DbSet<User> Users { get; set; } = null;
        public DbSet<Department> Departments { get; set; } = null;
        public DbSet<Employee> Employees { get; set; } = null;
        public DbSet<Education> Educations { get; set; } = null;
        public DbSet<WorkExperience> WorkExperience { get; set; } = null;
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
