using CodeGenerator.Infrastructure.Context.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;


namespace CodeGenerator.Infrastructure.Templates.CleanArquitecture.Database.StoredProcedure
{
    public static class Get
    {
        //public static async Task WriteGetBySpecificField(Project project, Table table)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    var pk = table.Columns.Where(f => f.PK).First();
        //    sb.AppendLine("-- =============================================");
        //    sb.AppendLine(string.Concat("-- Author: ", env.Autor));
        //    //sb.AppendLine(string.Concat("-- Create date: ", DateTime.Now.ToLongDateString(), " - ", DateTime.Now.ToLongTimeString()));
        //    sb.AppendLine(string.Concat("-- Create date: ", DateTime.Now.ToLongDateString()));
        //    sb.AppendLine(string.Concat("-- Description: Get by ", entity.GetBySpecificField));
        //    sb.AppendLine("-- =============================================");
        //    sb.AppendLine(string.Concat("CREATE OR ALTER PROCEDURE ", entity.SchemaName, ".[", env.StoredProceduresPrefix, entity.Name, "_Get_By", entity.GetBySpecificField, "]"));
        //    sb.AppendLine(string.Concat("\t", "@", entity.GetBySpecificField, " ", columnsInfo.First(f => f.Name == entity.GetBySpecificField).DataType));
        //    sb.AppendLine("AS");
        //    sb.AppendLine("BEGIN");
        //    sb.AppendLine("SET NOCOUNT ON;");
        //    sb.AppendLine("");
        //    sb.AppendLine("SELECT");

        //    foreach (var c in columnsInfo.Where(f => f.Name != "AuditoriaId"))
        //        sb.AppendLine(string.Concat("\ta.", c.Name, ","));
        //    sb.AppendLine("\tb.AuditoriaId,");
        //    sb.AppendLine("\tb.FechaModificacion,");
        //    var existFK = columnsInfo.FirstOrDefault(f => f.FK && f.Name != "AuditoriaId");
        //    sb.AppendLine(string.Concat("\tc.NombreCorto as NombreCortoUsuario", existFK != null ? "," : ""));
        //    int count = 0;
        //    var fkCount = columnsInfo.Where(f => f.FK && f.Name != "AuditoriaId").Count();
        //    foreach (var c in columnsInfo.Where(f => f.FK && f.Name != "AuditoriaId"))
        //    {
        //        count++;
        //        var fkTableInfo = tablesInfo.First(f => f.Name == c.FKTable);
        //        var fkColumnsInfo = await SqlProvider.GetColumnsInfo(fkTableInfo, connectionString);
        //        var fkColumnInfoPk = fkColumnsInfo.First(f => f.PK);
        //        var namedColumnInfo = fkColumnsInfo.FirstOrDefault(f => f.Name.ToLower().Contains("nombre") || (f.Name.ToLower().Contains("descripcion") && !f.Name.ToLower().Contains("descripcionid")) || f.Name.ToLower().Contains("codigo"));
        //        namedColumnInfo ??= fkColumnsInfo.First(f => f.PK);

        //        sb.AppendLine(string.Concat($"\tf{count}.{namedColumnInfo.Name} as {fkTableInfo.Name}{namedColumnInfo.Name}", count != fkCount ? "," : ""));

        //    }
        //    sb.AppendLine(string.Concat("FROM ", entity.Name, " a "));
        //    sb.AppendLine("INNER JOIN Auditoria b on a.AuditoriaId = b.AuditoriaId");
        //    sb.AppendLine("INNER JOIN Usuario c on b.UsuarioId = c.UsuarioId");
        //    count = 0;
        //    foreach (var c in columnsInfo.Where(f => f.FK && f.Name != "AuditoriaId"))
        //    {
        //        count++;
        //        var fkTableInfo = tablesInfo.First(f => f.Name == c.FKTable);
        //        var fkColumnsInfo = await SqlProvider.GetColumnsInfo(fkTableInfo, connectionString);
        //        var fkColumnInfoPk = fkColumnsInfo.First(f => f.PK);
        //        var namedColumnInfo = fkColumnsInfo.FirstOrDefault(f => f.Name.ToLower().Contains("nombre") || f.Name.ToLower().Contains("descripcion") || f.Name.ToLower().Contains("codigo"));
        //        namedColumnInfo ??= fkColumnsInfo.First(f => f.PK);

