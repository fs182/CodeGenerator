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
            modelBuilder.Entity<Project>();
            modelBuilder.Entity<Table>();
            modelBuilder.Entity<Column>();
            modelBuilder.Entity<Catalog>();
            modelBuilder.Entity<Property>();
        }

        
        public DbSet<TableTemp> TableTemps { get; set; }
        public DbSet<ColumnTemp> ColumnTemps { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<Property> Properties { get; set; }
    }
}
