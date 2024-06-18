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
        ORDER BY Esquema, t.Name";
    }
}
