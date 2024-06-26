using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeGenerator.Infrastructure.Context.Models
{
    [Table("Table", Schema = "dbo")]
    public class Table
    {
        [Key]
        public int TableId { get; set; }
        public short ProjectId { get; set; }
        public int ObjectId { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public long AuditId { get; set; }
        public List<Column> Columns { get; set; }
        public Catalog Catalog { get; set; }
    }
}
