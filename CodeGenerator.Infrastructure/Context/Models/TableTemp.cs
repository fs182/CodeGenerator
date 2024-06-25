using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeGenerator.Infrastructure.Context.Models
{
    [Table("TableTemp", Schema = "dbo")]
    public class TableTemp
    {
        [Key]
        public long TableTempId { get; set; }
        public short ProjectId { get; set; }
        public int ObjectId { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }
    }
}
