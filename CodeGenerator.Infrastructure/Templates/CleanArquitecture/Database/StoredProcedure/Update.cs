using CodeGenerator.Infrastructure.Context.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace CodeGenerator.Infrastructure.Templates.CleanArquitecture.Database.StoredProcedure
{
    public static class Update
    {
        public static void WriteUpdateSP(Project project, Table table)
        {
            var pk = table.Columns.First(f => f.IsPrimaryKey);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("-- =============================================");
            sb.AppendLine(string.Concat("-- Author: ", project.Autor));
            //sb.AppendLine(string.Concat("-- Create date: ", DateTime.Now.ToLongDateString(), " - ", DateTime.Now.ToLongTimeString()));
            //sb.AppendLine(string.Concat("-- Create date: ", DateTime.Now.ToLongDateString()));
            sb.AppendLine(string.Concat("-- Description: Update ", table.TableName));
            sb.AppendLine("-- =============================================");
            sb.AppendLine(string.Concat("CREATE OR ALTER PROCEDURE ", table.SchemaName, ".[", project.StoredProceduresPrefix, table.TableName, "_", "Update]"));
            sb.AppendLine("\t@PageNumber int,");
            sb.AppendLine("\t@RowsOfPage int,");
            sb.AppendLine("\t@ExistingRows int OUTPUT,");
            foreach (var c in table.Columns.OrderBy(f=>f.ColumnNumber))
            {
                var nullable = c.IsNullable ? " = null" : "";
                if (Helper.GetStringNetCoreType(c.SqlDataType) == "string")
                    sb.AppendLine(string.Concat("\t", "@", c.ColumnName, " ", c.SqlDataType, "(", c.MaxLength == -1 ? "MAX" : c.MaxLength, ")", nullable, ","));
                else if (Helper.GetStringNetCoreType(c.SqlDataType) == "decimal" && c.SqlDataType != "money")
                    sb.AppendLine(string.Concat("\t", "@", c.ColumnName, " ", c.SqlDataType, "(", c.Precision,",",c.Scale, ")", nullable, ","));
                else
                    sb.AppendLine(string.Concat("\t", "@", c.ColumnName, " ", c.SqlDataType, "", nullable, ","));
            }
            sb.Length = sb.Length - 3;
            sb.AppendLine("");
            sb.AppendLine("AS");
            sb.AppendLine("BEGIN");
            sb.AppendLine("\tSET NOCOUNT ON;");

            sb.AppendLine(string.Concat("\tUPDATE ", table.TableName));
            sb.AppendLine("\tSET");
            foreach (var c in table.Columns.Where(f => !f.IsIdentity).OrderBy(g=>g.ColumnNumber))
            {
                sb.AppendLine(string.Concat("\t\t\t[", c.ColumnName, "] = @", c.ColumnName, ","));
            }
            sb.Length = sb.Length - 3;
            sb.AppendLine("");
            sb.AppendLine(string.Concat("\tWHERE ", pk.ColumnName, " = @", pk.ColumnName));
            sb.AppendLine($"\tEXEC [{table.SchemaName}].[{table.TableName}_Get_Paginated]");
            sb.AppendLine("\t@PageNumber = @PageNumber,");
            sb.AppendLine("\t@RowsOfPage = @RowsOfPage,");
            sb.AppendLine("\t@ExistingRows = @ExistingRows OUTPUT");
            sb.AppendLine("END");
            using StreamWriter outputFile = new(Path.Combine("C:\\Fernando\\Oficina\\FinancialModel\\FinancialModel\\FinancialModel\\src\\FinancialModel.Database\\GenericStoredProcedures", string.Concat(table.SchemaName, ".", project.StoredProceduresPrefix, table.TableName, "_Update.sql")), false, Encoding.UTF8);
            outputFile.Write(sb.ToString());
            using SqlConnection conn = new(project.ConnectionString);
            using SqlCommand cmd = new(sb.ToString(), conn);
            cmd.CommandType = CommandType.Text;
            conn.Open();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
            conn.Dispose();
        }
    }
}
