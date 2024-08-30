using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeGenerator.Infrastructure.Context.Models
{
    [Table("RelatedProperty", Schema = "dbo")]
    public class RelatedProperty
    {
        [Key]
        public int RelatedPropertyId { get; set; }
        public int CatalogId { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public short PropertyOrder { get; set; }
        public bool IncludeInCreateCommand { get; set; }
        public bool IncludeInUpdateCommand { get; set; }
        public bool IncludeInDeleteCommand { get; set; }
        public bool IncludeInWizardCommand { get; set; }
        public bool IncludeInGetQuery { get; set; }
        public long AuditId { get; set; }
    }
}
