namespace CodeGenerator.Infrastructure.Context.Models
{
    public class Table
    {
        public short ProjectId { get; set; }
        public int ObjectId { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }
    }
}
