using CodeGenerator.Infrastructure.Context.Models;
using CodeGenerator.Infrastructure.Templates.CleanArquitecture.Application;
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
            //await Get.WriteGetPaginated(columnsInfo, env, entity, tablesInfo, connectionString);
            //await Get.WriteGetById(columnsInfo, env, entity, tablesInfo, connectionString);
            //await Get.WriteGetByNombre(columnsInfo, env, entity, tablesInfo, connectionString);
            //await Insert.WriteInsertSP(columnsInfo, env, entity);
            //await Insert.WriteInsertOnlySP(columnsInfo, env, entity);
            //await Update.WriteUpdateSP(columnsInfo, env, entity);
            //await Delete.WriteDeleteSP(columnsInfo, env, entity);
            //await Controller.WriteController(columnsInfo, env, entity);
            await Task.Run(() => { Command.WriteCreateCommand(project, table); });
            await Task.Run(() => { Command.WriteUpdateCommand(project, table); });
            await Task.Run(() => { Command.WriteDeleteCommand(project, table); });
            //await Handler.WriteGetHandler(columnsInfo, env, entity);
            //await Handler.WriteUpdateHandler(columnsInfo, env, entity);
            //await Handler.WriteCreateHandler(columnsInfo, env, entity);
            //await Handler.WriteDeleteHandler(columnsInfo, env, entity);
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
