using CodeGenerator.Infrastructure.Context.Models;
using System.Text;


namespace CodeGenerator.Infrastructure.Templates.CleanArquitecture.Infrastructure
{
    public static class Repository
    {
        public static void WriteCommandRepository(Project project, Table table)
        {
            if (!Directory.Exists(Path.Combine(project.InfrastructureRepositoriesPath, table.TableName)))
                Directory.CreateDirectory(Path.Combine(project.InfrastructureRepositoriesPath, table.TableName));
            using StreamWriter outputFile = new(Path.Combine(project.InfrastructureRepositoriesPath, table.TableName, string.Concat(table.TableName, "CommandRepository.cs")), false, Encoding.UTF8);
            outputFile.WriteLine(string.Concat("using ", project.Namespace, ".Infrastructure.Mappers;"));
            outputFile.WriteLine(string.Concat("using Microsoft.Data.SqlClient;"));
            outputFile.WriteLine(string.Concat("using Microsoft.EntityFrameworkCore;"));
            //if (table.SimplifiedCommand)
            outputFile.WriteLine(string.Concat("using MediatR;"));
            
            outputFile.WriteLine(string.Concat("namespace ", project.Namespace, ".Infrastructure.Repositories"));
            outputFile.WriteLine(string.Concat("{"));
            outputFile.WriteLine(string.Concat("    public partial class CatalogCommandRepository"));
            outputFile.WriteLine(string.Concat("    {"));

            var pk = table.Columns.First(f=>f.IsPrimaryKey);
            //if (!table.NoUpdateCommand)
            //{
                outputFile.WriteLine($"        public async Task<List<Application.Responses.{table.TableName}.CommandResponse>> {table.TableName}Update(Application.Commands.{table.TableName}.UpdateCommand command)");
                outputFile.WriteLine("        {");
                outputFile.WriteLine("            var parameters = new SqlParameter[]");
                outputFile.WriteLine("                {");
                outputFile.WriteLine("                        new SqlParameter() {ParameterName = \"@PageNumber\", SqlDbType =  System.Data.SqlDbType.Int, Value = command.PageNumber},");
                outputFile.WriteLine("                        new SqlParameter() {ParameterName = \"@RowsOfPage\", SqlDbType =  System.Data.SqlDbType.Int, Value = command.RowsOfPage},");
                outputFile.WriteLine("                        new SqlParameter() {ParameterName = \"@ExistingRows\", SqlDbType =  System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Output},");
                foreach (var c in table.Columns)
                {
                    var nullableValue = "";
                    if (c.IsNullable)
                        nullableValue = string.Concat(" == null ? DBNull.Value : command.", c.ColumnName);
                    outputFile.WriteLine(string.Concat("                        new SqlParameter() {ParameterName = \"@", c.ColumnName, "\", SqlDbType =  System.Data.SqlDbType.", Helper.GetStringSQLDBType(c.SqlDataType), ",", c.IsNullable ? " IsNullable = true," : "", " Value = command.", c.ColumnName, nullableValue, "},"));
                }
                outputFile.WriteLine("                };");
                outputFile.WriteLine($"            var result = new List<Context.StoredProcedureResult.Commands.{table.TableName}CommandResult>();");
                outputFile.Write(string.Concat("            await Task.Run(() => { result = _context.", table.TableName, "s.FromSqlRaw(\"[", table.SchemaName, "].[", table.TableName, "_Update] @PageNumber, @RowsOfPage, @ExistingRows OUTPUT,"));
                foreach (var c in table.Columns.Where(f => f.ColumnName != "AuditoriaId"))
                    outputFile.Write($" @{c.ColumnName},");
                outputFile.WriteLine(" @AuditoriaId\", parameters).ToList(); });");
                outputFile.WriteLine("            var existingRows = (int)parameters[2].Value;");
                outputFile.WriteLine($"            return {table.TableName}Mapper.Map(result, command.RowsOfPage, existingRows);");
                outputFile.WriteLine("        }");
            //}

            //if (!table.SimplifiedCommand)
                outputFile.WriteLine($"        public async Task<List<Application.Responses.{table.TableName}.CommandResponse>> {table.TableName}Delete(Application.Commands.{table.TableName}.DeleteCommand command)");
            //else
            //    outputFile.WriteLine($"        public async Task {table.TableName}Delete(Application.Commands.{table.TableName}.DeleteCommand command)");

            outputFile.WriteLine("        {");
            outputFile.WriteLine("            var parameters = new SqlParameter[]");
            outputFile.WriteLine("                {");
            //if (!table.SimplifiedCommand)
            //{
            outputFile.WriteLine("                        new SqlParameter() {ParameterName = \"@PageNumber\", SqlDbType =  System.Data.SqlDbType.Int, Value = command.PageNumber},");
            outputFile.WriteLine("                        new SqlParameter() {ParameterName = \"@RowsOfPage\", SqlDbType =  System.Data.SqlDbType.Int, Value = command.RowsOfPage},");
            outputFile.WriteLine("                        new SqlParameter() {ParameterName = \"@ExistingRows\", SqlDbType =  System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Output},");
            //}
            var mainColumn = table.Columns.FirstOrDefault(f => f.IsPrimaryKey);
            //if (!table.SimplifiedCommand)
            outputFile.WriteLine(string.Concat("                        new SqlParameter() {ParameterName = \"@", pk.ColumnName, "\", SqlDbType =  System.Data.SqlDbType.Int, Value = command.", pk.ColumnName, "}"));
            //else
            //if(mainColumn != null)    
            //    outputFile.WriteLine(string.Concat("                        new SqlParameter() {ParameterName = \"@", mainColumn.ColumnName, "\", SqlDbType =  System.Data.SqlDbType.", Helper.GetStringSQLDBType(mainColumn.SqlDataType), ", Value = command.", mainColumn.ColumnName, "}"));


            outputFile.WriteLine("                };");
            //if (!table.SimplifiedCommand)
            //{
            outputFile.WriteLine($"            var result = new List<Context.StoredProcedureResult.Commands.{table.TableName}CommandResult>();");
            outputFile.WriteLine(string.Concat("            await Task.Run(() => { result = _context.", table.TableName, "s.FromSqlRaw(\"[", table.SchemaName, "].[", table.TableName, "_Delete] @PageNumber, @RowsOfPage, @ExistingRows OUTPUT, @", pk.ColumnName, "\", parameters).ToList(); });"));
            //}
            //else
            //if (mainColumn != null)
            //{
            //    outputFile.WriteLine("            await Task.Run(() => {");
            //    outputFile.WriteLine($"             _context.Database.ExecuteSqlRaw(\"[{table.SchemaName}].[{table.TableName}_Delete_Only] @{mainColumn.ColumnName}\", parameters);");
            //    outputFile.WriteLine("            });");
            //}
                

            //if (!table.SimplifiedCommand)
            //{
            outputFile.WriteLine("            var existingRows = (int)parameters[2].Value;");
            outputFile.WriteLine($"            return {table.TableName}Mapper.Map(result, command.RowsOfPage, existingRows);");
            //}
            outputFile.WriteLine("        }");

            //if (!table.SimplifiedCommand)
            outputFile.WriteLine($"        public async Task<List<Application.Responses.{table.TableName}.CommandResponse>> {table.TableName}Create(Application.Commands.{table.TableName}.CreateCommand command)");
            //else
            //    outputFile.WriteLine($"        public async Task {table.TableName}Create(Application.Commands.{table.TableName}.CreateCommand command)");
            outputFile.WriteLine("        {");
            outputFile.WriteLine("            var parameters = new SqlParameter[]");
            outputFile.WriteLine("                {");
            //if (!table.SimplifiedCommand)
            //{
            outputFile.WriteLine("                    new SqlParameter() {ParameterName = \"@PageNumber\", SqlDbType =  System.Data.SqlDbType.Int, Value = command.PageNumber},");
            outputFile.WriteLine("                        new SqlParameter() {ParameterName = \"@RowsOfPage\", SqlDbType =  System.Data.SqlDbType.Int, Value = command.RowsOfPage},");
            outputFile.WriteLine("                        new SqlParameter() {ParameterName = \"@ExistingRows\", SqlDbType =  System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Output},");
            //}

            foreach (var c in table.Columns.Where(f => !f.IsIdentity))
            {
                var nullableValue = "";
                if (c.IsNullable)
                    nullableValue = string.Concat(" == null ? DBNull.Value : command.", c.ColumnName);
                outputFile.WriteLine(string.Concat("                        new SqlParameter() {ParameterName = \"@", c.ColumnName, "\", SqlDbType =  System.Data.SqlDbType.", Helper.GetStringSQLDBType(c.SqlDataType), ",", c.IsNullable ? " IsNullable = true," : "", " Value = command.", c.ColumnName, nullableValue, "},"));
            }

            outputFile.WriteLine("                };");
            //if (!table.SimplifiedCommand)
            //{
            outputFile.WriteLine($"            var result = new List<Context.StoredProcedureResult.Commands.{table.TableName}CommandResult>();");
            outputFile.Write(string.Concat("            await Task.Run(() => { result = _context.", table.TableName, "s.FromSqlRaw(\"[", table.SchemaName, "].[", table.TableName, "_Insert] @PageNumber, @RowsOfPage, @ExistingRows OUTPUT, "));

            //}
            //else
            //outputFile.WriteLine("            await Task.Run(() => {");
            //outputFile.Write($"             _context.Database.ExecuteSqlRawAsync(\"[{table.SchemaName}].[{table.TableName}_Insert] ");

            foreach (var c in table.Columns.Where(f => !f.IsIdentity && f.ColumnName != "AuditoriaId"))
                outputFile.Write($"@{c.ColumnName}, ");

            //if (!table.SimplifiedCommand)
            //{
            outputFile.WriteLine("@AuditoriaId\", parameters).ToList(); });");
            outputFile.WriteLine("            var existingRows = (int)parameters[2].Value;");
            outputFile.WriteLine($"            return {table.TableName}Mapper.Map(result, command.RowsOfPage, existingRows);");
            //}
            //else
            //    outputFile.WriteLine("@AuditoriaId\", parameters);");
            //    outputFile.WriteLine("            });");

            outputFile.WriteLine("        }");

            outputFile.WriteLine($"        public async Task<{Helper.GetStringNetCoreType(pk.SqlDataType)}> {table.TableName}CreateOnly(Application.Commands.{table.TableName}.CreateCommand command)");
            outputFile.WriteLine("        {");
            outputFile.WriteLine($"            var parameters = new SqlParameter[]");
            outputFile.WriteLine("                {");
            outputFile.WriteLine(string.Concat("                    new SqlParameter() {ParameterName = \"@", pk.ColumnName, "\", SqlDbType =  System.Data.SqlDbType.", Helper.GetStringSQLDBType(pk.SqlDataType), ", Direction = System.Data.ParameterDirection.Output},"));
            foreach (var c in table.Columns.Where(f => !f.IsIdentity))
            {
                var nullableValue = "";
                if (c.IsNullable)
                    nullableValue = string.Concat(" == null ? DBNull.Value : command.", c.ColumnName);
                outputFile.WriteLine(string.Concat("                        new SqlParameter() {ParameterName = \"@", c.ColumnName, "\", SqlDbType =  System.Data.SqlDbType.", Helper.GetStringSQLDBType(c.SqlDataType), ",", c.IsNullable ? " IsNullable = true," : "", " Value = command.", c.ColumnName, nullableValue, "},"));
            }
            outputFile.WriteLine("                };");
            outputFile.Write(string.Concat("            await Task.Run(() => { _context.Database.ExecuteSqlRaw(\"", table.SchemaName, ".", table.TableName, "_Insert_Only @", pk.ColumnName, " OUTPUT,"));
            foreach (var c in table.Columns.Where(f => !f.IsIdentity && f.ColumnName != "AuditoriaId"))
                outputFile.Write($"@{c.ColumnName}, ");
            outputFile.WriteLine("@AuditoriaId\", parameters);});");
            outputFile.WriteLine($"            var {Helper.GetCamel(pk.ColumnName)} = ({Helper.GetStringNetCoreType(pk.SqlDataType)})parameters[0].Value;");
            outputFile.WriteLine($"            return {Helper.GetCamel(pk.ColumnName)};");
            outputFile.WriteLine("        }");
            outputFile.WriteLine(string.Concat("    }"));
            outputFile.WriteLine(string.Concat("}"));
            outputFile.Close();
            outputFile.Dispose();
        }