        //        sb.AppendLine($"LEFT OUTER JOIN {fkTableInfo.SchemaName}.{fkTableInfo.Name} f{count} on a.{c.Name} = f{count}.{fkColumnInfoPk.Name}");

        //    }
        //    sb.AppendLine(string.Concat("WHERE a.", entity.GetBySpecificField, " = @", entity.GetBySpecificField));
        //    sb.AppendLine("");
        //    sb.AppendLine("END");

        //    using StreamWriter outputFile = new(Path.Combine(env.DBStoredProceduresPath, string.Concat(entity.SchemaName, ".", env.StoredProceduresPrefix, entity.Name, "_Get_By", entity.GetBySpecificField, ".sql")), false, Encoding.UTF8);
        //    await outputFile.WriteAsync(sb.ToString());

        //    using (SqlConnection conn = new SqlConnection(env.ConnectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand(sb.ToString(), conn))
        //        {
        //            cmd.CommandType = CommandType.Text;
        //            await conn.OpenAsync();
        //            try
        //            {
        //                await cmd.ExecuteNonQueryAsync();
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.Write(sb);
        //                throw ex;
        //            }
        //            return;
        //        }
        //    }
        //}


        public static void WriteGetById(Project project, Table table)
        {
            StringBuilder sb = new StringBuilder();
            var pk = table.Columns.Where(f => f.IsPrimaryKey).First();
            sb.AppendLine("-- =============================================");
            sb.AppendLine(string.Concat("-- Author: ", project.Autor));
            //sb.AppendLine(string.Concat("-- Create date: ", DateTime.Now.ToLongDateString(), " - ", DateTime.Now.ToLongTimeString()));
            //sb.AppendLine(string.Concat("-- Create date: ", DateTime.Now.ToLongDateString()));
            sb.AppendLine(string.Concat("-- Description: Get by Id ", table.TableName));
            sb.AppendLine("-- =============================================");
            sb.AppendLine(string.Concat("CREATE OR ALTER PROCEDURE ", table.SchemaName, ".[", project.StoredProceduresPrefix, table.TableName, "_Get_ById]"));

            var nullable = pk.IsNullable ? " = null" : "";
            if (Helper.GetStringNetCoreType(pk.SqlDataType) == "string")
                sb.AppendLine(string.Concat("\t", "@", pk.ColumnName, " ", pk.SqlDataType, "(", pk.MaxLength == -1 ? "MAX" : pk.MaxLength, ")", nullable));
            else if (Helper.GetStringNetCoreType(pk.SqlDataType) == "decimal" && pk.SqlDataType != "money")
                sb.AppendLine(string.Concat("\t", "@", pk.ColumnName, " ", pk.SqlDataType, "(", pk.Precision, ",", pk.Scale, ")", nullable));
            else
                sb.AppendLine(string.Concat("\t", "@", pk.ColumnName, " ", pk.SqlDataType, "", nullable));

            sb.AppendLine("AS");
            sb.AppendLine("BEGIN");
            sb.AppendLine("SET NOCOUNT ON;");
            sb.AppendLine("");
            sb.AppendLine("SELECT TOP 1");

            foreach (var c in table.Columns.Where(f => f.ColumnName != "AuditoriaId"))
                sb.AppendLine(string.Concat("\ta.", c.ColumnName, ","));
            sb.AppendLine("\tb.AuditoriaId,");
            sb.AppendLine("\tb.FechaModificacion,");
            var existFK = table.Columns.FirstOrDefault(f => f.IsForeignKey && f.ColumnName != "AuditoriaId");
            sb.AppendLine(string.Concat("\tc.NombreCorto as NombreCortoUsuario", existFK != null ? "," : ""));
            int count = 0;
            var fkCount = table.Columns.Where(f => f.IsForeignKey && f.ColumnName != "AuditoriaId").Count();
            foreach (var c in table.Columns.Where(f => f.IsForeignKey && f.ColumnName != "AuditoriaId"))
            {
                count++;
                var fkTableInfo = project.Tables.First(f => f.TableName == c.TableTarget);
                var fkColumnsInfo = fkTableInfo.Columns;
                var fkColumnInfoPk = fkColumnsInfo.First(f => f.IsPrimaryKey);
                var namedColumnInfo = fkColumnsInfo.FirstOrDefault(f => f.ColumnName.ToLower().Contains("nombre") || (f.ColumnName.ToLower().Contains("descripcion") && !f.ColumnName.ToLower().Contains("descripcionid")) || f.ColumnName.ToLower().Contains("codigo"));
                namedColumnInfo ??= fkColumnsInfo.First(f => f.IsPrimaryKey);

                sb.AppendLine(string.Concat($"\tf{count}.{namedColumnInfo.ColumnName} as {fkTableInfo.TableName}{namedColumnInfo.ColumnName}", count != fkCount ? "," : ""));

            }
            sb.AppendLine(string.Concat("FROM ", table.TableName, " a "));
            sb.AppendLine("INNER JOIN Auditoria b on a.AuditoriaId = b.AuditoriaId");
            sb.AppendLine("INNER JOIN Usuario c on b.UsuarioId = c.UsuarioId");
            count = 0;
            foreach (var c in table.Columns.Where(f => f.IsForeignKey && f.ColumnName != "AuditoriaId"))
            {
                count++;
                var fkTableInfo = project.Tables.First(f => f.TableName == c.TableTarget);
                var fkColumnsInfo = fkTableInfo.Columns;
                var fkColumnInfoPk = fkColumnsInfo.First(f => f.IsPrimaryKey);
                var namedColumnInfo = fkColumnsInfo.FirstOrDefault(f => f.ColumnName.ToLower().Contains("nombre") || f.ColumnName.ToLower().Contains("descripcion") || f.ColumnName.ToLower().Contains("codigo"));
                namedColumnInfo ??= fkColumnsInfo.First(f => f.IsPrimaryKey);

                sb.AppendLine($"LEFT OUTER JOIN {fkTableInfo.SchemaName}.{fkTableInfo.TableName} f{count} on a.{c.ColumnName} = f{count}.{fkColumnInfoPk.ColumnName}");

            }
            sb.AppendLine(string.Concat("WHERE a.", pk.ColumnName, " = @", pk.ColumnName));
            sb.AppendLine("");
            sb.AppendLine("END");

            using StreamWriter outputFile = new(Path.Combine("C:\\Fernando\\Oficina\\FinancialModel\\FinancialModel\\FinancialModel\\src\\FinancialModel.Database\\GenericStoredProcedures", string.Concat(table.SchemaName, ".", project.StoredProceduresPrefix, table.TableName, "_Get_ById.sql")), false, Encoding.UTF8);
            outputFile.Write(sb.ToString());
            outputFile.Close();
            outputFile.Dispose();

            using SqlConnection conn = new SqlConnection(project.ConnectionString);
            using SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            cmd.CommandType = CommandType.Text;
            conn.Open();
            try
            {
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
            }
            catch (Exception ex)
            {
                conn.Close();
                conn.Dispose();
                cmd.Dispose();
                Console.Write(sb);
                throw ex;
            }
            return;
        }

