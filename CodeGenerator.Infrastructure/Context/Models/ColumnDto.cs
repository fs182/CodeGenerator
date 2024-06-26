namespace CodeGenerator.Infrastructure.Context.Models
{
    public class ColumnDto
    {
        public short ProjectId { get; set; }
        public int ObjectId { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public byte ColumnNumber { get; set; }
        public string ColumnName { get; set; }
        public string SqlDataType { get; set; }
        public short MaxLength { get; set; }
        public byte Precision { get; set; }
        public byte Scale { get; set; }
        public bool IsNullable { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsForeignKey { get; set; }
        public string SchemaSource { get; set; }
        public string TableSource { get; set; }
        public string ColumnSource { get; set; }
        public string SchemaTarget { get; set; }
        public string TableTarget { get; set; }
        public string ColumnTarget { get; set; }
    }
}
