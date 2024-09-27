using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeGenerator.Infrastructure.Context.Models
{
    [Table("Property", Schema = "dbo")]
    public class Property
    {
        [Key]
        public int PropertyId { get; set; }
        public int CatalogId { get; set; }
        public int ColumnId { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsForSearchEngine { get; set; }
        public bool IsForForm { get; set; }
        public bool IsForGrid { get; set; }
        public bool IsReadOnly { get; set; }
        public bool CreateGetBy { get; set; }
        public bool CreateGetByIncludeController { get; set; }
        public bool CreateGetByReturnList { get; set; }
        public bool IsDescriptionColumn { get; set; }
        public short DescriptionOrder { get; set; }
        public string DescriptionPreText { get; set; }
        public string DescriptionPostText { get; set; }
        public short Order { get; set; }
        public string GridHeader { get; set; }
        public string FormTitle { get; set; }
        public string FormDescription { get; set; }
        public string SearchEngineTitle { get; set; }
        public long AuditId { get; set; }
    }
}
