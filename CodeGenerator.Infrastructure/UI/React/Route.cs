using CodeGenerator.Infrastructure.Context.Models;
using System.Text;

namespace CodeGenerator.Infrastructure.UI.React
{
    public static class Route
    {
        public static void WriteRoutes(Project project)
        {
            if (!Directory.Exists(Path.Combine(project.UIRoutesPath)))
                Directory.CreateDirectory(Path.Combine(project.UIRoutesPath));
            using StreamWriter outputFile = new(Path.Combine(project.UIRoutesPath, string.Concat("catalogsRoutes.js")), false, Encoding.UTF8);
            outputFile.WriteLine(string.Concat("import React from \"react\";"));
            outputFile.WriteLine(string.Concat("import Page from \"@jumbo/shared/Page\";"));

            foreach (var entity in project.Tables)
                outputFile.WriteLine(string.Concat("import ", entity.TableName, " from \"app/pages/catalogs/", entity.TableName, "\";"));
            
            outputFile.WriteLine(string.Concat("const catalogsRoutes = ["));
            foreach (var entity in project.Tables)
            {
                 outputFile.WriteLine(string.Concat("    {"));
                 outputFile.WriteLine(string.Concat("        path: \"/catalogs/", entity.TableName, "\","));
                 outputFile.WriteLine(string.Concat("        element: <Page component={", entity.TableName, "} />,"));
                 outputFile.WriteLine(string.Concat("    },"));
            }
            outputFile.WriteLine(string.Concat("];"));
            outputFile.WriteLine(string.Concat("export default catalogsRoutes;"));
            outputFile.Dispose();
        }
    }
}
