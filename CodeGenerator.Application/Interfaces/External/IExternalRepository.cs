using CodeGenerator.Application.Commands.General;
using CodeGenerator.Application.Responses;

namespace CodeGenerator.Application.Interfaces.External
{
    public interface IExternalRepository
    {
        Task<List<TableResponse>> GetMetadataTable(PopulateCommand command);
        Task<List<ColumnResponse>> GetMetadataColumn(PopulateCommand command);
    }
}
