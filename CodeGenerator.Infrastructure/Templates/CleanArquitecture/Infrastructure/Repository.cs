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

            outputFile.WriteLine(string.Concat($"using {project.Namespace}.Domain.Entities;"));
            outputFile.WriteLine(string.Concat("using Microsoft.Data.SqlClient;"));
            outputFile.WriteLine(string.Concat("using Microsoft.EntityFrameworkCore;"));

            outputFile.WriteLine(string.Concat("using MediatR;"));

            outputFile.WriteLine(string.Concat("namespace ", project.Namespace, ".Infrastructure.Repositories"));
            outputFile.WriteLine(string.Concat("{"));
            outputFile.WriteLine(string.Concat("    public partial class CatalogCommandRepository"));
            outputFile.WriteLine(string.Concat("    {"));

            var pk = table.Columns.First(f => f.IsPrimaryKey);

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

                var size = "";
                if(Helper.GetStringNetCoreType(c.SqlDataType) == "string")
                    size = Helper.GetStringNetCoreType(c.SqlDataType) == "string" ? $" Size = {c.MaxLength}," : "";
                if (Helper.GetStringNetCoreType(c.SqlDataType) == "decimal" && c.SqlDataType != "money")
                    size = Helper.GetStringNetCoreType(c.SqlDataType) == "string" ? $" Precision = {c.Precision}, Scale = {c.Scale}," : "";

                outputFile.WriteLine(string.Concat("                        new SqlParameter() {ParameterName = \"@", c.ColumnName, "\", SqlDbType =  System.Data.SqlDbType.", Helper.GetStringSQLDBType(c.SqlDataType), ",", size, c.IsNullable ? " IsNullable = true," : "", " Value = command.", c.ColumnName, nullableValue, "},"));
            }
            outputFile.WriteLine("                };");
            outputFile.WriteLine($"            var result = new List<Context.StoredProcedureResult.Commands.{table.TableName}CommandResult>();");
            outputFile.Write(string.Concat("            await Task.Run(() => { result = _context.", table.TableName, "s.FromSqlRaw(\"[", table.SchemaName, "].[", table.TableName, "_Update] @PageNumber, @RowsOfPage, @ExistingRows OUTPUT,"));
            foreach (var c in table.Columns.Where(f => f.ColumnName != "AuditoriaId"))
                outputFile.Write($" @{c.ColumnName},");
            outputFile.WriteLine(" @AuditoriaId\", parameters).ToList(); });");
            outputFile.WriteLine("            var existingRows = (int)parameters[2].Value;");
            outputFile.WriteLine($"            var mappedResult = _mapper.Map<List<Application.Responses.{table.TableName}.CommandResponse>>(result);");
            outputFile.WriteLine($"            if (mappedResult.Count != 0)");
            outputFile.WriteLine($"                mappedResult[0].TotalPages = AdditionalFields.SetTotalPages(command.RowsOfPage, existingRows);");
            outputFile.WriteLine($"            return mappedResult;");
            outputFile.WriteLine("        }");



            outputFile.WriteLine($"        public async Task<List<Application.Responses.{table.TableName}.CommandResponse>> {table.TableName}Delete(Application.Commands.{table.TableName}.DeleteCommand command)");


            outputFile.WriteLine("        {");
            outputFile.WriteLine("            var parameters = new SqlParameter[]");
            outputFile.WriteLine("                {");
            outputFile.WriteLine("                        new SqlParameter() {ParameterName = \"@PageNumber\", SqlDbType =  System.Data.SqlDbType.Int, Value = command.PageNumber},");
            outputFile.WriteLine("                        new SqlParameter() {ParameterName = \"@RowsOfPage\", SqlDbType =  System.Data.SqlDbType.Int, Value = command.RowsOfPage},");
            outputFile.WriteLine("                        new SqlParameter() {ParameterName = \"@ExistingRows\", SqlDbType =  System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Output},");
            var mainColumn = table.Columns.FirstOrDefault(f => f.IsPrimaryKey);

            outputFile.WriteLine(string.Concat("                        new SqlParameter() {ParameterName = \"@", pk.ColumnName, "\", SqlDbType =  System.Data.SqlDbType.Int, Value = command.", pk.ColumnName, "}"));
            outputFile.WriteLine("                };");
            outputFile.WriteLine($"            var result = new List<Context.StoredProcedureResult.Commands.{table.TableName}CommandResult>();");
            outputFile.WriteLine(string.Concat("            await Task.Run(() => { result = _context.", table.TableName, "s.FromSqlRaw(\"[", table.SchemaName, "].[", table.TableName, "_Delete] @PageNumber, @RowsOfPage, @ExistingRows OUTPUT, @", pk.ColumnName, "\", parameters).ToList(); });"));
            outputFile.WriteLine("            var existingRows = (int)parameters[2].Value;");
            outputFile.WriteLine($"            var mappedResult = _mapper.Map<List<Application.Responses.{table.TableName}.CommandResponse>>(result);");
            outputFile.WriteLine($"            if (mappedResult.Count != 0)");
            outputFile.WriteLine($"                mappedResult[0].TotalPages = AdditionalFields.SetTotalPages(command.RowsOfPage, existingRows);");
            outputFile.WriteLine($"            return mappedResult;");
            outputFile.WriteLine("        }");

            outputFile.WriteLine($"        public async Task<List<Application.Responses.{table.TableName}.CommandResponse>> {table.TableName}Create(Application.Commands.{table.TableName}.CreateCommand command)");
            outputFile.WriteLine("        {");
            outputFile.WriteLine("            var parameters = new SqlParameter[]");
            outputFile.WriteLine("                {");
            outputFile.WriteLine("                    new SqlParameter() {ParameterName = \"@PageNumber\", SqlDbType =  System.Data.SqlDbType.Int, Value = command.PageNumber},");
            outputFile.WriteLine("                        new SqlParameter() {ParameterName = \"@RowsOfPage\", SqlDbType =  System.Data.SqlDbType.Int, Value = command.RowsOfPage},");
            outputFile.WriteLine("                        new SqlParameter() {ParameterName = \"@ExistingRows\", SqlDbType =  System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Output},");

            foreach (var c in table.Columns.Where(f => !f.IsIdentity))
            {
                var nullableValue = "";
                if (c.IsNullable)
                    nullableValue = string.Concat(" == null ? DBNull.Value : command.", c.ColumnName);

                var size = "";
                if (Helper.GetStringNetCoreType(c.SqlDataType) == "string")
                    size = Helper.GetStringNetCoreType(c.SqlDataType) == "string" ? $" Size = {c.MaxLength}," : "";
                if (Helper.GetStringNetCoreType(c.SqlDataType) == "decimal" && c.SqlDataType != "money")
                    size = Helper.GetStringNetCoreType(c.SqlDataType) == "string" ? $" Precision = {c.Precision}, Scale = {c.Scale}," : "";

                outputFile.WriteLine(string.Concat("                        new SqlParameter() {ParameterName = \"@", c.ColumnName, "\", SqlDbType =  System.Data.SqlDbType.", Helper.GetStringSQLDBType(c.SqlDataType), ",",size, c.IsNullable ? " IsNullable = true," : "", " Value = command.", c.ColumnName, nullableValue, "},"));
            }

            outputFile.WriteLine("                };");
            outputFile.WriteLine($"            var result = new List<Context.StoredProcedureResult.Commands.{table.TableName}CommandResult>();");
            outputFile.Write(string.Concat("            await Task.Run(() => { result = _context.", table.TableName, "s.FromSqlRaw(\"[", table.SchemaName, "].[", table.TableName, "_Insert] @PageNumber, @RowsOfPage, @ExistingRows OUTPUT, "));

            foreach (var c in table.Columns.Where(f => !f.IsIdentity && f.ColumnName != "AuditoriaId"))
                outputFile.Write($"@{c.ColumnName}, ");

            outputFile.WriteLine("@AuditoriaId\", parameters).ToList(); });");
            outputFile.WriteLine("            var existingRows = (int)parameters[2].Value;");
            outputFile.WriteLine($"            var mappedResult = _mapper.Map<List<Application.Responses.{table.TableName}.CommandResponse>>(result);");
            outputFile.WriteLine($"            if (mappedResult.Count != 0)");
            outputFile.WriteLine($"                mappedResult[0].TotalPages = AdditionalFields.SetTotalPages(command.RowsOfPage, existingRows);");
            outputFile.WriteLine($"            return mappedResult;");
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

                var size = "";
                if (Helper.GetStringNetCoreType(c.SqlDataType) == "string")
                    size = Helper.GetStringNetCoreType(c.SqlDataType) == "string" ? $" Size = {c.MaxLength}," : "";
                if (Helper.GetStringNetCoreType(c.SqlDataType) == "decimal" && c.SqlDataType != "money")
                    size = Helper.GetStringNetCoreType(c.SqlDataType) == "string" ? $" Precision = {c.Precision}, Scale = {c.Scale}," : "";

                outputFile.WriteLine(string.Concat("                        new SqlParameter() {ParameterName = \"@", c.ColumnName, "\", SqlDbType =  System.Data.SqlDbType.", Helper.GetStringSQLDBType(c.SqlDataType), ",", size, c.IsNullable ? " IsNullable = true," : "", " Value = command.", c.ColumnName, nullableValue, "},"));
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
            outputFile.WriteLine(string.Concat($"using {project.Namespace}.Domain.Entities;"));
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

            outputFile.WriteLine(string.Concat("                        new SqlParameter() {ParameterName = \"@PageNumber\", SqlDbType =  System.Data.SqlDbType.Int, Value = query.PageNumber},"));
            outputFile.WriteLine(string.Concat("                        new SqlParameter() {ParameterName = \"@RowsOfPage\", SqlDbType =  System.Data.SqlDbType.Int, Value = query.RowsOfPage},"));
            outputFile.WriteLine(string.Concat("                        new SqlParameter() {ParameterName = \"@ExistingRows\", SqlDbType =  System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Output}"));


            outputFile.WriteLine(string.Concat("                };"));
            outputFile.WriteLine($"            var result = new List<Context.StoredProcedureResult.Queries.{table.TableName}GetPaginatedResult>();");

            outputFile.WriteLine(string.Concat("            await Task.Run(() => { result = _context.", table.TableName, "s.FromSqlRaw(\"[", table.SchemaName, "].[", table.TableName, "_Get_Paginated] @PageNumber, @RowsOfPage, @ExistingRows OUTPUT\", parameters).ToList(); });"));

            outputFile.WriteLine(string.Concat("            var existingRows = (int)parameters[2].Value;"));
            outputFile.WriteLine($"            var mappedResult = _mapper.Map<List<Application.Responses.{table.TableName}.GetResponse>>(result);");
            outputFile.WriteLine($"            if (mappedResult.Count != 0)");
            outputFile.WriteLine($"                mappedResult[0].TotalPages = AdditionalFields.SetTotalPages(query.RowsOfPage, existingRows);");
            outputFile.WriteLine($"            return mappedResult;");

            outputFile.WriteLine(string.Concat("        }"));
            outputFile.WriteLine(string.Concat("        public async Task<Application.Responses.", table.TableName, ".GetResponse> Get", table.TableName, "(Domain.Entities.", table.TableName, " query)"));
            outputFile.WriteLine(string.Concat("        {"));
            outputFile.WriteLine(string.Concat("            var parameters = new SqlParameter[]"));
            outputFile.WriteLine(string.Concat("                {"));
            outputFile.WriteLine(string.Concat("                        new SqlParameter() {ParameterName = \"@", pk.ColumnName, "\", SqlDbType =  System.Data.SqlDbType.", Helper.GetStringSQLDBType(pk.SqlDataType), ", Value = query.", pk.ColumnName, "},"));
            outputFile.WriteLine(string.Concat("                };"));
            outputFile.WriteLine($"            var result = new List<Context.StoredProcedureResult.Queries.{table.TableName}GetPaginatedResult>();");
            outputFile.WriteLine(string.Concat("            await Task.Run(() => { result = _context.", table.TableName, "s.FromSqlRaw(\"[", table.SchemaName, "].[", table.TableName, "_Get_ById] @", pk.ColumnName, "\", parameters).ToList(); });"));
            outputFile.WriteLine($"            return _mapper.Map<Application.Responses.{table.TableName}.GetResponse>(result);");
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
                outputFile.WriteLine(string.Concat("            await Task.Run(() => { result = _context.", table.TableName, "s.FromSqlRaw(\"", table.SchemaName, ".", table.TableName, "_Get_By", customMethod.ColumnName, " @", customMethod.ColumnName, "\", parameters).ToList() ;});"));
                outputFile.WriteLine($"            var {Helper.GetCamel(table.TableName)} = result.FirstOrDefault();");
                outputFile.WriteLine($"            return _mapper.Map<Application.Responses.{table.TableName}.GetResponse>(result);");
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
