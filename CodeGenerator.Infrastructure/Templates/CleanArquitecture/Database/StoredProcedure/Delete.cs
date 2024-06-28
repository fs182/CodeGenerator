using CodeGenerator.Infrastructure.Context.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace CodeGenerator.Infrastructure.Templates.CleanArquitecture.Database.StoredProcedure
{
    public static class Delete
    {
        public static void WriteDeleteSP(Project project, Table table)
        {
            var pk = table.Columns.First(f => f.IsPrimaryKey);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("-- =============================================");
            sb.AppendLine(string.Concat("-- Author: ", project.Autor));
            //sb.AppendLine(string.Concat("-- Create date: ", DateTime.Now.ToLongDateString(), " - ", DateTime.Now.ToLongTimeString()));
            //sb.AppendLine(string.Concat("-- Create date: ", DateTime.Now.ToLongDateString()));
            sb.AppendLine(string.Concat("-- Description: Delete ", table.TableName));
            sb.AppendLine("-- =============================================");
            sb.AppendLine(string.Concat("CREATE OR ALTER PROCEDURE ", table.SchemaName, ".[", project.StoredProceduresPrefix, table.TableName, "_Delete]"));
            sb.AppendLine("\t@PageNumber int,");
            sb.AppendLine("\t@RowsOfPage int,");
            sb.AppendLine("\t@ExistingRows int OUTPUT,");
            sb.AppendLine(string.Concat("\t", "@", pk.ColumnName, " ", pk.SqlDataType));
            sb.AppendLine("");
            sb.AppendLine("AS");
            sb.AppendLine("BEGIN");
            sb.AppendLine("\tSET NOCOUNT ON;");
            sb.AppendLine(string.Concat("\tDELETE ", table.TableName, " WHERE ", pk.ColumnName, " = @", pk.ColumnName));
            sb.AppendLine($"\tEXEC [{table.SchemaName}].[{table.TableName}_Get_Paginated]");
            sb.AppendLine("\t@PageNumber = @PageNumber,");
            sb.AppendLine("\t@RowsOfPage = @RowsOfPage,");
            sb.AppendLine("\t@ExistingRows = @ExistingRows OUTPUT");
            sb.AppendLine("END");
            using StreamWriter outputFile = new(Path.Combine("C:\\Fernando\\Oficina\\FinancialModel\\FinancialModel\\FinancialModel\\src\\FinancialModel.Database\\StoredProcedures\\", string.Concat(table.SchemaName, ".", project.StoredProceduresPrefix, table.TableName, "_Delete.sql")), false, Encoding.UTF8);
            outputFile.Write(sb.ToString());
            outputFile.Close();
            outputFile.Dispose();
            using (SqlConnection conn = new SqlConnection(project.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sb.ToString(), conn))
                {
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return;
                }
            }
        }
        public static void WriteDeleteOnlySP(Project project, Table table)
        {
            var pk = table.Columns.First(f => f.IsPrimaryKey);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("-- =============================================");
            sb.AppendLine(string.Concat("-- Author: ", project.Autor));
            //sb.AppendLine(string.Concat("-- Create date: ", DateTime.Now.ToLongDateString(), " - ", DateTime.Now.ToLongTimeString()));
            sb.AppendLine(string.Concat("-- Create date: ", DateTime.Now.ToLongDateString()));
            sb.AppendLine(string.Concat("-- Description: Delete ", table.TableName));
            sb.AppendLine("-- =============================================");
            sb.AppendLine(string.Concat("CREATE OR ALTER PROCEDURE ", table.SchemaName, ".[", project.StoredProceduresPrefix, table.TableName, "_Delete_Only]"));

            //if (entity.GetBySpecificField == null)
                sb.AppendLine(string.Concat("\t", "@", pk.ColumnName, " ", pk.SqlDataType));
            //else
            //    sb.AppendLine(string.Concat("\t", "@", entity.GetBySpecificField, " ", columnsInfo.First(f => f.Name == entity.GetBySpecificField).DataType));

            sb.AppendLine("");
            sb.AppendLine("AS");
            sb.AppendLine("BEGIN");
            sb.AppendLine("\tSET NOCOUNT ON;");
            //if (entity.GetBySpecificField == null)
                sb.AppendLine(string.Concat("\tDELETE ", table.TableName, " WHERE ", pk.ColumnName, " = @", pk.ColumnName));
            //else
            //    sb.AppendLine(string.Concat("\tDELETE ", table.TableName, " WHERE ", entity.GetBySpecificField, " = @", entity.GetBySpecificField));

            sb.AppendLine("END");
            using StreamWriter outputFile = new(Path.Combine("C:\\Fernando\\Oficina\\FinancialModel\\FinancialModel\\FinancialModel\\src\\FinancialModel.Database\\StoredProcedures\\", string.Concat(table.SchemaName, ".", project.StoredProceduresPrefix, table.TableName, "_Delete_Only.sql")), false, Encoding.UTF8);
            outputFile.Write(sb.ToString());
            outputFile.Close();
            outputFile.Dispose();
            using (SqlConnection conn = new SqlConnection(project.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sb.ToString(), conn))
                {
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return;
                }
            }
        }
    }
}
