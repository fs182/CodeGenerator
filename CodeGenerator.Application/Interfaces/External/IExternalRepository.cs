using CodeGenerator.Application.Commands.General;

namespace CodeGenerator.Application.Interfaces.External
{
    public interface IExternalRepository
    {
        Task GetMetadataTable(PopulateCommand command);
    }
}
