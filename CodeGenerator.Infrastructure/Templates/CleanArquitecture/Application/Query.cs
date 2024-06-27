using CodeGenerator.Infrastructure.Context.Models;
using System.Text;

namespace CodeGenerator.Infrastructure.Templates.CleanArquitecture.Application
{
    public static class Query
    {
        public static void WriteGetQuery(Project project, Table table)
        {
            //if (entity.ExcludeGetQuery)
            //    return;

            if (!Directory.Exists(Path.Combine(project.ApplicationQueriesPath, table.TableName)))
                Directory.CreateDirectory(Path.Combine(project.ApplicationQueriesPath, table.TableName));
            using StreamWriter outputFile = new(Path.Combine(project.ApplicationQueriesPath, table.TableName, string.Concat("GetQuery.cs")), false, Encoding.UTF8);
            outputFile.WriteLine(string.Concat("using ", project.Namespace, ".Application.Base;"));
            outputFile.WriteLine(string.Concat("using ", project.Namespace, ".Application.Responses.", table.TableName, ";"));
            outputFile.WriteLine("using MediatR;");
            outputFile.WriteLine(string.Concat("namespace ", project.Namespace, ".Application.Queries.", table.TableName));
            outputFile.WriteLine("{");
            //if (entity.GetBySpecificField == null)
                outputFile.WriteLine("    public class GetQuery : GetPaginated, IRequest<List<GetResponse>>");
            //else
            //    outputFile.WriteLine("    public class GetQuery : IRequest<List<GetResponse>>");
            outputFile.WriteLine("    {");
            //if (entity.GetBySpecificField != null)
            //    outputFile.WriteLine(string.Concat($"       public {SqlDataAdapter.GetStringNetCoreType(columnsInfo.First(f => f.Name == entity.GetBySpecificField).DataType)} {entity.GetBySpecificField} ", " { get; set; }"));

            outputFile.WriteLine("    }");
            outputFile.WriteLine("}");
            outputFile.Close();
            outputFile.Dispose();
        }
    }
}
