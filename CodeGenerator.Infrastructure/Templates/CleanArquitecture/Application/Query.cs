using CodeGenerator.Infrastructure.Context.Models;
using System.Text;

namespace CodeGenerator.Infrastructure.Templates.CleanArquitecture.Application
{
    public static class Query
    {
        public static void WriteGetQuery(Project project, Table table)
        {
            if (!Directory.Exists(Path.Combine(project.ApplicationQueriesPath, table.TableName)))
                Directory.CreateDirectory(Path.Combine(project.ApplicationQueriesPath, table.TableName));
            using StreamWriter outputFile = new(Path.Combine(project.ApplicationQueriesPath, table.TableName, string.Concat("GetQuery.cs")), false, Encoding.UTF8);
            outputFile.WriteLine(string.Concat("using ", project.Namespace, ".Application.Base;"));
            outputFile.WriteLine(string.Concat("using ", project.Namespace, ".Application.Responses.", table.TableName, ";"));
            outputFile.WriteLine("using MediatR;");
            outputFile.WriteLine(string.Concat("namespace ", project.Namespace, ".Application.Queries.", table.TableName));
            outputFile.WriteLine("{");
            outputFile.WriteLine("    public class GetQuery : GetPaginated, IRequest<List<GetResponse>>");
            outputFile.WriteLine("    {");
            outputFile.WriteLine("    }");
            outputFile.WriteLine("}");
            outputFile.Close();
            outputFile.Dispose();
        }

        public static void WriteGetByCustomQuery(Project project, Table table)
        {
            var customGetMethods = table.Columns.Where(f => f.Property.CreateGetBy && f.Property.CreateGetByIncludeController).ToList();
            foreach (var customMethod in customGetMethods)
            {
                if (!Directory.Exists(Path.Combine(project.ApplicationQueriesPath, table.TableName)))
                    Directory.CreateDirectory(Path.Combine(project.ApplicationQueriesPath, table.TableName));
                using StreamWriter outputFile = new(Path.Combine(project.ApplicationQueriesPath, table.TableName, string.Concat("GetByColumnQuery.cs")), false, Encoding.UTF8);
                outputFile.WriteLine("using MediatR;");
                outputFile.WriteLine(string.Concat("using ", project.Namespace, ".Application.Responses.", table.TableName, ";"));
                outputFile.WriteLine(string.Concat("namespace ", project.Namespace, ".Application.Queries.", table.TableName));
                outputFile.WriteLine("{");

                if(customMethod.Property.CreateGetByReturnList)
                    outputFile.WriteLine("    public class GetByColumnQuery : IRequest<List<GetResponse>>");
                else
                    outputFile.WriteLine("    public class GetByColumnQuery : IRequest<GetResponse>");
                outputFile.WriteLine("    {");
                outputFile.WriteLine($"        public {Helper.GetStringNetCoreType(customMethod.SqlDataType)} {customMethod.ColumnName} {{ get; set; }}");
                outputFile.WriteLine("    }");
                outputFile.WriteLine("}");
                outputFile.Close();
                outputFile.Dispose();
            }
        }
    }
}
