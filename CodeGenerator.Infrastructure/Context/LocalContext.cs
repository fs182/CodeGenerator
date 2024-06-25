using CodeGenerator.Infrastructure.Context.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeGenerator.Infrastructure.Context
{
    public class LocalContext : DbContext
    {
        public LocalContext(DbContextOptions<LocalContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TableTemp>();
            modelBuilder.Entity<ColumnTemp>();
        }

        
        public DbSet<TableTemp> TableTemps { get; set; }
        public DbSet<ColumnTemp> ColumnTemps { get; set; }
    }
}
