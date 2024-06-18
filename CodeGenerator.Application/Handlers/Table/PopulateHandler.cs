using CodeGenerator.Application.Commands.General;
using CodeGenerator.Application.Interfaces.General;
using MediatR;

namespace CodeGenerator.Application.Handlers.Table
{
    public class PopulateHandler : IRequestHandler<PopulateCommand, Unit>
    {
        private readonly IGeneralRepository _repository;
        public PopulateHandler(IGeneralRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(PopulateCommand command, CancellationToken cancellationToken)
        {
            await _repository.PopulateTable(command);
            return Unit.Value;
        }
    }
}
