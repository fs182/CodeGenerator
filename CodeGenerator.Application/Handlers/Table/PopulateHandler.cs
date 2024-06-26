using CodeGenerator.Application.Commands.General;
using CodeGenerator.Application.Interfaces.External;
using MediatR;

namespace CodeGenerator.Application.Handlers.Table
{
    public class PopulateHandler(IExternalRepository externalRepository, ILocalRepository localRepository) : IRequestHandler<PopulateCommand, Unit>
    {
        public async Task<Unit> Handle(PopulateCommand command, CancellationToken cancellationToken)
        {
            var tables = await externalRepository.GetMetadataTable(command);
            await localRepository.PopulateTable(tables, command);
            var columns = await externalRepository.GetMetadataColumn(command);
            await localRepository.PopulateCatalogProperty(columns, command);
            await localRepository.GetProject(command.ProjectId);
            return Unit.Value;
        }
    }
}
