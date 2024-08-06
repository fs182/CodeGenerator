using CodeGenerator.Infrastructure.Context.Models;
using CodeGenerator.Infrastructure.Templates.CleanArquitecture.API;
using CodeGenerator.Infrastructure.Templates.CleanArquitecture.Application;
using CodeGenerator.Infrastructure.Templates.CleanArquitecture.Database.StoredProcedure;
using CodeGenerator.Infrastructure.Templates.CleanArquitecture.Domain;
using CodeGenerator.Infrastructure.Templates.CleanArquitecture.Infrastructure;
using CodeGenerator.Infrastructure.Templates.CleanArquitecture.UI.React;

namespace CodeGenerator.Infrastructure
{
    public static class Writer
    {

        public static async Task GenerateProject(Project project)
        {
            var tables = project.Tables.Where(f => f.Catalog.IsEnabled).ToList();
            foreach (var t in tables)
            {
                await IndividualFiles(project, t);
            }
            await Task.Run(() => { 
                Templates.CleanArquitecture.Infrastructure.Context.WriteCommandContext(project);
                Templates.CleanArquitecture.Infrastructure.Context.WriteQueryContext(project);
                Mapper.WriteMappers(project);
                Route.WriteRoutes(project);
            });
        }

        private static async Task IndividualFiles(Project project, Table table)
        {
            await Task.Run(() => { 
                Get.WriteGetPaginated(project, table);
                Get.WriteGetById(project, table); 
                Get.WriteGetByCustom(project, table);
                Insert.WriteInsertSP(project, table);
                Insert.WriteInsertOnlySP(project, table);
                Update.WriteUpdateSP(project, table);
                Delete.WriteDeleteSP(project, table);
                Controller.WriteController(project, table); 
                Command.WriteCreateCommand(project, table); 
                Command.WriteUpdateCommand(project, table); 
                Command.WriteDeleteCommand(project, table); 
                Handler.WriteGetHandler(project, table);
                Handler.WriteGetByCustom(project, table);
                Handler.WriteUpdateHandler(project, table);
                Handler.WriteCreateHandler(project, table);
                Handler.WriteDeleteHandler(project, table);
                Query.WriteGetQuery(project, table);
                Query.WriteGetByCustomQuery(project, table);
                Response.WriteCommandResponse(project, table);
                Response.WriteGetResponse(project, table);
                Entity.WriteEntities(project, table);
                StoredProcedureResult.WriteCommands(project, table);
                StoredProcedureResult.WriteQueries(project, table);
                Component.WriteComponents(project, table);
                Component.WriteComponentItems(project, table);
                Component.WriteWizards(project, table);
                Interface.WriteCommandsInterfaces(project, table);
                Interface.WriteQueriesInterfaces(project, table);
                Repository.WriteCommandRepository(project, table);
                Repository.WriteQueryRepository(project, table);
            });
            
        }
    }
}
