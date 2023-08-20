using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class Lab2Context : IdentityDbContext<User>
    {
        public DbSet<User> Lab2Users { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Supplier> Suppliers { get; set; } = null!;

        public Lab2Context(DbContextOptions<Lab2Context> options)
            : base(options) { }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        var connectionString = new ConfigurationBuilder()
        //            .AddJsonFile("appsettings.json")
        //            .Build()["Lab2Db"];
        //        optionsBuilder.UseSqlServer(connectionString);
        //    }
        //}
    }
}