        public static void WriteGetByCustom(Project project, Table table)
        {
            var customGetMethods = table.Columns.Where(f => f.Property.CreateGetBy).ToList();
            foreach (var customMethod in customGetMethods)
            {
                StringBuilder sb = new StringBuilder();
                var pk = table.Columns.Where(f => f.IsPrimaryKey).First();
                sb.AppendLine("-- =============================================");
                sb.AppendLine(string.Concat("-- Author: ", project.Autor));
                //sb.AppendLine(string.Concat("-- Create date: ", DateTime.Now.ToLongDateString(), " - ", DateTime.Now.ToLongTimeString()));
                //sb.AppendLine(string.Concat("-- Create date: ", DateTime.Now.ToLongDateString()));
                sb.AppendLine(string.Concat("-- Description: Get by ", customMethod.ColumnName ," ", table.TableName));
                sb.AppendLine("-- =============================================");
                sb.AppendLine(string.Concat("CREATE OR ALTER PROCEDURE ", table.SchemaName, ".[", project.StoredProceduresPrefix, table.TableName, "_Get_By",customMethod.ColumnName,"]"));

                var nullable = customMethod.IsNullable ? " = null" : "";
                if (Helper.GetStringNetCoreType(customMethod.SqlDataType) == "string")
                    sb.AppendLine(string.Concat("\t", "@", customMethod.ColumnName, " ", customMethod.SqlDataType, "(", customMethod.MaxLength == -1 ? "MAX" : customMethod.MaxLength, ")", nullable));
                else if (Helper.GetStringNetCoreType(customMethod.SqlDataType) == "decimal" && customMethod.SqlDataType != "money")
                    sb.AppendLine(string.Concat("\t", "@", customMethod.ColumnName, " ", customMethod.SqlDataType, "(", customMethod.Precision, ",", customMethod.Scale, ")", nullable));
                else
                    sb.AppendLine(string.Concat("\t", "@", customMethod.ColumnName, " ", customMethod.SqlDataType, "", nullable));

                //sb.AppendLine(string.Concat("\t", "@", customMethod.ColumnName, " ", customMethod.SqlDataType, " (", customMethod.MaxLength, ")"));
                sb.AppendLine("AS");
                sb.AppendLine("BEGIN");
                sb.AppendLine("SET NOCOUNT ON;");
                sb.AppendLine("");
                sb.AppendLine("SELECT ");

                foreach (var c in table.Columns.Where(f => f.ColumnName != "AuditoriaId"))
                    sb.AppendLine(string.Concat("\ta.", c.ColumnName, ","));
                sb.AppendLine("\tb.AuditoriaId,");
                sb.AppendLine("\tb.FechaModificacion,");
                var existFK = table.Columns.FirstOrDefault(f => f.IsForeignKey && f.ColumnName != "AuditoriaId");
                sb.AppendLine(string.Concat("\tc.NombreCorto as NombreCortoUsuario", existFK != null ? "," : ""));
                int count = 0;
                var fkCount = table.Columns.Where(f => f.IsForeignKey && f.ColumnName != "AuditoriaId").Count();
                foreach (var c in table.Columns.Where(f => f.IsForeignKey && f.ColumnName != "AuditoriaId"))
                {
                    count++;
                    var fkTableInfo = project.Tables.First(f => f.TableName == c.TableTarget);
                    var fkColumnsInfo = fkTableInfo.Columns;
                    var fkColumnInfoPk = fkColumnsInfo.First(f => f.IsPrimaryKey);
                    var namedColumnInfo = fkColumnsInfo.FirstOrDefault(f => f.ColumnName.ToLower().Contains("nombre") || (f.ColumnName.ToLower().Contains("descripcion") && !f.ColumnName.ToLower().Contains("descripcionid")) || f.ColumnName.ToLower().Contains("codigo"));
                    namedColumnInfo ??= fkColumnsInfo.First(f => f.IsPrimaryKey);


                    sb.AppendLine(string.Concat($"\tf{count}.{namedColumnInfo.ColumnName} as {fkTableInfo.TableName}{namedColumnInfo.ColumnName}", count != fkCount ? "," : ""));

                }
                sb.AppendLine(string.Concat("FROM ", table.TableName, " a "));
                sb.AppendLine("INNER JOIN Auditoria b on a.AuditoriaId = b.AuditoriaId");
                sb.AppendLine("INNER JOIN Usuario c on b.UsuarioId = c.UsuarioId");
                count = 0;
                foreach (var c in table.Columns.Where(f => f.IsForeignKey && f.ColumnName != "AuditoriaId"))
                {
                    count++;
                    var fkTableInfo = project.Tables.First(f => f.TableName == c.TableTarget);
                    var fkColumnsInfo = fkTableInfo.Columns;
                    var fkColumnInfoPk = fkColumnsInfo.First(f => f.IsPrimaryKey);
                    var namedColumnInfo = fkColumnsInfo.FirstOrDefault(f => f.ColumnName.ToLower().Contains("nombre") || f.ColumnName.ToLower().Contains("descripcion") || f.ColumnName.ToLower().Contains("codigo"));
                    namedColumnInfo ??= fkColumnsInfo.First(f => f.IsPrimaryKey);

                    sb.AppendLine($"LEFT OUTER JOIN {fkTableInfo.SchemaName}.{fkTableInfo.TableName} f{count} on a.{c.ColumnName} = f{count}.{fkColumnInfoPk.ColumnName}");

                }
                sb.AppendLine(string.Concat($"WHERE a.{customMethod.ColumnName} = @{customMethod.ColumnName}"));
                sb.AppendLine("");
                sb.AppendLine("END");

                using StreamWriter outputFile = new(Path.Combine("C:\\Fernando\\Oficina\\FinancialModel\\FinancialModel\\FinancialModel\\src\\FinancialModel.Database\\GenericStoredProcedures", string.Concat(table.SchemaName, ".", project.StoredProceduresPrefix, table.TableName, "_Get_By", customMethod.ColumnName, ".sql")), false, Encoding.UTF8);
                outputFile.Write(sb.ToString());
                outputFile.Close();
                outputFile.Dispose();
                using SqlConnection conn = new SqlConnection(project.ConnectionString);
                using SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    conn.Close();
                    conn.Dispose();
                }
                catch (Exception ex)
                {
                    conn.Close();
                    conn.Dispose();
                    cmd.Dispose();
                    Console.Write(sb);
                    throw ex;
                }
            }
        }

