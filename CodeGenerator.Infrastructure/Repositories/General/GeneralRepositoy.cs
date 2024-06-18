using CodeGenerator.Application.Commands.General;
using CodeGenerator.Application.Interfaces.General;
using CodeGenerator.Infrastructure.Context;
using CodeGenerator.Infrastructure.Context.Scripts;
using Microsoft.EntityFrameworkCore;

namespace CodeGenerator.Infrastructure.Repositories.General
{
    public partial class GeneralRepository : IGeneralRepository
    {
        private readonly GeneralContext _context;
        public GeneralRepository(GeneralContext context) => _context = context;
        public async Task PopulateTable(PopulateCommand command)
        {
            await Task.Run(() => {

                var rowsModified = _context.Database.ExecuteSql($"{string.Format(TableScript.PopulateTables, command.ProjectId, command.AuditId)}"); 
            });
        }
    }   
}