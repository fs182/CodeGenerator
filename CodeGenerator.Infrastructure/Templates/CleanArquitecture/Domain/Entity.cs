using CodeGenerator.Infrastructure.Context.Models;
using System.Diagnostics.Metrics;
using System.Text;

namespace CodeGenerator.Infrastructure.Templates.CleanArquitecture.Domain
{
    public static class Entity
    {
        public static void WriteEntities(Project project, Table table)
        {
            if (!Directory.Exists(Path.Combine(project.DomainEntitiesPath)))
                Directory.CreateDirectory(Path.Combine(project.DomainEntitiesPath));
            using StreamWriter outputFile = new(Path.Combine(project.DomainEntitiesPath, string.Concat(table.TableName, ".cs")), false, Encoding.UTF8);
            outputFile.WriteLine(string.Concat("namespace ", project.Namespace, ".Domain.Entities"));
            outputFile.WriteLine("{");
            outputFile.WriteLine(string.Concat("    public partial class ", table.TableName, " : AdditionalFields"));
            outputFile.WriteLine("    {");
            foreach (var c in table.Columns.Where(f => f.ColumnName != "AuditoriaId"))
            {
                outputFile.WriteLine(string.Concat("        public ", Helper.GetStringNetCoreType(c.SqlDataType), c.IsNullable ? "?" : "", " ", c.ColumnName, " { get; set; }"));
            }

            string prefixFk = "";
            int countFk = 0;
            foreach (var c in table.Columns.Where(f => f.IsForeignKey && f.ColumnName != "AuditoriaId"))
            {
                var fkTableInfo = project.Tables.First(f => f.TableName == c.TableTarget);
                var fkColumnsInfo = fkTableInfo.Columns;
                var namedColumnInfo = fkColumnsInfo.FirstOrDefault(f => f.ColumnName.ToLower().Contains("nombre") || (f.ColumnName.ToLower().Contains("descripcion") && !f.ColumnName.ToLower().Contains("descripcionid")) || f.ColumnName.ToLower().Contains("codigo"));

                if (table.Columns.Count(f => f.TableTarget == c.TableTarget) > 1)
                {
                    countFk++;
                    prefixFk = countFk.ToString();
                }

                if (namedColumnInfo != null)
                {
                    outputFile.WriteLine(string.Concat("        public ", Helper.GetStringNetCoreType(namedColumnInfo.SqlDataType), c.IsNullable ? "?" : "", " ", fkTableInfo.TableName, namedColumnInfo.ColumnName,prefixFk, " { get; set; }"));
                }
                prefixFk = "";
            }

            outputFile.WriteLine("    }");
            outputFile.WriteLine("}");
            outputFile.Close();
            outputFile.Dispose();
        }
    }
}
