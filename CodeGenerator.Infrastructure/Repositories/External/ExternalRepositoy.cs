using CodeGenerator.Application.Commands.General;
using CodeGenerator.Application.Interfaces.External;
using CodeGenerator.Application.Responses;
using CodeGenerator.Infrastructure.Context;
using CodeGenerator.Infrastructure.Context.Scripts;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace CodeGenerator.Infrastructure.Repositories.External
{
    public partial class ExternalRepository : IExternalRepository
    {
        private readonly ExternalContext _context;
        public ExternalRepository(ExternalContext context) => _context = context;
        public async Task GetMetadataTable(PopulateCommand command)
        {
            var query = FormattableStringFactory.Create($"{string.Format(TableScript.GetMetadataTables, command.ProjectId, command.AuditId)}");
            
            await Task.Run(() => {
                var rowsModified =  _context.Database.SqlQuery<TableResponse>($@"
                    SELECT	
                        0 as TableId,
                        {2} as ProyectId,
		                t.object_id as ObjectId, 
		                SCHEMA_NAME(t.schema_id) as SchemaName,
    		            t.name as TableName,
                        cast({1} as bigint) as AuditId
                    FROM sys.tables AS t
                    WHERE T.name NOT IN ('__EFMigrationsHistory','sysdiagrams')
                    ORDER BY t.schema_id, t.Name").ToList(); 
            });
        }


    }   
}