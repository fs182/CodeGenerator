using CodeGenerator.Infrastructure.Context.Models;
using System.Text;

namespace CodeGenerator.Infrastructure.Templates.CleanArquitecture.Infrastructure
{
    public static class Context
    {
        public static void WriteCommandContext(Project project)
        {
            if (!Directory.Exists(Path.Combine(project.InfrastructureContextPath)))
                Directory.CreateDirectory(Path.Combine(project.InfrastructureContextPath));
            using StreamWriter outputFile = new(Path.Combine(project.InfrastructureContextPath, string.Concat("CatalogCommandContext.cs")), false, Encoding.UTF8);
            outputFile.WriteLine(string.Concat("using ", project.Namespace, ".Infrastructure.Context.StoredProcedureResult.Commands;"));
            outputFile.WriteLine(string.Concat("using Microsoft.EntityFrameworkCore;"));
            outputFile.WriteLine(string.Concat("namespace ", project.Namespace, ".Infrastructure.Context"));
            outputFile.WriteLine(string.Concat("{"));
            outputFile.WriteLine(string.Concat("    public class CatalogCommandContext : DbContext"));
            outputFile.WriteLine(string.Concat("    {"));
            outputFile.WriteLine(string.Concat("        public CatalogCommandContext(DbContextOptions<CatalogCommandContext> options) : base(options)"));
            outputFile.WriteLine(string.Concat("        {"));
            outputFile.WriteLine(string.Concat("        }"));
            outputFile.WriteLine(string.Concat("        protected override void OnModelCreating(ModelBuilder modelBuilder)"));
            outputFile.WriteLine(string.Concat("        {"));
            foreach (var entity in project.Tables.Where(f=>f.Catalog.IsEnabled))
                outputFile.WriteLine(string.Concat("            modelBuilder.Entity<", entity.TableName, "CommandResult>().HasNoKey();"));

            outputFile.WriteLine(string.Concat("        }"));
            foreach (var entity in project.Tables.Where(f => f.Catalog.IsEnabled))
                outputFile.WriteLine(string.Concat("        public DbSet<", entity.TableName, "CommandResult> ", entity.TableName, "s { get; set; }"));
            outputFile.WriteLine(string.Concat("    }"));
            outputFile.WriteLine(string.Concat("}"));
            outputFile.Close();
            outputFile.Dispose();
        }

        public static void WriteQueryContext(Project project)
        {
            if (!Directory.Exists(Path.Combine(project.InfrastructureContextPath)))
                Directory.CreateDirectory(Path.Combine(project.InfrastructureContextPath));
            using StreamWriter outputFile = new(Path.Combine(project.InfrastructureContextPath, string.Concat("CatalogQueryContext.cs")), false, Encoding.UTF8);
            outputFile.WriteLine(string.Concat("using ", project.Namespace, ".Infrastructure.Context.StoredProcedureResult.Queries;"));
            outputFile.WriteLine(string.Concat("using Microsoft.EntityFrameworkCore;"));
            outputFile.WriteLine(string.Concat("namespace ", project.Namespace, ".Infrastructure.Context"));
            outputFile.WriteLine(string.Concat("{"));
            outputFile.WriteLine(string.Concat("    public class CatalogQueryContext :DbContext"));
            outputFile.WriteLine(string.Concat("    {"));
            outputFile.WriteLine(string.Concat("        public CatalogQueryContext(DbContextOptions<CatalogQueryContext> options) : base(options)"));
            outputFile.WriteLine(string.Concat("        {"));
            outputFile.WriteLine(string.Concat("        }"));
            outputFile.WriteLine(string.Concat("        protected override void OnModelCreating(ModelBuilder modelBuilder)"));
            outputFile.WriteLine(string.Concat("        {"));
            foreach (var entity in project.Tables.Where(f => f.Catalog.IsEnabled))
                outputFile.WriteLine(string.Concat("            modelBuilder.Entity<", entity.TableName, "GetPaginatedResult>().HasNoKey();"));
            outputFile.WriteLine(string.Concat("        }"));
            foreach (var entity in project.Tables.Where(f => f.Catalog.IsEnabled))
                outputFile.WriteLine(string.Concat("        public DbSet<", entity.TableName, "GetPaginatedResult> ", entity.TableName, "s { get; set; }"));
            outputFile.WriteLine(string.Concat("    }"));
            outputFile.WriteLine(string.Concat("}"));
            outputFile.Close(); 
            outputFile.Dispose();
        }
    }
}
