using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeGenerator.Infrastructure.Context.Models
{
    [Table("Project", Schema = "dbo")]
    public class Project
    {
        [Key]
        public short ProjectId { get; set; }
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string Namespace { get; set; }
        public string ApiControllersPath { get; set; }
        public string ApplicationCommandsPath { get; set; }
        public string ApplicationHandlersPath { get; set; }
        public string ApplicationQueriesPath { get; set; }
        public string ApplicationResponsesPath { get; set; }
        public string ApplicationInterfacesPath { get; set; }
        public string DomainEntitiesPath { get; set; }
        public string InfrastructureContextPath { get; set; }
        public string InfrastructureStoredProceduresCommandsPath { get; set; }
        public string InfrastructureStoredProceduresQueriesPath { get; set; }
        public string InfrastructureMappersPath { get; set; }
        public string InfrastructureRepositoriesPath { get; set; }
        public string UIRoutesPath { get; set; }
        public string UIComponentsPath { get; set; }
        public string StoredProceduresPrefix { get; set; }
        public string Autor { get; set; }
        public long AuditId { get; set; }
        public List<Table> Tables { get; set; }
        public List<Catalog> Catalogs { get; set; }
    }
}
