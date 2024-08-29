using CodeGenerator.Infrastructure.Context.Models;
using System.Text;


namespace CodeGenerator.Infrastructure.Templates.CleanArquitecture.Application
{
    public static class Handler
    {
        public static void WriteCreateHandler(Project project, Table table)
        {
            if (!Directory.Exists(Path.Combine(project.ApplicationHandlersPath, table.TableName)))
                Directory.CreateDirectory(Path.Combine(project.ApplicationHandlersPath, table.TableName));
            using StreamWriter outputFile = new(Path.Combine(project.ApplicationHandlersPath, table.TableName, string.Concat("CreateHandler.cs")), false, Encoding.UTF8);
            outputFile.WriteLine($"using {project.Namespace}.Application.Commands.{table.TableName};");
            outputFile.WriteLine($"using {project.Namespace}.Application.Responses.{table.TableName};");
            outputFile.WriteLine($"using {project.Namespace}.Application.Interfaces;");
            outputFile.WriteLine("using MediatR;");
            outputFile.WriteLine($"namespace {project.Namespace}.Application.Handlers.{table.TableName}");
            outputFile.WriteLine("{");
            outputFile.WriteLine("    public class CreateHandler : IRequestHandler<CreateCommand, List<CommandResponse>>");
            outputFile.WriteLine("    {");
            outputFile.WriteLine("        private readonly ICatalogCommandRepository _repository;");
            outputFile.WriteLine("        private readonly ICustomRepository _customRepository;");
            outputFile.WriteLine("        public CreateHandler(ICatalogCommandRepository repository, ICustomRepository customRepository)");
            outputFile.WriteLine("        {");
            outputFile.WriteLine("            _repository = repository;");
            outputFile.WriteLine("            _customRepository = customRepository;");
            outputFile.WriteLine("        }");
            outputFile.WriteLine("        public async Task<List<CommandResponse>> Handle(CreateCommand command, CancellationToken cancellationToken)");          
            outputFile.WriteLine("        {");
            outputFile.WriteLine(string.Concat("            command.AuditoriaId = await _customRepository.SetAudit(new Commands.Auditoria.AuditoriaUniqueCommand { OperacionId = (short)Domain.Enums.Operation.", table.TableName, "Create, FechaModificacion = DateTime.Now, UsuarioId = command.UsuarioId });"));
            outputFile.WriteLine($"            return await _repository.{table.TableName}Create(command);");
            outputFile.WriteLine("        }");
            outputFile.WriteLine("    }");
            outputFile.WriteLine("}");
            outputFile.Close();
            outputFile.Dispose();
        }

        public static void WriteDeleteHandler(Project project, Table table)
        {
            if (!Directory.Exists(Path.Combine(project.ApplicationHandlersPath, table.TableName)))
                Directory.CreateDirectory(Path.Combine(project.ApplicationHandlersPath, table.TableName));
            using StreamWriter outputFile = new(Path.Combine(project.ApplicationHandlersPath, table.TableName, string.Concat("DeleteHandler.cs")), false, Encoding.UTF8);
            outputFile.WriteLine($"using {project.Namespace}.Application.Commands.{table.TableName};");
            outputFile.WriteLine($"using {project.Namespace}.Application.Interfaces;");
            outputFile.WriteLine($"using {project.Namespace}.Application.Responses.{table.TableName};");
            outputFile.WriteLine("using MediatR;");
            outputFile.WriteLine($"namespace {project.Namespace}.Application.Handlers.{table.TableName}");
            outputFile.WriteLine("{");
            outputFile.WriteLine("    public class DeleteHandler : IRequestHandler<DeleteCommand, List<CommandResponse>>");
            outputFile.WriteLine("    {");
            outputFile.WriteLine("        private readonly ICatalogCommandRepository _repository;");
            outputFile.WriteLine("        private readonly ICustomRepository _customRepository;");
            outputFile.WriteLine("        public DeleteHandler(ICatalogCommandRepository repository, ICustomRepository customRepository)");
            outputFile.WriteLine("        {");
            outputFile.WriteLine("            _repository = repository;");
            outputFile.WriteLine("            _customRepository = customRepository;");
            outputFile.WriteLine("        }");
            outputFile.WriteLine("        public async Task<List<CommandResponse>> Handle(DeleteCommand command, CancellationToken cancellationToken)");
            outputFile.WriteLine("        {");
            outputFile.WriteLine(string.Concat("            command.AuditoriaId = await _customRepository.SetAudit(new Commands.Auditoria.AuditoriaUniqueCommand { OperacionId = (short)Domain.Enums.Operation.", table.TableName ,"Delete, FechaModificacion = DateTime.Now, UsuarioId = command.UsuarioId });"));
            outputFile.WriteLine($"            return await _repository.{table.TableName}Delete(command);");
            outputFile.WriteLine("        }");
            outputFile.WriteLine("    }");
            outputFile.WriteLine("}");
            outputFile.Close();
            outputFile.Dispose();
        }

