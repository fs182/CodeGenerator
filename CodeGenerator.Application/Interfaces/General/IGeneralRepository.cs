using CodeGenerator.Application.Commands.General;

namespace CodeGenerator.Application.Interfaces.General
{
    public interface IGeneralRepository
    {
        Task PopulateTable(PopulateCommand command);
    }
}
