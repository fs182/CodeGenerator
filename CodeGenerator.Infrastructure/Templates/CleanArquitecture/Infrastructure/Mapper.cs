using CodeGenerator.Infrastructure.Context.Models;
using System.Text;

namespace CodeGenerator.Infrastructure.Templates.CleanArquitecture.Infrastructure
{
    public static class Mapper
    {
        public static void WriteMappers(Project project, Table table)
        {
            if (!Directory.Exists(Path.Combine(project.InfrastructureMappersPath)))
                Directory.CreateDirectory(Path.Combine(project.InfrastructureMappersPath));
            using StreamWriter outputFile = new(Path.Combine(project.InfrastructureMappersPath, string.Concat(table.TableName, "Mapper.cs")), false, Encoding.UTF8);

            int count = 0;
            var fkCount = table.Columns.Where(f => f.IsForeignKey && f.ColumnName != "AuditoriaId").Count();
            var foreingColumnsResult = "";
            var foreingColumnsF = "";
            foreach (var c in table.Columns.Where(f => f.IsForeignKey && f.ColumnName != "AuditoriaId"))
            {
                count++;
                var fkTableInfo = project.Tables.First(f => f.TableName == c.TableTarget);
                var fkColumnsInfo = fkTableInfo.Columns;
                var fkColumnInfoPk = fkColumnsInfo.First(f => f.IsPrimaryKey);
                var namedColumnInfo = fkColumnsInfo.FirstOrDefault(f => f.ColumnName.ToLower().Contains("nombre") || (f.ColumnName.ToLower().Contains("descripcion") && !f.ColumnName.ToLower().Contains("descripcionid")) || f.ColumnName.ToLower().Contains("codigo"));
                if (namedColumnInfo != null)
                {
                    foreingColumnsResult += string.Concat($"{fkTableInfo.TableName}{namedColumnInfo.ColumnName} = result.{fkTableInfo.TableName}{namedColumnInfo.ColumnName}, ");
                    foreingColumnsF += string.Concat($"{fkTableInfo.TableName}{namedColumnInfo.ColumnName} = f.{fkTableInfo.TableName}{namedColumnInfo.ColumnName}, ");
                }

            }

            outputFile.WriteLine(string.Concat("using ", project.Namespace, ".Application.Responses.", table.TableName, ";"));
            outputFile.WriteLine(string.Concat("using ", project.Namespace, ".Domain.Entities;"));
            outputFile.WriteLine(string.Concat("using ", project.Namespace, ".Infrastructure.Context.StoredProcedureResult.Commands;"));
            outputFile.WriteLine(string.Concat("using ", project.Namespace, ".Infrastructure.Context.StoredProcedureResult.Queries;"));
            outputFile.WriteLine(string.Concat("namespace ", project.Namespace, ".Infrastructure.Mappers"));
            outputFile.WriteLine(string.Concat("{"));
            outputFile.WriteLine(string.Concat("    public static class ", table.TableName, "Mapper"));
            outputFile.WriteLine(string.Concat("    {"));
            outputFile.WriteLine(string.Concat("        public static GetResponse Map(", table.TableName, "GetPaginatedResult result)"));
            outputFile.WriteLine(string.Concat("        {"));
            outputFile.Write(string.Concat("            return new GetResponse { "));
            foreach (var c in table.Columns)
                outputFile.Write(string.Concat(c.ColumnName, " = result.", c.ColumnName, ", "));
            outputFile.WriteLine(string.Concat(foreingColumnsResult, "NombreCortoUsuario = result.NombreCortoUsuario, FechaModificacion = result.FechaModificacion, TotalPages = 0 };"));
            outputFile.WriteLine(string.Concat("        }"));
            outputFile.WriteLine($"		public static List<CommandResponse> Map(List<{table.TableName}CommandResult> resultList, int rowsOfPages, int existingRows)");
            outputFile.WriteLine("        {");
            outputFile.Write(string.Concat("            return resultList.Select(f => new CommandResponse { "));
            foreach (var c in table.Columns)
                outputFile.Write(string.Concat(c.ColumnName, " = f.", c.ColumnName, ", "));
            outputFile.WriteLine(string.Concat(foreingColumnsF, "NombreCortoUsuario = f.NombreCortoUsuario, FechaModificacion = f.FechaModificacion, TotalPages = AdditionalFields.SetTotalPages(rowsOfPages, existingRows) }).ToList();"));
            outputFile.WriteLine("        }");
            outputFile.WriteLine(string.Concat("        public static List<GetResponse> Map(List<", table.TableName, "GetPaginatedResult> resultList, int rowsOfPages, int existingRows)"));
            outputFile.WriteLine(string.Concat("        {"));
            outputFile.Write(string.Concat("            return resultList.Select(f => new GetResponse { "));
            foreach (var c in table.Columns)
                outputFile.Write(string.Concat(c.ColumnName, " = f.", c.ColumnName, ", "));
            outputFile.WriteLine(string.Concat(foreingColumnsF, "NombreCortoUsuario = f.NombreCortoUsuario, FechaModificacion = f.FechaModificacion, TotalPages = AdditionalFields.SetTotalPages(rowsOfPages, existingRows) }).ToList();"));
            outputFile.WriteLine(string.Concat("        }"));
            outputFile.WriteLine(string.Concat("    }"));
            outputFile.WriteLine(string.Concat("}"));
            outputFile.Close();
            outputFile.Dispose();
        }
    }
}
