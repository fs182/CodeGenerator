
using CodeGenerator.Infrastructure.Context.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace CodeGenerator.Infrastructure.Templates.CleanArquitecture.Database.StoredProcedure
{
    public static class Insert
    {
        public static void WriteInsertSP(Project project, Table table)
        {
            var pk = table.Columns.First(f => f.IsPrimaryKey);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("-- =============================================");
            sb.AppendLine(string.Concat("-- Author: ", project.Autor));
            //sb.AppendLine(string.Concat("-- Create date: ", DateTime.Now.ToLongDateString(), " - ", DateTime.Now.ToLongTimeString()));
            //sb.AppendLine(string.Concat("-- Create date: ", DateTime.Now.ToLongDateString()));
            sb.AppendLine(string.Concat("-- Description: Insert ", table.TableName));
            sb.AppendLine("-- =============================================");
            sb.AppendLine(string.Concat("CREATE OR ALTER PROCEDURE ", table.SchemaName, ".[", project.StoredProceduresPrefix, table.TableName, "_Insert]"));
            sb.AppendLine("\t@PageNumber int,");
            sb.AppendLine("\t@RowsOfPage int,");
            sb.AppendLine("\t@ExistingRows int OUTPUT,");
            foreach (var c in table.Columns.Where(f => !f.IsIdentity))
            {
                var nullable = c.IsNullable ? " = null" : "";
                if (Helper.GetStringNetCoreType(c.SqlDataType) == "string")
                    sb.AppendLine(string.Concat("\t", "@", c.ColumnName, " ", c.SqlDataType, "(", c.MaxLength, ")", nullable, ","));
                else
                    sb.AppendLine(string.Concat("\t", "@", c.ColumnName, " ", c.SqlDataType, "", nullable, ","));
            }
            sb.Length = sb.Length - 3;
            sb.AppendLine("");
            sb.AppendLine("AS");
            sb.AppendLine("BEGIN");
            sb.AppendLine("\tSET NOCOUNT ON;");
            sb.AppendLine(string.Concat("\tINSERT INTO [", table.SchemaName, "].[", table.TableName, "] ("));
            foreach (var c in table.Columns.Where(f => !f.IsIdentity))
            {
                sb.AppendLine(string.Concat("\t\t\t[", c.ColumnName, "],"));
            }
            sb.Length = sb.Length - 3;
            sb.AppendLine(")");
            sb.AppendLine("\tVALUES (");
            foreach (var c in table.Columns.Where(f => !f.IsIdentity))
            {
                sb.AppendLine(string.Concat("\t\t\t@", c.ColumnName, ","));
            }
            sb.Length = sb.Length - 3;
            sb.AppendLine(")");
            sb.AppendLine($"\tEXEC [{table.SchemaName}].[{table.TableName}_Get_Paginated]");
            sb.AppendLine("\t@PageNumber = @PageNumber,");
            sb.AppendLine("\t@RowsOfPage = @RowsOfPage,");
            sb.AppendLine("\t@ExistingRows = @ExistingRows OUTPUT");
            sb.AppendLine("END");

            using StreamWriter outputFile = new(Path.Combine("C:\\Fernando\\Oficina\\FinancialModel\\FinancialModel\\FinancialModel\\src\\FinancialModel.Database\\StoredProcedures", string.Concat(table.SchemaName, ".", project.StoredProceduresPrefix, table.TableName, "_Insert.sql")), false, Encoding.UTF8);
            outputFile.Write(sb.ToString());
            using SqlConnection conn = new SqlConnection(project.ConnectionString);
            using SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            cmd.CommandType = CommandType.Text;
            conn.Open();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
            conn.Dispose();
        }


        public static void WriteInsertOnlySP(Project project, Table table)
        {
            var pk = table.Columns.First(f => f.IsPrimaryKey);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("-- =============================================");
            sb.AppendLine(string.Concat("-- Author: ", project.Autor));
            //sb.AppendLine(string.Concat("-- Create date: ", DateTime.Now.ToLongDateString(), " - ", DateTime.Now.ToLongTimeString()));
            //sb.AppendLine(string.Concat("-- Create date: ", DateTime.Now.ToLongDateString()));
            sb.AppendLine(string.Concat("-- Description: Insert Only ", table.TableName));
            sb.AppendLine("-- =============================================");
            sb.AppendLine(string.Concat("CREATE OR ALTER PROCEDURE ", table.SchemaName, ".[", project.StoredProceduresPrefix, table.TableName, "_Insert_Only]"));
            sb.AppendLine($"\t@{pk.ColumnName} {pk.SqlDataType} OUTPUT,");
            foreach (var c in table.Columns.Where(f => !f.IsIdentity && f != pk))
            {
                var nullable = c.IsNullable ? " = null" : "";
                if (Helper.GetStringSQLDBType(c.SqlDataType) == "string")
                    sb.AppendLine(string.Concat("\t", "@", c.ColumnName, " ", c.SqlDataType, "(", c.MaxLength, ")", nullable, ","));
                else
                    sb.AppendLine(string.Concat("\t", "@", c.ColumnName, " ", c.SqlDataType, "", nullable, ","));
            }
            sb.Length = sb.Length - 3;
            sb.AppendLine("");
            sb.AppendLine("AS");
            sb.AppendLine("BEGIN");
            sb.AppendLine("\tSET NOCOUNT ON;");
            sb.AppendLine(string.Concat("\tINSERT INTO [", table.SchemaName, "].[", table.TableName, "] ("));
            foreach (var c in table.Columns.Where(f => !f.IsIdentity))
            {
                sb.AppendLine(string.Concat("\t\t\t[", c.ColumnName, "],"));
            }
            sb.Length = sb.Length - 3;
            sb.AppendLine(")");
            sb.AppendLine("\tVALUES (");
            foreach (var c in table.Columns.Where(f => !f.IsIdentity))
            {
                sb.AppendLine(string.Concat("\t\t\t@", c.ColumnName, ","));
            }
            sb.Length = sb.Length - 3;
            sb.AppendLine(")");
            sb.AppendLine($"\tSET @{pk.ColumnName} = SCOPE_IDENTITY()");
            sb.AppendLine("END");

            using StreamWriter outputFile = new(Path.Combine("C:\\Fernando\\Oficina\\FinancialModel\\FinancialModel\\FinancialModel\\src\\FinancialModel.Database\\StoredProcedures", string.Concat(table.SchemaName, ".", project.StoredProceduresPrefix, table.TableName, "_Insert_Only.sql")), false, Encoding.UTF8);
            outputFile.Write(sb.ToString());
            using SqlConnection conn = new SqlConnection(project.ConnectionString);
            using SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            cmd.CommandType = CommandType.Text;
            conn.Open();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
            conn.Dispose();
            return;
        }
    }
}
