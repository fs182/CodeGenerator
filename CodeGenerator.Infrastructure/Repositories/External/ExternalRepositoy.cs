using AutoMapper;
using CodeGenerator.Application.Commands.General;
using CodeGenerator.Application.Interfaces.External;
using CodeGenerator.Application.Responses;
using CodeGenerator.Infrastructure.Context;
using CodeGenerator.Infrastructure.Context.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeGenerator.Infrastructure.Repositories.External
{
    public partial class ExternalRepository(ExternalContext context, IMapper mapper) : IExternalRepository
    {
        public async Task<List<TableResponse>> GetMetadataTable(PopulateCommand command)
        {
            var result = new List<TableDto>();
            await Task.Run(() => {
                result = [.. context.Database.SqlQuery<TableDto>($@"
                    SELECT	
		                cast({command.ProjectId} as smallint) as ProjectId,
                        t.object_id as ObjectId, 
		                SCHEMA_NAME(t.schema_id) as SchemaName,
    		            t.name as TableName
                    FROM sys.tables AS t
                    WHERE T.name NOT IN ('sysdiagrams')
                    ORDER BY t.schema_id, t.Name")]; 
            });
            return mapper.Map<List<TableResponse>>(result);
        }
        public async Task<List<ColumnResponse>> GetMetadataColumn(PopulateCommand command)
        {
            var result = new List<ColumnDto>();
            await Task.Run(() => {
                result = [.. context.Database.SqlQuery<ColumnDto>($@"
                    SELECT 
						cast({command.ProjectId} as smallint) as ProjectId,
						t.object_id as ObjectId,
						SCHEMA_NAME(t.schema_id) AS SchemaName,
						t.name AS TableName,
						cast(c.column_id as tinyint) as ColumnNumber,
						c.name AS ColumnName,		
						st.name as SqlDataType,
						cast(c.max_length as smallint) as MaxLength,
						cast(c.Precision as tinyint) as Precision,
						cast(c.Scale as tinyint) as Scale,
						c.is_nullable as IsNullable,
						c.is_identity as IsIdentity,
						case when pk.object_id is null then cast(0 as bit) else cast(1 as bit) end as IsPrimaryKey,
						case when fk.object_id is null then cast(0 as bit) else cast(1 as bit) end as IsForeignKey,
						fk.SchemaSource,
						fk.TableSource,
						fk.ColumnSource,
						fk.SchemaTarget,
						fk.TableTarget,
						fk.ColumnTarget
					FROM sys.tables AS t
					INNER JOIN sys.columns c ON t.OBJECT_ID = c.OBJECT_ID
					LEFT OUTER JOIN 
					(
						SELECT     x.object_id, IC.COLUMN_ID,
						           COL_NAME(IC.OBJECT_ID,IC.COLUMN_ID) AS COLUMNNAME
						FROM       SYS.INDEXES  X 
						INNER JOIN SYS.INDEX_COLUMNS  IC 
						        ON X.OBJECT_ID = IC.OBJECT_ID
						       AND X.INDEX_ID = IC.INDEX_ID
						WHERE      X.IS_PRIMARY_KEY = 1
					) PK
					ON c.object_id = pk.object_id and c.column_id = pk.column_id
					LEFT OUTER JOIN sys.types ST on c.user_type_id = st.user_type_id
					LEFT OUTER JOIN
					(
						SELECT  
						tab1.object_id,
						col1.column_id,
						sch1.name AS SchemaSource,	
						tab1.name AS TableSource,
						col1.name AS ColumnSource,
						sch2.name AS SchemaTarget,
						tab2.name AS TableTarget,
						col2.name AS ColumnTarget
						FROM sys.foreign_key_columns fkc
						INNER JOIN sys.objects obj
						    ON obj.object_id = fkc.constraint_object_id
						INNER JOIN sys.tables tab1
						    ON tab1.object_id = fkc.parent_object_id
						INNER JOIN sys.schemas sch1
						    ON tab1.schema_id = sch1.schema_id
						INNER JOIN sys.columns col1
						    ON col1.column_id = parent_column_id AND col1.object_id = tab1.object_id
						INNER JOIN sys.tables tab2
						    ON tab2.object_id = fkc.referenced_object_id
						INNER JOIN sys.schemas sch2
						    ON tab1.schema_id = sch2.schema_id
						INNER JOIN sys.columns col2
						    ON col2.column_id = referenced_column_id AND col2.object_id = tab2.object_id
					) Fk
					ON c.object_id = fk.object_id and c.column_id = fk.column_id
					where t.name not in ('__EFMigrationsHistory','sysdiagrams')
					Order by t.object_id, c.column_id
                ")];
            });
            return mapper.Map<List<ColumnResponse>>(result);
        }
    }   
}