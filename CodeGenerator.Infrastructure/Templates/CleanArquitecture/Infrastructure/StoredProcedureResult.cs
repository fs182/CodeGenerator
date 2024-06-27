using CodeGenerator.Infrastructure.Context.Models;
using System.Text;


namespace CodeGenerator.Infrastructure.Templates.CleanArquitecture.Infrastructure
{
    public static class StoredProcedureResult
    {
        public static void WriteCommands(Project project, Table table)
        {
            if (!Directory.Exists(Path.Combine(project.InfrastructureStoredProceduresCommandsPath)))
                Directory.CreateDirectory(Path.Combine(project.InfrastructureStoredProceduresCommandsPath));
            using StreamWriter outputFile = new(Path.Combine(project.InfrastructureStoredProceduresCommandsPath, string.Concat(table.TableName, "CommandResult.cs")), false, Encoding.UTF8);
            outputFile.WriteLine(string.Concat("using ", project.Namespace, ".Infrastructure.Context.StoredProcedureResult.Queries;"));
            outputFile.WriteLine(string.Concat("namespace ", project.Namespace, ".Infrastructure.Context.StoredProcedureResult.Commands"));
            outputFile.WriteLine(string.Concat("{"));
            outputFile.WriteLine(string.Concat("    public class ", table.TableName, "CommandResult : ", table.TableName, "GetPaginatedResult"));
            outputFile.WriteLine(string.Concat("    {"));
            outputFile.WriteLine(string.Concat("    }"));
            outputFile.WriteLine(string.Concat("}"));
            outputFile.Close();
            outputFile.Dispose();
        }
        public static void WriteQueries(Project project, Table table)
        {
            if (!Directory.Exists(Path.Combine(project.InfrastructureStoredProceduresQueriesPath)))
                Directory.CreateDirectory(Path.Combine(project.InfrastructureStoredProceduresQueriesPath));
            using StreamWriter outputFile = new(Path.Combine(project.InfrastructureStoredProceduresQueriesPath, string.Concat(table.TableName, "GetPaginatedResult.cs")), false, Encoding.UTF8);
            outputFile.WriteLine(string.Concat("namespace ", project.Namespace, ".Infrastructure.Context.StoredProcedureResult.Queries"));
            outputFile.WriteLine(string.Concat("{"));
            outputFile.WriteLine(string.Concat("    public class ", table.TableName, "GetPaginatedResult"));
            outputFile.WriteLine(string.Concat("    {"));
            foreach (var c in table.Columns)
                outputFile.WriteLine(string.Concat("        public ", Helper.GetStringNetCoreType(c.SqlDataType), (c.IsNullable && c.SqlDataType.ToLower() != "varchar") ? "?" : "", " ", c.ColumnName, " { get; set; }"));
            outputFile.WriteLine(string.Concat("        public string NombreCortoUsuario { get; set; }"));
            outputFile.WriteLine(string.Concat("        public DateTime FechaModificacion { get; set; }"));

            foreach (var c in table.Columns.Where(f => f.IsForeignKey && f.ColumnName != "AuditoriaId"))
            {
                var fkTableInfo = project.Tables.First(f => f.TableName == c.TableTarget);
                var namedColumnInfo = fkTableInfo.Columns.FirstOrDefault(f => f.ColumnName.ToLower().Contains("nombre") || (f.ColumnName.ToLower().Contains("descripcion") && !f.ColumnName.ToLower().Contains("descripcionid")) || f.ColumnName.ToLower().Contains("codigo"));
                if (namedColumnInfo != null)
                {
                    outputFile.WriteLine(string.Concat("        public ", Helper.GetStringNetCoreType(namedColumnInfo.SqlDataType), (c.IsNullable && c.SqlDataType.ToLower() != "varchar") ? "?" : "", " ", fkTableInfo.TableName, namedColumnInfo.ColumnName, " { get; set; }"));
                }
            }
            outputFile.WriteLine(string.Concat("    }"));
            outputFile.WriteLine(string.Concat("}"));
            outputFile.Close();
            outputFile.Dispose();
        }
    }
}