        public static void WriteGetHandler(Project project, Table table)
        {
            if (!Directory.Exists(Path.Combine(project.ApplicationHandlersPath, table.TableName)))
                Directory.CreateDirectory(Path.Combine(project.ApplicationHandlersPath, table.TableName));
            using StreamWriter outputFile = new(Path.Combine(project.ApplicationHandlersPath, table.TableName, string.Concat("GetHandler.cs")), false, Encoding.UTF8);
            outputFile.WriteLine(string.Concat("using ", project.Namespace, ".Application.Interfaces;"));
            outputFile.WriteLine(string.Concat("using ", project.Namespace, ".Application.Queries.", table.TableName, ";"));
            outputFile.WriteLine(string.Concat("using ", project.Namespace, ".Application.Responses.", table.TableName, ";"));
            outputFile.WriteLine("using MediatR;");
            outputFile.WriteLine(string.Concat("namespace ", project.Namespace, ".Application.Handlers.", table.TableName));
            outputFile.WriteLine("{");
            outputFile.WriteLine("    public class GetHandler : IRequestHandler<GetQuery, List<GetResponse>>");
            outputFile.WriteLine("    {");
            outputFile.WriteLine("        private readonly ICatalogQueryRepository _repository;");
            outputFile.WriteLine("        public GetHandler(ICatalogQueryRepository repository)");
            outputFile.WriteLine("        {");
            outputFile.WriteLine("            _repository = repository;");
            outputFile.WriteLine("        }");
            outputFile.WriteLine("        public async Task<List<GetResponse>> Handle(GetQuery query, CancellationToken cancellationToken)");
            outputFile.WriteLine("        {");
            outputFile.WriteLine(string.Concat("            return await _repository.Get", table.TableName, "s(query);"));
            outputFile.WriteLine("        }");
            outputFile.WriteLine("    }");
            outputFile.WriteLine("}");
            outputFile.Close();
            outputFile.Dispose();
        }

