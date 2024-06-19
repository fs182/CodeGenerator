using Microsoft.EntityFrameworkCore;

namespace CodeGenerator.Infrastructure.Context
{
    public class ExternalContext : DbContext
    {
        public ExternalContext(DbContextOptions<ExternalContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
