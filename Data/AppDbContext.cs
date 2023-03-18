using Microsoft.EntityFrameworkCore;
using dot.Models;

namespace dot.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExchangeRate>()
                .HasKey(e => e.Id);
        }


        public DbSet<ExchangeRate> ExchangeRates { get; set; }
    }
}