        public static void WriteGetPaginated(Project project, Table table)
        {
            var pk = table.Columns.First(f => f.IsPrimaryKey);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("-- =============================================");
            sb.AppendLine(string.Concat("-- Author: ", project.Autor));
            //sb.AppendLine(string.Concat("-- Create date: ", DateTime.Now.ToLongDateString(), " - ", DateTime.Now.ToLongTimeString()));
            //sb.AppendLine(string.Concat("-- Create date: ", DateTime.Now.ToLongDateString()));
            sb.AppendLine(string.Concat("-- Description: Get paginated ", table.TableName));
            sb.AppendLine("-- =============================================");
            sb.AppendLine(string.Concat("CREATE OR ALTER PROCEDURE ", table.SchemaName, ".[", project.StoredProceduresPrefix, table.TableName, "_Get_Paginated]"));
            sb.AppendLine("\t@PageNumber int,");
            sb.AppendLine("\t@RowsOfPage int,");
            sb.AppendLine("\t@ExistingRows int OUTPUT");
            sb.AppendLine();
            sb.AppendLine("AS");
            sb.AppendLine("BEGIN");
            sb.AppendLine("\tSET NOCOUNT ON;");
            sb.AppendLine("");
            sb.AppendLine(string.Concat("\tSET @ExistingRows = (SELECT COUNT(1) FROM ", table.SchemaName, ".", table.TableName, ");"));
            sb.AppendLine("");
            sb.AppendLine("\tWITH CATALOGO AS");
            sb.AppendLine("\t(");
            sb.AppendLine("\tSELECT ");
            foreach (var c in table.Columns.Where(f => f.ColumnName != "AuditoriaId"))
                sb.AppendLine(string.Concat("\t\ta.", c.ColumnName, ","));
            sb.AppendLine("\t\tb.AuditoriaId,");
            sb.AppendLine("\t\tb.FechaModificacion,");
            var existFK = table.Columns.FirstOrDefault(f => f.IsForeignKey && f.ColumnName != "AuditoriaId");
            //sb.AppendLine(string.Concat("\t\tc.NombreCorto as NombreCortoUsuario", existFK != null? ",":""));
            sb.AppendLine(string.Concat("\t\tc.NombreCorto as NombreCortoUsuario", existFK != null ? "," : ","));

            int count = 0;
            var fkCount = table.Columns.Where(f => f.IsForeignKey && f.ColumnName != "AuditoriaId").Count();
            string prefixFk = "";
            int countFk = 0;
            foreach (var c in table.Columns.Where(f => f.IsForeignKey && f.ColumnName != "AuditoriaId"))
            {
                if (table.Columns.Count(f => f.TableTarget == c.TableTarget) > 1)
                {
                    countFk++;
                    prefixFk = countFk.ToString();
                }

                count++;
                var fkTableInfo = project.Tables.First(f => f.TableName == c.TableTarget);
                var fkColumnsInfo = fkTableInfo.Columns;
                var fkColumnInfoPk = fkColumnsInfo.First(f => f.IsPrimaryKey);
                var namedColumnInfo = fkColumnsInfo.FirstOrDefault(f => f.ColumnName.ToLower().Contains("nombre") || (f.ColumnName.ToLower().Contains("descripcion") && !f.ColumnName.ToLower().Contains("descripcionid")) || f.ColumnName.ToLower().Contains("codigo"));
                namedColumnInfo ??= fkColumnsInfo.First(f => f.IsPrimaryKey);

                //sb.AppendLine(string.Concat($"\t\tf{count}.{namedColumnInfo.Name} as {fkTableInfo.Name}{namedColumnInfo.Name}", count != fkCount ? "," : ""));
                sb.AppendLine(string.Concat($"\t\tf{count}.{namedColumnInfo.ColumnName} as {fkTableInfo.TableName}{namedColumnInfo.ColumnName}{prefixFk}", count != fkCount ? "," : ","));
                prefixFk = "";
            }
            sb.Append("\t\tROW_NUMBER() OVER(");
            string relevantColumnName = pk.ColumnName;
            var nameDescriptionColumn = table.Columns.FirstOrDefault(f => f.ColumnName.ToLower().Contains("nombre") || f.ColumnName.ToLower().Contains("descripcion") || f.ColumnName.ToLower().Contains("codigo") || f.ColumnName.ToLower().Contains("secuencia"));
            if (nameDescriptionColumn != null)
                relevantColumnName = nameDescriptionColumn.ColumnName;

            if (table.TableName.ToLower() == "periodo")
                sb.Append(string.Concat("\tORDER BY Ano DESC, Mes DESC, DistribuidorId"));
            else
                sb.Append(string.Concat("ORDER BY a.", relevantColumnName));
            sb.AppendLine(") AS NUM");
            sb.AppendLine(string.Concat("\tFROM ", table.SchemaName, ".", table.TableName, " a"));
            sb.AppendLine("\tINNER JOIN Auditoria b on a.AuditoriaId = b.AuditoriaId");
            sb.AppendLine("\tINNER JOIN Usuario c on b.UsuarioId = c.UsuarioId");
            count = 0;
            foreach (var c in table.Columns.Where(f => f.IsForeignKey && f.ColumnName != "AuditoriaId"))
            {
                count++;
                var fkTableInfo = project.Tables.First(f => f.TableName == c.TableTarget);
                var fkColumnsInfo = fkTableInfo.Columns;
                var fkColumnInfoPk = fkColumnsInfo.First(f => f.IsPrimaryKey);
                var namedColumnInfo = fkColumnsInfo.FirstOrDefault(f => f.ColumnName.ToLower().Contains("nombre") || f.ColumnName.ToLower().Contains("descripcion") || f.ColumnName.ToLower().Contains("codigo"));
                namedColumnInfo ??= fkColumnsInfo.First(f => f.IsPrimaryKey);


                sb.AppendLine($"\tLEFT OUTER JOIN {fkTableInfo.SchemaName}.{fkTableInfo.TableName} f{count} on a.{c.ColumnName} = f{count}.{fkColumnInfoPk.ColumnName}");

            }
            sb.AppendLine("\t)");

            sb.AppendLine("\tSELECT * FROM CATALOGO");
            sb.AppendLine("\tWHERE NUM BETWEEN((@PageNumber -1) *@RowsOfPage) +1 AND @PageNumber *@RowsOfPage");
            sb.AppendLine("");
            sb.AppendLine("END");

            using StreamWriter outputFile = new(Path.Combine("C:\\Fernando\\Oficina\\FinancialModel\\FinancialModel\\FinancialModel\\src\\FinancialModel.Database\\GenericStoredProcedures", string.Concat(table.SchemaName, ".", project.StoredProceduresPrefix, table.TableName, "_Get_Paginated.sql")), false, Encoding.UTF8);
            outputFile.Write(sb.ToString());
            outputFile.Close();
            outputFile.Dispose();
            using SqlConnection conn = new SqlConnection(project.ConnectionString);
            using SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            cmd.CommandType = CommandType.Text;
            conn.Open();
            try
            {
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
            }
            catch (Exception ex)
            {
                conn.Close();
                conn.Dispose();
                cmd.Dispose();
                Console.Write(sb);
                throw ex;
            }
        }
    }
}
