namespace CodeGenerator.Application.Responses
{
    public class TableResponse 
    {
        public short ProjectId { get; set; }
        public int ObjectId { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }
    }
}
