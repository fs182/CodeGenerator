using CodeGenerator.Infrastructure.Context.Models;
using CodeGenerator.Infrastructure.UI.React;

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
            //await Context.WriteCommandContext(tablesInfo, env).ConfigureAwait(false);
            //await Context.WriteQueryContext(tablesInfo, env).ConfigureAwait(false);
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
            //await StoredProcedure.WriteCommands(columnsInfo, env, entity);
            //await StoredProcedure.WriteQueries(columnsInfo, env, entity, tablesInfo, connectionString);
            //await Mapper.WriteMappers(columnsInfo, env, entity, tablesInfo, connectionString);

            await Task.Run(() => { Component.WriteComponents(project, table); });
            await Task.Run(() => { Component.WriteComponentItems(project, table); });

            //await Interface.WriteCommandsInterfaces(columnsInfo, entity, env);
            //await Interface.WriteQueriesInterfaces(columnsInfo, entity, env);
            //await Repository.WriteCommandRepository(tablesInfo, entity, env, connectionString);
            //await Repository.WriteQueryRepository(entity, env, connectionString);
        }
    }
}