        public static void WriteGetByCustom(Project project, Table table)
        {

            var customGetMethods = table.Columns.Where(f => f.Property.CreateGetBy && f.Property.CreateGetByIncludeController).ToList();
            foreach (var customMethod in customGetMethods)
            {
                if (!Directory.Exists(Path.Combine(project.ApplicationHandlersPath, table.TableName)))
                    Directory.CreateDirectory(Path.Combine(project.ApplicationHandlersPath, table.TableName));
                using StreamWriter outputFile = new(Path.Combine(project.ApplicationHandlersPath, table.TableName, string.Concat("GetByColumnHandler.cs")), false, Encoding.UTF8);
                outputFile.WriteLine(string.Concat("using ", project.Namespace, ".Application.Interfaces;"));
                outputFile.WriteLine(string.Concat("using ", project.Namespace, ".Application.Queries.", table.TableName, ";"));
                outputFile.WriteLine(string.Concat("using ", project.Namespace, ".Application.Responses.", table.TableName, ";"));
                outputFile.WriteLine("using MediatR;");
                outputFile.WriteLine(string.Concat("namespace ", project.Namespace, ".Application.Handlers.", table.TableName));
                outputFile.WriteLine("{");
                if(customMethod.Property.CreateGetByReturnList)
                    outputFile.WriteLine($"    public class GetByColumnHandler : IRequestHandler<GetByColumnQuery, List<GetResponse>>");
                else
                    outputFile.WriteLine($"    public class GetByColumnHandler : IRequestHandler<GetByColumnQuery, GetResponse>");
                outputFile.WriteLine("    {");
                outputFile.WriteLine("        private readonly ICatalogQueryRepository _repository;");
                outputFile.WriteLine("        public GetByColumnHandler(ICatalogQueryRepository repository)");
                outputFile.WriteLine("        {");
                outputFile.WriteLine("            _repository = repository;");
                outputFile.WriteLine("        }");
                if (customMethod.Property.CreateGetByReturnList)
                    outputFile.WriteLine("        public async Task<List<GetResponse>> Handle(GetByColumnQuery query, CancellationToken cancellationToken)");
                else
                    outputFile.WriteLine("        public async Task<GetResponse> Handle(GetByColumnQuery query, CancellationToken cancellationToken)");
                outputFile.WriteLine("        {");
                outputFile.WriteLine(string.Concat("            return await _repository.Get", table.TableName, "By",customMethod.ColumnName,"(query.", customMethod.ColumnName, ");"));
                outputFile.WriteLine("        }");
                outputFile.WriteLine("    }");
                outputFile.WriteLine("}");
                outputFile.Close();
                outputFile.Dispose();
            }
        }

        public static void WriteUpdateHandler(Project project, Table table)
        {
            if (!Directory.Exists(Path.Combine(project.ApplicationHandlersPath, table.TableName)))
                Directory.CreateDirectory(Path.Combine(project.ApplicationHandlersPath, table.TableName));
            using StreamWriter outputFile = new(Path.Combine(project.ApplicationHandlersPath, table.TableName, string.Concat("UpdateHandler.cs")), false, Encoding.UTF8);
            outputFile.WriteLine($"using {project.Namespace}.Application.Interfaces;");
            outputFile.WriteLine($"using {project.Namespace}.Application.Commands.{table.TableName};");
            outputFile.WriteLine($"using {project.Namespace}.Application.Responses.{table.TableName};");
            outputFile.WriteLine("using MediatR;");
            outputFile.WriteLine("");
            outputFile.WriteLine($"namespace {project.Namespace}.Application.Handlers.{table.TableName}");
            outputFile.WriteLine("{");
            outputFile.WriteLine("    public class UpdateHandler : IRequestHandler<UpdateCommand, List<CommandResponse>>");
            outputFile.WriteLine("    {");
            outputFile.WriteLine("        private readonly ICatalogCommandRepository _repository;");
            outputFile.WriteLine("        private readonly ICustomRepository _customRepository;");
            outputFile.WriteLine("        public UpdateHandler(ICatalogCommandRepository repository, ICustomRepository customRepository)");
            outputFile.WriteLine("        {");
            outputFile.WriteLine("            _repository = repository;");
            outputFile.WriteLine("            _customRepository = customRepository;");
            outputFile.WriteLine("        }");
            outputFile.WriteLine("        public async Task<List<CommandResponse>> Handle(UpdateCommand command, CancellationToken cancellationToken)");
            outputFile.WriteLine("        {");
            outputFile.WriteLine(string.Concat("            command.AuditoriaId = await _customRepository.SetAudit(new Commands.Auditoria.AuditoriaUniqueCommand { OperacionId = (short)Domain.Enums.Operation.", table.TableName, "Update, FechaModificacion = DateTime.Now, UsuarioId = command.UsuarioId });"));
            outputFile.WriteLine($"            return await _repository.{table.TableName}Update(command);");
            outputFile.WriteLine("        }");
            outputFile.WriteLine("    }");
            outputFile.WriteLine("}");
            outputFile.Close();
            outputFile.Dispose();
        }

    }
}