        public static void WriteQueryRepository(Project project, Table table)
        {
            if (!Directory.Exists(Path.Combine(project.InfrastructureRepositoriesPath, table.TableName)))
                Directory.CreateDirectory(Path.Combine(project.InfrastructureRepositoriesPath, table.TableName));
            using StreamWriter outputFile = new(Path.Combine(project.InfrastructureRepositoriesPath, table.TableName, string.Concat(table.TableName, "QueryRepository.cs")), false, Encoding.UTF8);
            outputFile.WriteLine(string.Concat("using ", project.Namespace, ".Infrastructure.Mappers;"));
            outputFile.WriteLine(string.Concat("using Microsoft.Data.SqlClient;"));
            outputFile.WriteLine(string.Concat("using Microsoft.EntityFrameworkCore;"));
            outputFile.WriteLine(string.Concat("namespace ", project.Namespace, ".Infrastructure.Repositories"));
            outputFile.WriteLine(string.Concat("{"));
            outputFile.WriteLine(string.Concat("    public partial class CatalogQueryRepository"));
            outputFile.WriteLine(string.Concat("    {"));

            var pk = table.Columns.First(f => f.IsPrimaryKey);
            outputFile.WriteLine(string.Concat("        public async Task<List<Application.Responses.", table.TableName, ".GetResponse>> Get", table.TableName, "s(Application.Queries.", table.TableName, ".GetQuery query)"));
            outputFile.WriteLine(string.Concat("        {"));
            outputFile.WriteLine(string.Concat("            var parameters = new SqlParameter[]"));
            outputFile.WriteLine(string.Concat("                {"));
            //if (table.GetBySpecificField == null)
            //{
                outputFile.WriteLine(string.Concat("                        new SqlParameter() {ParameterName = \"@PageNumber\", SqlDbType =  System.Data.SqlDbType.Int, Value = query.PageNumber},"));
                outputFile.WriteLine(string.Concat("                        new SqlParameter() {ParameterName = \"@RowsOfPage\", SqlDbType =  System.Data.SqlDbType.Int, Value = query.RowsOfPage},"));
                outputFile.WriteLine(string.Concat("                        new SqlParameter() {ParameterName = \"@ExistingRows\", SqlDbType =  System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Output}"));
            //}
            //else
            //    outputFile.WriteLine(string.Concat("                        new SqlParameter() {ParameterName = \"@", table.GetBySpecificField, "\", SqlDbType =  System.Data.SqlDbType.", SqlDataAdapter.GetStringSQLDBType(columnsInfo.First(f => f.Name == table.GetBySpecificField).DataType), ", Value = query.", table.GetBySpecificField, "},"));

            outputFile.WriteLine(string.Concat("                };"));
            outputFile.WriteLine($"            var result = new List<Context.StoredProcedureResult.Queries.{table.TableName}GetPaginatedResult>();");
            //if (table.GetBySpecificField == null)
                outputFile.WriteLine(string.Concat("            await Task.Run(() => { result = _context.", table.TableName, "s.FromSqlRaw(\"[", table.SchemaName, "].[", table.TableName, "_Get_Paginated] @PageNumber, @RowsOfPage, @ExistingRows OUTPUT\", parameters).ToList(); });"));
            //else
            //    outputFile.WriteLine(string.Concat("            await Task.Run(() => { result = _context.", table.TableName, "s.FromSqlRaw(\"[", table.SchemaName, "].[", table.TableName, "_Get_By", table.GetBySpecificField, "] @", table.GetBySpecificField, " \", parameters).ToList(); });"));


            //if (table.GetBySpecificField == null)
            //{
                outputFile.WriteLine(string.Concat("            var existingRows = (int)parameters[2].Value;"));
                outputFile.WriteLine(string.Concat("            return ", table.TableName, "Mapper.Map(result, query.RowsOfPage,existingRows);"));
            //}
            //else
            //    outputFile.WriteLine(string.Concat("            return ", table.TableName, "Mapper.Map(result, 1, 1);"));

            outputFile.WriteLine(string.Concat("        }"));
            outputFile.WriteLine(string.Concat("        public async Task<Application.Responses.", table.TableName, ".GetResponse> Get", table.TableName, "(Domain.Entities.", table.TableName, " query)"));
            outputFile.WriteLine(string.Concat("        {"));
            outputFile.WriteLine(string.Concat("            var parameters = new SqlParameter[]"));
            outputFile.WriteLine(string.Concat("                {"));
            outputFile.WriteLine(string.Concat("                        new SqlParameter() {ParameterName = \"@", pk.ColumnName, "\", SqlDbType =  System.Data.SqlDbType.", Helper.GetStringSQLDBType(pk.SqlDataType), ", Value = query.", pk.ColumnName, "},"));
            outputFile.WriteLine(string.Concat("                };"));
            outputFile.WriteLine($"            var result = new List<Context.StoredProcedureResult.Queries.{table.TableName}GetPaginatedResult>();");
            outputFile.WriteLine(string.Concat("            await Task.Run(() => { result = _context.", table.TableName, "s.FromSqlRaw(\"[", table.SchemaName, "].[", table.TableName, "_Get_ById] @", pk.ColumnName, "\", parameters).ToList(); });"));
            outputFile.WriteLine(string.Concat("            return ", table.TableName, "Mapper.Map(result.First());"));
            outputFile.WriteLine(string.Concat("        }"));

            var customGetMethods = table.Columns.Where(f => f.Property.CreateGetBy).ToList();
            foreach (var customMethod in customGetMethods)
            {               
                outputFile.WriteLine($"        public async Task<Application.Responses.{table.TableName}.GetResponse> Get{table.TableName}By{customMethod.ColumnName}(string {customMethod.ColumnName.ToLower()})");
                outputFile.WriteLine("        {");
                outputFile.WriteLine("            var parameters = new SqlParameter[]");
                outputFile.WriteLine("                {");
                outputFile.WriteLine(string.Concat("                    new SqlParameter() {ParameterName = \"@", customMethod.ColumnName, "\", SqlDbType =  System.Data.SqlDbType.", Helper.GetStringSQLDBType(customMethod.SqlDataType), ", Value = ", customMethod.ColumnName.ToLower(), "},"));
                outputFile.WriteLine("                };");
                outputFile.WriteLine($"            var result = new List<Context.StoredProcedureResult.Queries.{table.TableName}GetPaginatedResult>();");
                outputFile.WriteLine(string.Concat("            await Task.Run(() => { result = _context.", table.TableName, "s.FromSqlRaw(\"", table.SchemaName, ".", table.TableName, "_Get_By",customMethod.ColumnName," @", customMethod.ColumnName, "\", parameters).ToList() ;});"));
                outputFile.WriteLine($"            var {Helper.GetCamel(table.TableName)} = result.FirstOrDefault();");
                outputFile.WriteLine($"            if ({Helper.GetCamel(table.TableName)} != null)");
                outputFile.WriteLine($"                return {table.TableName}Mapper.Map({Helper.GetCamel(table.TableName)});");
                outputFile.WriteLine("            return null;");
                outputFile.WriteLine("        }");
                outputFile.WriteLine("");
            }



            outputFile.WriteLine(string.Concat("    }"));
            outputFile.WriteLine(string.Concat("}"));
            outputFile.Close();
            outputFile.Dispose();
        }
    }
}
