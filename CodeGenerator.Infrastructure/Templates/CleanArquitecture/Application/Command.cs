using CodeGenerator.Infrastructure.Context.Models;
using System.Text;

namespace CodeGenerator.Infrastructure.Templates.CleanArquitecture.Application
{
    public static class Command
    {
        public static void WriteCreateCommand(Project project, Table table)
        {
            if (!Directory.Exists(Path.Combine(project.ApplicationCommandsPath, table.TableName)))
                Directory.CreateDirectory(Path.Combine(project.ApplicationCommandsPath, table.TableName));
            var pk = table.Columns.First(f => f.IsPrimaryKey);
            using StreamWriter outputFile = new(Path.Combine(project.ApplicationCommandsPath, table.TableName, string.Concat("CreateCommand.cs")), false, Encoding.UTF8);
            outputFile.WriteLine($"using {project.Namespace}.Application.Base;");
            outputFile.WriteLine($"using {project.Namespace}.Application.Responses.{table.TableName};");
            outputFile.WriteLine("using System.Text.Json.Serialization;");
            outputFile.WriteLine("using MediatR;");
            outputFile.WriteLine("");
            outputFile.WriteLine($"namespace {project.Namespace}.Application.Commands.{table.TableName}");
            outputFile.WriteLine("{");
            outputFile.WriteLine("    public class CreateCommand: IRequest<List<CommandResponse>>, IGetPaginated");
            outputFile.WriteLine("    {");
            foreach (var c in table.Columns.Where(f => f.ColumnName != "AuditoriaId").OrderBy(g => g.ColumnNumber))
                outputFile.WriteLine(string.Concat("        public ", Helper.GetStringNetCoreType(c.SqlDataType), c.IsNullable && Helper.GetStringNetCoreType(c.SqlDataType) != "string" ? "?" : "", " ", c.ColumnName, " { get; set; }"));           
            outputFile.WriteLine("        public int PageNumber { get; set; }");
            outputFile.WriteLine("        public int RowsOfPage { get; set; }");
            outputFile.WriteLine("        public long AuditoriaId { get; set; }");
            if(table.Columns.All(f=>f.ColumnName != "UsuarioId"))
                outputFile.WriteLine("        public int UsuarioId { get; set; }");
            outputFile.WriteLine("        [JsonIgnore]");
            outputFile.WriteLine("        public int ExistingRows { get; set; }");
            outputFile.WriteLine("    }");
            outputFile.WriteLine("}");
            outputFile.Close();
            outputFile.Dispose();
        }

        public static void WriteDeleteCommand(Project project, Table table)
        {
            if (!Directory.Exists(Path.Combine(project.ApplicationCommandsPath, table.TableName)))
                Directory.CreateDirectory(Path.Combine(project.ApplicationCommandsPath, table.TableName));
            var pk = table.Columns.First(f => f.IsPrimaryKey);
            using StreamWriter outputFile = new(Path.Combine(project.ApplicationCommandsPath, table.TableName, string.Concat("DeleteCommand.cs")), false, Encoding.UTF8);
            outputFile.WriteLine($"using {project.Namespace}.Application.Base;");
            outputFile.WriteLine($"using {project.Namespace}.Application.Responses.{table.TableName};");
            outputFile.WriteLine("using System.Text.Json.Serialization;");
            outputFile.WriteLine("using MediatR;");
            outputFile.WriteLine("");
            outputFile.WriteLine($"namespace {project.Namespace}.Application.Commands.{table.TableName}");
            outputFile.WriteLine("{");
            outputFile.WriteLine("    public class DeleteCommand: IRequest<List<CommandResponse>>, IGetPaginated");
            outputFile.WriteLine("    {");
            outputFile.WriteLine(string.Concat("        public ", Helper.GetStringNetCoreType(pk.SqlDataType), " ", pk.ColumnName, " { get; set; }"));
            outputFile.WriteLine("        public int PageNumber { get; set; }");
            outputFile.WriteLine("        public int RowsOfPage { get; set; }");
            outputFile.WriteLine("        public long AuditoriaId { get; set; }");
            if (pk.ColumnName != "UsuarioId")
                outputFile.WriteLine("        public int UsuarioId { get; set; }");
            outputFile.WriteLine("        [JsonIgnore]");
            outputFile.WriteLine("        public int ExistingRows { get; set; }");
            outputFile.WriteLine("    }");
            outputFile.WriteLine("}");
            outputFile.Close();
            outputFile.Dispose();
        }

