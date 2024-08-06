using CodeGenerator.Infrastructure.Context.Models;
using System.Text;


namespace CodeGenerator.Infrastructure.Templates.CleanArquitecture.API
{
    public static class Controller
    {
        public static void WriteController(Project project, Table table)
        {
            var pk = table.Columns.First(f => f.IsPrimaryKey);
            var customGetMethods = table.Columns.Where(f => f.Property.CreateGetBy && f.Property.CreateGetByIncludeController).ToList();
            using StreamWriter outputFile = new(Path.Combine(project.ApiControllersPath, string.Concat(table.TableName, "Controller.cs")), false, Encoding.UTF8);
            outputFile.WriteLine(string.Concat("using ", project.Namespace, ".API.Controllers.Base;"));
            outputFile.WriteLine(string.Concat("using ", project.Namespace, ".Application.Commands.", table.TableName, ";"));
            outputFile.WriteLine(string.Concat("using ", project.Namespace, ".Application.Queries.", table.TableName, ";"));
            outputFile.WriteLine(string.Concat("using ", project.Namespace, ".Application.Responses.", table.TableName, ";"));
            if (customGetMethods.Any(f=>f.ColumnName == "UsuarioId"))
                outputFile.WriteLine(string.Concat("using Microsoft.AspNetCore.Authorization;"));
            outputFile.WriteLine(string.Concat("using Microsoft.AspNetCore.Mvc;"));
            outputFile.WriteLine(string.Concat("using Microsoft.Data.SqlClient;"));
            outputFile.WriteLine("using Swashbuckle.AspNetCore.Annotations;");
            outputFile.WriteLine("using System.Net.Mime;");
            if (customGetMethods.Any(f => f.ColumnName == "UsuarioId"))
                outputFile.WriteLine(string.Concat("using System.Security.Claims;"));
            outputFile.WriteLine();
            outputFile.WriteLine(string.Concat("namespace ", project.Namespace, ".API.Controllers"));
            outputFile.WriteLine("{");
            if (customGetMethods.Any(f => f.ColumnName == "UsuarioId"))
                outputFile.WriteLine("    [Authorize]");
            outputFile.WriteLine("    [Consumes(MediaTypeNames.Application.Json)]");
            outputFile.WriteLine("    [Produces(MediaTypeNames.Application.Json)]");
            outputFile.WriteLine("    [ProducesResponseType(StatusCodes.Status200OK)]");
            outputFile.WriteLine("    [ProducesResponseType(StatusCodes.Status400BadRequest)]");
            outputFile.WriteLine(string.Concat("    public partial class ", table.TableName, "Controller : ApiControllerBase"));
            outputFile.WriteLine("    {");
            outputFile.WriteLine("");
            outputFile.WriteLine("        [HttpPost]");
            outputFile.WriteLine(string.Concat("        [Route(\"/", Helper.GetCamel(table.TableName), "/getPaginated\")]"));
            outputFile.WriteLine(string.Concat("        [SwaggerOperation(Description = \"Trae los registros de ", table.TableName, " paginados.\")]"));
            outputFile.WriteLine("        public async Task<ActionResult<List<GetResponse>>> GetPaginated([FromBody] GetQuery query)");
            outputFile.WriteLine("        {");
            outputFile.WriteLine("            try ");
            outputFile.WriteLine("            {");
            outputFile.WriteLine("                var result = await Mediator.Send(query);");
            outputFile.WriteLine("                return Ok(result);");
            outputFile.WriteLine("            } ");
            outputFile.WriteLine("            catch(Exception ex) {");
            outputFile.WriteLine("                return BadRequest(ex.Message);");
            outputFile.WriteLine("            }   ");
            outputFile.WriteLine("            ");
            outputFile.WriteLine("        }");
            outputFile.WriteLine("");
            foreach (var customMethod in customGetMethods)
            {
                outputFile.WriteLine($"        [HttpPost]");
                outputFile.WriteLine(string.Concat("        [Route(\"/", Helper.GetCamel(table.TableName), "/getBy",customMethod.ColumnName,"\")]"));
                outputFile.WriteLine($"        [SwaggerOperation(Description = \"Trae el registro de {table.TableName} que coincida con el {customMethod.ColumnName}.\")]");
                if (customMethod.ColumnName == "UsuarioId")
                    outputFile.WriteLine($"        public async Task<ActionResult<List<GetResponse>>> GetBy{customMethod.ColumnName}()");
                else
                    outputFile.WriteLine($"        public async Task<ActionResult<List<GetResponse>>> GetBy{customMethod.ColumnName}([FromBody] GetByColumnQuery query)");
                outputFile.WriteLine("        {");
                outputFile.WriteLine($"            try");
                outputFile.WriteLine("            {");
                if (customMethod.ColumnName == "UsuarioId")
                {
                    outputFile.WriteLine($"                var identity = HttpContext.User.Identity as ClaimsIdentity;");
                    outputFile.WriteLine($"                if (!identity.IsAuthenticated)");
                    outputFile.WriteLine($"                    return BadRequest(\"No autenticado\");");
                    outputFile.WriteLine(string.Concat("                var query = new GetByColumnQuery { ", customMethod.ColumnName, " = ", Helper.GetStringNetCoreType(customMethod.SqlDataType), ".Parse(identity.Claims.First(f => f.Type == \"UserId\").Value) };"));
                }                    
                outputFile.WriteLine($"                var result = await Mediator.Send(query);");
                outputFile.WriteLine($"                return Ok(result);");
                outputFile.WriteLine("            }");
                outputFile.WriteLine($"            catch (Exception ex)");
                outputFile.WriteLine("            {");
                outputFile.WriteLine($"                return BadRequest(ex.Message);");
                outputFile.WriteLine("            }");
                outputFile.WriteLine("        }");
                outputFile.WriteLine("");
            }
            outputFile.WriteLine("        [HttpPost]");
            outputFile.WriteLine(string.Concat("        [Route(\"/", Helper.GetCamel(table.TableName), "/update\")]"));
            outputFile.WriteLine(string.Concat("        [SwaggerOperation(Description = \"Actualiza un registro de ", table.TableName, ".\")]"));
            outputFile.WriteLine(string.Concat("        public async Task<ActionResult<List<CommandResponse>>> Update([FromBody] UpdateCommand command)"));
            outputFile.WriteLine("        {");
            outputFile.WriteLine("            try");
            outputFile.WriteLine("            {");
            outputFile.WriteLine("                command.AuditoriaId = 1;");
            outputFile.WriteLine(string.Concat("                var result = await Mediator.Send(command);"));
            outputFile.WriteLine("                return Ok(result);");
            outputFile.WriteLine("            }");
            outputFile.WriteLine("            catch (Exception ex)");
            outputFile.WriteLine("            {");
            outputFile.WriteLine("                return BadRequest(ex.Message);");
            outputFile.WriteLine("            }");
            outputFile.WriteLine("");
            outputFile.WriteLine("        }");
            outputFile.WriteLine("");
            outputFile.WriteLine("        [HttpPost]");
            outputFile.WriteLine(string.Concat("        [Route(\"/", Helper.GetCamel(table.TableName), "/delete\")]"));
            outputFile.WriteLine(string.Concat("        [SwaggerOperation(Description = \"Elimina un registro de ", table.TableName, ".\")]"));
            outputFile.WriteLine(string.Concat("        public async Task<ActionResult<List<CommandResponse>>> Delete(DeleteCommand command)"));
            outputFile.WriteLine("        {");
            outputFile.WriteLine("            try");
            outputFile.WriteLine("            {");
            outputFile.WriteLine("                command.AuditoriaId = 1;");
            outputFile.WriteLine(string.Concat("                var result = await Mediator.Send(command);"));
            outputFile.WriteLine("                return Ok(result);");
            outputFile.WriteLine("            }");
            outputFile.WriteLine("            catch (Exception ex)");
            outputFile.WriteLine("            {");
            outputFile.WriteLine("                if (ex is SqlException exception)");
            outputFile.WriteLine("                {");
            outputFile.WriteLine("                    if (exception.Errors[0].Number == 547)");
            outputFile.WriteLine("                        return BadRequest(\"No puede eliminar el registro porque tiene información relacionada.\");");
            outputFile.WriteLine("                }");
            outputFile.WriteLine("                return BadRequest(ex.Message);");
            outputFile.WriteLine("            }");
            outputFile.WriteLine("");
            outputFile.WriteLine("        }");
            outputFile.WriteLine("");
            outputFile.WriteLine("        [HttpPost]");
            outputFile.WriteLine(string.Concat("        [Route(\"/", Helper.GetCamel(table.TableName), "/create\")]"));
            outputFile.WriteLine(string.Concat("        [SwaggerOperation(Description = \"Crea un registro de ", table.TableName, ".\")]"));
            outputFile.WriteLine("        public async Task<ActionResult<List<CommandResponse>>> Create(CreateCommand command)");
            outputFile.WriteLine("        {");
            outputFile.WriteLine("            try");
            outputFile.WriteLine("            {");
            outputFile.WriteLine("                command. AuditoriaId = 1;");
            outputFile.WriteLine("                var result = await Mediator.Send(command);");
            outputFile.WriteLine("                return Ok(result);");
            outputFile.WriteLine("            }");
            outputFile.WriteLine("            catch (Exception ex)");
            outputFile.WriteLine("            {");
            outputFile.WriteLine("                return BadRequest(ex.Message);");
            outputFile.WriteLine("            }");
            outputFile.WriteLine("");
            outputFile.WriteLine("        }");
            outputFile.WriteLine("    }");
            outputFile.WriteLine("}");
            outputFile.Close();
            outputFile.Dispose();
        }
    }
}
