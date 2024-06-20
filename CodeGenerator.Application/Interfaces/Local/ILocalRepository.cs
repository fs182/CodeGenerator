using CodeGenerator.Application.Responses;

namespace CodeGenerator.Application.Interfaces.External
{
    public interface ILocalRepository
    {
        Task PopulateTable(List<TableResponse> tables);
    }
}