        public static void WriteUpdateCommand(Project project, Table table)
        {
            if (!Directory.Exists(Path.Combine(project.ApplicationCommandsPath, table.TableName)))
                Directory.CreateDirectory(Path.Combine(project.ApplicationCommandsPath, table.TableName));
            var pk = table.Columns.First(f => f.IsPrimaryKey);
            using StreamWriter outputFile = new(Path.Combine(project.ApplicationCommandsPath, table.TableName, string.Concat("UpdateCommand.cs")), false, Encoding.UTF8);
            outputFile.WriteLine($"using {project.Namespace}.Application.Base;");
            outputFile.WriteLine($"using {project.Namespace}.Application.Responses.{table.TableName};");
            outputFile.WriteLine("using MediatR;");
            outputFile.WriteLine("using System.Text.Json.Serialization;");
            outputFile.WriteLine($"namespace {project.Namespace}.Application.Commands.{table.TableName}");
            outputFile.WriteLine("{");
            outputFile.WriteLine($"    public class UpdateCommand: Domain.Entities.{table.TableName}, IRequest<List<CommandResponse>>, IGetPaginated");
            outputFile.WriteLine("    {");
            outputFile.WriteLine("        public int PageNumber { get; set; }");
            outputFile.WriteLine("        public int RowsOfPage { get; set; }");
            if (table.Columns.All(f => f.ColumnName != "UsuarioId"))
                outputFile.WriteLine("        public int UsuarioId { get; set; }");
            outputFile.WriteLine("        [JsonIgnore]");
            outputFile.WriteLine("        public int ExistingRows { get; set; }");
            outputFile.WriteLine("    }");
            outputFile.WriteLine("}");
            outputFile.Close();
            outputFile.Dispose();
        }

        public static void WriteWizardCommand(Project project, Table table)
        {
            if (!Directory.Exists(Path.Combine(project.ApplicationCommandsPath, table.TableName)))
                Directory.CreateDirectory(Path.Combine(project.ApplicationCommandsPath, table.TableName));
            var pk = table.Columns.First(f => f.IsPrimaryKey);
            using StreamWriter outputFile = new(Path.Combine(project.ApplicationCommandsPath, table.TableName, string.Concat("WizardCommand.cs")), false, Encoding.UTF8);
            outputFile.WriteLine("using MediatR;");
            outputFile.WriteLine("");
            outputFile.WriteLine($"namespace {project.Namespace}.Application.Commands.{table.TableName}");
            outputFile.WriteLine("{");
            outputFile.WriteLine("    public class WizardCommand: IRequest<Unit>");
            outputFile.WriteLine("    {");
            foreach (var c in table.Columns.Where(f => f.ColumnName != "AuditoriaId").OrderBy(g => g.ColumnNumber))
                outputFile.WriteLine(string.Concat("        public ", Helper.GetStringNetCoreType(c.SqlDataType), c.IsNullable && Helper.GetStringNetCoreType(c.SqlDataType) != "string" ? "?" : "", " ", c.ColumnName, " { get; set; }"));
            outputFile.WriteLine("        public long AuditoriaId { get; set; }");
            if (table.Columns.All(f => f.ColumnName != "UsuarioId"))
                outputFile.WriteLine("        public int UsuarioId { get; set; }");
            foreach (var p in table.Catalog.RelatedProperties.Where(f=> f.IncludeInWizardCommand).OrderBy(f=>f.PropertyOrder))
            {
                var c = project.Tables.First(f => f.TableName == p.TableName).Columns.First(g => g.ColumnName == p.ColumnName);
                outputFile.WriteLine(string.Concat("        public ", Helper.GetStringNetCoreType(c.SqlDataType), c.IsNullable && Helper.GetStringNetCoreType(c.SqlDataType) != "string" ? "?" : "", " ", c.ColumnName, " { get; set; }"));
            }
                
            outputFile.WriteLine("    }");
            outputFile.WriteLine("}");
            outputFile.Close();
            outputFile.Dispose();
        }
    }
}
