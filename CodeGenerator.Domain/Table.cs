namespace CodeGenerator.Domain
{
    public class Table
    {
        public int ObjectId { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public long AuditId { get; set; }
    }
}
