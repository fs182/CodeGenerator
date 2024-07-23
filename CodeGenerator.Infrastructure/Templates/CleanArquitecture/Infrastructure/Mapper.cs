using CodeGenerator.Infrastructure.Context.Models;
using System.Text;

namespace CodeGenerator.Infrastructure.Templates.CleanArquitecture.Infrastructure
{
    public static class Mapper
    {
        public static void WriteMappers(Project project)
        {
            if (!Directory.Exists(Path.Combine(project.InfrastructureMappersPath)))
                Directory.CreateDirectory(Path.Combine(project.InfrastructureMappersPath));
            using StreamWriter outputFile = new(Path.Combine(project.InfrastructureMappersPath, string.Concat("AutoMapperProfile.cs")), false, Encoding.UTF8);

            outputFile.WriteLine(string.Concat("using AutoMapper;"));
            outputFile.WriteLine(string.Concat("using ", project.Namespace, ".Infrastructure.Context.StoredProcedureResult.Commands;"));
            outputFile.WriteLine(string.Concat("using ", project.Namespace, ".Infrastructure.Context.StoredProcedureResult.Queries;"));
            outputFile.WriteLine(string.Concat("namespace ", project.Namespace, ".Infrastructure.Mappers"));
            outputFile.WriteLine(string.Concat("{"));
            outputFile.WriteLine(string.Concat("    public class AutoMapperProfile : Profile"));
            outputFile.WriteLine(string.Concat("    {"));
            outputFile.WriteLine(string.Concat("        public AutoMapperProfile()"));
            outputFile.WriteLine(string.Concat("        {"));

            foreach (var c in project.Tables.Where(f => f.Catalog.IsEnabled))
            {
                outputFile.WriteLine(string.Concat($"            CreateMap<{c.TableName}GetPaginatedResult, Application.Responses.{c.TableName}.GetResponse>();"));
                outputFile.WriteLine(string.Concat($"            CreateMap<{c.TableName}CommandResult, Application.Responses.{c.TableName}.CommandResponse>();"));
            }
            outputFile.WriteLine(string.Concat("        }"));
            outputFile.WriteLine(string.Concat("    }"));
            outputFile.WriteLine(string.Concat("}"));
            outputFile.Close();
            outputFile.Dispose();
        }
    }
}
