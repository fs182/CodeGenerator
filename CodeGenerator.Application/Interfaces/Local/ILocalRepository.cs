using CodeGenerator.Application.Commands.General;
using CodeGenerator.Application.Responses;

namespace CodeGenerator.Application.Interfaces.External
{
    public interface ILocalRepository
    {
        Task PopulateTable(List<TableResponse> tables, PopulateCommand command);
        Task PopulateColumn(List<ColumnResponse> columns, PopulateCommand command);
    }
}
