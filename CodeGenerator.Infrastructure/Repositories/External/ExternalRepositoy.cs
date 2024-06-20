using AutoMapper;
using CodeGenerator.Application.Commands.General;
using CodeGenerator.Application.Interfaces.External;
using CodeGenerator.Application.Responses;
using CodeGenerator.Infrastructure.Context;
using CodeGenerator.Infrastructure.Context.Models;
using CodeGenerator.Infrastructure.Context.Scripts;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace CodeGenerator.Infrastructure.Repositories.External
{
    public partial class ExternalRepository(ExternalContext context, IMapper mapper) : IExternalRepository
    {
        public async Task<List<TableResponse>> GetMetadataTable(PopulateCommand command)
        {
            var query = FormattableStringFactory.Create($"{string.Format(TableScript.GetMetadataTables, command.ProjectId, command.AuditId)}");
            var result = new List<Table>();
            await Task.Run(() => {
                result = [.. context.Database.SqlQuery<Table>($@"
                    SELECT	
                        {2} as ProyectId,
		                t.object_id as ObjectId, 
		                SCHEMA_NAME(t.schema_id) as SchemaName,
    		            t.name as TableName
                    FROM sys.tables AS t
                    WHERE T.name NOT IN ('sysdiagrams')
                    ORDER BY t.schema_id, t.Name")]; 
            });
            return mapper.Map<List<TableResponse>>(result);
        }
    }   
}