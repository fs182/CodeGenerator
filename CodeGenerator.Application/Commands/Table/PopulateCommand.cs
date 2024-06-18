using MediatR;

namespace CodeGenerator.Application.Commands.General
{
    public class PopulateCommand : IRequest<Unit>
    {
        public short ProjectId { get; set; }
        public long AuditId { get; set; }
    }
}
