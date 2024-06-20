using Microsoft.EntityFrameworkCore;

namespace CodeGenerator.Infrastructure.Context
{
    public class LocalContext : DbContext
    {
        public LocalContext(DbContextOptions<ExternalContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
