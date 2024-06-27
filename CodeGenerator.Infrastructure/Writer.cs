using CodeGenerator.Infrastructure.Context.Models;
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
            //await Command.WriteCreateCommand(columnsInfo, env, entity);
            //await Command.WriteUpdateCommand(columnsInfo, env, entity);
            //await Command.WriteDeleteCommand(columnsInfo, env, entity);
            //await Handler.WriteGetHandler(columnsInfo, env, entity);
            //await Handler.WriteUpdateHandler(columnsInfo, env, entity);
            //await Handler.WriteCreateHandler(columnsInfo, env, entity);
            //await Handler.WriteDeleteHandler(columnsInfo, env, entity);
            //await Query.WriteGetQuery(columnsInfo, env, entity);
            //await Response.WriteCommandResponse(columnsInfo, env, entity);
            //await Response.WriteGetResponse(columnsInfo, env, entity);
            //await Entities.WriteEntities(columnsInfo, env, entity, tablesInfo, connectionString);
            await Task.Run(() => { StoredProcedureResult.WriteCommands(project, table); });
            await Task.Run(() => { StoredProcedureResult.WriteQueries(project, table); });
            //await Mapper.WriteMappers(columnsInfo, env, entity, tablesInfo, connectionString);

            await Task.Run(() => { Component.WriteComponents(project, table); });
            await Task.Run(() => { Component.WriteComponentItems(project, table); });

            //await Interface.WriteCommandsInterfaces(columnsInfo, entity, env);
            //await Interface.WriteQueriesInterfaces(columnsInfo, entity, env);
            await Task.Run(() => { Repository.WriteCommandRepository(project, table); });
            await Task.Run(() => { Repository.WriteQueryRepository(project, table); });
        }
    }
}
