using CodeGenerator.Application.Commands.General;
using CodeGenerator.Application.Interfaces.External;
using MediatR;

namespace CodeGenerator.Application.Handlers.Table
{
    public class PopulateHandler : IRequestHandler<PopulateCommand, Unit>
    {
        private readonly IExternalRepository _repository;
        public PopulateHandler(IExternalRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(PopulateCommand command, CancellationToken cancellationToken)
        {
            await _repository.GetMetadataTable(command);
            return Unit.Value;
        }
    }
}
