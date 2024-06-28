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
            await Task.Run(() => { Templates.CleanArquitecture.Infrastructure.Context.WriteCommandContext(project); });
            await Task.Run(() => { Templates.CleanArquitecture.Infrastructure.Context.WriteQueryContext(project); });
            await Task.Run(() => { Route.WriteRoutes(project); });
        }

        private static async Task IndividualFiles(Project project, Table table)
        {
            await Task.Run(() => { Get.WriteGetPaginated(project, table); });
            await Task.Run(() => { Get.WriteGetById(project, table); });
            await Task.Run(() => { Get.WriteGetByNombre(project, table); });
            //await Task.Run(() => { Insert.WriteInsertSP(project, table); });
            //await Task.Run(() => { Insert.WriteInsertOnlySP(project, table); });
            //await Task.Run(() => { Update.WriteUpdateSP(project, table); });
            await Task.Run(() => { Delete.WriteDeleteSP(project, table); });
            await Task.Run(() => { Controller.WriteController(project, table); });
            await Task.Run(() => { Command.WriteCreateCommand(project, table); });
            await Task.Run(() => { Command.WriteUpdateCommand(project, table); });
            await Task.Run(() => { Command.WriteDeleteCommand(project, table); });
            await Task.Run(() => { Handler.WriteGetHandler(project, table);});
            await Task.Run(() => { Handler.WriteUpdateHandler(project, table);});
            await Task.Run(() => { Handler.WriteCreateHandler(project, table);});
            await Task.Run(() => { Handler.WriteDeleteHandler(project, table);});
            await Task.Run(() => { Query.WriteGetQuery(project, table);});
            await Task.Run(() => { Response.WriteCommandResponse(project, table);});
            await Task.Run(() => { Response.WriteGetResponse(project, table); });
            await Task.Run(() => { Entity.WriteEntities(project, table); });
            await Task.Run(() => { StoredProcedureResult.WriteCommands(project, table); });
            await Task.Run(() => { StoredProcedureResult.WriteQueries(project, table); });
            await Task.Run(() => { Mapper.WriteMappers(project, table); });
            await Task.Run(() => { Component.WriteComponents(project, table); });
            await Task.Run(() => { Component.WriteComponentItems(project, table); });
            await Task.Run(() => { Interface.WriteCommandsInterfaces(project, table); });
            await Task.Run(() => { Interface.WriteQueriesInterfaces(project, table); });
            await Task.Run(() => { Repository.WriteCommandRepository(project, table); });
            await Task.Run(() => { Repository.WriteQueryRepository(project, table); });
        }
    }
}
