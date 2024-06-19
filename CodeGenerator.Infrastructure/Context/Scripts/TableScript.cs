namespace CodeGenerator.Infrastructure.Context.Scripts
{
    public class TableScript
    {
        public const string PopulateTables = @"
        DELETE dbo.[Table] where ProjectId = {0}           

        INSERT INTO dbo.[Table] VALUES (ProjectId, ObjectId, SchemaName, TableName, AuditId)

            SELECT	
            {0},
		    t.object_id, 
		    SCHEMA_NAME(t.schema_id),
    		t.name,
            {1}
        FROM sys.tables AS t
        WHERE T.name NOT IN ('__EFMigrationsHistory','sysdiagrams')
        ORDER BY t.schema_id, t.Name";

        public const string GetMetadataTables = @"
            SELECT	
            {0} as ProyectId,
		    t.object_id as ObjectId, 
		    SCHEMA_NAME(t.schema_id) as SchemaName,
    		t.name as TableName,
            {1} as AuditId
        FROM sys.tables AS t
        WHERE T.name NOT IN ('__EFMigrationsHistory','sysdiagrams')
        ORDER BY t.schema_id, t.Name";
    }
}
