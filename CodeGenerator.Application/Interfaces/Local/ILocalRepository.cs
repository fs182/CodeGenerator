using CodeGenerator.Application.Commands.General;
using CodeGenerator.Application.Responses;

namespace CodeGenerator.Application.Interfaces.External
{
    public interface ILocalRepository
    {
        Task GetProject(short projectId);
        Task PopulateTable(List<TableResponse> tables, PopulateCommand command);
        Task PopulateCatalogProperty(List<ColumnResponse> columns, PopulateCommand command);
    }
}
