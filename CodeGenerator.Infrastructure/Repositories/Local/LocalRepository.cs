using AutoMapper;
using CodeGenerator.Application.Commands.General;
using CodeGenerator.Application.Interfaces.External;
using CodeGenerator.Application.Responses;
using CodeGenerator.Infrastructure.Context.Scripts;
using CodeGenerator.Infrastructure.Context;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace CodeGenerator.Infrastructure.Repositories.Local
{
    public partial class LocalRepository(LocalContext context, IMapper mapper) : IExternalRepository
    {
        public async Task PopulateTable(List<TableResponse> tables)
        {
            
            await Task.Run(() => {
                context.Database.ExecuteSql($@"
                    SELECT	
                        {2} as ProyectId,
		                t.object_id as ObjectId, 
		                SCHEMA_NAME(t.schema_id) as SchemaName,
    		            t.name as TableName
                    FROM sys.tables AS t
                    WHERE T.name NOT IN ('sysdiagrams')
                    ORDER BY t.schema_id, t.Name");
            });
            return mapper.Map<List<TableResponse>>(result);
        }
    }
}
