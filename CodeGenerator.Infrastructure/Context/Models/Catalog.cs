using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeGenerator.Infrastructure.Context.Models
{
    [Table("Catalog", Schema = "dbo")]
    public class Catalog
    {
        [Key]
        public int CatalogId { get; set; }
        public int TableId { get; set; }
        public bool IsEnabled { get; set; }
        public bool HasMenu { get; set; }
        public bool HasGrid { get; set; }
        public bool HasForm { get; set; }
        public bool HasSearchEngine { get; set; }
        public bool HasPagination { get; set; }
        public bool HasOnlyBackend { get; set; }
        public bool IsEditable { get; set; }
        public bool CanBeDeleted { get; set; }
        public bool CanBeCreated { get; set; }
        public bool CanBeUpdated { get; set; }
        public string FormName { get; set; }
        public string FormDescription { get; set; }
        public string GridName { get; set; }
        public string GridDescription { get; set; }
        public long AuditId { get; set; }
        public List<Property> Properties { get; set; }
    }
}
