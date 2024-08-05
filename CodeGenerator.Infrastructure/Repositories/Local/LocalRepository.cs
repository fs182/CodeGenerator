using AutoMapper;
using CodeGenerator.Application.Commands.General;
using CodeGenerator.Application.Interfaces.External;
using CodeGenerator.Application.Responses;
using CodeGenerator.Infrastructure.Context;
using CodeGenerator.Infrastructure.Context.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeGenerator.Infrastructure.Repositories.Local
{
    public partial class LocalRepository(LocalContext context, IMapper mapper) : ILocalRepository
    {
        public async Task GetProject(short projectId)
        {
			var project = new Project(); 
            await Task.Run(() => {
				project = context.Projects.Where(f => f.ProjectId == projectId).
					Include(g => g.Tables).
                    ThenInclude(g => g.Columns).
                    ThenInclude(g => g.Property).
                    Include(g=> g.Catalogs).
                    ThenInclude(g => g.Properties).
                    First();
            });
			project.Tables.ForEach(f=> f.Catalog = project.Catalogs.First(g => g.TableId == f.TableId));
			await Writer.GenerateProject(project);
        }

        public async Task PopulateTable(List<TableResponse> tables, PopulateCommand command)
        {
            var tempTables = mapper.Map<List<TableTemp>>(tables);
            await Task.Run(() => {
                context.Database.ExecuteSql($@"DELETE dbo.TableTemp WHERE ProjectId = {command.ProjectId}");
                context.BulkInsert(tempTables);
                context.Database.ExecuteSql($@"

					UPDATE Q
					SET objectId = P.ObjectId
					FROM 
					(SELECT b.ObjectId, b.TableName
					FROM dbo.[Column] a RIGHT OUTER JOIN 
					dbo.[TableTemp] B ON a.TableName = b.TableName and a.ObjectId = b.ObjectId
					WHERE a.TableId IS NULL) p INNER JOIN dbo.[Column] q on p.TableName = q.TableName


					UPDATE Q
					SET objectId = P.ObjectId
					FROM 
					(SELECT b.ObjectId, b.TableName
					FROM dbo.[Table] a RIGHT OUTER JOIN 
					dbo.[TableTemp] B ON a.TableName = b.TableName and a.ObjectId = b.ObjectId
					WHERE a.TableId IS NULL) p INNER JOIN dbo.[Table] q on p.TableName = q.TableName

					INSERT INTO dbo.[Table]
					SELECT 
						a.ProjectId, 
						a.ObjectId, 
						a.SchemaName, 
						a.TableName, 
						cast({command.AuditId} as bigint) as AuditId 
					FROM dbo.TableTemp a
                    LEFT OUTER JOIN dbo.[Table] b on a.ObjectId = b.ObjectId
                    WHERE b.TableId IS NULL and a.ProjectId = {command.ProjectId}");
            });
        }

        public async Task PopulateCatalogProperty(List<ColumnResponse> columns, PopulateCommand command)
        {
            var tempColumns = mapper.Map<List<ColumnTemp>>(columns);
            await Task.Run(() => {
                context.Database.ExecuteSql($@"DELETE dbo.ColumnTemp WHERE ProjectId = {command.ProjectId}");
                context.BulkInsert(tempColumns);
                context.Database.ExecuteSql($@"
					
					DECLARE @NoDeleteColumn as Table (ColumnTempId int Primary Key)

					INSERT @NoDeleteColumn 
					SELECT a.ColumnTempId 
					FROM dbo.ColumnTemp a INNER JOIN dbo.[Table] b 
					on a.ObjectId = b.ObjectId
					LEFT OUTER JOIN dbo.[Column] c on 
									a.[ObjectId] = c.[ObjectId] and
									a.[ColumnNumber] = c.[ColumnNumber] and
									a.[SchemaName] = c.[SchemaName] and
									a.[TableName] = c.[TableName] 
					WHERE c.ObjectId is null and a.ProjectId = {command.ProjectId} 	
					
					DELETE a
					FROM dbo.ColumnTemp a INNER JOIN dbo.[Table] b 
					on a.ObjectId = b.ObjectId
					LEFT OUTER JOIN dbo.[Column] c on 
						a.[ObjectId] = c.[ObjectId] and
						a.[ColumnNumber] = c.[ColumnNumber] and
						a.[SchemaName] = c.[SchemaName] and
						a.[TableName] = c.[TableName] and
						(a.[ColumnName] <> c.[ColumnName] or
						a.[SqlDataType] <> c.[SqlDataType] or
						a.[MaxLength] <> c.[MaxLength] or
						a.[Precision] <> c.[Precision] or
						a.[Scale] <> c.[Scale] or
						a.[IsNullable] <> c.[IsNullable] or
						a.[IsIdentity] <> c.[IsIdentity] or
						a.[IsPrimaryKey] <> c.[IsPrimaryKey] or
						a.[IsForeignKey] <> c.[IsForeignKey] or
						isnull(a.[SchemaSource],'') <> isnull(c.[SchemaSource],'') or
						isnull(a.[TableSource],'') <> isnull(c.[TableSource],'') or
						isnull(a.[ColumnSource],'') <> isnull(c.[ColumnSource],'') or
						isnull(a.[SchemaTarget],'') <> isnull(c.[SchemaTarget],'') or
						isnull(a.[TableTarget],'') <> isnull(c.[TableTarget],'') or
						isnull(a.[ColumnTarget],'') <> isnull(c.[ColumnTarget],''))
					LEFT OUTER JOIN @NoDeleteColumn d on a.ColumnTempId = d.ColumnTempId
					WHERE c.ObjectId is null and d.ColumnTempId is null and a.ProjectId = {command.ProjectId} 	

					DECLARE @DeleteColumn as Table (ColumnId int Primary Key)
					

					INSERT INTO @DeleteColumn
					SELECT a.ColumnId 
					FROM dbo.[Column] a INNER JOIN dbo.[Table] b 
					on a.ObjectId = b.ObjectId
					LEFT OUTER JOIN dbo.[Column] c on 
						a.[ObjectId] = c.[ObjectId] and
						a.[ColumnNumber] = c.[ColumnNumber] and
						a.[SchemaName] = c.[SchemaName] and
						a.[TableName] = c.[TableName] and
						(a.[ColumnName] <> c.[ColumnName] or
						a.[SqlDataType] <> c.[SqlDataType] or
						a.[MaxLength] <> c.[MaxLength] or
						a.[Precision] <> c.[Precision] or
						a.[Scale] <> c.[Scale] or
						a.[IsNullable] <> c.[IsNullable] or
						a.[IsIdentity] <> c.[IsIdentity] or
						a.[IsPrimaryKey] <> c.[IsPrimaryKey] or
						a.[IsForeignKey] <> c.[IsForeignKey] or
						isnull(a.[SchemaSource],'') <> isnull(c.[SchemaSource],'') or
						isnull(a.[TableSource],'') <> isnull(c.[TableSource],'') or
						isnull(a.[ColumnSource],'') <> isnull(c.[ColumnSource],'') or
						isnull(a.[SchemaTarget],'') <> isnull(c.[SchemaTarget],'') or
						isnull(a.[TableTarget],'') <> isnull(c.[TableTarget],'') or
						isnull(a.[ColumnTarget],'') <> isnull(c.[ColumnTarget],'') )
					WHERE c.ObjectId is not null and b.ProjectId = {command.ProjectId} 					
	
					DELETE a
					FROM dbo.[Property] a INNER JOIN @DeleteColumn b on a.ColumnId = b.ColumnId

					DELETE a
					FROM dbo.[Column] a INNER JOIN @DeleteColumn b on a.ColumnId = b.ColumnId

					INSERT INTO dbo.[Column]
					SELECT       
					b.[TableId]
					,a.[ObjectId]
					,a.[SchemaName]
					,a.[TableName]
					,a.[ColumnNumber]
					,a.[ColumnName]
					,a.[SqlDataType]
					,a.[MaxLength]
					,a.[Precision]
					,a.[Scale]
					,a.[IsNullable]
					,a.[IsIdentity]
					,a.[IsPrimaryKey]
					,a.[IsForeignKey]
					,a.[SchemaSource]
					,a.[TableSource]
					,a.[ColumnSource]
					,a.[SchemaTarget]
					,a.[TableTarget]
					,a.[ColumnTarget]
					,cast({command.AuditId} as bigint) as Audit
					FROM dbo.ColumnTemp a INNER JOIN dbo.[Table] b 
					on a.ObjectId = b.ObjectId
					LEFT OUTER JOIN dbo.[Column] c on 
						a.[ObjectId] = c.[ObjectId] and
						a.[SchemaName] = c.[SchemaName] and
						a.[TableName] = c.[TableName] and
						a.[ColumnNumber] = c.[ColumnNumber] and
						a.[ColumnName] = c.[ColumnName] and
						a.[SqlDataType] = c.[SqlDataType] and
						a.[MaxLength] = c.[MaxLength] and
						a.[Precision] = c.[Precision] and
						a.[Scale] = c.[Scale] and
						a.[IsNullable] = c.[IsNullable] and
						a.[IsIdentity] = c.[IsIdentity] and
						a.[IsPrimaryKey] = c.[IsPrimaryKey] and
						a.[IsForeignKey] = c.[IsForeignKey] and
						isnull(a.[SchemaSource],'') = isnull(c.[SchemaSource],'') and
						isnull(a.[TableSource],'') = isnull(c.[TableSource],'') and
						isnull(a.[ColumnSource],'') = isnull(c.[ColumnSource],'') and
						isnull(a.[SchemaTarget],'') = isnull(c.[SchemaTarget],'') and
						isnull(a.[TableTarget],'') = isnull(c.[TableTarget],'') and
						isnull(a.[ColumnTarget],'') = isnull(c.[ColumnTarget],'') 
					WHERE c.ObjectId is null and a.ProjectId = {command.ProjectId}


					INSERT INTO [dbo].[Catalog]
					     SELECT
					            cast({command.ProjectId} as bigint) as ProjectId,
								a.TableId,
					            cast (1 as bit) as IsEnabled, 
					            cast (1 as bit) as HasMenu,
					            cast (1 as bit) as HasGrid,
					            cast (1 as bit) as HasForm,
					            cast (1 as bit) as HasSearchEngine,
					            cast (1 as bit) as HasPagination, 
					            cast (1 as bit) as HasOnlyBackend,
								cast (1 as bit) as IsEditable, 
								cast (1 as bit) as CanBeDeleted, 
								cast (1 as bit) as CanBeCreated, 
								cast (1 as bit) as CanBeUpdated, 
					            a.TableName + '[Form name]' as FormName, 
					            a.TableName + '[Form description]' as FormDescription,
								a.TableName as GridName, 
								a.TableName + '[Grid description]' as GridDescription, 
					            cast({command.AuditId} as bigint) as AuditId
					FROM dbo.[Table] a LEFT OUTER JOIN dbo.[Catalog] b
						ON a.TableId = b.TableId
						WHERE b.CatalogId IS NULL and a.ProjectId = {command.ProjectId}

					INSERT INTO [dbo].[Property]
					SELECT          
						c.CatalogId
						,a.ColumnId
						,cast(1 as bit) as IsEnabled
						,cast(1 as bit) as IsForSearchEngine
						,cast(1 as bit) as IsForForm
						,cast(1 as bit) as IsForGrid
						,cast(0 as bit) as IsReadOnly
						,cast(0 as bit) as CreateGetBy
						,cast(0 as bit) as CreateGetByIncludeController
						,cast(1 as smallint) as [Order]
						,a.ColumnName + '[Grid header]' as GridHeader
						,a.ColumnName + '[Form title]' as FormTitle
						,a.ColumnName + '[Form description]' as FormDescription
						,a.ColumnName + '[Search engine title]' as SearchEngineTitle
						,cast({command.AuditId} as bigint) as AuditId
					
					FROM dbo.[Column] a 
						INNER JOIN dbo.[Table] b on a.TableId = b.TableId
						INNER JOIN dbo.[Catalog] c on b.TableId = c.TableId
						LEFT OUTER JOIN dbo.[Property] d on a.ColumnId = d.ColumnId
						WHERE d.PropertyId IS NULL AND b.ProjectId = {command.ProjectId}
                ");
            });
        }
    }
}
