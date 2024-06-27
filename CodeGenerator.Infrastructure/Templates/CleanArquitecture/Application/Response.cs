using CodeGenerator.Infrastructure.Context.Models;
using System.Text;

namespace CodeGenerator.Infrastructure.Templates.CleanArquitecture.Application
{
    public static class Response
    {
        public static void WriteCommandResponse(Project project, Table table)
        {
            if (!Directory.Exists(Path.Combine(project.ApplicationResponsesPath, table.TableName)))
                Directory.CreateDirectory(Path.Combine(project.ApplicationResponsesPath, table.TableName));
            using StreamWriter outputFile = new(Path.Combine(project.ApplicationResponsesPath, table.TableName, string.Concat("CommandResponse.cs")), false, Encoding.UTF8);
            outputFile.WriteLine(string.Concat("namespace ", project.Namespace, ".Application.Responses.", table.TableName));
            outputFile.WriteLine("{");
            outputFile.WriteLine("    public class CommandResponse : GetResponse");
            outputFile.WriteLine("    {");
            outputFile.WriteLine("    }");
            outputFile.WriteLine("}");
            outputFile.Close();
            outputFile.Dispose();
        }
        public static void WriteGetResponse(Project project, Table table)
        {
            if (!Directory.Exists(Path.Combine(project.ApplicationResponsesPath, table.TableName)))
                Directory.CreateDirectory(Path.Combine(project.ApplicationResponsesPath, table.TableName));
            using StreamWriter outputFile = new(Path.Combine(project.ApplicationResponsesPath, table.TableName, string.Concat("GetResponse.cs")), false, Encoding.UTF8);
            outputFile.WriteLine(string.Concat("namespace ", project.Namespace, ".Application.Responses.", table.TableName));
            outputFile.WriteLine("{");
            outputFile.WriteLine(string.Concat("    public class GetResponse : Domain.Entities.", table.TableName));
            outputFile.WriteLine("    {");
            outputFile.WriteLine("    }");
            outputFile.WriteLine("}");
            outputFile.Close();
            outputFile.Dispose();
        }
    }
}
