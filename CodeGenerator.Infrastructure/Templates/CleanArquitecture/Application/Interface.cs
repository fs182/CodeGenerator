﻿using CodeGenerator.Infrastructure.Context.Models;
using System.Text;


namespace CodeGenerator.Infrastructure.Templates.CleanArquitecture.Application
{
    public static class Interface
    {
        public static void WriteCommandsInterfaces(Project project, Table table)
        {
            var pk = table.Columns.First(f => f.IsPrimaryKey);
            if (!Directory.Exists(Path.Combine(project.ApplicationInterfacesPath, table.TableName)))
                Directory.CreateDirectory(Path.Combine(project.ApplicationInterfacesPath, table.TableName));
            using StreamWriter outputFile = new(Path.Combine(project.ApplicationInterfacesPath, table.TableName, string.Concat("I", table.TableName, "CommandRepository.cs")), false, Encoding.UTF8);
                outputFile.WriteLine("using MediatR;");
            outputFile.WriteLine(string.Concat("namespace ", project.Namespace, ".Application.Interfaces"));
            outputFile.WriteLine(string.Concat("{"));
            outputFile.WriteLine(string.Concat("    public partial interface ICatalogCommandRepository"));
            outputFile.WriteLine(string.Concat("    {"));
            outputFile.WriteLine($"        Task<List<Responses.{table.TableName}.CommandResponse>> {table.TableName}Update(Commands.{table.TableName}.UpdateCommand command);");
            outputFile.WriteLine($"        Task<List<Responses.{table.TableName}.CommandResponse>> {table.TableName}Delete(Commands.{table.TableName}.DeleteCommand command);");
            outputFile.WriteLine($"        Task<List<Responses.{table.TableName}.CommandResponse>> {table.TableName}Create(Commands.{table.TableName}.CreateCommand command);");
            outputFile.WriteLine($"        Task<{Helper.GetStringNetCoreType(pk.SqlDataType)}> {table.TableName}CreateOnly(Commands.{table.TableName}.CreateCommand command);");
            outputFile.WriteLine(string.Concat("    }"));
            outputFile.WriteLine(string.Concat("}"));
            outputFile.Close();
            outputFile.Dispose();
        }


        public static void WriteQueriesInterfaces(Project project, Table table)
        {
            if (!Directory.Exists(Path.Combine(project.ApplicationInterfacesPath)))
                Directory.CreateDirectory(Path.Combine(project.ApplicationInterfacesPath));
            using StreamWriter outputFile = new(Path.Combine(project.ApplicationInterfacesPath, table.TableName, string.Concat("I", table.TableName, "QueryRepository.cs")), false, Encoding.UTF8);
            outputFile.WriteLine(string.Concat("namespace ", project.Namespace, ".Application.Interfaces"));
            outputFile.WriteLine(string.Concat("{"));
            outputFile.WriteLine(string.Concat("    public partial interface ICatalogQueryRepository"));
            outputFile.WriteLine(string.Concat("    {"));

            outputFile.WriteLine($"        Task<List<Responses.{table.TableName}.GetResponse>> Get{table.TableName}s(Queries.{table.TableName}.GetQuery query);");
            outputFile.WriteLine($"        Task<Responses.{table.TableName}.GetResponse> Get{table.TableName}(Domain.Entities.{table.TableName} query);");

            var customGetMethods = table.Columns.Where(f => f.Property.CreateGetBy).ToList();

            foreach (var customMethod in customGetMethods)
            {
                if(customMethod.Property.CreateGetByReturnList)
                    outputFile.WriteLine($"        Task<List<Responses.{table.TableName}.GetResponse>> Get{table.TableName}By{customMethod.ColumnName}({Helper.GetStringNetCoreType(customMethod.SqlDataType)} {Helper.GetCamel(customMethod.ColumnName)});");
                else
                    outputFile.WriteLine($"        Task<Responses.{table.TableName}.GetResponse> Get{table.TableName}By{customMethod.ColumnName}({Helper.GetStringNetCoreType(customMethod.SqlDataType)} {Helper.GetCamel(customMethod.ColumnName)});");
            }
                
            outputFile.WriteLine(string.Concat("    }"));
            outputFile.WriteLine(string.Concat("}"));
            outputFile.Close();
            outputFile.Dispose();
        }
    }
}
